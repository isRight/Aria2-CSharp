#region 类信息
/*----------------------------------------------------------------
 * 模块名：Aria2cTaskEvent
 * 创建者：isRight
 * 修改者列表：
 * 创建日期：2016.12.29
 * 模块描述：aria2c任务信息事件
 *----------------------------------------------------------------*/
#endregion

namespace FlyVR.Aria2
{
    using System;

    public sealed class Aria2cTaskEvent : EventArgs
    {
        public Aria2cTaskEvent()
        {

        }

        public Aria2cTaskEvent(Aria2cTask task)
        {
            Gid = task.Gid;
            Task = task;
        }

        /// <summary>
        /// 任务标识符
        /// </summary>
        public string Gid { get; set; }

        /// <summary>
        /// 任务信息
        /// </summary>
        public Aria2cTask Task { get; set; }

    }
}
