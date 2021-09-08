using NetMQ;
using NetMQ.Sockets;
using System;

namespace ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args) {
            using ResponseSocket server = new("@tcp://localhost:5556"); // bind
            using RequestSocket client = new(">tcp://localhost:5556");  // connect
                                                                        // Send a message from the client socket
            client.SendFrame("Hello");

            // Receive the message from the server socket
            string m1 = server.ReceiveFrameString();
            Console.WriteLine("From Client: {0}", m1);

            // Send a response back from the server
            server.SendFrame("Hi Back");

            // Receive the response from the client socket
            string m2 = client.ReceiveFrameString();
            Console.WriteLine("From Server: {0}", m2);
        }
    }
}
