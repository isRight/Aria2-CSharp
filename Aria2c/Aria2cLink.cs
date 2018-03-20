#region 类信息
/*----------------------------------------------------------------
 * 模块名：Aria2cLink
 * 创建者：isRight
 * 修改者列表：
 * 创建日期：2016.12.29
 * 模块描述：下载任务链接信息
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

    public sealed class Aria2cServerLink
    {
        public Aria2cServerLink()
        {

        }
  
        public Aria2cServerLink(XmlRpcStruct rpcStruct)
        {
            List<string> keyList = rpcStruct.Keys as List<string>;
            List<object> valueList = rpcStruct.Values as List<object>;

            for (int i = 0; i < rpcStruct.Count; i++)
            {
                string key = keyList[i];
                object value = valueList[i];

                if (key == "uri")
                {
                    this.Uri = value as string;
                }
                else if (key == "currentUri")
                {
                    this.CurrentUri = value as string;
                }
                else if (key == "downloadSpeed")
                {
                    this.DownloadSpeed = Convert.ToInt64(value as string);
                }
                else
                {
                    throw new Exception("无法将属性转换到Aria2cServerLink中");
                }
            }
        }

        /// <summary>
        /// 原始地址
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// 当前下载地址
        /// </summary>
        public string CurrentUri { get; set; }

        /// <summary>
        /// 下载速度 byte/s
        /// </summary>
        public long DownloadSpeed { get; set; }
    }


    public  class Aria2cLink
    {

        public Aria2cLink()
        {

        }

        /// <summary>
        /// 将rpc信息转换到服务器信息中
        /// </summary>
        /// <param name="rpcStruct"></param>
        public Aria2cLink(XmlRpcStruct rpcStruct)
        {
            List<string> keyList = rpcStruct.Keys as List<string>;
            List<object> valueList = rpcStruct.Values as List<object>;

            for (int i = 0; i < rpcStruct.Count; i++)
            {
                string key = keyList[i];
                object value = valueList[i];

                if (key == "index")
                {
                    this.Index = Convert.ToInt64(value as string);
                }
                else if (key == "servers")
                {
                    this.Servers = Aria2cTools.ConvertToAria2cServerLink(value as XmlRpcStruct[]);
                }
                else
                {
                    throw new Exception("无法将属性转换到Aria2cServer中");
                }
            }
        }

        /// <summary>
        /// 文件索引
        /// </summary>
        public long Index;

        private List<Aria2cServerLink> _serverList = new List<Aria2cServerLink>();

        /// <summary>
        /// 服务器地址
        /// </summary>
        public List<Aria2cServerLink> Servers
        {
            get
            {
                return _serverList;
            }

            set
            {
                _serverList = value;
            }
        }

        public void AddServer(Aria2cServerLink server)
        {
            _serverList.Add(server);
        }

        public bool RemoveServer(Aria2cServerLink server)
        {
            return _serverList.Remove(server);
        }
    }
}
