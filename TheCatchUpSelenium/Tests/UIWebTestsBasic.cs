using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheCatchUpSelenium.WebDriverImplementation;
using OpenQA.Selenium;

namespace TheCatchUpSelenium.Tests
{
    /// <summary>
    /// Test the Roster Page for Hudl
    /// </summary>
    [TestClass]
    public partial class UIWebTest
    {
        // Our "wrapped" Web Driver
        public static IHUDLWebDriver hudlDriver = null;

        /// <summary>
        /// Clean up after ourselves here - shut down the browser
        /// </summary>
        [ClassCleanup()]
        static public void ClassCleanup()
        {
            //Shut down the browser and any drivers
            hudlDriver.Close();
        }

        /// <summary>
        /// Initialize our Test run:
        /// -launch the browser (I use Chrome, it by far works the best)
        /// -Login to the hudl site
        /// -Navigate to our Roster site
        /// -"Clean up" any existing records
        /// </summary>
        /// <param name="tc"></param>
        [ClassInitialize()]
        static public void ClassInit(TestContext tc)
        {
            hudlDriver = SEWebDriver.Launch(BrowserType.Chrome);
            LoginToSite();
            NavigateToRosterPage();
            CleanupRoster(); //sometimes people leave garbage around in this site so we are going to clean it up before we start
        }

        /// <summary>
        /// Cleanup the list of remaining players
        /// </summary>
        [TestCleanup()]
        public void TestCleanup()
        {
            //Find each player and blow the thing away forever
            CleanupRoster();
        }
        
        /// <summary>
        /// Simple check to just make sure we can navigate to the right page
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void BasicRosterCheck()
        {
            string pageUrl = hudlDriver.GetPageUrl();
            Assert.IsTrue(pageUrl.ToLower().Contains(Constants.RosterUrl), "URL check failed with: " + pageUrl);
            string pageTitle = hudlDriver.GetPageTitle();
            Assert.IsTrue(pageTitle.Equals(@"Roster - Manage - Men's Varsity Football - Hudl"), "Page Title check failed with: " + pageTitle);
        }

        /// <summary>
        /// Create a "basic" player entry, verify its creation
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void BasicAtheleteAdd()
        {
            const string FirstName = "Martin";
            const string LastName = "Drake";
            const string JerseyNum = "23";
            const string Email = "makonnen@drake.com";

            BasicPersonCreate(FirstName, LastName, JerseyNum, Email);
       
            string firstName = hudlDriver.GetWebElement(FindBy.Selector, Constants.Selectors.Athelete.first_name).GetValue();
            string lastName = hudlDriver.GetWebElement(FindBy.Selector, Constants.Selectors.Athelete.last_name).GetValue();
            string email = hudlDriver.GetWebElement(FindBy.Selector, Constants.Selectors.Athelete.email).GetValue();
            Assert.IsTrue(firstName.Equals(FirstName));
            Assert.IsTrue(lastName.Equals(LastName));
            Assert.IsTrue(email.Equals(Email));
        }

        /// <summary>
        /// Verify an athelete can be disabled
        /// Add a basic athelete
        /// Make sure we can "disable" him/her
        /// Basic verification of the disabled status
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void BasicAtheleteDisable()
        {
            //Should be covered by TestCleanup Scenarios, and basically therefore every test that "adds" a player
            const string FirstName = "Martin";
            const string LastName = "Drake";
            const string JerseyNum = "23";
            const string Email = "makonnen@drake.com";
            this.BasicPersonCreate(FirstName, LastName, JerseyNum, Email);
            hudlDriver.GetWebElement(FindBy.Selector, Constants.Selectors.Athelete.last_name).Click();
            hudlDriver.GetWebElement(FindBy.Selector, "a.toggle_disabled_link").Click();
            string value = hudlDriver.GetWebElement(FindBy.Selector, "a.toggle_disabled_link").GetValue();
            Assert.IsTrue(value.Equals("Enable"));
        }

        [TestMethod]
        [Ignore]
        [TestCategory("Manual")]
        [Priority(1)]
        public void BasicAtheleteDelete()
        {
          //Delete operations are implicity tested by the test cleanup method that does deletion after every "add" a record test that runs
        }

        /// <summary>
        /// Basic verification of the Edit functionality of an existing record
        /// Add a simple record, save it, then "edit" the phone number field and save it
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void BasicAtheleteEdit()
        {
            const string FirstName = "Martin";
            const string LastName = "Drake";
            const string JerseyNum = "23";
            const string Email = "makonnen@drake.com";
            this.BasicPersonCreate(FirstName, LastName, JerseyNum, Email);
            hudlDriver.GetWebElement(FindBy.Selector, Constants.Selectors.Athelete.last_name).Click();
            hudlDriver.GetWebElement(FindBy.Selector, ".edit_player_link").Click();
            hudlDriver.GetWebElement(FindBy.Id, "edit_cell_number").SetValue("111-333-4444");
            hudlDriver.GetWebElement(FindBy.Id, "edit_save_changes").Click();
            //TODO: Would love to verify at this point that the cell number was updated, but it appears the cell number values are "hidden" input values
        }

        #region
        /// <summary>
        /// This "roster" gets filthy polluted because it seems other people? are adding their own data to it
        /// I don't make an assumptions about quality of the data coming in and I just blow away the roster everytime and start from scratch
        /// </summary>
        private static void CleanupRoster()
        {
            foreach (IHUDLWebElement element in hudlDriver.GetWebElements(FindBy.Selector, Constants.Selectors.Athelete.last_name))
            {
                //Set focus to the player
                element.Click();
                //Trashcan icon appears, click it
                hudlDriver.GetWebElement(FindBy.Selector, "img[src='http://sc.hudl.com/images/trash.gif'").Click();
                //Set focus to the correct dialog
                try { hudlDriver.GetWebElement(FindBy.Id, "ban_dialog").Click(); }
                catch (InvalidOperationException)
                {
                    //If we couldn't click the dialog, then maybe we are alright anyway
                }
                //Click the YES button now
                hudlDriver.GetWebElement(FindBy.Id, "delete_from_team").Click();
            }
        }

        /// <summary>
        /// Create a simple person, used by several methods
        /// </summary>
        private void BasicPersonCreate(string FirstName, string LastName, string JerseyNum, string Email)
        {
            hudlDriver.GetWebElement(FindBy.Id, Constants.IDs.Athelete.first_name).SetValue(FirstName);
            hudlDriver.GetWebElement(FindBy.Id, Constants.IDs.Athelete.last_name).SetValue(LastName);

            hudlDriver.GetWebElement(FindBy.Id, Constants.IDs.Athelete.jersey).SetValue(JerseyNum);
            //Just default to the first "2*" year here
            hudlDriver.GetWebElement(FindBy.Id, Constants.IDs.Athelete.classDropDown).SetValue("2"); 

            hudlDriver.GetWebElement(FindBy.Id, Constants.IDs.Athelete.position).Click();
            hudlDriver.GetWebElement(FindBy.Id, Constants.IDs.Athelete.positionFB).SetValue(Keys.Space);
            hudlDriver.GetWebElement(FindBy.Id, Constants.IDs.Athelete.positionSB).SetValue(Keys.Space);
            hudlDriver.GetWebElement(FindBy.Id, Constants.IDs.Athelete.position).Click();

            hudlDriver.GetWebElement(FindBy.Id, Constants.IDs.Athelete.email).SetValue(Email);

            hudlDriver.GetWebElement(FindBy.Id, Constants.IDs.Athelete.add_player_link).Click();
        }       

        /// <summary>
        /// Login to the site
        /// </summary>
        private static void LoginToSite()
        {
            hudlDriver.Navigate(Constants.LoginUrl);
            IHUDLWebElement webElt = hudlDriver.GetWebElement(FindBy.Id, "email");
            webElt.SetValue(Constants.Login);
            webElt = hudlDriver.GetWebElement(FindBy.Id, "password");
            webElt.SetValue(Constants.PW);
            webElt = hudlDriver.GetWebElement(FindBy.Id, "logIn");
            webElt.Click();
        }

        /// <summary>
        /// Navigate to our Roster Page
        /// </summary>
        private static void NavigateToRosterPage()
        {
            hudlDriver.Navigate(Constants.RosterUrl);
        }
        #endregion
    }
}

