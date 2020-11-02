"use strict";

var hubUrl = document.getElementById("main-container").getAttribute("data-hubUrl");
console.log(hubUrl);
var connection = new signalR.HubConnectionBuilder().withUrl(hubUrl).build();
 
var sendMessageButton = document.getElementById("sendMessage");
function handleClick(event) {   

    var user = document.getElementById("User").value;
    var message = document.getElementById("MessageContent").value;

    var formUrl ='/Home/SendMessage';
 
    fetch(formUrl, {
        headers: { "Content-Type": "application/json; charset=utf-8" },
        method: 'POST',
        body: JSON.stringify({
            User: user,
            MessageContent: message,
        })
    }) 
}
sendMessageButton.addEventListener('click', handleClick);

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.start().then(function () {
    console.log("started");
}).catch(function (err) {
    return console.error(err.toString());
});
