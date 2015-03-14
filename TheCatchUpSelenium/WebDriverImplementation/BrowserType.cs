using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCatchUpSelenium
{
    /// <summary>
    /// Valid Browser Types
    /// </summary>
    public enum BrowserType
    {
        IE, //Shockingly, IE11 doesn't work without making a reg key hack: https://code.google.com/p/selenium/wiki/InternetExplorerDriver
        Firefox,
        Chrome
        //Nothing else is supported
    }
}
