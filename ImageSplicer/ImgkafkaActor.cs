using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ImageSplicer.Common;
using ImageSplicer.Common.Util;
using ImageSplicer.Data;
using KafkaNet;
using KafkaNet.Common;
using KafkaNet.Model;
using KafkaNet.Protocol;
using Newtonsoft.Json;

namespace ImageSplicer
{
    /// <summary>
    /// 数据接收处理器
    /// </summary>
    public class ImgkafkaActor
    {
        private static string _topicName = ConfigurationHelper.GetValue("InputTopic", "sendBelt");
        private static string _kafkaAddr = ConfigurationHelper.GetValue("KafkaAddr", "http://37.95.104.127:9092");

        private static string _outputtopicName = ConfigurationHelper.GetValue("OutputTopic", "sendTopic");
        private static int _queueMax = Int32.Parse(ConfigurationHelper.GetValue("QueueMax", "600"));
        private CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private Task _task = null;
        private Task _taskSend = null;
        public void Initial()
        {
            _task = Task.Run(() =>
            {
                
                var options1 = new KafkaOptions(new Uri(_kafkaAddr), new Uri(_kafkaAddr))
                {
                    Log = new ConsoleLog()
                };
                var consumer = new Consumer(new ConsumerOptions(_topicName, new BrokerRouter(options1)) { Log = new ConsoleLog() });

                // 从数据文件加载
                List<KafkaPartOffset> kafkaPartOffset;
                XmlDataControl.ReadConfig(out kafkaPartOffset);
                if (kafkaPartOffset != null && kafkaPartOffset.Count > 0)
                {
                    OffsetPosition[] offsetPositions = new OffsetPosition[kafkaPartOffset.Count];
                    for (int index = 0; index < kafkaPartOffset.Count; index++)
                    {
                        offsetPositions[index] = new OffsetPosition(kafkaPartOffset[index].PartitionId, kafkaPartOffset[index].Offset);
                    }
                    consumer.SetOffsetPosition(offsetPositions);
                }
                foreach (var data in consumer.Consume())
                {
                    if (_tokenSource.Token.IsCancellationRequested)
                    {
                        _tokenSource.Token.ThrowIfCancellationRequested();
                    }
                    else
                    {
                        Log4NetHelper.Instance.Debug("接收Kafka数据成功：" + data.Meta.PartitionId +"-"+ data.Meta.Offset + ", data");
                        // 保存数据到配置文件
                        XmlDataControl.WriteConfig(data.Meta.PartitionId, data.Meta.Offset);

                        Send2Quere(data.Value);
                    }
                }
            }, _tokenSource.Token);

            _taskSend = Task.Run(() =>
            {
                var options2 = new KafkaOptions(new Uri(_kafkaAddr))
                {
                    Log = new ConsoleLog()
                };
                var producer = new Producer(new BrokerRouter(options2))
                {
                    BatchSize = 10,
                    BatchDelayTime = TimeSpan.FromMilliseconds(2000)
                };
                while (true)
                {
                    try
                    {
                        string message = ResultSpliceQueue.GetFromQueue();
                        if (string.IsNullOrEmpty(message))
                        {
                            Thread.Sleep(1000);
                            continue;
                        }
                        else
                        {
                            producer.SendMessageAsync(_outputtopicName, new[] {new Message(message)}).Wait();
                            Console.WriteLine("Posted messages. AsyncCount:{0}", producer.AsyncCount);
                            Log4NetHelper.Instance.Debug("发送Kafka数据成功：" + message);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log4NetHelper.Instance.Error("从队列发送Kafka数据错误:" + (ex.InnerException != null ? ex.InnerException.Message : ex.Message));
                    }
                    Thread.Sleep(1000);
                }

            }, _tokenSource.Token);

        }

        private static void Send2Quere(byte[] imagedata)
        {
            while (ResultImageQueue.IsFull(_queueMax))
            {
                Thread.Sleep(200);
                Log4NetHelper.Instance.Debug("接收Kafka数据量:" + ResultImageQueue.GetCount() + "，等待处理");
            }

            Img4kafka img4Kafka = JsonConvert.DeserializeObject<Img4kafka>(imagedata.ToUtf8String());
            if (img4Kafka != null)
            {
                ResultImageQueue.AddToQueue(img4Kafka);
            }
        }

        public void Unload()
        {
            _tokenSource.Cancel();
            try
            {
                Console.WriteLine("ImgkafkaActor Exit...");
                _task.Wait();
                _taskSend.Wait();
            }
            catch (AggregateException e)
            {
                Console.WriteLine("Exception messages:");
                foreach (var ie in e.InnerExceptions)
                    Console.WriteLine("   {0}: {1}", ie.GetType().Name, ie.Message);

                Console.WriteLine("\nTask status: {0}", _task.Status);
                Console.WriteLine("\nTaskSend status: {0}", _taskSend.Status);
            }
            finally
            {
                _tokenSource.Dispose();
            }
        }
    }
}
