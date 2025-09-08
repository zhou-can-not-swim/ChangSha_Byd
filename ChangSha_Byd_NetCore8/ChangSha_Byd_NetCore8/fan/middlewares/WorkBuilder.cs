namespace ChangSha_Byd_NetCore8.fan.middlewares
{
    public class WorkBuilder<TWorkContext> where TWorkContext : IWorkContext
    {
        private List<Func<WorkDelegate<TWorkContext>, WorkDelegate<TWorkContext>>> _middlewares = new List<Func<WorkDelegate<TWorkContext>, WorkDelegate<TWorkContext>>>();

        public WorkBuilder<TWorkContext> Use(Func<WorkDelegate<TWorkContext>, WorkDelegate<TWorkContext>> mw)
        {
            _middlewares.Add(mw);
            return this;
        }

        /// <summary>
        /// 使用中间件
        /// </summary>
        /// <typeparam name="TMiddleware"></typeparam>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public WorkBuilder<TWorkContext> Use<TMiddleware>() where TMiddleware : IWorkMiddleware<TWorkContext>
        {
            return Use((WorkDelegate<TWorkContext> next) => async delegate (TWorkContext context)
            {
                TMiddleware requiredService = context.ServiceProvider.GetRequiredService<TMiddleware>();
                if (requiredService == null)
                {
                    throw new NullReferenceException("无法获取" + typeof(TMiddleware).FullName + "实例!");
                }

                await requiredService.InvokeAsync(context, next);
            });
        }

        /// <summary>
        /// 构建委托
        /// </summary>
        /// <returns></returns>
        public WorkDelegate<TWorkContext> Build()
        {
            WorkDelegate<TWorkContext> workDelegate = (TWorkContext context) => Task.CompletedTask;
            _middlewares.Reverse();
            foreach (Func<WorkDelegate<TWorkContext>, WorkDelegate<TWorkContext>> middleware in _middlewares)
            {
                workDelegate = middleware(workDelegate);
            }

            return workDelegate;
        }
    }
}
