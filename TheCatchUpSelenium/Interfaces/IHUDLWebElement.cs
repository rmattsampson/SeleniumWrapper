using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCatchUpSelenium
{
    /// <summary>
    /// Wrapper around the IWebElement interface, shield ourselves from Selenium changes and only expose what we need
    /// </summary>
    public interface IHUDLWebElement
    {
        /// <summary>
        /// Click on a web element
        /// </summary>
        void Click(bool waitForLoad=true);

        /// <summary>
        /// Set an web elements value (e.g. a text box)
        /// </summary>
        void SetValue(string o);

        /// <summary>
        /// Get a web elements value (e.g. a text box)
        /// </summary>
        string GetValue();

        /// <summary>
        /// Upload a file, special operation only supported on "File" input controls
        /// </summary>
        void Upload(string filePath);
    }
}
