import * as signalR from '@microsoft/signalr'

const API_BASE_URL = 'http://localhost:8081';

const connection = new signalR.HubConnectionBuilder()
  .withUrl("http://localhost:8081/hub")
  .build()
export async function startNewConnection(type) {
    console.log('开始连接-->');
    console.log(connection._connectionState);
	if (connection._connectionState == "Disconnected") {
		//建立连接
		connection.start().then(function () {

			console.log('Signal 连接成功');
		}).catch(function (err) {

			console.log('signalr 连接失败:' + err);
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

export async function stopConnection() {
	connection.stop().then(function () {
		console.log('停止 SignalR 连接成功');
	}).catch(function (err) {
		console.log('停止 SignalR 连接失败 : ' + err);
	});
}


// import * as signalR from '@microsoft/signalr'

// const API_BASE_URL = 'http://localhost:8081';

// const connection = new signalR.HubConnectionBuilder()
//   .withUrl("http://localhost:8081/hub")
//   .build()

// // 添加引用计数和连接状态跟踪
// let connectionRefCount = 0;
// let connectionPromise = null;

// export async function startNewConnection(type) {
//     console.log('开始连接-->');
    
//     // 增加引用计数
//     connectionRefCount++;
//     console.log('连接引用计数:', connectionRefCount);
    
//     if (connection._connectionState == "Disconnected") {
//         // 如果没有正在进行的连接尝试，则创建新的连接Promise
//         if (!connectionPromise) {
//             connectionPromise = connection.start();
//         }
        
//         connectionPromise.then(function () {
//             console.log('Signal 连接成功');
//         }).catch(function (err) {
//             console.log('signalr 连接失败:' + err);
//             // 连接失败时重置Promise，以便可以重试
//             connectionPromise = null;
//         });
//     }
    
//     // 根据类型注册事件处理器
//     if (type == "duiduoji") {
//         connection.on("receiveCWStockerMsg", function (output) {
//             RefreshPlcMsg(output, 1);
//         });

//         connection.on("receiveQHStockerMsg", function (output) {
//             RefreshPlcMsg(output, 2);
//         });
//     }
    
//     // 返回连接Promise
//     return connectionPromise;
// }

// export async function stopConnection() {
//     // 减少引用计数
//     connectionRefCount--;
//     console.log('连接引用计数:', connectionRefCount);
    
//     // 只有当没有组件使用连接时才真正断开连接
//     if (connectionRefCount <= 0) {
//         connectionRefCount = 0; // 确保不会变成负数
        
//         if (connection._connectionState == "Connected") {
//             connection.stop().then(function () {
//                 console.log('停止 SignalR 连接成功');
//                 connectionPromise = null; // 重置Promise
//             }).catch(function (err) {
//                 console.log('停止 SignalR 连接失败 : ' + err);
//             });
//         }
//     }
// }

// // 强制断开连接（不考虑引用计数）
// export async function forceStopConnection() {
//     connectionRefCount = 0;
//     if (connection._connectionState == "Connected") {
//         connection.stop().then(function () {
//             console.log('强制停止 SignalR 连接成功');
//             connectionPromise = null;
//         }).catch(function (err) {
//             console.log('强制停止 SignalR 连接失败 : ' + err);
//         });
//     }
// }