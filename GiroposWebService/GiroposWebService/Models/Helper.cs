using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiroposWebService.Models
{
    public class Helper
    {
        public static string PrintObject(Object obj)
        {


            return JsonConvert.SerializeObject(obj);


        }
    }
}