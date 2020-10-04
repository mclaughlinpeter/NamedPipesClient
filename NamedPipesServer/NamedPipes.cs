using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamedPipesServer
{
    public interface IIpcClient
    {
        void Send(string data);
    }

    public interface IIpcServer : IDisposable
    {
        void Start();
        void Stop();

        event EventHandler<DataReceivedEventArgs> Received;
    }

    [Serializable]
    public sealed class DataReceivedEventArgs : EventArgs
    {
        public DataReceivedEventArgs(string data)
        {
            this.Data = data;
        }

        public string Data { get; private set; }
    }

    public sealed class NamedPipeServer : IIpcServer
    {
        private readonly NamedPipeServerStream server = new NamedPipeServerStream(typeof(IIpcClient).Name, PipeDirection.In);

        private void OnReceived(DataReceivedEventArgs e)
        {
            var handler = this.Received;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler<DataReceivedEventArgs> Received;

        StreamReader reader;
        public NamedPipeServer()
        {
            reader = new StreamReader(this.server);
        }

        public void Start()
        {


            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    //this.server.WaitForConnection();                    

                    //using (var reader = new StreamReader(this.server))
                    //{
                    //    this.OnReceived(new DataReceivedEventArgs(reader.ReadToEnd()));
                    //}

                    try
                    {
                        this.server.WaitForConnection();
                    }
                    catch (Exception)
                    {
                        this.server.Disconnect();
                        continue;
                    }
                    this.OnReceived(new DataReceivedEventArgs(reader.ReadToEnd()));

                    //using (var reader = new StreamReader(this.server))
                    //{
                    //    this.OnReceived(new DataReceivedEventArgs(reader.ReadToEnd()));
                    //}
                }
            });
        }

        public void Stop()
        {
            this.server.Disconnect();
        }

        void IDisposable.Dispose()
        {
            this.Stop();

            this.server.Dispose();
        }
    }
}
