using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AbsenSupir.Api.Data
{
    public class Enumeration
    {
        public enum PaidStatus
        {
            NeedConfirmation = 0,
            Paid = 1,
            UnPaid = 2,
        }
    }
}
