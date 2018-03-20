using System;

#region 类信息
/*----------------------------------------------------------------
 * 模块名：Aria2cGlobalStatEvent
 * 创建者：isRight
 * 修改者列表：
 * 创建日期：2016.12.29
 * 模块描述：aria2c全局状态事件
 *----------------------------------------------------------------*/
#endregion


namespace FlyVR.Aria2
{
    public sealed class Aria2cGlobalStatEvent : EventArgs
    {
        public Aria2cGlobalStatEvent()
        {

        }

        public Aria2cGlobalStatEvent(Aria2cGlobalStat stat)
        {
            Stat = stat;
        }

        /// <summary>
        /// 全局状态
        /// </summary>
        public Aria2cGlobalStat Stat { get; internal set; }
    }
}
