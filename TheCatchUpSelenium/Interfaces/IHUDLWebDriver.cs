using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCatchUpSelenium
{
    [Serializable]
    public enum FindBy
    {
        /// <summary>
        /// The ID of the web element
        /// </summary>
        Id,
        /// <summary>
        /// Find a link by its text
        /// </summary>
        LinkText,
        /// <summary>
        /// CSS Selector - http://www.w3.org/TR/CSS2/selector.html
        /// </summary>
        Selector,
        /// <summary>
        /// XPATH - http://msdn.microsoft.com/en-us/library/ms256086.aspx
        /// </summary>
        XPath
    }

    /// <summary>
    /// Wrapper around the IWebDriver interface, shield ourselves from Selenium changes and only expose what we need
    /// </summary>
    public interface IHUDLWebDriver
    {
        /// <summary>
        ///  Navigate to a specified url
        /// </summary>
        void Navigate(string url);

        /// <summary>
        /// Close the web client
        /// </summary>
        void Close();

        /// <summary>
        /// Find and Get a IWebElement
        /// </summary>
        IHUDLWebElement GetWebElement(FindBy findBy, string text);

        /// <summary>
        /// Find and Get a IWebElement
        /// </summary>
        IList<IHUDLWebElement> GetWebElements(FindBy findBy, string text);

        /// <summary>
        /// Get the title of the web page
        /// </summary>
        string GetPageTitle();

        /// <summary>
        /// Get the URL of the web page
        /// </summary>
        string GetPageUrl();
    
    }
}
