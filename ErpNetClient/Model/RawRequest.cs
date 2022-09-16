using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Skyware.ErpNetFS.Model
{

    /// <summary>
    /// Represent raw request (comand+data) to be sent to the fiscal printer
    /// </summary>
    public class RawRequest
    {

        [JsonPropertyName("rawRequest")]
        public string Request { get; set; }

    }
}
