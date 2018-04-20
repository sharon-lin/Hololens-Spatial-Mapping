  public class PythonConnector : MonoBehaviour
    {
        private Task exchangeTask;
        private Windows.Networking.Sockets.StreamSocket socket;
        private void connectPython()
        {
            string PORT_NO = "80";
            string SERVER_IP = "SERVER_IP";
            ConnectUWP(SERVER_IP, PORT_NO).Wait();
        }

        private async Task ConnectUWP(string host, string port)
        {

                Debug.Log("ConnectUWP");
                if (exchangeTask != null) StopExchange();
                socket = new Windows.Networking.Sockets.StreamSocket();
                Windows.Networking.HostName serverHost = new Windows.Networking.HostName(host);
                try {
                    await socket.ConnectAsync(serverHost, port);
                    byteWriter = socket.OutputStream.AsStreamForWrite();
                    writer = new StreamWriter(byteWriter) { AutoFlush = true };
                } catch (Exception e) {
                    errorStatus = e.ToString();
                    Debug.Log("connection error:" +errorStatus);
                }

        }

        public void StopExchange()
        {
            if (exchangeTask != null)
            {
                exchangeTask.Wait();
                socket.Dispose();
                writer.Dispose();
                socket = null;
                exchangeTask = null;
                writer = null;
            }

        }

        void Update()
        {

            connectPython();

            byte[] data_to_send;
            // send data size to Python
            writer.Write(data_to_send.Length + ":");

            // send data over
            try {
                byteWriter.Write(data_to_send, 0, data_to_send.Length);
                byteWriter.Flush();
            }
            catch (Exception e)
            {
                Debug.Log("write error:" + e.ToString());
            }

            // read result from Python
            Stream readStream = socket.InputStream.AsStreamForRead();
            int bytesReceived = 0;

            byte[] readBuffer = new byte[1000];
            int numBytesRead;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                while ((numBytesRead = readStream.Read(readBuffer, 0, readBuffer.Length)) > 0)
                {
                    ms.Write(readBuffer, 0, numBytesRead);
                    string res = bytesReceived.ToString();
                }
                received = Encoding.ASCII.GetString(ms.ToArray(), 0, (int)ms.Length);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }

            // stop socket connection
            StopExchange();

        }
    }
