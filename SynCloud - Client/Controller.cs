using Newtonsoft.Json;
using System;
using System.IO;
using Syncloud.Model;
using System.Collections.ObjectModel;
using System.Windows;
using Syncloud.Pages;

namespace Syncloud
{
    class Controller
    {
        private Controller() {
            /* ==== Appdata Path Declaration ==== */
            string appData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Syncloud");
            Directory.CreateDirectory(appData);

            /* ==== SyncFolders Initialisation ====*/
            _syncFoldersFile = Path.Combine(appData, "syncFolders.json");
            SyncFolders = File.Exists(_syncFoldersFile) ? JsonConvert.DeserializeObject<ObservableCollection<SyncFolder>>(File.ReadAllText(_syncFoldersFile)) : new ObservableCollection<SyncFolder>();
            

            /* ==== Settings Initialisation ==== */
            _settingsFile = Path.Combine(appData, "settings.json");
            AppSettings = File.Exists(_settingsFile) ? JsonConvert.DeserializeObject<Model.Settings>(File.ReadAllText(_settingsFile)) : new Model.Settings(Model.Settings.Languages.English);

            /* ==== ApiClient Initialisation ==== */
            ApiClient = new ApiClient("http://localhost:8000/");
        }

        /* == Private members == */
        private readonly string _settingsFile;
        private readonly string _syncFoldersFile;
        private static Controller _instance = null;
        private static readonly object _lock = new object();

        /* == Properties = */
        public ApiClient ApiClient { get; set; }
        public ObservableCollection<SyncFolder> SyncFolders { get; set; }
        public Model.Settings AppSettings { get; set; }
        public static Controller Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new Controller();
                    }
                    return _instance;
                }
            }
        }

        public void UpdateSettings()
        {

            //Update jobs list in file
            string settingsJSON = JsonConvert.SerializeObject(AppSettings, Formatting.Indented);

            //Writing to path the json data
            File.WriteAllText(_settingsFile, settingsJSON);

        }

        public void UpdateSyncFolders()
        {
            //Update jobs list in file
            string syncFoldersJSON = JsonConvert.SerializeObject(SyncFolders, Formatting.Indented);

            //Writing to path the json data
            File.WriteAllText(_syncFoldersFile, syncFoldersJSON);
        }

        public void UpdateToken(string token)
        {
            if (token == "")
            {
                Application.Current.MainWindow.Content = new Login();
            }
            var s = AppSettings;
            s.Token = token;
            AppSettings = s;
            UpdateSettings();
            
        }
    }
}
