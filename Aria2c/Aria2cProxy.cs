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
 * 模块名：Aria2cProxy
 * 创建者：任洪壮
 * 修改者列表：
 * 创建日期：2016.12.29
 * 模块描述：aria2c RPC接封装类
 *           aria2c参数说明及RPC接口请参考
 *           http://aria2.github.io/manual/en/html/aria2c.html#aria2.addUri
 *----------------------------------------------------------------*/
#endregion

namespace FlyVR.Aria2
{
    using CookComputing.XmlRpc;

    public interface Aria2cProxy : IXmlRpcProxy
    {
        /// <summary>
        /// 添加 HTTP/FTP/SFTP/BitTorrent地址 类型的下载链接
        /// </summary>
        /// <param name="uris">下载链接</param>
        /// <returns>成功返回下载任务的标识符gid</returns>
        /// 详情参考 http://aria2.github.io/manual/en/html/aria2c.html#aria2.addUri
        [XmlRpcMethod("aria2.addUri")]
        string AddUri(string[] uris);

        /// <summary>
        /// 添加 HTTP/FTP/SFTP/BitTorrent地址 类型的下载链接
        /// </summary>
        /// <param name="uris">下载链接</param>
        /// <param name="options">下载参数</param>
        /// <returns>成功返回下载任务的标识符gid</returns>
        /// 详情参考 http://aria2.github.io/manual/en/html/aria2c.html#aria2.addUri
        [XmlRpcMethod("aria2.addUri")]
        string AddUri(string[] uris, XmlRpcStruct options);


        /// <summary>
        /// 添加 HTTP/FTP/SFTP/BitTorrent地址 类型的下载链接
        /// </summary>
        /// <param name="uris">下载链接</param>
        /// <param name="options">下载参数</param>
        /// <param name="position">插入任务列表位置</param>
        /// <returns>成功返回下载任务的标识符gid</returns>
        /// 详情参考 http://aria2.github.io/manual/en/html/aria2c.html#aria2.addUri
        [XmlRpcMethod("aria2.addUri")]
        string AddUri(string[] uris, XmlRpcStruct options, int position);


        /// <summary>
        /// 添加种子文件下载
        /// </summary>
        /// <param name="torrent">种子文件转换成64位编码字符串</param>
        /// <returns>成功返回下载任务的标识符gid</returns>
        /// 详情参考 http://aria2.github.io/manual/en/html/aria2c.html#aria2.addUri
        [XmlRpcMethod("aria2.addTorrent")]
        string AddTorrent(byte[] torrent);

        /// <summary>
        /// 添加种子文件下载
        /// </summary>
        /// <param name="torrent">种子文件转换成64位编码字符串</param>
        /// <param name="uris">web seeding地址</param>
        /// <returns>成功返回下载任务的标识符gid</returns>
        /// 详情参考 http://aria2.github.io/manual/en/html/aria2c.html#aria2.addUri
        [XmlRpcMethod("aria2.addTorrent")]
        string AddTorrent(byte[] torrent, string[] uris);

        /// <summary>
        /// 添加种子文件下载
        /// </summary>
        /// <param name="torrent">种子文件转换成64位编码字符串</param>
        /// <param name="uris">web seeding地址</param>
        /// <param name="options">下载参数</param>
        /// <returns>成功返回下载任务的标识符gid</returns>
        /// 详情参考 http://aria2.github.io/manual/en/html/aria2c.html#aria2.addUri
        [XmlRpcMethod("aria2.addTorrent")]
        string AddTorrent(byte[] torrent, string[] uris, XmlRpcStruct options);

        /// <summary>
        /// 添加种子文件下载
        /// </summary>
        /// <param name="torrent">种子文件转换成64位编码字符串</param>
        /// <param name="uris">web seeding地址</param>
        /// <param name="options">下载参数</param>
        /// <param name="position">插入任务列表位置</param>
        /// <returns>成功返回下载任务的标识符gid</returns>
        /// 详情参考 http://aria2.github.io/manual/en/html/aria2c.html#aria2.addUri
        [XmlRpcMethod("aria2.addTorrent")]
        string AddTorrent(byte[] torrent, string[] uris, XmlRpcStruct options, int position);


        /// <summary>
        /// 添加磁链接文件下载
        /// </summary>
        /// <param name="args">磁链接文件转换成64位编码字符串</param>
        /// 详情参考 http://aria2.github.io/manual/en/html/aria2c.html#aria2.addMetalink
        [XmlRpcMethod("aria2.addMetalink")]
        string AddMetalink(byte[] metalink);


        /// <summary>
        /// 添加磁链接文件下载
        /// </summary>
        /// <param name="args">磁链接文件转换成64位编码字符串</param>
        /// <param name="options">下载参数</param>
        /// 详情参考 http://aria2.github.io/manual/en/html/aria2c.html#aria2.addMetalink
        [XmlRpcMethod("aria2.addMetalink")]
        string AddMetalink(byte[] metalink, XmlRpcStruct options);


        /// <summary>
        /// 添加磁链接文件下载
        /// </summary>
        /// <param name="args">磁链接文件转换成64位编码字符串</param>
        /// <param name="options">下载参数</param>
        /// <param name="position">插入任务列表位置</param>
        /// 详情参考 http://aria2.github.io/manual/en/html/aria2c.html#aria2.addMetalink
        [XmlRpcMethod("aria2.addMetalink")]
        string AddMetalink(byte[] metalink, XmlRpcStruct options, int position);

        /// <summary>
        /// 移除下载任务
        /// </summary>
        /// <param name="gid">下载任务标识符</param>
        /// <returns>下载任务标识符</returns>
        /// 详情参考 http://aria2.github.io/manual/en/html/aria2c.html#aria2.remove
        [XmlRpcMethod("aria2.remove")]
        string Remove(string gid);


        /// <summary>
        /// 强行移除下载任务
        /// </summary>
        /// <param name="gid">下载任务标识符</param>
        /// <returns>下载任务标识符</returns>
        /// 详情参考 http://aria2.github.io/manual/en/html/aria2c.html#aria2.forceRemove
        [XmlRpcMethod("aria2.forceRemove")]
        string ForceRemove(string gid);

        /// <summary>
        /// 暂停下载任务
        /// </summary>
        /// <param name="gid">下载任务标识符</param>
        /// <returns>下载任务标识符</returns>
        /// 详情参考 http://aria2.github.io/manual/en/html/aria2c.html#aria2.pause
        [XmlRpcMethod("aria2.pause")]
        string Pause(string gid);

        /// <summary>
        /// 暂停所有下载任务
        /// </summary>
        /// <returns>“OK”</returns>
        /// 详情参考 http://aria2.github.io/manual/en/html/aria2c.html#aria2.pauseAll
        [XmlRpcMethod("aria2.pauseAll")]
        string PauseAll();


        /// <summary>
        /// 强制暂停下载任务
        /// </summary>
        /// <param name="gid">下载任务标识符</param>
        /// <returns>下载任务标识符</returns>
        /// 详情参考 http://aria2.github.io/manual/en/html/aria2c.html#aria2.forcePause
        [XmlRpcMethod("aria2.forcePause")]
        string ForcePause(string gid);


        /// <summary>
        /// 强制暂停所有下载任务
        /// </summary>
        /// <returns>“OK”</returns>
        /// 详情参考 http://aria2.github.io/manual/en/html/aria2c.html#aria2.forcePauseAll
        [XmlRpcMethod("aria2.forcePauseAll")]
        string ForcePauseAll();

        /// <summary>
        /// 取消暂停下载任务
        /// </summary>
        /// <param name="gid">下载任务标识符</param>
        /// <returns>下载任务标识符</returns>
        /// 详情参考 http://aria2.github.io/manual/en/html/aria2c.html#aria2.unpause
        [XmlRpcMethod("aria2.unpause")]
        string UnPause(string gid);


        /// <summary>
        /// 强制取消暂停所有下载任务
        /// </summary>
        /// <returns>“OK”</returns>
        /// 详情参考 http://aria2.github.io/manual/en/html/aria2c.html#aria2.unpauseAll
        [XmlRpcMethod("aria2.unpauseAll")]
        string UnPauseAll();


        /// <summary>
        ///  获取指定任务的下载信息
        /// </summary>
        /// <param name="gid">下载任务标识符</param>
        /// <param name="keys">需要获取属名称，空返回所有属性信息</param>
        /// 详情参考 http://aria2.github.io/manual/en/html/aria2c.html#aria2.tellStatus
        /// <returns>下载任务的属性信息</returns>
        [XmlRpcMethod("aria2.tellStatus")]
        XmlRpcStruct TellStatus(string gid, params string[] keys);

        /// <summary>
        /// 获取任务的所有下载地址链接
        /// </summary>
        /// <param name="gid">任务标识符</param>
        /// <returns>地址列表</returns>
        /// 详情参考 http://aria2.github.io/manual/en/html/aria2c.html#aria2.getUris
        [XmlRpcMethod("aria2.getUris")]
        XmlRpcStruct[] GetUris(string gid);


        /// <summary>
        /// 获取任务文件信息
        /// </summary>
        /// <param name="gid">任务标识符</param>
        /// <returns>文件信息</returns>
        /// 详细信息参考：http://aria2.github.io/manual/en/html/aria2c.html#aria2.getFiles
        [XmlRpcMethod("aria2.getFiles")]
        XmlRpcStruct[] GetFiles(string gid);


        /// <summary>
        /// 获取种子下载链接信息
        /// </summary>
        /// <param name="gid">任务标识符</param>
        /// <returns>链接信息列表</returns>
        /// 详细信息参考：http://aria2.github.io/manual/en/html/aria2c.html#aria2.getPeers
        [XmlRpcMethod("aria2.getPeers")]
        XmlRpcStruct[] GetPeers(string gid);


        /// <summary>
        /// 获取HTTP(S)/FTP/SFTP下载任务服务器地址信息
        /// </summary>
        /// <param name="gid">任务标识符</param>
        /// <returns>服务器地址信息</returns>
        /// 详细信息参考：http://aria2.github.io/manual/en/html/aria2c.html#aria2.getServers
        [XmlRpcMethod("aria2.getServers")]
        XmlRpcStruct[] GetServers(string gid);

        /// <summary>
        /// 获取所有正在下载的任务信息列表
        /// <param name="keys">需要获取的属性名称</param>
        /// <returns>所有正在下载的任务信息列表</returns>
        /// 详情参考 http://aria2.github.io/manual/en/html/aria2c.html#aria2.tellActive
        [XmlRpcMethod("aria2.tellActive")]
        XmlRpcStruct[] TellActive(params string[] keys);


        /// <summary>
        /// 正在暂停或等待的任务信息列表
        /// </summary>
        /// <param name="offset">等待任务列表偏移量</param>
        /// <param name="num">查找的个数</param>
        /// <param name="keys">查找的属性关键字</param>
        /// <returns>正在暂停或等待的任务信息列表</returns>
        /// 详情参考 http://aria2.github.io/manual/en/html/aria2c.html#aria2.tellWaiting
        [XmlRpcMethod("aria2.tellWaiting")]
        XmlRpcStruct[] TellWaiting(int offset, int num, params string[] keys);

        /// <summary>
        /// 已停止的任务信息列表
        /// </summary>
        /// <param name="offset">任务列表偏移量</param>
        /// <param name="num">查找的个数</param>
        /// <param name="keys">查找的属性关键字</param>
        /// <returns>已停止任务信息列表</returns>
        /// 详情参考 http://aria2.github.io/manual/en/html/aria2c.html#aria2.tellStopped
        [XmlRpcMethod("aria2.tellStopped")]
        XmlRpcStruct[] TellStopped(int offset, int num, params string[] keys);

        /// <summary>
        /// 更改下载任务在列表中的顺序
        /// </summary>
        /// <param name="gid">任务标识符</param>
        /// <param name="pos">位置</param>
        /// <param name="how">更改方式, "POS_SET"：列表首位, "POS_CUR"：移动到pos指定位置,  "POS_END"：列表末尾</param>
        /// <returns>更改后的位置</returns>
        /// 详情参考 http://aria2.github.io/manual/en/html/aria2c.html#aria2.changePosition
        [XmlRpcMethod("aria2.changePosition")]
        int ChangePosition(string gid, int pos, string how);

        /// <summary>
        /// 更改下载任务的链接信息
        /// </summary>
        /// <param name="gid">任务ID</param>
        /// <param name="fileIndex">一个下载任务可能包含多个下载文件，指定文件索引</param>
        /// <param name="delUris">要删除的现在链接</param>
        /// <param name="addUris">要添加的下载链接</param>
        /// <returns>删除的链接个数，插入的链接个数</returns>
        /// 详情参考 http://aria2.github.io/manual/en/html/aria2c.html#aria2.changeUri
        [XmlRpcMethod("aria2.changeUri")]
        int[] ChangeUri(string gid, int fileIndex, string[] delUris, string[] addUris);

        /// <summary>
        /// 更改下载任务的链接信息
        /// </summary>
        /// <param name="gid">任务标识符</param>
        /// <param name="fileIndex">一个下载任务可能包含多个下载文件，指定文件索引， 从1开始</param>
        /// <param name="delUris">要删除的现在链接</param>
        /// <param name="addUris">要添加的下载链接</param>
        /// <param name="position">链接插入的位置</param>
        /// <returns>删除的链接个数，插入的链接个数</returns>
        /// 详情参考 http://aria2.github.io/manual/en/html/aria2c.html#aria2.changeUri
        [XmlRpcMethod("aria2.changeUri")]
        int[] ChangeUri(string gid, int fileIndex, string[] delUris, string[] addUris, int position);

        /// <summary>
        /// 获取下载任务设置
        /// </summary>
        /// <param name="gid">任务标识符</param>
        /// <returns>设置信息</returns>
        /// 详情参考 http://aria2.github.io/manual/en/html/aria2c.html#aria2.getOption
        [XmlRpcMethod("aria2.getOption")]
        XmlRpcStruct GetOption(string gid);

        /// <summary>
        /// 下载任务设置
        /// </summary>
        /// <param name="gid">任务标识符</param>
        /// <param name="options">设置参数</param>
        /// <returns>“OK”</returns>
        /// 详情参考 http://aria2.github.io/manual/en/html/aria2c.html#aria2.changeOption
        [XmlRpcMethod("aria2.changeOption")]
        string ChangeOption(string gid, XmlRpcStruct options);

        /// <summary>
        /// 获取全局下载设置
        /// </summary>
        /// <returns>设置信息</returns>
        /// 详情参考 http://aria2.github.io/manual/en/html/aria2c.html#aria2.getGlobalOption
        [XmlRpcMethod("aria2.getGlobalOption")]
        XmlRpcStruct GetGlobalOption();

        /// <summary>
        /// 下载任务设置
        /// </summary>
        /// <param name="options">设置参数</param>
        /// <returns>“OK”</returns>
        /// 详情参考 http://aria2.github.io/manual/en/html/aria2c.html#aria2.changeGlobalOption
        [XmlRpcMethod("aria2.changeGlobalOption")]
        string ChangeGlobalOption(XmlRpcStruct options);


        /// <summary>
        /// 获取全局下载状态
        /// </summary>
        /// <returns>状态信息</returns>
        /// 详情参考 http://aria2.github.io/manual/en/html/aria2c.html#aria2.getGlobalStat
        [XmlRpcMethod("aria2.getGlobalStat")]
        XmlRpcStruct GetGlobalStat();


        /// <summary>
        /// 清空下载结果，释放内存
        /// </summary>
        /// <returns>状态信息</returns>
        /// <returns>“OK”</returns>
        /// 详情参考 http://aria2.github.io/manual/en/html/aria2c.html#aria2.purgeDownloadResult
        [XmlRpcMethod("aria2.purgeDownloadResult")]
        string PurgeDownloadResult();

        /// <summary>
        /// 清空下载结果，释放内存
        /// </summary>
        /// <param name="gid">任务标识符</param>
        /// <returns>“OK”</returns>
        /// 详情参考 http://aria2.github.io/manual/en/html/aria2c.html#aria2.removeDownloadResult
        [XmlRpcMethod("aria2.removeDownloadResult")]
        string RemoveDownloadResult(string gid);


        /// <summary>
        /// 版本信息
        /// </summary>
        /// <returns></returns>
        /// 详情参考 http://aria2.github.io/manual/en/html/aria2c.html#aria2.getVersion
        [XmlRpcMethod("aria2.getVersion")]
        XmlRpcStruct GetVersion();


        /// <summary>
        /// RPC客户端与Aria2服务端会话信息
        /// </summary>
        /// <returns></returns>
        /// 详情参考 http://aria2.github.io/manual/en/html/aria2c.html#aria2.getSessionInfo
        [XmlRpcMethod("aria2.getSessionInfo")]
        XmlRpcStruct GetSessionInfo();

        /// <summary>
        /// Aria2关机
        /// </summary>
        /// <returns></returns>
        /// 详情参考 http://aria2.github.io/manual/en/html/aria2c.html#aria2.shutdown
        [XmlRpcMethod("aria2.shutdown")]
        string Shutdown();

        /// <summary>
        /// Aria2强制关机
        /// </summary>
        /// <returns></returns>
        /// 详情参考 http://aria2.github.io/manual/en/html/aria2c.html#aria2.forceShutdown
        [XmlRpcMethod("aria2.forceShutdown")]
        string ForceShutdown();

        /// <summary>
        /// 保存当前链接会话
        /// </summary>
        /// <returns>"OK"</returns>
        /// 详情参考 http://aria2.github.io/manual/en/html/aria2c.html#aria2.saveSession
        [XmlRpcMethod("aria2.saveSession")]
        string SaveSession();
    }
}
