#region 类信息
/*----------------------------------------------------------------
 * 模块名：Aria2c
 * 创建者：isRight
 * 修改者列表：
 * 创建日期：2016.12.29
 * 模块描述：aria2c接口
 *----------------------------------------------------------------*/
#endregion

namespace FlyVR.Aria2
{
    using System;
    using System.Collections.Generic;
    using System.Timers;

    public sealed class Aria2c : IDisposable
    {
        #region 私有变量
        private Aria2cGlobalStat globalStat;
        private Dictionary<string, Aria2cTask> downLoadDict;
        private Timer timer;
        private object objlock;

        #endregion

        #region 构造销毁
        public Aria2c()
        {
            objlock = new object();
            globalStat = new Aria2cGlobalStat();
            downLoadDict = new Dictionary<string, Aria2cTask>();

            StartTimer();
        }
        public void Dispose()
        {
            StopTimer();
        }

        #endregion

        #region 属性       
        /// <summary>
        /// 是否正在运行
        /// </summary>
        private bool IsRunning
        {
            get
            {
                try
                {
                    string version = Aria2cWarpper.GetVersion();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return false;
                }
            }
        }

        /// <summary>
        /// 是否正在下载
        /// </summary>
        public bool IsLoading
        {
            get
            {
                bool isLoading = false;
                lock (objlock)
                {
                    foreach (var task in downLoadDict.Values)
                    {
                        if (task.Status == Aria2cTaskStatus.Active ||
                            task.Status == Aria2cTaskStatus.Waiting)
                        {
                            isLoading = true;
                            break;
                        }
                    }
                }

                return isLoading;
            }
        }
        #endregion

        #region 事件
        /// <summary>
        /// 下载开始事件
        /// </summary>
        public event EventHandler<Aria2cTaskEvent> OnStart;

        /// <summary>
        /// 下载暂停事件
        /// </summary>
        public event EventHandler<Aria2cTaskEvent> OnPaused;

        /// <summary>
        /// 取消暂停，继续下载事件
        /// </summary>

        public event EventHandler<Aria2cTaskEvent> OnUnPause;

        /// <summary>
        /// 下载结束事件
        /// </summary>
        public event EventHandler<Aria2cTaskEvent> OnFinish;

        /// <summary>
        /// 下载全部结束事件
        /// </summary>
        public event EventHandler<EventArgs> OnAllFinish;

        /// <summary>
        /// 暂停所有任务事件
        /// </summary>
        public event EventHandler<EventArgs> OnPausedAll;

        /// <summary>
        /// 取消暂停没，继续下载所有任务
        /// </summary>
        public event EventHandler<EventArgs> OnUnPauseAll;

        /// <summary>
        /// 移除任务事件
        /// </summary>
        public event EventHandler<Aria2cEvent> OnRemoved;

        /// <summary>
        /// 正在下载事件
        /// </summary>
        public event EventHandler<Aria2cTaskEvent> OnLoading;

        /// <summary>
        /// 下载出错事件
        /// </summary>
        public event EventHandler<Aria2cTaskEvent> OnError;

        /// <summary>
        /// 下载等待事件
        /// </summary>
        public event EventHandler<Aria2cTaskEvent> OnWaiting;

        /// <summary>
        /// 全局状态发生化
        /// </summary>
        public event EventHandler<Aria2cGlobalStatEvent> OnGlobalStatusChanged;

        #endregion

        #region 状态查询
        /// <summary>
        /// 初始化定时器
        /// </summary>
        private void StartTimer()
        {
            if (IsRunning)
            {
                timer = new Timer(1000);
                timer.Elapsed += new ElapsedEventHandler(OnTimeOut);
                timer.AutoReset = true;
                timer.Enabled = true;
                timer.Start();
            }
            else
            {
                throw new Exception("服务未初始化");
            }
        }

        /// <summary>
        /// 关闭定时器
        /// </summary>
        private void StopTimer()
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
                timer = null;
            }
        }

        /// <summary>
        /// 定时器超时事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="evt"></param>
        public void OnTimeOut(object sender, EventArgs evt)
        {
            GetTaskStatus();
            GetGlobalStatus();
        }

        /// <summary>
        /// 获取任务状态信息
        /// </summary>
        private void GetTaskStatus()
        {
            try
            {
                //复制key
                List<string> keyList = new List<string>();
                foreach (var key in downLoadDict.Keys)
                {
                    keyList.Add(key);
                }


                //遍历查询状态
                foreach (var gid in keyList)
                {
                    try
                    {
                        Aria2cTask task = TellStatus(gid);
                        if (downLoadDict[gid].Status != task.Status)
                        {
                            OnStatusChanged(task);
                        }

                        if (task.Status == Aria2cTaskStatus.Active)
                        {
                            OnLoading?.Invoke(this, new Aria2cTaskEvent(task));
                        }

                        UpdateDownTask(task);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// 任务状态变换
        /// </summary>
        /// <param name="task"></param>
        private void OnStatusChanged(Aria2cTask task)
        {
            switch (task.Status)
            {
                case Aria2cTaskStatus.Active:
                    OnUnPause?.Invoke(this, new Aria2cTaskEvent(task));
                    break;
                case Aria2cTaskStatus.Complete:
                    {
                        OnFinish?.Invoke(this, new Aria2cTaskEvent(task));
                        RemoveDownTask(task.Gid);
                        if (CountDownTask() == 0)
                        {
                            OnAllFinish?.Invoke(this, new EventArgs());
                        }
                    }
                    break;
                case Aria2cTaskStatus.Error:
                    OnError?.Invoke(this, new Aria2cTaskEvent(task));
                    break;
                case Aria2cTaskStatus.Paused:
                    OnPaused?.Invoke(this, new Aria2cTaskEvent(task));
                    break;
                case Aria2cTaskStatus.Removed:
                    OnRemoved?.Invoke(this, new Aria2cEvent(task.Gid));
                    downLoadDict.Remove(task.Gid);
                    break;
                case Aria2cTaskStatus.Waiting:
                    OnWaiting?.Invoke(this, new Aria2cTaskEvent(task));
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 全局状态
        /// </summary>
        private void GetGlobalStatus()
        {
            try
            {
                Aria2cGlobalStat status = this.GetGlobalStat();
                if (status == null) return;

                if (globalStat.DownloadSpeed != status.DownloadSpeed ||
                    globalStat.UploadSpeed != status.UploadSpeed ||
                    globalStat.NumActive != status.NumActive ||
                    globalStat.NumWaiting != status.NumWaiting ||
                    globalStat.NumStopped != status.NumStopped ||
                    globalStat.NumStoppedTotal != status.NumStoppedTotal)
                {
                    OnGlobalStatusChanged?.Invoke(this, new Aria2cGlobalStatEvent(status));
                    globalStat = status;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        #endregion

        #region 字典维护
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="task"></param>
        private void AddDownTask(Aria2cTask task)
        {
            lock (objlock)
            {
                downLoadDict[task.Gid] = task;
            }
        }

        /// <summary>
        /// 更新任务
        /// </summary>
        /// <param name="task"></param>
        private void UpdateDownTask(Aria2cTask task)
        {
            lock (objlock)
            {
                downLoadDict[task.Gid] = task;
            }
        }

        /// <summary>
        /// 移除任务
        /// </summary>
        /// <param name="gid"></param>
        private void RemoveDownTask(string gid)
        {
            lock (objlock)
            {
                downLoadDict.Remove(gid);
            }
        }

        /// <summary>
        /// 计算当前任务数量
        /// </summary>
        /// <returns></returns>
        private int CountDownTask()
        {
            int count = 0;
            lock (objlock)
            {
                count = downLoadDict.Count;
            }

            return count;
        }
        #endregion

        #region 自定义接口
        /// <summary>
        /// 恢复下载任务
        /// </summary>
        /// <param name="gid"></param>
        /// <returns></returns>
        public Aria2cTask RestoreTask(string gid)
        {
            var task = this.TellStatus(gid);
            if (task != null)
            {
                AddDownTask(task);
                return task;
            }

            return null;
        }

        /// <summary>
        /// 恢复下载
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public string RestoreUri(string uri)
        {
            var activelist = this.TellActive();
            var waitList = this.TellWaiting(0, 1024);
            var stopList = this.TellStopped(0, 1024);
            activelist.AddRange(waitList);
            activelist.AddRange(stopList);

            foreach (var task in activelist)
            {
                if (string.Equals(task.Files[0].Uris[0].Uri, uri,
                    StringComparison.CurrentCultureIgnoreCase))
                {
                    AddDownTask(task);
                    return task.Gid;
                }
            }

            return string.Empty;
        }
        #endregion

        #region Aria2c 接口

        /// <summary>
        /// 添加下载任务
        /// </summary>
        /// <param name="uri">下载地址</param>
        /// <param name="fileName">输出文件名</param>
        /// <param name="dir">下载文件夹</param>
        /// <returns>成功返回任务标识符，失败返回空</returns>
        public string AddUri(string uri, string fileName = "", string dir = "")
        {
            try
            {
                string gid = Aria2cWarpper.AddUri(uri, fileName, dir);
                if (!string.IsNullOrWhiteSpace(gid))
                {
                    Aria2cTask task = Aria2cWarpper.TellStatus(gid);
                    OnStart?.Invoke(this, new Aria2cTaskEvent(task));
                    AddDownTask(task);
                }
                return gid;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// 下载种子链接文件
        /// </summary>
        /// <param name="torrent">种子文件</param>
        /// <param name="fileName">输出文件名</param>
        /// <param name="dir">下载目录</param>
        /// <returns>成功返回任务标识符，失败返回空</returns>
        public string AddTorrent(string torrent, string fileName = "", string dir = "")
        {
            try
            {
                string gid = Aria2cWarpper.AddTorrent(torrent, fileName, dir);
                if (!string.IsNullOrWhiteSpace(gid))
                {
                    Aria2cTask task = Aria2cWarpper.TellStatus(gid);
                    OnStart?.Invoke(this, new Aria2cTaskEvent(task));
                    AddDownTask(task);
                }
                return gid;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// 下载磁链接文件
        /// </summary>
        /// <param name="metalink">磁链接文件</param>
        /// <param name="dir">下载目录</param>
        /// <returns>成功返回任务标识符，失败返回空</returns>
        public string AddMetalink(string metalink, string fileName = "", string dir = "")
        {
            try
            {
                string gid = Aria2cWarpper.AddMetalink(metalink, fileName, dir);
                if (!string.IsNullOrWhiteSpace(gid))
                {
                    Aria2cTask task = Aria2cWarpper.TellStatus(gid);
                    OnStart?.Invoke(this, new Aria2cTaskEvent(task));
                    AddDownTask(task);
                }
                return gid;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// 删除正在下载任务
        /// </summary>
        /// <param name="gid">任务标识符</param>
        /// <returns>成功返回true， 失败返回fale</returns>
        public bool Remove(string gid)
        {
            try
            {
                bool result = Aria2cWarpper.Remove(gid);
                if (result)
                {
                    OnRemoved?.Invoke(this, new Aria2cEvent(gid));
                    RemoveDownTask(gid);
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// 强制删除任务
        /// </summary>
        /// <param name="gid">任务标识符</param>
        /// <returns>成功返回true， 失败返回fale</returns>
        public bool ForceRemove(string gid)
        {
            try
            {
                bool result = Aria2cWarpper.ForceRemove(gid);
                if (result)
                {
                    OnRemoved?.Invoke(this, new Aria2cEvent(gid));
                    RemoveDownTask(gid);
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// 暂停任务
        /// </summary>
        /// <param name="gid">任务标识符</param>
        /// <returns>成功返回true， 失败返回fale</returns>
        public bool Pause(string gid)
        {
            try
            {
                bool pause = Aria2cWarpper.Pause(gid);
                if (pause)
                {
                    Aria2cTask task = Aria2cWarpper.TellStatus(gid);
                    OnPaused?.Invoke(this, new Aria2cTaskEvent(task));
                }
                return pause;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// 强制暂停任务
        /// </summary>
        /// <param name="gid">任务标识符</param>
        /// <returns>成功返回true， 失败返回fale</returns>
        public bool ForcePause(string gid)
        {
            try
            {
                bool pause = Aria2cWarpper.ForcePause(gid);
                if (pause)
                {
                    Aria2cTask task = Aria2cWarpper.TellStatus(gid);
                    OnPaused?.Invoke(this, new Aria2cTaskEvent(task));
                }
                return pause;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// 暂停所有任务
        /// </summary>
        /// <returns>成功返回true， 失败返回fale</returns>
        public bool PauseAll()
        {
            try
            {
                bool result = Aria2cWarpper.PauseAll();
                if (result)
                {
                    OnPausedAll?.Invoke(this, new EventArgs());
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// 强制暂停所有任务
        /// </summary>
        /// <returns>成功返回true， 失败返回fale</returns>
        public bool ForcePauseAll()
        {
            try
            {
                bool result = Aria2cWarpper.ForcePauseAll();
                if (result)
                {
                    OnPausedAll?.Invoke(this, new EventArgs());
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// 取消暂停任务
        /// </summary>
        /// <param name="gid">任务标识符</param>
        /// <returns>成功返回true， 失败返回fale</returns>
        public bool UnPause(string gid)
        {
            try
            {
                bool result = Aria2cWarpper.UnPause(gid);
                if (result)
                {
                    Aria2cTask task = Aria2cWarpper.TellStatus(gid);
                    OnUnPause?.Invoke(this, new Aria2cTaskEvent(task));
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }


        /// <summary>
        /// 取消暂停所有任务
        /// </summary>
        /// <param name="gid">任务标识符</param>
        /// <returns>成功返回true， 失败返回fale</returns>
        public bool UnPauseAll()
        {
            try
            {
                bool result = Aria2cWarpper.UnPauseAll();
                if (result)
                {
                    OnUnPauseAll?.Invoke(this, new EventArgs());
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// 获取下载状态
        /// </summary>
        /// <param name="gid">任务标识符</param>
        /// <param name="task">获取的任务信息</param>
        /// <param name="keys">属性过滤字段</param>
        /// <returns>成功返回任务信息，失败返回空</returns>
        public Aria2cTask TellStatus(string gid, params string[] keys)
        {
            try
            {
                return Aria2cWarpper.TellStatus(gid, keys);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取下载任务的链接地址
        /// </summary>
        /// <param name="gid">任务标识符</param>
        /// <returns>成功返回下载地址列表，失败返回空</returns>
        public List<Aria2cUri> GetUris(string gid)
        {
            try
            {
                return Aria2cWarpper.GetUris(gid);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取下载任务的文件信息
        /// </summary>
        /// <param name="gid">任务标识符</param>
        /// <returns>成功返回文件信息列表，失败返回空</returns>
        public List<Aria2cFile> GetFiles(string gid)
        {
            try
            {
                return Aria2cWarpper.GetFiles(gid);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取BT下载种子链接信息
        /// </summary>
        /// <param name="gid">任务标识符</param>
        /// <returns>成功返回种子链接信息列表，失败返回空</returns>
        public List<Aria2cPeers> GetPeers(string gid)
        {
            try
            {
                return Aria2cWarpper.GetPeers(gid);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取普通下载服务器链接信息
        /// </summary>
        /// <param name="gid">任务标识符</param>
        /// <returns>成功返回服务器链接列表，失败返回空</returns>
        public List<Aria2cLink> GetServers(string gid)
        {
            try
            {
                return Aria2cWarpper.GetServers(gid);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        /// <summary>
        /// 所有正在下载的任务信息
        /// </summary>
        /// <param name="tasks">返回的任务信息列表</param>
        /// <param name="keys">查找的属性关键字</param>
        /// <returns>成功返回正在下载的任务信息列表，失败返回空</returns>
        public List<Aria2cTask> TellActive(params string[] keys)
        {
            try
            {
                return Aria2cWarpper.TellActive(keys);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        /// <summary>
        /// 所有正在等待的任务信息
        /// </summary>
        /// <param name="tasks">返回的任务信息列表</param>
        /// <param name="keys">查找的属性关键字</param>
        /// <returns>成功返回正在等待的任务信息列表，失败返回空</returns>
        public List<Aria2cTask> TellWaiting(int offset, int num, params string[] keys)
        {
            try
            {
                return Aria2cWarpper.TellWaiting(offset, num, keys);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        /// <summary>
        /// 停止的的任务信息
        /// </summary>
        /// <param name="tasks">返回的任务信息列表</param>
        /// <param name="keys">查找的属性关键字</param>
        /// <returns>成功返回停止的的任务信息列表,失败返回空</returns>
        public List<Aria2cTask> TellStopped(int offset, int num, params string[] keys)
        {
            try
            {
                return Aria2cWarpper.TellStopped(offset, num, keys);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        /// <summary>
        /// 改变任务在列表中的位置
        /// </summary>
        /// <param name="gid">任务标识符param>
        /// <param name="pos">位置</param>
        /// <param name="how">更改方式, "POS_SET"：列表首位, "POS_CUR"：移动到pos指定位置,  "POS_END"：列表末尾</param>
        /// <returns>成功返回true， 失败返回fale</returns>
        public bool ChangePosition(string gid, int pos, PosType how)
        {
            try
            {
                return Aria2cWarpper.ChangePosition(gid, pos, how);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// 更改下载任务的链接信息
        /// </summary>
        /// <param name="gid">任务标识符</param>
        /// <param name="fileIndex">一个下载任务可能包含多个下载文件，指定文件索引， 从1开始</param>
        /// <param name="delUris">要删除的现在链接</param>
        /// <param name="addUris">要添加的下载链接</param>
        /// <param name="position">链接插入的位置</param>
        /// <returns>成功返回true， 失败返回fale</returns>
        public bool ChangeUri(string gid, int fileIndex, string[] delUris, string[] addUris)
        {
            try
            {
                return Aria2cWarpper.ChangeUri(gid, fileIndex, delUris, addUris);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// 获取下载任务设置
        /// </summary>
        /// <param name="gid">任务标识符</param>
        /// <param name="option">设置信息</param>
        /// <returns>成功返回任务设置， 失败返回空</returns>
        public Aria2cOption GetOption(string gid)
        {
            try
            {
                return Aria2cWarpper.GetOption(gid);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        /// <summary>
        /// 下载任务设置
        /// </summary>
        /// <param name="gid">任务标识符</param>
        /// <param name="options">设置参数</param>
        /// <returns>成功返回true， 失败返回fale</returns>
        public bool ChangeOption(string gid, Aria2cOption option)
        {
            try
            {
                return Aria2cWarpper.ChangeOption(gid, option);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// 获取全局下载设置
        /// </summary>
        /// <param name="option">设置信息</param>
        /// <returns>成功返回任务设置， 失败返回空</returns>
        public Aria2cOption GetGlobalOption()
        {
            try
            {
                return Aria2cWarpper.GetGlobalOption();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        /// <summary>
        /// 设置全局下载设置
        /// </summary>
        /// <param name="option">设置信息</param>
        /// <returns>成功返回true， 失败返回fale</returns>
        public bool ChangeGlobalOption(Aria2cOption option)
        {
            try
            {
                return Aria2cWarpper.ChangeGlobalOption(option);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }


        /// <summary>
        /// 获取全局下载状态
        /// </summary>
        /// <param name="globalStat">设置信息</param>
        /// <returns>成功返回全局下载状态， 失败返回空</returns>
        public Aria2cGlobalStat GetGlobalStat()
        {
            try
            {
                return Aria2cWarpper.GetGlobalStat();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        /// <summary>
        /// 清空内存
        /// </summary>
        /// <returns>成功返回true， 失败返回fale</returns>
        public bool PurgeDownloadResult()
        {
            try
            {
                return Aria2cWarpper.PurgeDownloadResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// 移除指定任务信息, 清空内存
        /// </summary>
        /// <param name="gid">任务标识符</param>
        /// <returns>成功返回true， 失败返回fale</returns>
        public bool RemoveDownloadResult(string gid)
        {
            try
            {
                return Aria2cWarpper.RemoveDownloadResult(gid);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// 获取版本信息
        /// </summary>
        /// <returns>成功返回版本号，失败返回空字符串</returns>
        public string GetVersion()
        {
            try
            {
                return Aria2cWarpper.GetVersion();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取会话ID
        /// </summary>
        /// <returns>成功返回会话ID，失败返回空字符串</returns>
        public string GetSessionInfo()
        {
            try
            {
                return Aria2cWarpper.GetSessionInfo();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// 关机
        /// </summary>
        public bool Shutdown()
        {
            try
            {
                return Aria2cWarpper.Shutdown();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// 强制关机
        /// </summary>
        public bool ForceShutdown()
        {
            try
            {
                return Aria2cWarpper.ForceShutdown();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// 保存会话
        /// </summary>
        public bool SaveSession()
        {
            try
            {
                return Aria2cWarpper.SaveSession();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        #endregion
    }
}
