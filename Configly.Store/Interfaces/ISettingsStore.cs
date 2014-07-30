using System;
using Configly.Store.Entities;

namespace Configly.Store.Interfaces
{
    public interface ISettingsStore<T>
    {
        T Get();
        event EventHandler<SettingsRefreshArgs> OnRefresh;
    }
}
