using Newtonsoft.Json;
using System;
using System.IO;

namespace SynCloud
{
    class Controller
    {
        private Controller() {
            //Appdata folder path
            string appData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SynCloud");
            Directory.CreateDirectory(appData);

            /* ==== Settings initialisation ==== */

            _settingsFile = Path.Combine(appData, "settings.json");

            if (File.Exists(_settingsFile))
            {
                AppSettings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(_settingsFile));
            }
            else
            {
                AppSettings = new Settings(Settings.Languages.English);
            }
        }
        /* == Private members == */
        private string _settingsFile;
        private static Controller _instance = null;
        private static readonly object _lock = new object();

        /* == Properties = */
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
            File.WriteAllText(this._settingsFile, settingsJSON);

        }
    }
}
