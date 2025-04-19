using System;
using System.Collections.Generic;
using System.Text;

namespace Skyware.ErpNetFS.Model;

public class ServerVariables
{
    public string Version { get; set; } = string.Empty;
    public string ServerId { get; set; } = string.Empty;
    public bool AutoDetect { get; set; } = true;
    public int UdpBeaconPort { get; set; } = 8001;
    public string ExcludePortList { get; set; } = string.Empty;
    public int DetectionTimeout { get; set; } = 30;
}
