using Newtonsoft.Json;
using System;
using System.IO;
using Syncloud.Model;
using System.Collections.ObjectModel;

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
            AppSettings = File.Exists(_settingsFile) ? JsonConvert.DeserializeObject<Settings>(File.ReadAllText(_settingsFile)) : new Settings(Settings.Languages.English);
            
        }

        /* == Private members == */
        private string _settingsFile;
        private string _syncFoldersFile;
        private static Controller _instance = null;
        private static readonly object _lock = new object();

        /* == Properties = */
        public ObservableCollection<SyncFolder> SyncFolders { get; set; }
        public Settings AppSettings { get; set; }
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
    }
}
