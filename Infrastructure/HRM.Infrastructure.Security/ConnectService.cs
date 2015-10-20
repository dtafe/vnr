using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Infrastructure.Security
{
    public static class ConnectService
    {
        /// <summary>
        /// Tạo 1 weblient để gọi service
        /// </summary>
        /// <returns></returns>
        public static WebClient CreateWebClient()
        {
            var webClient = new WebClient();
            const string creds = "jbob" + ":" + "jbob12345";
            var bcreds = Encoding.ASCII.GetBytes(creds);
            var base64Creds = Convert.ToBase64String(bcreds);
            webClient.Headers.Add("Authorization", "Basic " + base64Creds);
            return webClient;
        }
        public static void SetLicense()
        {
            Aspose.Cells.License license = new Aspose.Cells.License();
            license.SetLicense("Aspose.Cells.lic");
        }
    }
}
