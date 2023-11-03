// ref. uOSC.uOscServer

using uOSC;

namespace Hatbor.VMC
{
    public sealed class OscServer
    {
        readonly Udp udp = new uOSC.DotNet.Udp();
        readonly Thread thread = new uOSC.DotNet.Thread();
        readonly Parser parser = new();

        public int MessageCount => parser.messageCount;
        public Message Dequeue() => parser.Dequeue();

        bool isStarted;

        public void StartServer(int port)
        {
            if (isStarted) return;

            udp.StartServer(port);
            thread.Start(UpdateMessage);
            isStarted = true;
        }

        public void StopServer()
        {
            if (!isStarted) return;

            thread.Stop();
            udp.Stop();
            isStarted = false;
        }

        void UpdateMessage()
        {
            while (udp.messageCount > 0)
            {
                var buf = udp.Receive();
                var pos = 0;
                parser.Parse(buf, ref pos, buf.Length);
            }
        }
    }
}