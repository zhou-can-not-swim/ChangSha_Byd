import * as signalR from '@aspnet/signalr'
var connection = new signalR.HubConnectionBuilder().withUrl("http://localhost:52789/wmsHub").build();

export function startNewConnection() {
	if(connection.connectionState == 0) {
		connection.start().then(function() {

			console.log('signalr success');
		}).catch(function(err) {
			alert("connection-error");
			console.log('signalr error:' + err);
		});

		connection.on("showWCSMsg", function(msg) {
			console.log('执行js  showWCSMsg 方法')
			showWCSMsg(msg)

		});
		connection.on("receiveTaskStatusMessage", function(objRes) {
				console.log('执行js  receiveTaskStatusMessage 方法')
			receiveTaskStatusMessage(objRes)

		});
	


	}

}
