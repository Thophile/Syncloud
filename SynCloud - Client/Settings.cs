using System;
using System.Collections.Generic;
using System.Text;

namespace SynCloud
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
