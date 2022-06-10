"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (message) {
    let msg = `<div class="message" >
                    <p class="text"> ${message}</p>
                </div>`;
    $(".messages-chat").append(msg);
    document.getElementById('messageInput').value = ''
});

connection.on("Connected", function (username) {

    let id = "#" + username
    console.log(id)
    $(id).removeClass("offline")
    $(id).addClass("online")
})

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

$("#sendButton").click(function () {
    let message = $("#messageInput").val();
    connection.invoke("SendMessage", message).catch(function (err) {
        return console.error(err.toString());
    });
})