using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace CampusNetwork
{
    public class CampusNetworkLogin
    {

        private static readonly Uri Address = new Uri("http://gw.bupt.edu.cn/");
        public static async Task<bool> Login(string userId,string password)
        {
            try
            {
                string postData = PostDataBuilder(userId, password);
                using (HttpClient client = new HttpClient() { BaseAddress = Address })
                {
                    var response = await client.PostAsync("", new StringContent(postData, Encoding.UTF8, "text/html"));

                    var content = response.Content;
                    var stream = await content.ReadAsStreamAsync();
                    HtmlAgilityPack.HtmlDocument doc = new HtmlDocument();
                    doc.Load(stream,Encoding.UTF8);

                    if (doc.DocumentNode.InnerText.Contains("You have successfully logged into our system."))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        private static string PostDataBuilder(string userId, string password)
        {
            return $"DDDDD={userId}&upass={password}&0MKKey=";
        }
    }
}
