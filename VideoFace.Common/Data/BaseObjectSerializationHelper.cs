using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace VideoFace.Common.Data
{
    public static class ObjectSerializationHelper
    {
        public static byte[] BinarySerialize(this Object obj)
        {
            BinaryFormatter serializer = new BinaryFormatter();
            using (var memStream = new MemoryStream())
            {
                serializer.Serialize(memStream, obj);

                memStream.Position = 0;

                byte[] bytes = new byte[memStream.Length];
                memStream.Read(bytes, 0, bytes.Length);

                return bytes;
            }
        }

        public static Object BinaryDeserialize(this byte[] bytes)
        {
            BinaryFormatter serializer = new BinaryFormatter();
            using (var memStream = new MemoryStream())
            {
                memStream.Write(bytes, 0, bytes.Length);

                memStream.Position = 0;

                var obj = serializer.Deserialize(memStream) as object;
                return obj;
            }
        }
    }
}
