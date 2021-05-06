using System;
using System.Collections.Generic;
using System.Text;

namespace Syncloud.Model
{
    public struct Settings
    {
        public Settings(Languages Language , string Token = "")
        {
            this.Language = Language;
            this.Token = Token;
        }

        public Languages Language { get; set; }
        public string Token { get; set; }
        public enum Languages
        {
            English,
            Français
        }
    }
}
