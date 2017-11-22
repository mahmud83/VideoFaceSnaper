using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KafkaNet;
using KafkaNet.Common;
using KafkaNet.Model;
using KafkaNet.Protocol;
using Newtonsoft.Json;
using VideoFace.Common;
using VideoFace.Common.Util;
using VideoFaceSnaper.Data;

namespace VideoFaceSnaper
{
    /// <summary>
    /// 数据处理器
    /// </summary>
    public class ImgkafkaActor
    {
        private static string _kafkaAddr = ConfigurationHelper.GetValue("KafkaAddr", "http://127.0.0.1:9092");
        private static string _outputtopicName = ConfigurationHelper.GetValue("OutputTopic", "mssend");
        private CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private Task _taskSend = null;
        public void Initial()
        {
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
                        // 取消任务判断
                        if (_tokenSource.IsCancellationRequested)
                        {
                            break;
                        }

                        string message = ResultImgQueue.GetFromQueue();
                        if (string.IsNullOrEmpty(message))
                        {
                            Thread.Sleep(1000);
                            continue;
                        }
                        else
                        {
                            producer.SendMessageAsync(_outputtopicName, new[] {new Message(message) }).Wait();
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

        public void Send2Quere(string imagedata)
        {
            while (ResultImgQueue.IsFull(1000))
            {
                Thread.Sleep(200);
                Log4NetHelper.Instance.Debug("发送Kafka数据量:" + ResultImgQueue.GetCount() + "，等待处理");
            }

            if (!string.IsNullOrEmpty(imagedata))
            {
                ResultImgQueue.AddToQueue(imagedata);
            }
        }

        public void Unload()
        {
            try
            {
                _tokenSource.Cancel();

                Console.WriteLine("ImgkafkaActor Exit...");
                _taskSend.Wait();
            }
            catch (AggregateException e)
            {
                Console.WriteLine("Exception messages:");
                foreach (var ie in e.InnerExceptions)
                    Console.WriteLine("   {0}: {1}", ie.GetType().Name, ie.Message);

                Console.WriteLine("\nTaskSend status: {0}", _taskSend.Status);
            }
            finally
            {
                _tokenSource.Dispose();
            }
        }
    }
}
