using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Syncloud
{
    public class ApiClient : HttpClient
    {
        public string Token { get; set; }

        public ApiClient(string uri)
        {
            this.BaseAddress = new Uri(uri);
        }
    }
}
