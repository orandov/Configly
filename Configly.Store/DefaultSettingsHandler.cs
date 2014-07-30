using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Configly.Entities;


namespace Configly.Store
{
    public class DefaultSettingsHandler<T> where T : class, new()
    {
        private Type _type;
        public DefaultSettingsHandler()
        {
            _type = typeof(T);
        }

        public List<Property> CreateDefaultSettingProperties()
        {
            var settings = new List<Property>();

            AddPropertiesWithDefaultValues(settings);

            return settings;
        }

        private void AddPropertiesWithDefaultValues(List<Property> settings)
        {
            List<PropertyInfo> properties = _type.GetProperties().ToList();

            foreach (PropertyInfo property in properties)
            {

                string defaultValue = GetPropertyDefaultValue(property);
                string description = GetPropertyDescription(property);
                string type = property.PropertyType.ToString();

                settings.Add(new Property
                {
                    Key = property.Name,
                    Value = defaultValue,
                    Description = description,
                    Type = type
                });
            }
        }

        private static string GetPropertyDescription(PropertyInfo property)
        {
            Type displayType = typeof(DisplayNameAttribute);
            string description = string.Empty;

            var displayAttribute = property.GetCustomAttributes(displayType, true).FirstOrDefault();

            if (displayAttribute as DisplayNameAttribute != null)
            {
                description = (displayAttribute as DisplayNameAttribute).DisplayName;
            }
            return description;
        }

        private static string GetPropertyDefaultValue(PropertyInfo property)
        {
            Type defaultValueType = typeof(DefaultValueAttribute);
            string defaultValue = default(string);
            var defaultAttribute = property.GetCustomAttributes(defaultValueType, true).FirstOrDefault();

            if (defaultAttribute as DefaultValueAttribute != null)
            {
                var value = (defaultAttribute as DefaultValueAttribute).Value;

                if (value == null)
                {
                    defaultValue = null;
                }
                else
                {
                    defaultValue = value.ToString();
                }
            }
            else
            {
                string d = property.PropertyType.IsValueType ? Activator.CreateInstance(property.PropertyType).ToString() : null;

                defaultValue = d;
            }
            return defaultValue;
        }

    }
}
