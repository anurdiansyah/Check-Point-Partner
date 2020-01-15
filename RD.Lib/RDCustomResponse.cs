using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RD.Lib
{
    public partial class RDCustomResponse
    {
        public string ReferenceNumber { get; set; }
        public bool IsSuccess { get; set; }
        public Int32 StatusCode { get; set; }
        public string Message { get; set; }
        public RDDatatable DataTable { get; set; }
        public string JsonData { get; set; }

        public RDCustomResponse()
        {
            ReferenceNumber = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            IsSuccess = false;
            StatusCode = 500;
            Message = "Null Response";
            JsonData = string.Empty;
        }

    }
}
