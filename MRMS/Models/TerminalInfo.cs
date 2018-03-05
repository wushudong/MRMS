using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MRMS.Models
{
    [DataContract]
    public class TerminalInfo : NamedModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Display(Name = "终端编号")]
        public string TerminalId { get; set; }
        
        [DataMember]
        [Display(Name = "AC220V继电器")]
        //TRUE代表打开继电器 FALSE代表关闭继电器 RW
        public bool? AC_Relay1 { get; set; }
        [DataMember]
        [Display(Name = "电控门锁状态")]
        //TRUE代表门锁是开 FALSE代表门锁是关 R
        public bool? Lock_Status { get; set; }
        [DataMember]
        [Display(Name = "电控门锁开锁")]
        //写非0值表示开电控门锁 W
        public byte? Lock_ACT { get; set; }
        [DataMember]
        [Display(Name = "电脑开机状态")]
        //TRUE表示电脑已开机 FALSE表示电脑关机 R
        public bool? Computer_Status { get; set; }
        [DataMember]
        [Display(Name = "电脑开机控制")]
        //写1表示按一下开机键 写4强制关机 写0按一下关机键，正常关机 W
        public byte? Computer_Control { get; set; }
        [DataMember]
        [Display(Name = "系统开关")]
        //TRUE系统开 FALSE系统关 RW
        public bool? System { get; set; }
        [DataMember]
        [Display(Name = "投影机开关")]
        //TRUE投影机开 FALSE投影机关 RW
        public bool? Projector { get; set; }
        [DataMember]
        [Display(Name = "电动幕布升降")]
        //TRUE幕布上升 FALSE幕布下降 RW
        public bool? Projection_Screen { get; set; }
        [DataMember]
        [Display(Name = "投影机信号源选择")]
        //1 台式电脑 2 手提电脑 3 数字展台 RW
        public byte? Projection_Signal { get; set; }
        [DataMember]
        [Display(Name = "电脑信号源选择")]
        //1 台式电脑 2 手提电脑 3 数字展台 RW
        public byte? Computer_Signal { get; set; }
        [DataMember]
        [Display(Name = "主音量增减")]
        //正娄代表增加音量 负数代表减少音量 W
        public byte? Volume { get; set; }
        [DataMember]
        [Display(Name = "主音量静音")]
        //TRUE表示静音 FALSE表示非静音 RW
        public bool? Volume_Mute { get; set; }
        [DataMember]
        [Display(Name = "报警输出")]
        //True 报警打开 False 报警关闭
        public bool? DC_Relay2 { get; set; }
        [DataMember]
        [Display(Name = "光耦1输出状态")]
        //True 开 False 关
        public bool? OC1 { get; set; }
        [DataMember]
        [Display(Name = "光耦2输出状态")]
        //True 开 False 关
        public bool? OC2 { get; set; }
        [DataMember]
        [Display(Name = "最后一次操作代码，给服务器判断终端最近的一次操作是什么状态")]
        //向服务器反馈中控系统最近的一次操作
        //1联线 
        //2终端自动下课
        //3离线
        //4远程操作 
        //5断电重启恢复先前的状态
        //6重启恢复初始值
        public byte? Opereate_Last { get; set; }
        [DataMember]
        [Display(Name = "交换机LAN1状态")]
        //True 连接 False 断网
        public bool? LAN1 { get; set; }
        [DataMember]
        [Display(Name = "交换机LAN2状态")]
        //True 连接 False 断网
        public bool? LAN2 { get; set; }
        [DataMember]
        [Display(Name = "交换机LAN3状态")]
        //True 连接 False 断网
        public bool? LAN3 { get; set; }
        [DataMember]
        [Display(Name = "交换机LAN4状态")]
        //True 连接 False 断网
        public bool? LAN4 { get; set; }
        [DataMember]
        [Display(Name = "设备布防状态")]
        //True 布防 False 撤防
        public bool? Alarm_Control { get; set; }
        [DataMember]
        [Display(Name = "布防传感器输入,接的投影机报警信号")]
        //True 动作 False 未动作
        public bool? Alarm_In1 { get; set; }
        [DataMember]
        [Display(Name = "布防传感器输入,接的门磁报警信号")]
        //True 动作 False 未动作
        public bool? Alarm_In2 { get; set; }
        
        [DataMember]
        [Display(Name = "是否连接")]
        public bool IsConnected { get; set; }

        //[DataMember]
        //[Display(Name = "记录时间")]
        //[JsonConverter(typeof(JavaScriptDateTimeConverter))]
        //public DateTime? LogTime { get; set; }
    }
}
