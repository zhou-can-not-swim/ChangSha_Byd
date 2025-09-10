import * as signalR from '@microsoft/signalr'

const API_BASE_URL = 'http://localhost:8081';

const connection = new signalR.HubConnectionBuilder()
  .withUrl("http://localhost:8081/hub")
  .build()
export function startNewConnection(type) {
    console.log('startNewConnection');
    console.log(connection._connectionState);
	if (connection._connectionState == "Disconnected") {
		//建立连接
		connection.start().then(function () {

			console.log('signalr success');
		}).catch(function (err) {

			console.log('signalr error:' + err);
		});
		// //报警信息
		// connection.on("showAlarmMsg", function (output) {


		// 	RefreshdAlarmMsg(output)

		// });
		// //展示log信息
		// connection.on("showMsg", function (output) {

		// 	RefreshMsg(output)

		// });

		//如果传进来的类型是duiduoji
		if (type == "duiduoji") {

			connection.on("receiveCWStockerMsg", function (output) {
				RefreshPlcMsg(output, 1);
			});

			connection.on("receiveQHStockerMsg", function (output) {
				RefreshPlcMsg(output, 2);
			});
		}


	}
}