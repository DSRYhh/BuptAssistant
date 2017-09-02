using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Xamarin.Forms;

namespace ElectricityBill
{

    public class ElectricityBill : ContentPage
    {
        private static Uri queryUri = new Uri(@"https://webapp.bupt.edu.cn/w_dianfei.html");
        private static Uri baseUri = new Uri(@"https://webapp.bupt.edu.cn");

        public static async Task<double> Balance(string dormId, string userName, string password)
        {
            var cookies = await LogIn(userName, password);
            
            return 0;
        }

        private static async Task<CookieCollection> LogIn(string userName, string password)
        {
            var postData = $"username={userName}&password={password}";
            var data = new UTF8Encoding().GetBytes(postData);

            var handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                CookieContainer = new CookieContainer()
            };

            using (var client = new HttpClient(handler) {BaseAddress = baseUri})
            {
                var response = await client.PostAsync(@"/wap/login/commit.html",
                    new StringContent(postData, Encoding.UTF8, "application/x-www-form-urlencoded"));
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return handler.CookieContainer.GetCookies(baseUri);
                }
                throw new ArgumentException();
            }
        }

    }
}
