using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ImageSplicer.Common;

namespace ImageSplicer
{
    /// <summary>
    /// 传输对象
    /// </summary>
    public class KafkaPartOffset
    {
        public int PartitionId { get; private set; }
        public long Offset { get; private set; }

        public KafkaPartOffset(int ppartitionId, long poffset)
        {
            PartitionId = ppartitionId;
            Offset = poffset;
        }
    }

    public static class XmlDataControl
    {
        private static string _kafkaFile = AppDomain.CurrentDomain.BaseDirectory+ "KafkaData.xml";
        private static int _kafkaPartitionIdMax = 16;
        static Configurator _configurator = null;
        /// <summary>
        /// 读取Kafka数据的分区及偏移值
        /// </summary>
        /// <param name="kafkaPartOffset"></param>
        public static void ReadConfig(out List<KafkaPartOffset> kafkaPartOffset)
        {
            kafkaPartOffset = null;
            // 数据文件不存在
            if (!File.Exists(_kafkaFile))
            {
                return;
            }

            InitConfig();

            kafkaPartOffset = new List<KafkaPartOffset>();
            for (int index = 0; index< _kafkaPartitionIdMax; index++)
            {
                string stroffset = _configurator.GetValue("PartitionId" + index.ToString("00"), "Offset", "0");
                if (stroffset != "0")
                {
                    long offset = long.Parse(stroffset);
                    kafkaPartOffset.Add(new KafkaPartOffset(index, offset));
                }
            }
        }

        /// <summary>
        /// 存储Kafka数据的分区及偏移值
        /// </summary>
        /// <param name="partitionId"></param>
        /// <param name="offset"></param>
        public static void WriteConfig(int partitionId, long offset)
        {
            InitConfig();
            _configurator.AddValue("PartitionId" + partitionId.ToString("00"), "Offset", offset.ToString(), true);
            _configurator.Save(Configurator.FileType.Xml);   
        }

        private static void InitConfig()
        {
            if (_configurator != null) return;

            _configurator = new Configurator();
            _configurator.LoadFromFile(_kafkaFile, Configurator.FileType.Xml);
        }
    }
}
