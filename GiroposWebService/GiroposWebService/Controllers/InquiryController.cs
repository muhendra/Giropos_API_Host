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
    public class InquiryController : ApiController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        [HttpPost]
        public HttpResponseMessage Post([FromBody] InquiryParam values)
        {
            log.Info("REQUEST INQUIRY PARAMETER | " + Helper.PrintObject(values));
            InquiryParam res = values;

            string merchantId = ConfigurationManager.AppSettings["MerchantId"];
            string merchantKey = ConfigurationManager.AppSettings["MerchantKey"];

            //IF VALIDASI LOGIN/SECRET KEY
            InquiryResult r = InquiryResult.GetResult(res);
            log.Info("RESULT INQUIRY PARAMETER | " + Helper.PrintObject(r));
            return Request.CreateResponse(HttpStatusCode.OK, r);
        }

        public string Get()
        {
            return "Welcome To Web API";
        }
    }
}
