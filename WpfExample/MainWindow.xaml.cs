using NetMQ;
using NetMQ.Sockets;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace WpfExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow() {
            InitializeComponent();

            (new Thread(new ThreadStart(ServerThreadFunction))).Start();
            (new Thread(new ThreadStart(ClientAThreadFunction))).Start();
            (new Thread(new ThreadStart(ClientBThreadFunction))).Start();
        }

        private void ServerThreadFunction() {
            PublisherSocket puber = new();
            puber.Bind("tcp://*:5556");

            while (true) {
                puber.SendMoreFrame("TopicA").SendFrame("alarm from A");
                Dispatcher.BeginInvoke(() => ServerList.Items.Add("Send alarm from A"));
                puber.SendMoreFrame("TopicB").SendFrame("alarm from B");

                Dispatcher.BeginInvoke(() => ServerList.Items.Add("Send alarm from B"));

                Thread.Sleep(500);
            }
        }

        private void ClientAThreadFunction() {
            SubscriberSocket suber = new();
            suber.Connect("tcp://localhost:5556");
            suber.Subscribe("TopicA");

            while (true) {
                string topic = suber.ReceiveFrameString();
                string content = suber.ReceiveFrameString();

                Dispatcher.BeginInvoke(() => ClientAList.Items.Add("receive: " + content));

                Thread.Sleep(500);
            }
        }

        private void ClientBThreadFunction() {
            SubscriberSocket suber = new();
            suber.Connect("tcp://localhost:5556");
            suber.Subscribe("TopicB");

            while (true) {
                string topic = suber.ReceiveFrameString();
                string content = suber.ReceiveFrameString();

                Dispatcher.BeginInvoke(() => ClientBList.Items.Add("receive: "+ content));

                Thread.Sleep(500);
            }
        }
    }
}