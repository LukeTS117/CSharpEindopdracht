using System;
using System.IO;
using System.Net.Sockets;
using System.Text;



namespace ServerClient
{
    class ServerClient
    {
        /* Data Protocol 
 * 
 *  From Client:                    param:
 *     <loc> = location             (float x, float y) 
 *     <dir> = direction            (byte)  
 *     <msg> = text message         (String)
 *     <bmb> = bomb placed          (byte x, byte y)
 *     <exp> = exploded             (byte) 
 *     <ded> = dead                 -
 */
        public enum Tag
        {
            loc = 1, dir = 2, msg = 3, bmb = 4, exp = 5, ded = 6
        }


        public struct TaggedMessage
        {
            public string message;
            public Tag tag;

            public TaggedMessage(Tag tag, string message)
            {
                this.tag = tag;
                this.message = message;
            }
        }


        private static Encoding encoding = Encoding.UTF8;

        public static string ReadTextMessage(NetworkStream nws)
        {
            StreamReader stream = new StreamReader(nws, encoding);
            return stream.ReadLine();
        }

        public static void WriteTextMessage(NetworkStream nws, string message)
        {
            StreamWriter stream = new StreamWriter(nws, encoding);
            stream.WriteLine(message);
            stream.Flush();
        }




        public static byte[] ReadBytes(NetworkStream nws, int count)
        {
            byte[] bytes = new byte[count]; 
            int readCount = 0;

            while (readCount < count)
            {
                int left = count - readCount;
                int read = nws.Read(bytes, readCount, left);

                if(read == 0)
                {
                    throw new Exception("Lost Connection during read!");
                }

                readCount += read;
            }

            return bytes;
        }

        public static string ReadMessage(NetworkStream nws)
        {
            byte[] lengthBytes = ReadBytes(nws, sizeof(int));

            int length = BitConverter.ToInt32(lengthBytes, 0);

            byte[] messageBytes = ReadBytes(nws, length);

            string message = encoding.GetString(messageBytes);

            return message;
        }


        public static TaggedMessage ReadTaggedMessage(NetworkStream nws)
        {
            byte[] lengthBytes = ReadBytes(nws, sizeof(int));
            int length = BitConverter.ToInt32(lengthBytes, 0);

            byte[] tagBytes = ReadBytes(nws, sizeof(int));

            byte[] messageBytes = ReadBytes(nws, length);
            Tag tag = (Tag)BitConverter.ToInt32(tagBytes, 0);

            string message = encoding.GetString(messageBytes);

            return new TaggedMessage(tag, message);
        }


        public static void SendMessage(NetworkStream nws, string message)
        {
            byte[] messageBytes = encoding.GetBytes(message);

            int length = messageBytes.Length;

            byte[] lengthBytes = BitConverter.GetBytes(length);

            nws.Write(lengthBytes, 0, lengthBytes.Length);
            nws.Write(messageBytes, 0, length);
        }

        public static void SendTaggedMessage(NetworkStream nws, Tag tag, string message = null)
        {
            byte[] messageBytes = encoding.GetBytes(message);

            byte[] tagBytes = BitConverter.GetBytes((int)tag);

            int length = messageBytes.Length; 
            byte[] lengthBytes = BitConverter.GetBytes(length);



            nws.Write(lengthBytes, 0, lengthBytes.Length);
           // Console.WriteLine($"Send: {BitConverter.ToInt32(lengthBytes, 0)}");
            nws.Write(tagBytes, 0, tagBytes.Length);
           // Console.WriteLine($"Send: {BitConverter.ToInt32(tagBytes, 0)}");
            nws.Write(messageBytes, 0, length);
           // Console.WriteLine($"Send: {encoding.GetString(messageBytes)}");
        }

    }
}



