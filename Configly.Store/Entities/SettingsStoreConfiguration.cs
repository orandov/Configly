namespace Configly.Store.Entities
{
    public class SettingsStoreConfiguration
    {
        public SettingsStoreConfiguration() { }

        public string Scope { get; set; }
        public string ServerUrl { get; set; }


        public override string ToString()
        {
            return string.Format("Scope: {0}, ServerUrl: {1}", Scope, ServerUrl);
        }
    }
}
