#region 类信息
/*----------------------------------------------------------------
 * 模块名：Aria2cTask
 * 创建者：isRight
 * 修改者列表：
 * 创建日期：2016.12.29
 * 模块描述：下载任务信息
 *----------------------------------------------------------------*/
#endregion

namespace FlyVR.Aria2
{
    using CookComputing.XmlRpc;
    using System;
    using System.Collections.Generic;

    public enum Aria2cTaskStatus
    {
        //正在下载
        Active,

        //任务队列等待
        Waiting,

        //暂停
        Paused,

        //错误，停止
        Error,

        //下载完成
        Complete,

        //被用户移除
        Removed,
    }

    public sealed class Aria2cTask
    {
        public Aria2cTask()
        {

        }

        /// <summary>
        /// 将RPC信息转换为下载任务
        /// </summary>
        /// <param name="rpcStruct">RPC信息</param>
        public Aria2cTask(XmlRpcStruct rpcStruct)
        {
            List<string> keyList = rpcStruct.Keys as List<string>;
            List<object> valueList = rpcStruct.Values as List<object>;

            for (int i = 0; i < rpcStruct.Count; i++)
            {
                string key = keyList[i];
                object value = valueList[i];

                if (key == "gid")
                {
                    this.Gid = value as string;
                }
                else if (key == "status")
                {
                    this.Status = ConvertToAria2cTaskStatus(value as string);
                }
                else if (key == "totalLength")
                {
                    this.TotalLength = Convert.ToInt64(value as string);
                }
                else if (key == "completedLength")
                {
                    this.CompletedLength = Convert.ToInt64(value as string);
                }
                else if (key == "uploadLength")
                {
                    this.UploadLength = Convert.ToInt64(value as string);
                }
                else if (key == "bitfield")
                {
                    this.Bitfield = value as string;
                }
                else if (key == "downloadSpeed")
                {
                    this.DownloadSpeed = Convert.ToInt64(value as string);
                }
                else if (key == "uploadSpeed")
                {
                    this.UploadSpeed = Convert.ToInt64(value as string);
                }
                else if (key == "infoHash")
                {
                    this.InfoHash = value as string;
                }
                else if (key == "numSeeders")
                {
                    this.NumSeeders = Convert.ToInt64(value as string);
                }
                else if (key == "numPieces")
                {
                    this.NumPieces = Convert.ToInt64(value as string);
                }
                else if (key == "seeder")
                {
                    this.Seeder = (value as string) == "true" ? true : false;
                }
                else if (key == "pieceLength")
                {
                    this.PieceLength = Convert.ToInt64(value as string);
                }
                else if (key == "connections")
                {
                    this.Connections = Convert.ToInt64(value as string);
                }
                else if (key == "errorCode")
                {
                    this.ErrorCode = Convert.ToInt64(value as string);
                }
                else if (key == "errorMessage")
                {
                    this.ErrorMessage = value as string;
                }
                else if (key == "followedBy")
                {
                    this.FollowedBy = value;
                }
                else if (key == "following")
                {
                    this.Following = value;
                }
                else if (key == "belongsTo")
                {
                    this.BelongsTo = value as string;
                }
                else if (key == "dir")
                {
                    this.Dir = value as string;
                }
                else if (key == "files")
                {
                    this.Files = Aria2cTools.ConvertToAria2cFiles(value as XmlRpcStruct[]);
                }
                else if (key == "bittorrent")
                {
                    this.Bittorrent = new Aria2cBittorrent(value as XmlRpcStruct);
                }
                else if (key == "verifiedLength")
                {
                    this.VerifiedLength = Convert.ToInt64(value as string);
                }
                else if (key == "verifyIntegrityPending")
                {
                    this.VerifyIntegrityPending = (value as string) == "true" ? true : false;
                }
                else
                {
                    throw new Exception("Aria2cTask不包含该属性");
                }
            }
        }

        /// <summary>
        /// 将字符串转化为任务状态
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Aria2cTaskStatus ConvertToAria2cTaskStatus(string str)
        {
            if (str == "active")
            {
                return Aria2cTaskStatus.Active;
            }
            else if (str == "waiting")
            {
                return Aria2cTaskStatus.Waiting;
            }
            else if (str == "paused")
            {
                return Aria2cTaskStatus.Paused;
            }
            else if (str == "error")
            {
                return Aria2cTaskStatus.Error;
            }
            else if (str == "complete")
            {
                return Aria2cTaskStatus.Complete;
            }
            else if (str == "removed")
            {
                return Aria2cTaskStatus.Removed;
            }
            else
            {
                throw new Exception("Aria2cTaskStatus不包含该属性");
            }
        }

        /// <summary>
        /// 任务标识符
        /// </summary>
        public string Gid { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public Aria2cTaskStatus Status { get; set; }

        /// <summary>
        /// 任务总长度 单位byte
        /// </summary>
        public long TotalLength { get; set; }

        /// <summary>
        /// 下载完成长度 单位byte
        /// </summary>
        public long CompletedLength { get; set; }

        /// <summary>
        /// 上传总长度
        /// </summary>
        public long UploadLength { get; set; }


        /// <summary>
        /// 位标识符
        /// </summary>
        public string Bitfield { get; set; }

        /// <summary>
        /// 下载速度 单位byte/s
        /// </summary>
        public long DownloadSpeed { get; set; }

        /// <summary>
        /// 上传速度 单位byte/s
        /// </summary>
        public long UploadSpeed { get; set; }

        /// <summary>
        /// 哈希值， 只对种子下载有效
        /// </summary>
        public string InfoHash { get; set; }

        /// <summary>
        /// 资源链接数，只对种子下载有效
        /// </summary>
        public long NumSeeders { get; set; }

        /// <summary>
        /// 本地做种标识, 只对种子下载有效
        /// </summary>
        public bool Seeder { get; set; }

        /// <summary>
        /// 文件块长度
        /// </summary>
        public long PieceLength { get; set; }

        /// <summary>
        /// 文件快数量
        /// </summary>
        public long NumPieces { get; set; }

        /// <summary>
        /// 服务器链接数
        /// </summary>
        public long Connections { get; set; }

        /// <summary>
        /// 最后一个错误代码
        /// </summary>
        public long ErrorCode { get; set; }

        /// <summary>
        /// 错误信息描述
        /// </summary>
        public string ErrorMessage { get; set; }



        private List<string> _followedByList = new List<string>();

        /// <summary>
        /// 所有子任务标识符
        /// 待定
        /// </summary>
        public object FollowedBy { get; set; }

        //public void AddFollowedGid(string gid)
        //{
        //    _followedByList.Add(gid);
        //}

        //public bool RemoveFollowedGid(string gid)
        //{
        //    return _followedByList.Remove(gid);
        //}


        /// <summary>
        /// 所有子任务标识符, 同FollowedBy
        /// 待定
        /// </summary>
        public object Following { get; set; }


        /// <summary>
        /// 父级任务标识符
        /// </summary>
        public string BelongsTo { get; set; }


        /// <summary>
        /// 任务下载目录
        /// </summary>
        public string Dir { get; set; }

        private List<Aria2cFile> _fileList = new List<Aria2cFile>();
        /// <summary>
        /// 父级任务标识符
        /// </summary>
        public List<Aria2cFile> Files
        {
            get
            {
                return _fileList;
            }
            set
            {
                _fileList = value;
            }
        }

        public void AddAria2cFile(Aria2cFile file)
        {
            _fileList.Add(file);
        }

        public bool RemoveAria2cFile(Aria2cFile file)
        {
            return _fileList.Remove(file);
        }

        /// <summary>
        /// 种子文件信息
        /// </summary>
        public Aria2cBittorrent Bittorrent { get; set; }

        /// <summary>
        /// 已确定的文件长度
        /// </summary>
        public long VerifiedLength { get; set; }

        /// <summary>
        /// 是否在判定任务文件长度
        /// </summary>
        public bool VerifyIntegrityPending { get; set; }
    }
}
