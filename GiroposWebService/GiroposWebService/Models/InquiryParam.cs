using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiroposWebService.Models
{
    public class InquiryParam
    {
        public string nomor_va { get; set; }
        public string kode_inst { get; set; }
        public string channel_id { get; set; }
        public string waktu_proses { get; set; }
    }
}