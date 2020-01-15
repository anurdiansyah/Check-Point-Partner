using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD.Lib
{
    public class RDToken
    {
        public string RefId { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiredDate { get; set; } = new DateTime(1900, 1, 1);

    }
}
