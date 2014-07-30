namespace Configly.Entities
{
    public class SetSettingsRequest
    {
        public SetSettingsRequest()
        {

        }
        public SetSettingsRequest(Setting setting)
        {
            SettingToSave = setting;
        }
        public Setting  SettingToSave { get; set; }
    }
}
