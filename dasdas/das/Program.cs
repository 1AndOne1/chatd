using System.Net.Sockets;
using System.Net;
using System.Text;

namespace client
{
    class Program
    {
        public static async Task SaveMsg(EndPoint adres, string msg)
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, 12345);
            string[] data = msg.Split(':');
            string fileName = GenerateFilenAME(adres.ToString().Substring(0, 11), data[0]);
            Console.WriteLine(fileName);
            using var sw = new StreamWriter($"C:/Users/user/Desktop/das/{fileName}.txt", true);
            await sw.WriteAsync(adres.ToString() + '_' + msg + "\n");
            sw.Flush();
        }
        public static string GenerateFilenAME(string firstIp, string secondIp)
        {
            try
            {
                for (int i = 0; i < firstIp.Length; i++)
                {
                    int.TryParse(firstIp.Substring(i, 1), out int val);
                    int.TryParse(secondIp.Substring(i, 1), out int val2);

                    if (val > val2)
                    {
                        return ModIP(secondIp) + "_" + ModIP(firstIp);
                    }
                    else if (val2 > val)
                    {
                        return ModIP(firstIp) + "_" + ModIP(secondIp);
                    }
                }
                
            }
            catch(Exception ex) {
                Console.WriteLine(ex);
            }
            return null;
        }
        public static string ModIP(EndPoint adres, IPEndPoint ipPoint)
        {
            return adres.ToString().Replace('.', '_').Substring(0, 11);

        }
        public static string ModIP(string adres)
        {
            string modifiedAdres = adres.Replace('.', '_');

            if (modifiedAdres.Length > 11)
            {
                modifiedAdres = modifiedAdres.Substring(0, 11);
            }

            return modifiedAdres;
        }
        public static async Task Main()
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, 12345);
            socket.Bind(ipPoint);
            socket.Listen();
            while (true)
            {
                using var tcpClient = await socket.AcceptAsync();
                System.Console.WriteLine("Есть подключиние");
                byte[] bytesRead = new byte[255];
                int count = await tcpClient.ReceiveAsync(bytesRead, SocketFlags.None);
                string msg = Encoding.UTF8.GetString(bytesRead);
                await Console.Out.WriteLineAsync("Принято сообщение");

                if (count > 0)
                {
                    await SaveMsg(tcpClient.RemoteEndPoint, msg);
                }
            }
        }
    }
}