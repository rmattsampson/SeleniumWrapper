using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;

namespace TheCatchUpSelenium.WebDriverImplementation
{
    /// <summary>
    /// Selenium Web Driver wrapper for HUDL and implements HUDL Web Driver interface
    /// </summary>
    public class SEWebDriver : IHUDLWebDriver
    {
        const int NUM_RETRIES = 4;

        const int RETRY_WAIT_TIME_MS = 500;
        private IWebDriver webDriver;

        /// <summary>
        /// Constructor private to prevent insantiation of the class (outside of this class)
        /// </summary>
        private SEWebDriver()
        { }

        /// <summary>
        /// Launch a Selenium web driver browser
        /// </summary>
        public static IHUDLWebDriver Launch()
        {
            //Launch IE by default
            return Launch(BrowserType.IE);
        }

        /// <summary>
        /// Launch the specified Selenium web driver browser
        /// </summary>
        public static IHUDLWebDriver Launch(BrowserType browserType)
        {
            SEWebDriver hudlDriver = new SEWebDriver();
            
            switch (browserType)
            {
                case BrowserType.IE:
                    hudlDriver.webDriver = new InternetExplorerDriver();
                    break;
                case BrowserType.Firefox:
                    hudlDriver.webDriver = new FirefoxDriver();
                    break;
                case BrowserType.Chrome:
                    hudlDriver.webDriver = new ChromeDriver();
                    break;
                default:
                    throw new InvalidOperationException(browserType + " is not supported");
            }
            hudlDriver.webDriver.Manage().Window.Maximize();
            return hudlDriver;
        }

        /// <summary>
        /// Find an element on the page
        /// </summary>
        public IHUDLWebElement GetWebElement(FindBy findBy, string text)
        {
            IWebElement element = null;
            // Sometimes the browser AJAX-heavy pages are done loading earlier
            // than they actually are.  Give it a bit of time before we fail.
            for (int i = 0; i < NUM_RETRIES; ++i)
            {
                switch (findBy)
                {
                    case FindBy.Id:
                        element = this.webDriver.FindElement(By.Id(text));
                        break;
                    case FindBy.LinkText:
                        element = this.webDriver.FindElement(By.LinkText(text));
                        break;
                    case FindBy.Selector:
                        element = this.webDriver.FindElement(By.CssSelector(text));
                        break;
                    default:
                        throw new NotImplementedException("Find by " + findBy + " is not yet implemented.");
                }
                if (element!=null)
                { break; }
                System.Threading.Thread.Sleep(RETRY_WAIT_TIME_MS);
            }
            return new SEWebElement(element);
        }

        /// <summary>
        /// Find an element on the page
        /// </summary>
        public IList<IHUDLWebElement> GetWebElements(FindBy findBy, string text)
        {
            IReadOnlyCollection<IWebElement> elements = new List<IWebElement>();
            // Sometimes the browser AJAX-heavy pages are done loading earlier
            // than they actually are.  Give it a bit of time before we fail.
            for (int i = 0; i < NUM_RETRIES; ++i)
            {
                switch (findBy)
                {
                    case FindBy.Id:
                        elements = this.webDriver.FindElements(By.Id(text));
                        break;
                    case FindBy.LinkText:
                        elements = this.webDriver.FindElements(By.LinkText(text));
                        break;
                    case FindBy.Selector:
                        elements = this.webDriver.FindElements(By.CssSelector(text));
                        break;
                    default:
                        throw new NotImplementedException("Find by " + findBy + " is not yet implemented.");
                }
                if (elements.Count() != 0)
                { break; }
                System.Threading.Thread.Sleep(RETRY_WAIT_TIME_MS);
            }

            IList<IHUDLWebElement> allWebElts = new List<IHUDLWebElement>();
            foreach (var element in elements)
            {
                allWebElts.Add(new SEWebElement(element));
            }

            return allWebElts;
        }

        /// <summary>
        /// Close the browser, delete our cookies and kill and driver processes
        /// </summary>
        public void Close()
        {
            this.webDriver.Manage().Cookies.DeleteAllCookies();
            this.webDriver.Close();
            //Quit should "kill" the browser driver process (ChromeDriver.exe)
            this.webDriver.Quit();
        }

        /// <summary>
        /// Go to a specified url
        /// </summary>
        public void Navigate(string url)
        {
            this.webDriver.Navigate().GoToUrl(url);
        }

        /// <summary>
        /// Get the url of the current page
        /// </summary>
        public string GetPageUrl()
        {
            return this.webDriver.Url;
        }

        /// <summary>
        /// Get the title of the current page
        /// </summary>
        public string GetPageTitle()
        {
            return this.webDriver.Title;
        }
    }
}
