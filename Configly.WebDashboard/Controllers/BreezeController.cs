using System.Linq;
using System.Web.Http;
using Breeze.ContextProvider;
using Breeze.WebApi2;
using Configly.WebDashboard.Models;
using Newtonsoft.Json.Linq;
using Configly.Entities;

namespace Configly.WebDashboard.Controllers
{
    [BreezeController]
    public class BreezeController : ApiController
    {

        private readonly SettingsContextProvider
           _contextProvider = new SettingsContextProvider();

        [HttpGet]
        public string Metadata()
        {
            return _contextProvider.Metadata();
        }

        [HttpGet]
        public IQueryable<Setting> Settings()
        {
            return _contextProvider.Context.Settings;
        }

        [HttpGet]
        public IQueryable<Connection> Connections()
        {
            return _contextProvider.Context.Connections;
        }

        [HttpGet]
        public IQueryable<PropertyHistory> PropertyHistories()
        {
            return _contextProvider.Context.PropertyHistories;
        }

        [HttpPost]
        public SaveResult SaveChanges(JObject saveBundle)
        {
            return _contextProvider.SaveChanges(saveBundle);
        }


    }
}