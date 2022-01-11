using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GiroposWebService.Models
{
    public class PaymentResult
    {
        public string respon_code { get; set; }
        public string respon_mess { get; set; }
        public string nomor_va { get; set; }
        public string kode_inst { get; set; }
        public string channel_id { get; set; }
        public decimal nominal { get; set; }
        public decimal admin { get; set; }
        public string refnumber { get; set; }
        public string waktu_proses { get; set; }
        public string nopen { get; set; }

        public static PaymentResult GetResult(PaymentParam x)
        {
            PaymentResult res = new PaymentResult();

            string jsonPost = JsonConvert.SerializeObject(x);

            try
            {
                string connString = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                using (SqlCommand cmd = new SqlCommand("[dbo].[sp_MNCL_PaymentVA_GiroPOS]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // set up the parameters
                    cmd.Parameters.Add("@VA_NUMB", SqlDbType.VarChar, 50);
                    cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal, 2000);
                    cmd.Parameters.Add("@ADMINFEE", SqlDbType.Decimal, 500);
                    cmd.Parameters.Add("@REF_NUMB", SqlDbType.VarChar, 50);
                    cmd.Parameters.Add("@WAKTU_PROSES", SqlDbType.VarChar, 50);

                    // set parameter values
                    cmd.Parameters["@VA_NUMB"].Value = x.nomor_va;
                    cmd.Parameters["@AMOUNT"].Value = x.nominal;
                    cmd.Parameters["@ADMINFEE"].Value = x.admin;
                    cmd.Parameters["@REF_NUMB"].Value = x.refnumber;
                    cmd.Parameters["@WAKTU_PROSES"].Value = x.waktu_proses;

                    // open connection and execute stored procedure
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    // read output value
                    res.respon_code = "00";
                    res.respon_mess = "OK";
                    res.nomor_va = x.nomor_va;
                    res.kode_inst = "700";
                    res.channel_id = "9001";
                    res.nominal = x.nominal;
                    res.admin = x.admin;
                    res.refnumber = x.refnumber;
                    res.waktu_proses = x.waktu_proses;
                    res.nopen = "50000";

                    conn.Close();
                }

                string jsonRes = JsonConvert.SerializeObject(res);
            }
            catch (SqlException ex)
            {
                res.respon_code = "9614";
                res.respon_mess = "VA Number " + x.nomor_va + " does not exist";
            }

            return res;
        }
    }
}