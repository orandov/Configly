using System;

namespace Configly.Store.Entities
{
    public class SettingsRefreshArgs : EventArgs
    {
        public object Settings { get; set; }

        public SettingsRefreshArgs(object settings)
        {
            // TODO: Complete member initialization
            this.Settings = settings;
        }
    }
}
