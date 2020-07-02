using System;
using System.Collections.Generic;
using System.Text;

namespace Dsevents
{
    class Event
    {
        public string ID { get; set; }
        public int Seq { get; set; }
        public string ProcessID { get; set; }
        public string ChannelID { get; set; }
        public string MessageID { get; set; }
    }
}
