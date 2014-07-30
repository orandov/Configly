using System.ComponentModel;

namespace TestClientNuget
{
    public class TestSettings
    {
        [DefaultValue("Default Value of First Property")]
        public string FirstProperty { get; set; }

        [DisplayName("Some int property"), DefaultValue(99)]
        public int SecondProperty { get; set; }

        [DefaultValue(30)]
        public int Timeout { get; set; }

        public bool IsLive { get; set; }

        [DisplayName("FX Webservice")]
        public string FXUrl { get; set; }
    }
}
