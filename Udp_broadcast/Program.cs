using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
namespace Udp_broadcast
{
	class Program
	{
        static string remoteAddress;
        static int remotePort;
        static int localPort;

        static void Main(string[] args)
        {
	        try
	        {
		        Console.Write("Введите порт для прослушивания: "); // локальный порт
		        localPort = Int32.Parse(Console.ReadLine());
		        Console.Write("Введите удаленный адрес для подключения: ");
		        remoteAddress = Console.ReadLine(); // адрес, к которому мы подключаемся
		        Console.Write("Введите порт для подключения: ");
		        remotePort = Int32.Parse(Console.ReadLine()); // порт, к которому мы подключаемся
		        Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
		        receiveThread.Start();
		        SendMessage();
	        }
	        catch(Exception e)
	        {
		        Console.WriteLine(e.Message);
	        }
        }

        private static void SendMessage()
        {
	        UdpClient sender = new UdpClient();
	        try
	        {
		        while (true)
		        {
			        string msg = Console.ReadLine();
			        byte[] data = Encoding.Unicode.GetBytes(msg);
			        sender.Send(data, data.Length, remoteAddress, remotePort);
		        }
	        }
	        catch (Exception e)
	        {
		        Console.WriteLine(e.Message);
	        }
	        finally
	        {
		        sender.Close();
	        }
        }

        private static void ReceiveMessage()
        {
	        UdpClient reciver = new UdpClient(localPort);
	        IPEndPoint remoteIp = null;
	        try
	        {
		        while (true)
		        {
			        byte[] data = reciver.Receive(ref remoteIp);
			        string msg = Encoding.Unicode.GetString(data);
			        Console.WriteLine("Собеседник: {0}", msg);
		        }
	        }
	        catch (Exception e)
	        {
		        Console.WriteLine(e.Message);
	        }
        }
	}
}

