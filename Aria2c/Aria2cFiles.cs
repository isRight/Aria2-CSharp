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
 * 模块名：Aria2cFiles
 * 创建者：任洪壮
 * 修改者列表：
 * 创建日期：2016.12.29
 * 模块描述：aria2c下载任务文件信息
 *----------------------------------------------------------------*/
#endregion


namespace FlyVR.Aria2
{
    using CookComputing.XmlRpc;
    using System;
    using System.Collections.Generic;

    public sealed class Aria2cFile
    {
        public Aria2cFile()
        {

        }

        /// <summary>
        /// 将rpc信息转换为下载任务文件信息
        /// </summary>
        /// <param name="rpcStruct"></param>
        public Aria2cFile(XmlRpcStruct rpcStruct)
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
                else if (key == "path")
                {
                    this.Path = value as string;
                }
                else if (key == "length")
                {
                    this.Length = Convert.ToInt64(value as string);
                }
                else if (key == "completedLength")
                {
                    this.CompletedLength = Convert.ToInt64(value as string);
                }
                else if (key == "selected")
                {
                    this.Selected = (value as string) == "true" ? true : false;
                }
                else if (key == "uris")
                {
                    this.Uris = ConvertToAria2cUris(value as XmlRpcStruct[]);
                }
                else
                {
                    throw new Exception("Aria2cFile不包含该属性");
                }
            }
        }

        /// <summary>
        /// 将rpc信息转换为下载任务文件链接信息
        /// </summary>
        /// <param name="rpcStruct"></param>
        /// <returns></returns>
        public List<Aria2cUri> ConvertToAria2cUris(params XmlRpcStruct[] rpcStruct)
        {
            List<Aria2cUri> uriList = new List<Aria2cUri>();

            if (rpcStruct != null)
            {
                foreach (XmlRpcStruct rpc in rpcStruct)
                {
                    Aria2cUri uri = new Aria2cUri(rpc);
                    uriList.Add(uri);
                }
            }

            return uriList;
        }

        /// <summary>
        /// 文件在下载任务文件列表中的索引
        /// </summary>
        public long Index { get; set; }


        /// <summary>
        /// 文件路径，包含文件名
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 文件长度 单位 byte
        /// </summary>
        public long Length { get; set; }

        /// <summary>
        /// 已下载的文件长度 单位byte
        /// </summary>
        public long CompletedLength { get; set; }

        /// <summary>
        /// 文件是否被选中
        /// </summary>
        public bool Selected { get; set; }


        List<Aria2cUri> _uriList = new List<Aria2cUri>();

        /// <summary>
        /// 文件下载地址
        /// </summary>
        public List<Aria2cUri> Uris
        {
            get
            {
                return _uriList;
            }
            set
            {
                _uriList = value;
            }
        }

        /// <summary>
        /// 添加uri
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public void AddUri(Aria2cUri uri)
        {
            _uriList.Add(uri);
        }

        /// <summary>
        /// 移除uri
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public bool Remove(Aria2cUri uri)
        {
            return _uriList.Remove(uri);
        }

    }


}
