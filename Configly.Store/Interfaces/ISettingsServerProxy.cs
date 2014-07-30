using System;
using Configly.Entities;

namespace Configly.Store.Interfaces
{
    public interface ISettingsServerProxy 
    {
        Setting GetAndUpdate(Setting setting);
        bool IsConnectedToServer();

        event EventHandler OnReconnection;
        event EventHandler<Setting> OnRefresh;

    }
}
