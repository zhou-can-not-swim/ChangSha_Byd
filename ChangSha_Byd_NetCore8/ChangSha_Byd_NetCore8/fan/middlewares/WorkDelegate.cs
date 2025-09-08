namespace ChangSha_Byd_NetCore8.fan.middlewares
{
    //往里面传入IWorkContext的实现类,也即是ScanContext
    public delegate Task WorkDelegate<TWorkContext>(TWorkContext context) where TWorkContext : IWorkContext;

    /// <summary>
    /// 工作上下文
    /// </summary>
    public interface IWorkContext
    {
        IServiceProvider ServiceProvider { get; }
    }

    /// <summary>
    /// 自定义中间件，
    /// 可以携带自己定义的上下文而不是固定的HttpConetxt
    /// </summary>
    /// <typeparam name="TWorkConext"></typeparam>
    public interface IWorkMiddleware<TWorkConext> where TWorkConext : IWorkContext
    {
        Task InvokeAsync(TWorkConext context, WorkDelegate<TWorkConext> next);
    }
}
