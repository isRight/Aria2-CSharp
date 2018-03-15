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
 * 模块名：Aria2cOption
 * 创建者：任洪壮
 * 修改者列表：
 * 创建日期：2016.12.29
 * 模块描述：下载任务设置
 *----------------------------------------------------------------*/
#endregion

namespace FlyVR.Aria2
{
    using CookComputing.XmlRpc;
    using System.Collections.Generic;

    public sealed class Aria2cOption
    {
        Dictionary<string, string> optionDictionary = new Dictionary<string, string>();

        public Aria2cOption()
        {

        }

        /// <summary>
        /// 将rpc信息转换为设置信息
        /// </summary>
        /// <param name="rpcStruct"></param>
        /// <returns></returns>
        public Aria2cOption(XmlRpcStruct rpcStruct)
        {
            List<string> keyList = rpcStruct.Keys as List<string>;
            List<object> valueList = rpcStruct.Values as List<object>;

            for (int i = 0; i < rpcStruct.Count; i++)
            {
                string key = keyList[i];
                string value = valueList[i] as string;

                this.SetOption(key, value);
            }
        }

        public XmlRpcStruct ToXmlRpcStruct()
        {
            XmlRpcStruct rpcStruct = new XmlRpcStruct();

            foreach (KeyValuePair<string, string> kvp in optionDictionary)
            {
                rpcStruct.Add(kvp.Key, kvp.Value);
            }

            return rpcStruct;
        }

        public Dictionary<string, string> Option
        {
            get
            {
                return optionDictionary;
            }
            set
            {
                optionDictionary = value;
            }
        }

        public void SetOption(string key, string value)
        {
            optionDictionary[key] = value;
        }

        public bool RemoveOption(string key)
        {
            return optionDictionary.Remove(key);
        }
    }
}
