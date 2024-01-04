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

                // 發送檔案
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProductionLog.dll");// @"C:\path\to\your\file.ext";
                string fileName = Path.GetFileName(filePath);
                byte[] fileNameBytes = System.Text.Encoding.UTF8.GetBytes(fileName);
                byte[] fileNameLen = BitConverter.GetBytes(fileNameBytes.Length);

                NetworkStream stream = client.GetStream();
                stream.Write(fileNameLen, 0, 4);
                stream.Write(fileNameBytes, 0, fileNameBytes.Length);

                // 發送檔案數據
                byte[] fileData = File.ReadAllBytes(filePath);
                byte[] fileDataLen = BitConverter.GetBytes(fileData.Length);
                Console.WriteLine(fileData.Length);
                stream.Write(fileDataLen, 0, 4);
                stream.Write(fileData, 0, fileData.Length);

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
