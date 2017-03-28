using System;
using System.Collections.Generic;
using System.Linq;
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
        private static Uri BaseUri = new Uri(@"http://ydcx.bupt.edu.cn");

        public static async Task<double> Balance(string dormId)
        {
            using (HttpClient client = new HttpClient() { BaseAddress = BaseUri })
            {
                var response = await client.GetAsync($"see.aspx?useid={dormId}");
                if (response.IsSuccessStatusCode)
                {
                    var readStreamTask = await response.Content.ReadAsStreamAsync();
                    HtmlDocument doc = new HtmlDocument();
                    doc.Load(readStreamTask);

                    //TODO handle invaild dormId

                    Regex pattern = new Regex(@"^fanbaolong='([0-9]+)'$");
                    var headNode = doc.DocumentNode.Descendants("script");
                    foreach (var node in headNode)
                    {
                        var match = pattern.Match(node.InnerText);
                        if (match.Success)
                        {
                            return double.Parse(match.Groups[1].Value);
                        }
                    }
                    throw new ArgumentException();
                }
                else
                {
                    throw new HttpRequestException();
                }
            }
            throw new HttpRequestException();
        }
    }
}
