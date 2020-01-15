using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RD.Lib
{
    public partial class RDDatatable
    { 
        public int draw { get; set; }
        public int recordsFiltered { get; set; }
        public int recordsTotal { get; set; }
        public object data { get; set; }
    }
}
