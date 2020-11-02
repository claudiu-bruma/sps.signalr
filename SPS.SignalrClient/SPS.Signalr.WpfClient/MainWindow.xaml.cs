using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.SignalR.Client;
using SPS.Common;

namespace SignalRChatClient
{

    public partial class MainWindow : Window
    {
        HubConnection connection;
        const string hubUrl = @"http://localhost:62631/ChatHub";
        public MainWindow()
        {
            InitializeComponent();

            connection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };
        }

        private async void connectButton_Click(object sender, RoutedEventArgs e)
        {
            connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    var newMessage = $"{user}: {message}";
                    messagesList.Items.Add(newMessage);
                });
            });

            try
            {
                await connection.StartAsync();
                messagesList.Items.Add("Connection started");
                connectButton.IsEnabled = false;
                sendButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                messagesList.Items.Add(ex.Message);
            }
        }

        private async void sendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var aesEncriptedMesage = AesCryptoHelper.EncryptString_Aes(messageTextBox.Text);

                await connection.InvokeAsync("SendMessage", userTextBox.Text, aesEncriptedMesage);

            }
            catch (Exception ex)
            {
                messagesList.Items.Add(ex.Message);
            }

        }
    }
}