using System;
using System.Collections.Generic;
using System.Text;

namespace Syncloud.Model
{
    public struct Settings
    {
        public Settings(Languages Language)
        {
            this.Language = Language;
        }

        public Languages Language { get; set; }

        public enum Languages
        {
            English,
            Français
        }
    }
}
