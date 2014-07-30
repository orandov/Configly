using System.ComponentModel;

namespace Configly.Store.Tests
{
    public class TestSettings
    {
        [DefaultValue("Default Value of First Property")]
        public string FirstProperty { get; set; }

        [DisplayName("Some int property")]
        public int SecondProperty { get; set; }

        [DefaultValue(30)]
        public int Timeout { get; set; }

    }
}
