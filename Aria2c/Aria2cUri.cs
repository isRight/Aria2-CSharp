#region 类信息
/*----------------------------------------------------------------
 * 
 *        
 *                    /  \~~~/  \
 *            ,------(     ..    )
 *          /         \__     __/
 *         /|         (  \  | (          
 *         ^ \    /___\  /\ |   
 *            |__|    |__|-"
 *              
 *
 * Copyright (C) 2016 讯飞幻境（北京）科技有限公司
 *
 * 模块名：Aria2cUri
 * 创建者：任洪壮
 * 修改者列表：
 * 创建日期：2016.12.29
 * 模块描述：aria2c下载任务uri
 *----------------------------------------------------------------*/
#endregion

using CookComputing.XmlRpc;
using System;
using System.Collections.Generic;

namespace FlyVR.Aria2
{
    /// <summary>
    /// Uri状态
    /// </summary>
    public enum Aria2cUriStatus
    {
        //正在使用
        Used,   

        //下载任务队列中等待
        Waiting,
    }

    /// <summary>
    /// 下载文件Uri
    /// </summary>
    public sealed class Aria2cUri
    {
        public Aria2cUri()
        {

        }

        /// <summary>
        /// 将rpc信息转换为下载任务文件链接信息
        /// </summary>
        /// <param name="rpcStruct"></param>
        public Aria2cUri(XmlRpcStruct rpcStruct)
        {
            List<string> keyList = rpcStruct.Keys as List<string>;
            List<object> valueList = rpcStruct.Values as List<object>;

            for (int i = 0; i < rpcStruct.Count; i++)
            {
                string key = keyList[i];
                object value = valueList[i];

                if (key == "status")
                {
                    this.Status = ConvertToAria2cUriStatus(value as string);
                }
                else if (key == "uri")
                {
                    this.Uri = value as string;
                }
                else
                {
                    throw new Exception("无法将属性转换到Aria2cUri中");
                }

            }
        }

        /// <summary>
        /// 将字符串转换为链接状态信息
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Aria2cUriStatus ConvertToAria2cUriStatus(string str)
        {
            if (str == "used")
            {
                return Aria2cUriStatus.Used;
            }
            else if (str == "waiting")
            {
                return Aria2cUriStatus.Waiting;
            }
            else
            {
                throw new Exception("Aria2cUriStatusAria2cFile不包含该属性");
            }
        }

        /// <summary>
        /// 文件Uri
        /// </summary>
        public string Uri { get; set; }


        /// <summary>
        /// Uri状态
        /// </summary>
        public Aria2cUriStatus Status { get; set; }
    }
}
