#region 类信息
/*----------------------------------------------------------------
 * 模块名：Aria2cGlobalStat
 * 创建者：isRight
 * 修改者列表：
 * 创建日期：2016.12.29
 * 模块描述：全局状态信息
 *----------------------------------------------------------------*/
#endregion

namespace FlyVR.Aria2
{
    using System;
    using CookComputing.XmlRpc;
    using System.Collections.Generic;

    public sealed class Aria2cGlobalStat
    {
        public Aria2cGlobalStat()
        {

        }

        public Aria2cGlobalStat(XmlRpcStruct rpcStruct)
        {
            List<string> keyList = rpcStruct.Keys as List<string>;
            List<object> valueList = rpcStruct.Values as List<object>;

            for (int i = 0; i < rpcStruct.Count; i++)
            {
                string key = keyList[i];
                object value = valueList[i];

                if (key == "downloadSpeed")
                {
                    this.DownloadSpeed = Convert.ToInt64(value as string);
                }
                else if (key == "uploadSpeed")
                {
                    this.UploadSpeed = Convert.ToInt64(value as string);
                }
                else if (key == "numActive")
                {
                    this.NumActive = Convert.ToInt64(value as string);
                }
                else if (key == "numWaiting")
                {
                    this.NumWaiting = Convert.ToInt64(value as string);
                }
                else if (key == "numStopped")
                {
                    this.NumStopped = Convert.ToInt64(value as string);
                }
                else if (key == "numStoppedTotal")
                {
                    this.NumStoppedTotal = Convert.ToInt64(value as string);
                }
                else
                {
                    throw new Exception("无法将属性转换到Aria2cGlobalStat中");
                }
            }
        }

        /// <summary>
        /// 全局下载速度 byte/s
        /// </summary>
        public long DownloadSpeed { get; set; }

        /// <summary>
        /// 全局上传速度 byte/s
        /// </summary>
        public long UploadSpeed { get; set; }

        /// <summary>
        /// 当前激活的任务数量
        /// </summary>
        public long NumActive { get; set; }

        /// <summary>
        /// 正在等待的任务数量
        /// </summary>
        public long NumWaiting { get; set; }

        /// <summary>
        /// 已经停止的任务数量
        /// </summary>
        public long NumStopped { get; set; }

        /// <summary>
        /// 当前会话总共停止的任务数量
        /// </summary>
        public long NumStoppedTotal { get; set; }

    }
}
