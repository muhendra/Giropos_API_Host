using GiroposWebService.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GiroposWebService.Controllers
{
    public class PaymentController : ApiController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpPost]
        public HttpResponseMessage Post([FromBody] PaymentParam values)
        {
            log.Info("REQUEST PAYMENT PARAMETER | " + Helper.PrintObject(values));
            PaymentParam res = values;
            
            string merchantKey = ConfigurationManager.AppSettings["MerchantKey"];

            //IF VALIDASI LOGIN/SECRET KEY
            if (Hash.Authentication(res.hashing, res.nomor_va + merchantKey))
            {
                PaymentResult r = PaymentResult.GetResult(res);
                log.Info("RESULT PAYMENT PARAMETER | " + Helper.PrintObject(r));
                return Request.CreateResponse(HttpStatusCode.OK, r);
            }

            else
            {
                PaymentResult pay = new PaymentResult();
                pay.respon_code = "9613";
                pay.respon_mess = "Invalid Token";
                log.Info("RESULT PAYMENT PARAMETER | " + Helper.PrintObject(pay));

                return Request.CreateResponse(HttpStatusCode.Forbidden, pay);
            }
        }

        public string Get()
        {
            return "Welcome To Web API";
        }
    }
}