using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using BililiveRecorder.FlvProcessor;
using Newtonsoft.Json;
using NLog;

namespace BililiveRecorder.Core.Config
{
    [JsonObject(memberSerialization: MemberSerialization.OptIn)]
    public class ConfigV1 : INotifyPropertyChanged
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 当前工作目录
        /// </summary>
        [JsonIgnore]
        [Utils.DoNotCopyProperty]
        public string WorkDirectory { get => _workDirectory; set => SetField(ref _workDirectory, value); }


        /// <summary>
        /// 房间号列表
        /// </summary>
        [JsonProperty("roomlist")]
        public List<RoomV1> RoomList { get; set; } = new List<RoomV1>();

        /// <summary>
        /// 启用的功能
        /// </summary>
        [JsonProperty("feature")]
        public EnabledFeature EnabledFeature { get => _enabledFeature; set => SetField(ref _enabledFeature, value); }

        /// <summary>
        /// 剪辑-过去的时长(秒)
        /// </summary>
        [JsonProperty("clip_length_future")]
        public uint ClipLengthFuture { get => _clipLengthFuture; set => SetField(ref _clipLengthFuture, value); }

        /// <summary>
        /// 剪辑-将来的时长(秒)
        /// </summary>
        [JsonProperty("clip_length_past")]
        public uint ClipLengthPast { get => _clipLengthPast; set => SetField(ref _clipLengthPast, value); }

        /// <summary>
        /// 自动切割模式
        /// </summary>
        [JsonProperty("cutting_mode")]
        public AutoCuttingMode CuttingMode { get => _cuttingMode; set => SetField(ref _cuttingMode, value); }

        /// <summary>
        /// 自动切割数值（分钟/MiB）
        /// </summary>
        [JsonProperty("cutting_number")]
        public uint CuttingNumber { get => _cuttingNumber; set => SetField(ref _cuttingNumber, value); }

        /// <summary>
        /// 录制断开重连时间间隔 毫秒
        /// </summary>
        [JsonProperty("timing_stream_retry")]
        public uint TimingStreamRetry { get => _timingStreamRetry; set => SetField(ref _timingStreamRetry, value); }

        /// <summary>
        /// 连接直播服务器超时时间 毫秒
        /// </summary>
        [JsonProperty("timing_stream_connect")]
        public uint TimingStreamConnect { get => _timingStreamConnect; set => SetField(ref _timingStreamConnect, value); }

        /// <summary>
        /// 弹幕服务器重连时间间隔 毫秒
        /// </summary>
        [JsonProperty("timing_danmaku_retry")]
        public uint TimingDanmakuRetry { get => _timingDanmakuRetry; set => SetField(ref _timingDanmakuRetry, value); }

        /// <summary>
        /// HTTP API 检查时间间隔 秒
        /// </summary>
        [JsonProperty("timing_check_interval")]
        public uint TimingCheckInterval { get => _timingCheckInterval; set => SetField(ref _timingCheckInterval, value); }

        /// <summary>
        /// 最大未收到新直播数据时间 毫秒
        /// </summary>
        [JsonProperty("timing_watchdog_timeout")]
        public uint TimingWatchdogTimeout { get => _timingWatchdogTimeout; set => SetField(ref _timingWatchdogTimeout, value); }

        /// <summary>
        /// 请求 API 时使用的 Cookie
        /// </summary>
        [JsonProperty("cookie")]
        public string Cookie { get => _cookie; set => SetField(ref _cookie, value); }

        /// <summary>
        /// 是否同时录制弹幕
        /// </summary>
        [JsonProperty("record_danmaku")]
        public bool RecordDanmaku { get => _recordDanmaku; set => SetField(ref _recordDanmaku, value); }

        /// <summary>
        /// 是否记录弹幕原始数据
        /// </summary>
        [JsonProperty("record_danmaku_raw")]
        public bool RecordDanmakuRaw { get => _recordDanmakuRaw; set => SetField(ref _recordDanmakuRaw, value); }

        /// <summary>
        /// 是否同时录制 SuperChat
        /// </summary>
        [JsonProperty("record_danmaku_sc")]
        public bool RecordDanmakuSuperChat { get => _recordDanmakuSuperChat; set => SetField(ref _recordDanmakuSuperChat, value); }

        /// <summary>
        /// 是否同时录制 礼物
        /// </summary>
        [JsonProperty("record_danmaku_gift")]
        public bool RecordDanmakuGift { get => _recordDanmakuGift; set => SetField(ref _recordDanmakuGift, value); }

        /// <summary>
        /// 是否同时录制 上船
        /// </summary>
        [JsonProperty("record_danmaku_guard")]
        public bool RecordDanmakuGuard { get => _recordDanmakuGuard; set => SetField(ref _recordDanmakuGuard, value); }

        /// <summary>
        /// 触发 <see cref="System.Xml.XmlWriter.Flush"/> 的弹幕个数
        /// </summary>
        [JsonProperty("record_danmaku_flush_interval")]
        public uint RecordDanmakuFlushInterval { get => _recordDanmakuFlushInterval; set => SetField(ref _recordDanmakuFlushInterval, value); }

        /// <summary>
        /// 替换api.live.bilibili.com服务器为其他反代，可以支持在云服务器上录制
        /// </summary>
        [JsonProperty("live_api_host")]
        public string LiveApiHost { get => _liveApiHost; set => SetField(ref _liveApiHost, value); }

        [JsonProperty("record_filename_format")]
        public string RecordFilenameFormat
        {
            get => _record_filename_format;
            set => SetField(ref _record_filename_format, value);
        }

        [JsonProperty("clip_filename_format")]
        public string ClipFilenameFormat
        {
            get => _clip_filename_format;
            set => SetField(ref _clip_filename_format, value);
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) { return false; }
            logger.Trace("设置 [{0}] 的值已从 [{1}] 修改到 [{2}]", propertyName, field, value);
            field = value; OnPropertyChanged(propertyName); return true;
        }
        #endregion

        private string _workDirectory;

        private uint _clipLengthPast = 20;
        private uint _clipLengthFuture = 10;
        private uint _cuttingNumber = 10;
        private EnabledFeature _enabledFeature = EnabledFeature.RecordOnly;
        private AutoCuttingMode _cuttingMode = AutoCuttingMode.Disabled;

        private uint _timingWatchdogTimeout = 10 * 1000;
        private uint _timingStreamRetry = 6 * 1000;
        private uint _timingStreamConnect = 5 * 1000;
        private uint _timingDanmakuRetry = 15 * 1000;
        private uint _timingCheckInterval = 5 * 60;

        private string _cookie = string.Empty;

        private string _record_filename_format = @"{roomid}-{name}/录制-{roomid}-{date}-{time}-{title}.flv";
        private string _clip_filename_format = @"{roomid}-{name}/剪辑片段-{roomid}-{date}-{time}-{title}.flv";

        private bool _recordDanmaku = false;
        private bool _recordDanmakuRaw = false;
        private bool _recordDanmakuSuperChat = false;
        private bool _recordDanmakuGift = false;
        private bool _recordDanmakuGuard = false;
        private uint _recordDanmakuFlushInterval = 20;

        private string _liveApiHost = "https://api.live.bilibili.com";
    }
}
