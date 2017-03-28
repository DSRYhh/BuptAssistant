using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace EcardData
{
    public class EcardSystem
    {
        private readonly string _password;
        private readonly string _id;
        private const string Address = "http://ecard.bupt.edu.cn";
        private readonly CookieCollection _cookies = new CookieCollection();
        private string _startQueryTime;
        private string _endQueryTime;

        public DateTime StartQueryTime
        {
            set
            {
                this._startQueryTime = value.ToString(@"yyyy-MM-dd");
            }
        }

        public DateTime EndQueryTime
        {
            set
            {
                this._endQueryTime = value.ToString(@"yyyy-MM-dd");
            }
        }
        

        private string _viewState = string.Empty;
        private string _eventVaildation = string.Empty;
        private int _pageCount = 0;
        private bool _isFirstQuery = true;

        private readonly List<TransactionRecord> _queryResult = new List<TransactionRecord>();
        //public List<TransactionRecord> DetailQueryResult
        //{
        //    get
        //    {
        //        GetDetail();
        //        return _queryResult;
        //    }
        //}

        public EcardSystem(string id, string password, DateTime startQueryTime, DateTime endQueryTime)
        {
            this._password = password;
            this._id = id;

            this.StartQueryTime = startQueryTime;
            this.EndQueryTime = endQueryTime;
        }


        public async Task Login()
        {
            var loginPage = await InitCookieAsync();
            string loginPostData = LoginDataBuilder(loginPage);
            await GetCookieAsync(loginPostData);
        }

        public async Task<List<TransactionRecord>> GetDetail()
        {
            _viewState = string.Empty;
            _eventVaildation = string.Empty;
            _pageCount = 0;
            _isFirstQuery = true;

            await GetPageCountAsync();
            await FetchDataAsync();

            return _queryResult;
        }

        public async Task<double> GetBalance()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                CookieContainer = new CookieContainer()
            };
            handler.CookieContainer.Add(new Uri(Address),_cookies);
            using (HttpClient client = new HttpClient(handler) { BaseAddress = new Uri(Address) })
            {
                var response = await client.GetAsync(@"/User/Home.aspx");
                if (response.IsSuccessStatusCode)
                {
                    var readStreamTask = await response.Content.ReadAsStreamAsync();
                    HtmlDocument doc = new HtmlDocument();
                    doc.Load(readStreamTask, Encoding.UTF8);

                    var texts = from item in doc.DocumentNode.Descendants("div")
                                from span in item.Descendants("span")
                                select span.InnerText;
                    foreach (string innerText in texts)
                    {
                        Regex pattern = new Regex(@"主钱包余额：([0-9]*\.[0-9]+|[0-9]+)元");
                        var match = pattern.Match(innerText);
                        if (match.Success)
                        {
                            return double.Parse(match.Groups[1].Value);
                        }
                    }
                }
                else
                {
                    throw new WebException() { Source = "GetBalance" };
                }

                //HttpWebRequest request = WebRequest.Create(Address + @"/User/Home.aspx") as HttpWebRequest;
                //request.CookieContainer = new CookieContainer();
                //request.CookieContainer.Add(_cookies);
                //request.Method = "GET";
                //request.KeepAlive = true;
                //request.ContentType = @"application/x-www-form-urlencoded";//key point!
                //request.Accept = @"text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";

                //var responseTask = await request.GetResponseAsync();
                //var response = (HttpWebResponse)responseTask;
                //    HtmlDocument doc = new HtmlDocument();
                //doc.Load(response.GetResponseStream(), Encoding.UTF8);


            }

            throw new Exception("Page parsing error. Occur when getting balance.");
        }



        private async Task<HtmlDocument> InitCookieAsync()
        {

            HttpClientHandler handler = new HttpClientHandler
            {
                AllowAutoRedirect = false,
                CookieContainer = new CookieContainer()
            };

            using (HttpClient client = new HttpClient(handler) { BaseAddress = new Uri(Address) })
            {
                HttpResponseMessage response = await client.GetAsync(@"/login.aspx");
                if (response.IsSuccessStatusCode)
                {
                    var readStreamTask = await response.Content.ReadAsStreamAsync();
                    HtmlDocument doc = new HtmlDocument();
                    doc.Load(readStreamTask);

                    //cookie
                    _cookies.Add(handler.CookieContainer.GetCookies(new Uri(Address)));

                    return doc;
                }
                else
                {
                    throw new WebException() { Source = "IninCookieAsync" };
                }
            }
        }

        private async Task GetCookieAsync(string postData)
        {
            var data = new UTF8Encoding().GetBytes(postData);

            HttpClientHandler handler = new HttpClientHandler
            {
                AllowAutoRedirect = false,
                CookieContainer = new CookieContainer(),
            };
            handler.CookieContainer.Add(new Uri(Address), _cookies);

            using (HttpClient client = new HttpClient(handler) { BaseAddress = new Uri(Address) })
            {
                HttpResponseMessage response = await client.PostAsync(@"/login.aspx", new StringContent(postData, Encoding.UTF8, "application/x-www-form-urlencoded"));
                if (response.StatusCode == HttpStatusCode.Found)
                {
                    //cookie
                    _cookies.Add(handler.CookieContainer.GetCookies(new Uri(Address)));
                }
                else
                {
                    throw new AuthenticationFailedException();
                }
            }

        }

        private async Task GetPageCountAsync()
        {
            string postData = await QueryDataBuilderAsync();
            _isFirstQuery = true;

            HttpClientHandler handler = new HttpClientHandler
            {
                CookieContainer = new CookieContainer(),
                AllowAutoRedirect = false
            };
            handler.CookieContainer.Add(new Uri(Address),_cookies);
            using (HttpClient client = new HttpClient(handler) { BaseAddress = new Uri(Address) })
            {
                HttpResponseMessage response = await client.PostAsync(@"/User/ConsumeInfo.aspx", new StringContent(postData, Encoding.UTF8, "application/x-www-form-urlencoded"));
                if (response.IsSuccessStatusCode)
                {
                    var readStreamTask = await response.Content.ReadAsStreamAsync();
                    HtmlDocument doc = new HtmlDocument();
                    doc.Load(readStreamTask);

                    var aspNetPager = doc.GetElementbyId("ContentPlaceHolder1_AspNetPager1");
                    if (aspNetPager == null)
                    {
                        _pageCount = 1;
                        return;
                    }
                    string str = aspNetPager.OuterHtml;
                    const string pattern =
                        @"<a class=""aspnetpager"" href=""javascript:__doPostBack\(&#39;ctl00\$ContentPlaceHolder1\$AspNetPager1&#39;,&#39;(\d+)&#39;\)"">&gt;&gt;<\/a>";

                    var res = Regex.Match(str, pattern);

                    foreach (var entry in Regex.Match(str, pattern).Groups)
                    {
                        if (int.TryParse(entry.ToString(), out _pageCount)) break;
                    }
                }
                else
                {
                    throw new WebException() { Source = "GetPageCountAsync" };
                }
            }
        }

        private async Task FetchDataAsync()
        {
            var records = new List<TransactionRecord>();
            for (var page = 1; page <= _pageCount; page++)
            {
                records.AddRange(await ParsePageAsync(page));
            }

            this._queryResult.AddRange(records);
        }

        private async Task<IEnumerable<TransactionRecord>> ParsePageAsync(int page)
        {
            string postData = await QueryDataBuilderAsync(page);

            HttpClientHandler handler = new HttpClientHandler
            {
                CookieContainer = new CookieContainer(),
                AllowAutoRedirect = false
            };
            handler.CookieContainer.Add(new Uri(Address),_cookies);
            using (HttpClient client = new HttpClient(handler) { BaseAddress = new Uri(Address) })
            {
                var response = await client.PostAsync(@"/User/ConsumeInfo.aspx",
                    new StringContent(postData, Encoding.UTF8, "application/x-www-form-urlencoded"));
                if (response.IsSuccessStatusCode)
                {
                    var readStreamTask = await response.Content.ReadAsStreamAsync();
                    HtmlDocument doc = new HtmlDocument();
                    doc.Load(readStreamTask, Encoding.UTF8);

                    //update VIEWSTATE and EVENTVALIDATION
                    _viewState = UpperCaseUrlEncode(doc.GetElementbyId("__VIEWSTATE").Attributes["value"].Value);
                    _eventVaildation = UpperCaseUrlEncode(doc.GetElementbyId("__EVENTVALIDATION").Attributes["value"].Value);

                    var table = doc.GetElementbyId("ContentPlaceHolder1_gridView");

                    var pageTrans = FormParser(table);
                    return pageTrans;
                }
                else
                {
                    throw new HttpRequestException();
                }
            }
        }


        private string LoginDataBuilder(HtmlDocument doc)
        {
            List<KeyValuePair<string, string>> dataList = new List<KeyValuePair<string, string>>();
            string viewState = doc.GetElementbyId("__VIEWSTATE").GetAttributeValue("value", "");
            //string lastFocus = doc.GetElementbyId("__LASTFOCUS").Attributes["value"].Value;
            string lastFocus = string.Empty;
            //string eventTarget = doc.GetElementbyId("__EVENTTARGET").GetAttributeValue("value", "");
            string eventTarget = "btnLogin";
            //string eventArgument = doc.GetElementbyId("__EVENTARGUMENT").GetAttributeValue("value", "");
            string eventArgument = string.Empty;
            string viewStateGenerator = doc.GetElementbyId("__VIEWSTATEGENERATOR").GetAttributeValue("value", "");
            string eventValidation = doc.GetElementbyId("__EVENTVALIDATION").GetAttributeValue("value", "");
            string hfIsManager = "0";




            dataList.Add(new KeyValuePair<string, string>("__LASTFOCUS", lastFocus));
            dataList.Add(new KeyValuePair<string, string>("__EVENTTARGET", eventTarget));
            dataList.Add(new KeyValuePair<string, string>("__EVENTARGUMENT", eventArgument));
            //dataList.Add(new KeyValuePair<string, string>("__VIEWSTATE", HttpUtility.UrlEncode(viewState)));
            dataList.Add(new KeyValuePair<string, string>("__VIEWSTATE", Uri.EscapeDataString(viewState)));
            dataList.Add(new KeyValuePair<string, string>("__VIEWSTATEGENERATOR", viewStateGenerator));
            //dataList.Add(new KeyValuePair<string, string>("__EVENTVALIDATION", HttpUtility.UrlEncode(eventValidation)));
            dataList.Add(new KeyValuePair<string, string>("__EVENTVALIDATION", Uri.EscapeDataString(eventValidation)));
            dataList.Add(new KeyValuePair<string, string>("txtUserName", _id));
            dataList.Add(new KeyValuePair<string, string>("txtPassword", _password));
            dataList.Add(new KeyValuePair<string, string>("hfIsManager", hfIsManager));


            return PostDataBuilder(dataList);

        }

        private static string PostDataBuilder(IEnumerable<KeyValuePair<string, string>> dataList)
        {
            string postData = string.Empty;
            foreach (var entry in dataList)
            {
                string item = $"{entry.Key}={entry.Value}";
                if (postData.Equals(string.Empty))
                {
                    postData += item;
                }
                else
                {
                    postData += $"&{item}";
                }
            }
            return postData;
        }

        private static IEnumerable<TransactionRecord> FormParser(HtmlNode table)
        {

            List<TransactionRecord> ret = new List<TransactionRecord>();
            foreach (var item in table.ChildNodes) //item is a transaction record
            {
                if (item.Attributes["class"] != null)
                {
                    if (item.Attributes["class"].Value.Equals("HeaderStyle"))
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }
                var list = new List<string>();
                foreach (var entry in item.ChildNodes)
                {
                    string value = entry.InnerText;
                    //value = Regex.Replace(value, @"\s+", "");
                    value = Regex.Replace(value, @"\t|\n|\r", "");
                    value = value.Trim();
                    if (value.Equals("")) continue;
                    if (value.Contains(@"&nbsp;")) continue;
                    value = value.Replace(",", "");

                    //Console.Write(value + " ");

                    list.Add(value);
                }
                //if (item.ChildNodes.Count != 0) Console.WriteLine();
                if (list.Count == 0) continue;
                ret.Add(new TransactionRecord(list));

            }

            return ret;
        }

        private async Task<string> QueryDataBuilderAsync(int pageIndex = 0)
        {
            //TODO change this to save data traffic 
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                CookieContainer = new CookieContainer()
            };
            handler.CookieContainer.Add(new Uri(Address),_cookies);
            using (HttpClient client = new HttpClient(handler) { BaseAddress = new Uri(Address) })
            {
                var response = await client.GetAsync(@"/User/ConsumeInfo.aspx");
                if (response.IsSuccessStatusCode)
                {
                    var readStreamTask = await response.Content.ReadAsStreamAsync();
                    HtmlDocument doc = new HtmlDocument();
                    doc.Load(readStreamTask, Encoding.UTF8);

                    //initial VIEWSTATE
                    if (_viewState.Equals(string.Empty))
                    {
                        _viewState = UpperCaseUrlEncode(doc.GetElementbyId("__VIEWSTATE").Attributes["value"].Value);
                        _eventVaildation = UpperCaseUrlEncode(doc.GetElementbyId("__EVENTVALIDATION").Attributes["value"].Value);
                    }

                    string eventTarget = string.Empty;
                    string eventArgument = string.Empty;
                    if (pageIndex != 0)
                    {
                        eventTarget = "ctl00$ContentPlaceHolder1$AspNetPager1";
                        eventTarget = UpperCaseUrlEncode(eventTarget);
                        eventArgument = pageIndex.ToString();
                    }

                    var postDataList = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("__EVENTTARGET", eventTarget),
                        new KeyValuePair<string, string>("__EVENTARGUMENT", eventArgument),
                        new KeyValuePair<string, string>("__VIEWSTATE", _viewState),
                        new KeyValuePair<string, string>("__VIEWSTATEGENERATOR",
                            doc.GetElementbyId("__VIEWSTATEGENERATOR").Attributes["value"].Value),
                        new KeyValuePair<string, string>("__EVENTVALIDATION", _eventVaildation),
                        new KeyValuePair<string, string>("ctl00%24ContentPlaceHolder1%24rbtnType", "0"),
                        new KeyValuePair<string, string>("ctl00%24ContentPlaceHolder1%24txtStartDate",
                            this._startQueryTime),
                        new KeyValuePair<string, string>("ctl00%24ContentPlaceHolder1%24txtEndDate", this._endQueryTime),
                    };

                    if (_isFirstQuery)
                    {
                        postDataList.Add(new KeyValuePair<string, string>("ctl00%24ContentPlaceHolder1%24btnSearch",
                            UpperCaseUrlEncode("查  询")));
                        _isFirstQuery = false;
                    }

                    return PostDataBuilder(postDataList);
                }
            }
            throw new WebException();
        }

        private static string UpperCaseUrlEncode(string s)
        {
            //char[] temp = HttpUtility.UrlEncode(s).ToCharArray();
            char[] temp = Uri.EscapeDataString(s).ToCharArray();
            for (int i = 0; i < temp.Length - 2; i++)
            {
                if (temp[i] == '%')
                {
                    temp[i + 1] = char.ToUpper(temp[i + 1]);
                    temp[i + 2] = char.ToUpper(temp[i + 2]);
                }
            }
            return new string(temp);
        }

    }
}