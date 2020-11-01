using Microsoft.AspNetCore.SignalR;
using SPS.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SPS.SignalrClient.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            var aesDecriptedMesage = AesCryptoHelper.DecryptString_Aes(message);

            SaveMessageToLog(user, aesDecriptedMesage);

            await Clients.All.SendAsync("ReceiveMessage", user, aesDecriptedMesage);
        }

        private static void SaveMessageToLog(string user, string aesDecriptedMesage)
        { 

                    string path = @"logs/";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@$"logs/ChatLog-{DateTime.Today.ToString("dd-MM-yyyy")}.txt", true))
            {
                file.WriteLine($@"{DateTime.Now} : {user } - {aesDecriptedMesage}");
            }
        }
    }
}
