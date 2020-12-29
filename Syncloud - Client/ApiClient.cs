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

        public async Task<bool> Login(string uid, string password)
        {
            var user = new Dictionary<string,string>()
            {
                { "uid", uid },
                { "password", password }
            };
            var json = JsonConvert.SerializeObject(user);

            var data = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage message = await PostAsync("login", data);
           
            Token = await message.Content.ReadAsStringAsync();
            return message.IsSuccessStatusCode;
        }

        public async Task<bool> Register(string username, string mail, string password)
        {
            var user = new Dictionary<string, string>()
            {
                {"username", username },
                {"mail" , mail },
                {"password", password }
            };
            var json = JsonConvert.SerializeObject(user);

            var data = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage message = await PostAsync("register", data);
            var response = await message.Content.ReadAsStringAsync();
            return message.IsSuccessStatusCode;


        }

    }
}
