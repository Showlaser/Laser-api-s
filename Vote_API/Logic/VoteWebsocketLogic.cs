using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Vote_API.Logic
{
    public class VoteWebsocketLogic
    {
        public static ManualResetEvent _allDone = new(false);

        public VoteWebsocketLogic()
        {
            string serverUrl = Environment.GetEnvironmentVariable("SERVERURL") ?? throw new NoNullAllowedException(
                "Environment variable SERVERURL was empty. Set it using the SERVERURL environment variable");

            int websocketPort = Convert.ToInt32(Environment.GetEnvironmentVariable("WEBSOCKETPORT") ?? throw new NoNullAllowedException(
                "Environment variable WEBSOCKETPORT was empty. Set it using the WEBSOCKETPORT environment variable"));

            StartServer(serverUrl, websocketPort);
        }

        public void StartServer(string serverAddress, int websocketPort)
        {
            byte[] bytes = new byte[1024];
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint address = IPEndPoint.Parse($"{serverAddress}:{websocketPort}");

            try
            {
                listener.Bind(address);
                listener.Listen();

                while (true)
                {
                    _allDone.Reset();
                    Console.WriteLine("Waiting for a connection");
                    listener.BeginAccept(Callback, listener);
                    _allDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void Callback(IAsyncResult result)
        {
            _allDone.Set();
            Socket? listener = result.AsyncState as Socket;
            Socket handler = listener?.EndAccept(result) ?? throw new InvalidOperationException();

            StateObject state = new()
            {
                workSocket = handler
            };
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                ReadCallback, state);
        }

        public void ReadCallback(IAsyncResult result)
        {
            string content = string.Empty;
            StateObject? state = result.AsyncState as StateObject;
            Socket? handler = state?.workSocket;
            if (handler == null || state == null)
            {
                return;
            }

            int bytesRead = handler.EndReceive(result);
            if (bytesRead == 0)
            {
                return;
            }

            state.sb.Append(Encoding.ASCII.GetString(
                state.buffer, 0, bytesRead));

            // Check for end-of-file tag. If it is not there, read 
            // more data.
            content = state.sb.ToString();
            if (content.IndexOf("<EOF>") > -1)
            {
                // All the data has been read from the 
                // client. Display it on the console.
                Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                    content.Length, content);
                // Echo the data back to the client.
                Send(handler, content);
            }
            else
            {
                // Not all data received. Get more.
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    ReadCallback, state);
            }
        }

        private void Send(Socket handler, string data)
        {
            // Convert the string data to byte data using ASCII encoding.


            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                SendCallback, handler);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
