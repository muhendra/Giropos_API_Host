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
    public class InquiryResult
    {
        public string respon_code;
        public string respon_mess;
        public string nomor_va;
        public decimal nominal;
        public decimal admin;
        public string kode_inst;
        public string channel_id;
        public string nama;
        public string info;
        public string rekgiro;
        public string waktu_proses;

        public static InquiryResult GetResult(InquiryParam x)
        {
            InquiryResult res = new InquiryResult();

            string jsonPost = JsonConvert.SerializeObject(x);

            try
            {
                string connString = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                using (SqlCommand cmd = new SqlCommand("[dbo].[sp_MNCL_InquiryVA_GiroPOS]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // set up the parameters
                    cmd.Parameters.Add("@VA_NUMB", SqlDbType.VarChar, 50);
                    cmd.Parameters.Add("@NOMINAL", SqlDbType.Decimal, 2000).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@ADMINFEE", SqlDbType.Decimal, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@NAMA", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@REKGIRO", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;

                    // set parameter values
                    cmd.Parameters["@VA_NUMB"].Value = x.nomor_va;

                    // open connection and execute stored procedure
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    // read output value
                    res.respon_code = "00";
                    res.respon_mess = "OK";
                    res.info = "";
                    res.kode_inst = "700";
                    res.channel_id = "9001";
                    res.waktu_proses = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    res.nomor_va = x.nomor_va;
                    res.nominal = Convert.ToDecimal(cmd.Parameters["@NOMINAL"].Value);
                    res.admin = Convert.ToDecimal(cmd.Parameters["@ADMINFEE"].Value);
                    res.nama = (cmd.Parameters["@NAMA"].Value).ToString();
                    res.rekgiro = (cmd.Parameters["@REKGIRO"].Value).ToString();
                    
                    conn.Close();
                }

                string jsonRes = JsonConvert.SerializeObject(res);
            }
            catch(SqlException ex)
            {

            }

            return res;
        }
    }
}