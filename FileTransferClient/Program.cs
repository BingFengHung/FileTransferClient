using System;
using System.IO;
using System.Net.Sockets;

namespace FileTransferClient
{
    internal class Program
    {
        public static void StartClient()
        {
            try
            {
                // 伺服器的IP和Port
                string ip = "127.0.0.1";
                int port = 8000;
                TcpClient client = new TcpClient(ip, port);

                // 連接到伺服器
                Console.WriteLine("Connected to server.");
                NetworkStream stream = client.GetStream();

                string[] files = { "ProductionLog.dll", "temp.txt", "恭喜阿公溪發壓發大財.txt", "MapleStoryV234.zip" };

                foreach (var file in files)
                {
                    // 發送檔案
                    string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file);// @"C:\path\to\your\file.ext";
                    string fileName = Path.GetFileName(filePath);
                    byte[] fileNameBytes = System.Text.Encoding.UTF8.GetBytes(fileName);
                    byte[] fileNameLen = BitConverter.GetBytes(fileNameBytes.Length);

                    stream.Write(fileNameLen, 0, 4);
                    stream.Write(fileNameBytes, 0, fileNameBytes.Length);

                    // 發送檔案數據
                    byte[] fileData = File.ReadAllBytes(filePath);
                    byte[] fileDataLen = BitConverter.GetBytes(fileData.Length);
                    Console.WriteLine(fileData.Length);
                    stream.Write(fileDataLen, 0, 4);
                    stream.Write(fileData, 0, fileData.Length);
                }

                // 傳送完成標誌
                byte[] endSignal = BitConverter.GetBytes(-1);
                stream.Write(endSignal, 0, 4);

                // 關閉連線
                stream.Close();
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void Main(string[] args)
        {
            StartClient();
        }
    }
}
