"use strict";

var hubUrl = document.getElementById("main-container").getAttribute("data-hubUrl");
console.log(hubUrl);
var connection = new signalR.HubConnectionBuilder().withUrl(hubUrl).build();
 

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
