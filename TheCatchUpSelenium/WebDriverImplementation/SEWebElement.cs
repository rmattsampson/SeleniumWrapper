using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCatchUpSelenium.WebDriverImplementation
{
    /// <summary>
    /// Implementation of the "HUDL" WebElement interface
    /// </summary>
    public class SEWebElement : IHUDLWebElement
    {
        private IWebElement webElement;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="webElt"></param>
        public SEWebElement(IWebElement webElt)
        {
            this.webElement = webElt;
        }
        
        /// <summary>
        /// Click the element, and optionally wait after clicking
        /// </summary>
        public void Click(bool waitForLoad=true)
        {
            webElement.Click();
            if (waitForLoad)
            {
                //Sometimes things just take a second (or half a second) to load
                System.Threading.Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// File upload
        /// </summary>
        public void Upload(string filePath)
        {
            this.webElement.SendKeys(filePath);
            System.Threading.Thread.Sleep(1000);
        }

        /// <summary>
        /// Should work for basic Set Value operations
        /// </summary>
        public void SetValue(string text)
        {
            this.webElement.SendKeys(text);            
        }

        public string GetValue()
        {
            return this.webElement.Text;
        }
    }
}
