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
 * 模块名：Aria2cTools
 * 创建者：任洪壮
 * 修改者列表：
 * 创建日期：2016.12.29
 * 模块描述：工具方法
 *----------------------------------------------------------------*/
#endregion

 namespace FlyVR.Aria2
{
    using CookComputing.XmlRpc;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public enum PosType
    {
        //头部
        [Description("POS_SET")]
        POS_SET,

        //当前位置
        [Description("POS_CUR")]
        POS_CUR,

        //尾部
        [Description("POS_END")]
        POS_END,
    }

    public sealed class Aria2cTools
    {
        private Aria2cTools()
        {

        }

        /// <summary>
        /// 将RPC信息转化为任务状态
        /// </summary>
        /// <param name="rpcStruct"></param>
        /// <returns></returns>
        public static List<Aria2cTask> ConvertToAria2cTasks(XmlRpcStruct[] rpcStruct)
        {
            List<Aria2cTask> taskList = new List<Aria2cTask>();

            foreach (XmlRpcStruct rpc in rpcStruct)
            {
                Aria2cTask task = new Aria2cTask(rpc);
                taskList.Add(task);
            }

            return taskList;
        }


        /// <summary>
        /// 将rpc信息转换为下载任务文件信息
        /// </summary>
        /// <param name="rpcStruct"></param>
        /// <returns></returns>
        public static List<Aria2cFile> ConvertToAria2cFiles(params XmlRpcStruct[] rpcStruct)
        {
            List<Aria2cFile> fileList = new List<Aria2cFile>();

            foreach (XmlRpcStruct rpc in rpcStruct)
            {
                Aria2cFile file = new Aria2cFile(rpc);
                fileList.Add(file);
            }

            return fileList;
        }

        /// <summary>
        /// 将rpc信息转换为下载任务文件链接信息
        /// </summary>
        /// <param name="rpcStruct"></param>
        /// <returns></returns>
        public static List<Aria2cUri> ConvertToAria2cUris(params XmlRpcStruct[] rpcStruct)
        {
            List<Aria2cUri> uriList = new List<Aria2cUri>();

            foreach (XmlRpcStruct rpc in rpcStruct)
            {
                Aria2cUri uri = new Aria2cUri(rpc);
                uriList.Add(uri);
            }

            return uriList;
        }
    

        /// <summary>
        /// 将rpc信息转换为种子链接信息
        /// </summary>
        /// <param name="rpcStruct">rpc信息</param>
        /// <returns>种子链接信息列表</returns>
        public static List<Aria2cPeers> ConvertToAria2cPeers(params XmlRpcStruct[] rpcStruct)
        {
            List<Aria2cPeers> peerList = new List<Aria2cPeers>();

            foreach (XmlRpcStruct rpc in rpcStruct)
            {
                Aria2cPeers peer = new Aria2cPeers(rpc);
                peerList.Add(peer);
            }

            return peerList;
        }


        /// <summary>
        /// 将rpc信息转换到服务器信息中
        /// </summary>
        /// <param name="rpcStruct"></param>
        /// <returns>服务器信息列表</returns>
        public static List<Aria2cLink> ConvertToAria2cServers(params XmlRpcStruct[] rpcStruct)
        {
            List<Aria2cLink> serverList = new List<Aria2cLink>();

            foreach (XmlRpcStruct rpc in rpcStruct)
            {
                Aria2cLink server = new Aria2cLink(rpc);
                serverList.Add(server);
            }

            return serverList;
        }

        /// <summary>
        /// 将rpc信息转换到服务器地址信息中
        /// </summary>
        /// <param name="rpcStruct"></param>
        /// <returns>服务器信息地址列表</returns>
        public static List<Aria2cServerLink> ConvertToAria2cServerLink(params XmlRpcStruct[] rpcStruct)
        {
            List<Aria2cServerLink> linkList = new List<Aria2cServerLink>();

            foreach (XmlRpcStruct rpc in rpcStruct)
            {
                Aria2cServerLink serverLink = new Aria2cServerLink(rpc);               
                linkList.Add(serverLink);
            }

            return linkList;
        }

        /// <summary>
        /// 获取XmlRpcStruct中的值
        /// </summary>
        /// <param name="rpcStruct"></param>
        /// <param name="key">主键名</param>
        /// <returns></returns>
        public static object GetRpcStructValue(XmlRpcStruct rpcStruct, string key)
        {
            List<string> keyList = rpcStruct.Keys as List<string>;
            List<object> valueList = rpcStruct.Values as List<object>;

            for (int i = 0; i < rpcStruct.Count; i++)
            {
                if (key == keyList[i])
                {
                    object value = valueList[i];
                    return value;
                }
            }

            throw new Exception("XmlRpcStruct中不包含该主键");
        }

        /// <summary>
        /// 获取枚举类型描述
        /// </summary>
        /// <param name="value"></param>
        /// <returns>返回描述，没有描述将直接返回枚举名称</returns>
        public static string GetEnumDescription(Enum value)
        {
            string str = value.ToString();
            System.Reflection.FieldInfo field = value.GetType().GetField(str);
            object[] objs = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            if (objs == null || objs.Length == 0)
            {
                return str;
            }

            System.ComponentModel.DescriptionAttribute da = (System.ComponentModel.DescriptionAttribute)objs[0];
            return da.Description;
        }


        /// <summary>
        /// 根据路径获取文件目录
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>文件目录</returns>
        public static string GetDirectoryName(string path)
        {
            return System.IO.Path.GetDirectoryName(path);
        }

        /// <summary>
        /// 根据路径获取文件名
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static string GetFileName(string path)
        {
            return System.IO.Path.GetFileName(path);
        }

        /// <summary>
        /// 根据路径获取文件名,不带后缀名
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static string GetFileNameWithoutExtension(string path)
        {
            return System.IO.Path.GetFileNameWithoutExtension(path);
        }

        /// <summary>
        /// 将纪元时间转换为标准时间
        /// </summary>
        /// <param name="second">从1970.1.1开始，到现在经过的秒数</param>
        /// <returns>UTC时间</returns>
        public static DateTime EpochToUtcTime(long second)
        {
            DateTime dateTime = new DateTime(1970, 1, 1);
            dateTime.AddSeconds(second);
            return dateTime;
        }

        /// <summary>
        /// 纪元时间转换为字符串
        /// </summary>
        /// <param name="second">从1970.1.1开始，到现在经过的秒数</param>
        /// <returns>UTC时间字符串</returns>
        public static string EpochToString(long second)
        {
            DateTime dateTime = new DateTime(1970, 1, 1);
            dateTime.AddSeconds(second);
            string dateString = dateTime.ToString("yyyy-MM-dd hh:mm:ss");
            return dateString;
        }

        /// <summary>
        /// 将UTC时间转换为字符串
        /// </summary>
        /// <param name="second">UTC时间</param>
        /// <returns>UTC时间字符串</returns>
        public static string DateTimeToString(DateTime dateTime)
        {
            string dateString = dateTime.ToString("yyyy-MM-dd hh:mm:ss");
            return dateString;
        }
    }
}
