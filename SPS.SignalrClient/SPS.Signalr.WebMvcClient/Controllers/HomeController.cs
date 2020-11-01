using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using SPS.Common;
using SPS.Signalr.WebMvcClient.Models;

namespace SPS.Signalr.WebMvcClient.Controllers
{
    public class Message
    {
        public string User { get; set; }
        public string MessageContent { get; set; }
    }
    public class HomeController : Controller
    {
        HubConnection connection;
        const string hubUrl = @"http://local.docker.internal:44384/ChatHub"; 

        public HomeController( )
        { 
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendMessage(Message message)
        {    
            connection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .Build();

            await connection.StartAsync();
            var aesEncriptedMesage = AesCryptoHelper.EncryptString_Aes(message.MessageContent);
            await connection.InvokeAsync("SendMessage", message.User, aesEncriptedMesage);

            return View("Index");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
