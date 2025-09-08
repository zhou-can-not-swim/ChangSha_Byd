using ChangSha_Byd_NetCore8.Extends.PlcServices;
using Microsoft.Extensions.Options;
using Microsoft.FSharp.Core;
using Sharp7;

namespace ChangSha_Byd_NetCore8.fan
{
    public abstract class S7PlcCtrl : IDisposable
    {
        protected S7Client Client { get; set; }
        protected readonly string _plcName;
        protected readonly IOptionsMonitor<S7PlcOptItem> _optionsMonitor;

        public virtual string Name => _plcName;

        public S7PlcCtrl(string plcName, IOptionsMonitor<S7PlcOptItem> optionsMonitor)
        {
            if (string.IsNullOrEmpty(plcName))
            {
                throw new ArgumentException("'plcName' cannot be null or empty", "plcName");
            }

            _plcName = plcName;
            _optionsMonitor = optionsMonitor;
            Client = new S7Client();
        }

        public virtual void Dispose()
        {
            if (Client != null && Client.Connected)
            {
                try
                {
                    Client.Disconnect();   //断开连接
                }
                catch (Exception)
                {
                    // 忽略断开连接时的异常
                }
            }
        }

        public virtual bool Connect()
        {
            try
            {
                var options = _optionsMonitor.Get(_plcName);
                int result = Client.ConnectTo(options.IpAddr, options.Rack, options.Slot);
                return result == 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual void Disconnect()
        {
            if (Client != null && Client.Connected)
            {
                Client.Disconnect();
            }
        }
        public virtual Task DisconnectAsync()
        {
            if (Client == null)
            {
                return Task.CompletedTask;
            }

            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            new Thread((ThreadStart)delegate
            {
                try
                {
                    Client.Disconnect();
                    Client = null;
                    tcs.SetResult(null);
                }
                catch (Exception ex)
                {
                    //_logger.LogWarning("尝试与PLC=" + Name + "断开连接失败：" + ex.Message + "\r\n" + ex.StackTrace);
                    Client = null;
                    tcs.SetException(ex);
                }
            }).Start();
            return tcs.Task;
        }

        public virtual async Task EnsureConnectedAsync(bool force = false)
        {
            if (force || Client == null || !Client.Connected)
            {
                FSharpResult<S7Client, ApiError> fSharpResult = await CreateClientAndConnectAsync();
                if (fSharpResult.IsError)
                {
                    throw new Exception(fSharpResult.ErrorValue.ToString());
                }
                //127.0.0.1
                Client = fSharpResult.ResultValue;
            }
        }

        protected async Task<FSharpResult<byte[], string>> ReadDBAsync(int DB, int start, int size)
        {
            return await Task.Run(() =>
            {
                try
                {
                    byte[] buffer = new byte[size];
                    int result = Client.DBRead(DB, start, size, buffer);
                    //#region
                    //string data = PointTableProcessor.ProcessBuffer(buffer);
                    //Console.WriteLine(data);
                    //#endregion


                    if (result == 0)
                    {
                        return FSharpResult<byte[], string>.NewOk(buffer);
                    }
                    else
                    {
                        return FSharpResult<byte[], string>.NewError($"读取DB失败，错误代码: {result}");
                    }
                }
                catch (Exception ex)
                {
                    return FSharpResult<byte[], string>.NewError($"读取异常: {ex.Message}");
                }
            });
        }

        protected async Task<FSharpResult<bool, string>> WriteDBAsync(int DB, int start, byte[] buffer)
        {
            return await Task.Run(() =>
            {
                try
                {
                    int result = Client.DBWrite(DB, start, buffer.Length, buffer);

                    if (result == 0)
                    {
                        return FSharpResult<bool, string>.NewOk(true);
                    }
                    else
                    {
                        return FSharpResult<bool, string>.NewError($"写入DB失败，错误代码: {result}");
                    }
                }
                catch (Exception ex)
                {
                    return FSharpResult<bool, string>.NewError($"写入异常: {ex.Message}");
                }
            });
        }

        protected virtual Task<FSharpResult<S7Client, ApiError>> CreateClientAndConnectAsync()
        {
            TaskCompletionSource<FSharpResult<S7Client, ApiError>> tcs = new TaskCompletionSource<FSharpResult<S7Client, ApiError>>();
            new Thread((ThreadStart)delegate
            {
                S7Client s7Client = new S7Client();
                try
                {
                    S7PlcOptItem s7PlcOptItem = _optionsMonitor.Get(_plcName);
                    int num = s7Client.ConnectTo(s7PlcOptItem.IpAddr, s7PlcOptItem.Rack, s7PlcOptItem.Slot);
                    if (num == 0)
                    {
                        tcs.SetResult(FSharpResult<S7Client, ApiError>.NewOk(s7Client));
                    }
                    else
                    {
                        tcs.SetResult(FSharpResult<S7Client, ApiError>.NewError(S7ErrorCodeHelper.GenerateApiError(Name, num)));
                    }
                }
                catch (Exception exception)
                {
                    tcs.SetException(exception);
                }
            }).Start();
            return tcs.Task;
        }
    }
}