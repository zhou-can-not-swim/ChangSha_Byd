using System.Collections.Concurrent;

namespace ChangSha_Byd_NetCore8.Extends.PlcServices
{
    /// <summary>
    /// 添加S7PLC配置(还没看懂是干什么的)
    /// </summary>
    public class S7PlcOptsBuilder
    {
        private ConcurrentDictionary<string, S7PlcOptItem> _maps;

        public S7PlcOptsBuilder()
        {
            _maps = new ConcurrentDictionary<string, S7PlcOptItem>();
        }

        /// <summary>
        /// 添加S7PLC配置
        /// </summary>
        /// <param name="name"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public S7PlcOptsBuilder AddPlcItem(string name, S7PlcOptItem item)
        {
            if (!_maps.TryAdd(name, item))
            {
                throw new Exception("添加" + name + "失败！");
            }

            return this;
        }

        internal ConcurrentDictionary<string, S7PlcOptItem> BuildMaps()
        {
            return _maps;
        }
    }
}
