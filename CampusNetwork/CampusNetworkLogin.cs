using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CampusNetwork
{
    public class CampusNetworkLogin
    {
        private static readonly Uri Address = new Uri("http://gw.bupt.edu.cn/");
        public static async Task Login(string userId,string password)
        {
            string postData = PostDataBuilder(userId, password);
            using (HttpClient client = new HttpClient() {BaseAddress = Address})
            {
                await client.PostAsync("", new StringContent(postData, Encoding.UTF8, "text/html"));
            }
        }

        private static string PostDataBuilder(string userId, string password)
        {
            return $"DDDDD={userId}&upass={password}&0MKKey=";
        }
    }
}
