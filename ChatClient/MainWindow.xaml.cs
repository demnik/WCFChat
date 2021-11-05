using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using ChatClient.ServiceChat;

namespace ChatClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IServiceChatCallback
    {
        private bool isConnected;
        private ServiceChatClient client;
        private int ID = 0;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        void ConnectUser()
        {
            if (!isConnected)
            {
                client = new ServiceChatClient(new InstanceContext(this));
                ID = client.Connect(UserNameTextBox.Text);
                UserNameTextBox.IsEnabled = false;
                btnConnDis.Content = "Отключиться";
                isConnected = true;
            }
        }

        void DisconnectUser()
        {
            if (isConnected)
            {
                client.Disconnect(ID);
                client = null;
                UserNameTextBox.IsEnabled = true;
                btnConnDis.Content = "Подключиться";
                isConnected = false;
            }
        }

        private void btnConnDis_Click(object sender, RoutedEventArgs e)
        {
            if (isConnected)
            {
                DisconnectUser();
                return;
            }

            if (!isConnected)
            {
                ConnectUser();
                return;
            }
                
        }

        public void MsgCallback(string msg)
        {
            lbChat.Items.Add(msg);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DisconnectUser();
        }

        private void MessageTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                client.SendMessage(MessageTextBox.Text, ID);
                MessageTextBox.Clear();
            }
        }
    }
}
