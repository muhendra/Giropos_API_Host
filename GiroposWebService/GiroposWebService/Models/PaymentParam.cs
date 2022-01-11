using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiroposWebService.Models
{
    public class PaymentParam
    {
        public string nomor_va { get; set; }
        public string kode_inst { get; set; }
        public string channel_id { get; set; }
        public decimal nominal { get; set; }
        public decimal admin { get; set; }
        public string refnumber { get; set; }
        public string waktu_proses { get; set; }
        public string nopen { get; set; }
        public string hashing { get; set; }
    }
}