using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tigerbox.Objects.Enums;

namespace Tigerbox.Objects
{
    public class TigerBoxMessage
    {
        public TigerMedia Media { get; set; }

        public TigerActions Action { get; set; }

        public MessageType MessageType { get; set; }
    }
}
