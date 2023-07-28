﻿using JT808.Protocol.Extensions.Streamax.Enums;
using JT808.Protocol.Formatters;
using JT808.Protocol.Interfaces;
using JT808.Protocol.MessagePack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace JT808.Protocol.Extensions.Streamax.MessageBody
{
    /// <summary>
    /// 升级结果上报
    /// </summary>
    public class JT808_0x0B0A : JT808MessagePackFormatter<JT808_0x0B0A>, JT808Bodies, IJT808Analyze
    {
        public ushort MsgId => 0x0B0A;

        public string Description => "升级结果上报";
        /// <summary>
        /// 线路编号
        /// </summary>
        public uint GprsId { get; set; }
        /// <summary>
        /// 升级结果代码
        /// </summary>
        public byte UpdateResultCode { get; set; }
        /// <summary>
        /// 时间
        /// YYMMDDhhmmss
        /// BCD[6]
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// 升级文件
        /// 最长1024
        /// </summary>
        public string UpdateFile { get; set; }
        /// <summary>
        /// 跳过数据体序列化
        /// </summary>
        public bool SkipSerialization => false;

        public void Analyze(ref JT808MessagePackReader reader, Utf8JsonWriter writer, IJT808Config config)
        {
            JT808_0x0B0A value = new();
            value.GprsId = reader.ReadUInt32();
            writer.WriteNumber($"[{value.GprsId.ReadNumber()}]线路编号", value.GprsId);
            value.UpdateResultCode = reader.ReadByte();
            writer.WriteNumber($"[{value.UpdateResultCode.ReadNumber()}]升级结果代码-{Enum.GetName(typeof(UpdateResultCode), value.UpdateResultCode)}", value.UpdateResultCode);
            value.Time = reader.ReadDateTime_yyMMddHHmmss();
            writer.WriteString($"[{value.Time:yyMMddHHmmss}]时间", value.Time.ToString("yyyy-MM-dd HH:mm:ss"));
            var virtualHex = reader.ReadVirtualArray(reader.ReadCurrentRemainContentLength());
            value.UpdateFile = reader.ReadRemainStringContent();
            writer.WriteString($"[{virtualHex.ToArray().ToHexString()}]升级文件", value.UpdateFile);
        }

        public override JT808_0x0B0A Deserialize(ref JT808MessagePackReader reader, IJT808Config config)
        {
            JT808_0x0B0A value = new();
            value.GprsId = reader.ReadUInt32();
            value.UpdateResultCode = reader.ReadByte();
            value.Time = reader.ReadDateTime_yyMMddHHmmss();
            value.UpdateFile = reader.ReadRemainStringContent();
            return value;
        }

        public override void Serialize(ref JT808MessagePackWriter writer, JT808_0x0B0A value, IJT808Config config)
        {
            writer.WriteUInt32(value.GprsId);
            writer.WriteByte(value.UpdateResultCode);
            writer.WriteDateTime_yyMMddHHmmss(value.Time);
            writer.WriteString(value.UpdateFile);
        }
    }
}
