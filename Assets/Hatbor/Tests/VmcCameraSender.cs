using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using uOSC;

namespace Hatbor.Tests
{
    static class VmcCameraSender
    {
        public static int FindFreePort()
        {
            using var client = new UdpClient(0);
            return ((IPEndPoint)client.Client.LocalEndPoint).Port;
        }

        public static void Send(int port, Pose pose, float fieldOfView)
        {
            var message = new Message("/VMC/Ext/Cam", "Camera",
                pose.position.x, pose.position.y, pose.position.z,
                pose.rotation.x, pose.rotation.y, pose.rotation.z, pose.rotation.w,
                fieldOfView);
            using var stream = new MemoryStream();
            message.Write(stream);
            var bytes = stream.ToArray();
            using var client = new UdpClient();
            client.Send(bytes, bytes.Length, "127.0.0.1", port);
        }
    }
}
