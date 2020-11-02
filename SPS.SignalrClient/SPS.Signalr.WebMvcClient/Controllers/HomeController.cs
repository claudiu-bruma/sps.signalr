using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
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
        private string hubUrl = @"http://SPS.SignalrClient_1:80/ChatHub"; 

        public HomeController( )
        { 
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody]Message message)
        {    
            connection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .Build();

            await connection.StartAsync();
            var aesEncriptedMesage = AesCryptoHelper.EncryptString_Aes(message.MessageContent);
            await connection.InvokeAsync("SendMessage", message.User, aesEncriptedMesage);

            return new OkResult();
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
