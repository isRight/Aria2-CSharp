#region 类信息
/*----------------------------------------------------------------
 * 模块名：Aria2cPeers
 * 创建者：isRight
 * 修改者列表：
 * 创建日期：2016.12.29
 * 模块描述：aria2c下载任务链接信息
 *----------------------------------------------------------------*/
#endregion


namespace FlyVR.Aria2
{
    using CookComputing.XmlRpc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public sealed class Aria2cPeers
    {
        public Aria2cPeers()
        {

        }

        /// <summary>
        /// 将rpc信息转换为种子链接信息
        /// </summary>
        /// <param name="rpcStruct">rpc信息</param>
        public Aria2cPeers(XmlRpcStruct rpcStruct)
        {
            List<string> keyList = rpcStruct.Keys as List<string>;
            List<object> valueList = rpcStruct.Values as List<object>;

            for (int i = 0; i < rpcStruct.Count; i++)
            {
                string key = keyList[i];
                object value = valueList[i];

                if (key == "peerId")
                {
                    this.PeerId = Convert.ToInt64(value as string);
                }
                else if (key == "ip")
                {
                    this.IP = value as string;
                }
                else if (key == "port")
                {
                    this.Port = Convert.ToInt64(value as string);
                }
                else if (key == "bitfield")
                {
                    this.Bitfield = value as string;
                }
                else if (key == "amChoking")
                {
                    this.AmChoking = (value as string) == "true" ? true : false;
                }
                else if (key == "peerChoking")
                {
                    this.PeerChoking = (value as string) == "true" ? true : false;
                }
                else if (key == "downloadSpeed")
                {
                    this.downloadSpeed = Convert.ToInt64(value as string);
                }
                else if (key == "uploadSpeed")
                {
                    this.uploadSpeed = Convert.ToInt64(value as string);
                }
                else if (key == "seeder")
                {
                    this.seeder = (value as string) == "true" ? true : false;
                }
                else
                {
                    throw new Exception("无法将属性转换到Aria2cPeers中");
                }
            }
        }

        /// <summary>
        /// 链接ID
        /// </summary>
        public long PeerId { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 端口号
        /// </summary>
        public long Port { get; set; }

        /// <summary>
        /// 未知
        /// </summary>
        public string Bitfield { get; set; }

        /// <summary>
        /// 下载链接阻塞标识
        /// </summary>
        public bool AmChoking { get; set; }

        /// <summary>
        /// 并行阻塞标识
        /// </summary>
        public bool PeerChoking { get; set; }

        /// <summary>
        /// 下载速度 byte/s
        /// </summary>
        public long downloadSpeed { get; set; }

        /// <summary>
        /// 上传速度 byte/s
        /// </summary>
        public long uploadSpeed { get; set; }

        /// <summary>
        /// 做种标识
        /// </summary>
        public bool seeder { get; set; }

    }
}
