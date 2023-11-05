// This code is based on uOSC.
// https://github.com/hecomi/uOSC/blob/master/Assets/uOSC/Runtime/uOscServer.cs
//
// The MIT License (MIT)
//
// Copyright (c) 2017 hecomi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

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