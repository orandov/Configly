namespace Configly.Entities.Interfaces
{
    public interface ISettingsService
    {
        GetSettingsResponse GetAndUpdate(GetSettingsRequest request);

        //Setting Set(SetSettingsRequest request);
    }
}
