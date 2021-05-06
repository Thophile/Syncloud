using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Syncloud
{
    public class ApiClient : HttpClient
    {
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

            Controller.Instance.UpdateToken(await message.Content.ReadAsStringAsync());

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
            return message.IsSuccessStatusCode;

        }

        public async Task<bool> GetList()
        {
            
            using (var request = new HttpRequestMessage(HttpMethod.Get, this.BaseAddress + "list"))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Controller.Instance.AppSettings.Token);
                HttpResponseMessage message = await SendAsync(request);
                var ar = JsonConvert.DeserializeObject<List<string>>(await message.Content.ReadAsStringAsync());
                foreach(string name in ar)
                {
                    try
                    {
                        if (!Controller.Instance.SyncFolders.Any(p => p.Name == name))
                        {
                            Controller.Instance.SyncFolders.Add(new Model.SyncFolder(name));
                        }
                    }catch(JsonSerializationException e)
                    {
                        System.Windows.MessageBox.Show(e.Message);
                    }
                }

                Controller.Instance.UpdateSyncFolders();

                if (message.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    Controller.Instance.UpdateToken("");
                }
                return message.IsSuccessStatusCode || message.StatusCode == System.Net.HttpStatusCode.Forbidden;
            }
        }
        
        public async Task<bool> SendFolder(Model.SyncFolder folder)
        {
            var fileName = folder.Name + ".zip";
            var tmp = Path.GetTempPath() + "\\" + fileName;
            if(File.Exists(tmp))File.Delete(tmp);

            ZipFile.CreateFromDirectory(folder.Path, tmp);


            MultipartFormDataContent multiPartContent = new MultipartFormDataContent("--boundary");
            multiPartContent.Headers.ContentType.MediaType = "multipart/form-data";
            Stream fileStream = File.OpenRead(tmp);
            multiPartContent.Add(new StreamContent(fileStream), "file", fileName);

            using (var request = new HttpRequestMessage(HttpMethod.Post, this.BaseAddress + "folder"))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Controller.Instance.AppSettings.Token);
                request.Content = multiPartContent;
                HttpResponseMessage message = await SendAsync(request);

                System.Windows.MessageBox.Show(await message.Content.ReadAsStringAsync());

                if (message.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    Controller.Instance.UpdateToken("");
                }
                return message.IsSuccessStatusCode || message.StatusCode == System.Net.HttpStatusCode.Forbidden;
            }
        }
        
        public async Task<bool> GetFolder(Model.SyncFolder folder)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, this.BaseAddress + "folder?name=" + folder.Name))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Controller.Instance.AppSettings.Token);
                HttpResponseMessage message = await SendAsync(request);
                if (message.StatusCode == System.Net.HttpStatusCode.InternalServerError) { return false; }
                //MessageBox.Show(await message.Content.ReadAsStringAsync());
                var tmpPath = Path.GetTempPath() + folder.Name + ".zip";
                using (var fs = new FileStream(tmpPath, FileMode.Create))
                {
                    await message.Content.CopyToAsync(fs);
                }
                    using (ZipArchive zip = ZipFile.OpenRead(tmpPath))
                    {  
                        foreach (ZipArchiveEntry e in zip.Entries)
                        {
                            var name = e.FullName.Split('\\')[e.Name.Split('\\').Length - 1];
                            var prefix = String.Join('\\', e.FullName.Split('\\').SkipLast(1));
                            var path = folder.Path + "\\" + prefix + (prefix == "" ? "" :"\\");
                            System.Windows.MessageBox.Show("Path : "+path+"\n Name : " + name);
                            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                            e.ExtractToFile(path+name, true);
                        }
                    }


                if (message.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    Controller.Instance.UpdateToken("");
                }
                return message.IsSuccessStatusCode || message.StatusCode == System.Net.HttpStatusCode.Forbidden;
            }
        }

        public async Task<bool> DeleteFolder(Model.SyncFolder folder)
        {
            
            using (var request = new HttpRequestMessage(HttpMethod.Post, this.BaseAddress + "delete_folder"))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Controller.Instance.AppSettings.Token);
                string json = JsonConvert.SerializeObject(
                    new Dictionary<string, string>()
                    { { "name", folder.Name } }
                    );
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage message = await SendAsync(request);

                if (message.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    Controller.Instance.UpdateToken("");
                }
                return message.IsSuccessStatusCode || message.StatusCode == System.Net.HttpStatusCode.Forbidden;
            }

        }
    }
}
