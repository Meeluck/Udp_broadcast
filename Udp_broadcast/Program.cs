using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
namespace Udp_broadcast
{
	class Program
	{
		static IPAddress remoteAddress; // хост для отправки данных
		const int remotePort = 8001; // порт для отправки данных
		const int localPort = 8001; // локальный порт для прослушивания входящих подключений
		static string username;

		static void Main(string[] args)
		{
			try
			{
				Console.Write("Введите свое имя:");
				username = Console.ReadLine();
				remoteAddress = IPAddress.Parse("235.5.5.11");
				Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
				receiveThread.Start();
				SendMessage(); // отправляем сообщение
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		private static void SendMessage()
		{
			UdpClient sender = new UdpClient();
			IPEndPoint endPoint = new IPEndPoint(remoteAddress, remotePort);
			try
			{
				while (true)
				{
					string message = Console.ReadLine();
					message = String.Format("{0}: {1}", username, message);
					byte[] data = Encoding.Unicode.GetBytes(message);
					sender.Send(data, data.Length, endPoint);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				throw;
			}
			finally
			{
				sender.Close();
			}
		}

		private static void ReceiveMessage()
		{
			UdpClient receiver = new UdpClient(localPort);
			receiver.JoinMulticastGroup(remoteAddress, 20);
			IPEndPoint remoteIp = null;
			string localAddress = LocalIPAddress();
			try
			{
				while (true)
				{
					byte[] data = receiver.Receive(ref remoteIp); // получаем данные
					if (remoteIp.Address.ToString().Equals(localAddress))
						continue;
					string message = Encoding.Unicode.GetString(data);
					Console.WriteLine(message);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				throw;
			}
		}
		private static string LocalIPAddress()
		{
			string localIp = "";
			IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
			foreach (IPAddress ip in host.AddressList)
			{
				if (ip.AddressFamily == AddressFamily.InterNetwork)
				{
					localIp = ip.ToString();
					break;
				}
			}
			return localIp;
		}
	}
}

