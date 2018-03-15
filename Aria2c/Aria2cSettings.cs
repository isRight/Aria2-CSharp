namespace FlyVR.Aria2
{
    public sealed class Aria2cSettings
    {
        /// <summary>
        /// exe路径
        /// </summary>
        public string Aria2Path
        {
            get;
            set;
        }


        /// <summary>
        /// exe启动参数
        /// </summary>
        public string Aria2Args
        {
            get;
            set;
        }

        /// <summary>
        /// RPC连接地址
        /// </summary>
        public string Aria2Host
        {
            get;
            set;
        }

        /// <summary>
        /// RPC连接端口
        /// </summary>
        public int Aria2Port
        {
            get;
            set;
        }
    }
}
