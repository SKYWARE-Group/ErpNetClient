using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Skyware.ErpNetFS.Model
{

    public enum StatusMessageType
    {
        [EnumMember(Value = "")]
        Unknown,
        [EnumMember(Value = "reserved")]
        Reserved,
        [EnumMember(Value = "info")]
        Info,
        [EnumMember(Value = "warning")]
        Warning,
        [EnumMember(Value = "error")]
        Error
    }

    public class StatusMessage
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public StatusMessageType Type { get; set; } = StatusMessageType.Unknown;

        /* Error and Warning Codes are strings with length of 5 characters.
        First 3 characters are the type of the error, i.e., ERR, WRN.
        Next 2 characters are digits, representing the ID of the error or warning. */
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Code { get; set; }
        public string Text { get; set; } = string.Empty;
    }

    //public class DeviceStatus
    //{
    //    public bool Ok { get; protected set; } = true;
    //    public IList<StatusMessage> Messages { get; protected set; } = new List<StatusMessage>();

    //    public void AddMessage(StatusMessage statusMessage)
    //    {
    //        if (statusMessage.Type == StatusMessageType.Unknown)
    //        {
    //            throw new ArgumentException("status message type cannot be unknown");
    //        }
    //        if (statusMessage.Type == StatusMessageType.Reserved)
    //        {
    //            // Ignore reserved messages
    //            return;
    //        }
    //        if (statusMessage.Type == StatusMessageType.Error)
    //        {
    //            Ok = false;
    //        }
    //        if (string.IsNullOrEmpty(statusMessage.Code))
    //        {
    //            statusMessage.Code = null;
    //        }
    //        Messages.Add(statusMessage);
    //    }

    //    public void AddInfo(string text)
    //    {
    //        AddMessage(new StatusMessage
    //        {
    //            Type = StatusMessageType.Info,
    //            Text = text
    //        });
    //    }

    //    public void AddError(string code, string text)
    //    {
    //        AddMessage(new StatusMessage
    //        {
    //            Type = StatusMessageType.Error,
    //            Code = code,
    //            Text = text
    //        });
    //    }

    //    public void AddWarning(string code, string text)
    //    {
    //        AddMessage(new StatusMessage
    //        {
    //            Type = StatusMessageType.Warning,
    //            Code = code,
    //            Text = text
    //        });
    //    }
    //}

    public class DeviceStatusWithDateTime
    {

        public bool Ok { get; protected set; } = true;
        public IList<StatusMessage> Messages { get; protected set; } = new List<StatusMessage>();

        public DateTime DeviceDateTime { get; set; }

    }
    public class DeviceStatusWithRawResponse 
    {

        public bool Ok { get; protected set; } = true;
        public IList<StatusMessage> Messages { get; protected set; } = new List<StatusMessage>();

        public string RawResponse { get; set; } = string.Empty;

    }

    public class DeviceStatusWithCashAmount 
    {
        public bool Ok { get; protected set; } = true;
        public IList<StatusMessage> Messages { get; protected set; } = new List<StatusMessage>();

        public decimal Amount { get; set; }

    }

    public class DeviceStatusWithReceiptInfo 
    {
        public bool Ok { get; set; } = false;

        public IList<StatusMessage> Messages { get; set; } = new List<StatusMessage>();

        /// <summary>
        /// The receipt number.
        /// </summary>
        public string ReceiptNumber { get; set; } = string.Empty;

        /// <summary>
        /// The receipt date and time.
        /// </summary>
        public DateTime ReceiptDateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// The receipt amount.
        /// </summary>
        public decimal ReceiptAmount { get; set; } = 0m;

        /// <summary>
        /// The fiscal memory number.
        /// </summary>
        public string FiscalMemorySerialNumber { get; set; } = string.Empty;


    }

}
