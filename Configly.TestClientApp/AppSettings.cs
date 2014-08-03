using System.ComponentModel;

namespace Configly.TestClientApp
{
    public class AppSettings
    {
        [DisplayName("Application Version"), DefaultValue(6.9)]
        public double ApplicationVersion { get; set; }

        [DisplayName("EQ Web Service Integration Url"), DefaultValue("http://www.google.com/eq/SDCorp/SDcorp.asmx")]
        public string EQServiceUrl { get; set; }

        [DisplayName("Url for common static web resources"), DefaultValue("http://localhost/CommonUI/")]
        public string CommonUIPartialPath { get; set; }

        [DisplayName("FX Portfolio Url"), DefaultValue("www.bing.com/fx/portfolio.asp")]
        public string FXPortfolioUrl { get; set; }
    }
}
