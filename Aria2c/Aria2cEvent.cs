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
 * 模块名：Aria2cGlobalStatEvent
 * 创建者：任洪壮
 * 修改者列表：
 * 创建日期：2016.12.29
 * 模块描述：aria2c事件
 *----------------------------------------------------------------*/
#endregion

namespace FlyVR.Aria2
{
    using System;

    public sealed class Aria2cEvent : EventArgs
    {
        public Aria2cEvent()
        {

        }

        public Aria2cEvent(string gid)
        {
        }

        /// <summary>
        /// 任务标识符
        /// </summary>
        public string Gid { get; internal set; }
    }
}
