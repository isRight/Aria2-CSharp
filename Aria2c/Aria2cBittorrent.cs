#region 类信息
/*----------------------------------------------------------------
 * 模块名：Aria2cBittorrent
 * 创建者：isRight
 * 修改者列表：
 * 创建日期：2016.12.29
 * 模块描述：aria2c种子任务信息
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

    /// <summary>
    /// 种子文件模式
    /// </summary>
    public enum Aria2cBittorrentMode
    {
        Single,
        Multi,
    }

    public sealed class Aria2cBittorrent
    {

        public Aria2cBittorrent()
        {

        }

        /// <summary>
        /// 将rpc信息转换为种子信息
        /// </summary>
        /// <param name="rpcStruct"></param>
        public Aria2cBittorrent(XmlRpcStruct rpcStruct)
        {
            List<string> keyList = rpcStruct.Keys as List<string>;
            List<object> valueList = rpcStruct.Values as List<object>;

            for (int i = 0; i < rpcStruct.Count; i++)
            {
                string key = keyList[i];
                object value = valueList[i];

                if (key == "announceList")
                {
                    this.AnnounceList = value as string[];
                }
                else if (key == "comment")
                {
                    this.Comment = value as string;
                }
                else if (key == "creationDate")
                {
                    this.CreationDate = Convert.ToInt64(value as string);
                }
                else if (key == "mode")
                {
                    this.Mode = ConvertToAria2cBittorrentMode(value as string);
                }
                else if (key == "name")
                {
                    this.Name = ConvertToBittorrentInfoName(value as XmlRpcStruct);
                }
                else
                {
                    throw new Exception("Aria2cBittorrent不包含该属性");
                }

            }
        }

        /// <summary>
        /// 将字符串转换为Aria2cBittorrentMode
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Aria2cBittorrentMode ConvertToAria2cBittorrentMode(string str)
        {
            if (str == "single")
            {
                return Aria2cBittorrentMode.Single;
            }
            else if (str == "multi")
            {
                return Aria2cBittorrentMode.Multi;
            }
            else
            {
                throw new Exception("Aria2cBittorrentMode不包含该属性");
            }
        }

        /// <summary>
        /// 将rpc信息转换为种子信息名称
        /// </summary>
        /// <param name="rpcStruct"></param>
        /// <returns></returns>
        public static string ConvertToBittorrentInfoName(XmlRpcStruct rpcStruct)
        {
            List<string> keyList = rpcStruct.Keys as List<string>;
            List<object> valueList = rpcStruct.Values as List<object>;

            if (rpcStruct.ContainsKey("name"))
            {
                string value = valueList[0] as string;
                return value;
            }

            return string.Empty;
        }

        /// <summary>
        /// 匿名下载地址
        /// </summary>
        public string[] AnnounceList { get; set;}

        /// <summary>
        /// 种子文件描述
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 种子文件创建时间， 从1970-01-01 00:00:00 UTC开始记秒
        /// </summary>
        public long CreationDate { get; set; }

        /// <summary>
        /// 种子包含文件的模式
        /// </summary>
        public Aria2cBittorrentMode Mode { get; set; }

        /// <summary>
        /// 暂定
        /// </summary>
        public string Name { get; set; } 


    }
}
