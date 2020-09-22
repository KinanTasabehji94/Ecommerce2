var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.on("ReceiveMessage", function (fromUser, message) {
    var msg = fromUser + ": " + message;
    var li = document.createElement("li");
    li.textContent = msg;
    $("#list").prepend(li); //the opposit of append (last is first)
});

connection.start();

$("#btnSend").on("click", function () {
    var fromUser = $("#txtUser").val();
    var message = $("#txtMessage").val();

    connection.invoke("SendMessage", fromUser, message);
});