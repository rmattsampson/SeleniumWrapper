using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheCatchUpSelenium.WebDriverImplementation;
using OpenQA.Selenium;
using System.IO;

namespace TheCatchUpSelenium.Tests
{
    public partial class UIWebTest
    {
        /// <summary>
        /// Create an advanced Player entry, verify its creation
        /// We fill in almost all the values for a player starting with the "basic" values
        /// like name and email, then expanding the additional information options to add
        /// a bunch of extra details
        /// We do some very basic verfication of the data after the record is saved
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void AdvancedAtheleteAdd()
        {
            const string FirstName = "West";
            const string LastName = "McDre";
            const string JerseyNum = "43";
            const string Email = "remix@djkhaled.com";
            const string Phone = "212-867-5309";
            const string Carrier = "Verizon";
            const string StAddr = "2222 Nicollet Ave";
            const string City = "Minneapolis";
            const string State = "MN";
            const string Height = "6";
            const string Inches = "4";
            const string Weight = "200";
            const string PostalCode = "55344";

            hudlDriver.GetWebElement(FindBy.Id, Constants.IDs.Athelete.first_name).SetValue(FirstName);
            hudlDriver.GetWebElement(FindBy.Id, Constants.IDs.Athelete.last_name).SetValue(LastName);

            hudlDriver.GetWebElement(FindBy.Id, Constants.IDs.Athelete.jersey).SetValue(JerseyNum);

            hudlDriver.GetWebElement(FindBy.Id, Constants.IDs.Athelete.classDropDown).SetValue("2"); //Just default to the first "2*" year here

            hudlDriver.GetWebElement(FindBy.Id, Constants.IDs.Athelete.position).Click();
            hudlDriver.GetWebElement(FindBy.Id, Constants.IDs.Athelete.positionFB).SetValue(Keys.Space);
            hudlDriver.GetWebElement(FindBy.Id, Constants.IDs.Athelete.position).Click();

            hudlDriver.GetWebElement(FindBy.Id, Constants.IDs.Athelete.email).SetValue(Email);

            hudlDriver.GetWebElement(FindBy.Id, Constants.IDs.Athelete.add_more_details).Click();
            //Add more details now

            hudlDriver.GetWebElement(FindBy.Id, Constants.IDs.Athelete.cellnumber).SetValue(Phone);
            hudlDriver.GetWebElement(FindBy.Id, Constants.IDs.Athelete.cellcarrier).SetValue(Carrier); //Just pick Verizon
            hudlDriver.GetWebElement(FindBy.Id, Constants.IDs.Athelete.street).SetValue(StAddr);
            hudlDriver.GetWebElement(FindBy.Id, Constants.IDs.Athelete.city).SetValue(City);
            hudlDriver.GetWebElement(FindBy.Id, Constants.IDs.Athelete.zipcode).SetValue(PostalCode);
            hudlDriver.GetWebElement(FindBy.Id, Constants.IDs.Athelete.state).SetValue(State);
            hudlDriver.GetWebElement(FindBy.Id, Constants.IDs.Athelete.additional_notes).
                SetValue("Sure are a lot of things to test here  Sure are a lot of things to test here Sure are a lot of things to test here Sure are a lot of things to test here");
            hudlDriver.GetWebElement(FindBy.Id, Constants.IDs.Athelete.height).SetValue(Height);
            hudlDriver.GetWebElement(FindBy.Id, Constants.IDs.Athelete.inches).SetValue(Inches);
            hudlDriver.GetWebElement(FindBy.Id, Constants.IDs.Athelete.weight).SetValue(Weight);

            hudlDriver.GetWebElement(FindBy.Id, Constants.IDs.Athelete.add_player_link).Click();

            //basic verification that it was added
            string firstName = hudlDriver.GetWebElement(FindBy.Selector, Constants.Selectors.Athelete.first_name).GetValue();
            string lastName = hudlDriver.GetWebElement(FindBy.Selector, Constants.Selectors.Athelete.last_name).GetValue();
            Assert.IsTrue(firstName.Equals(FirstName));
            Assert.IsTrue(lastName.Equals(LastName));
        }

        /// <summary>
        /// Test out the excel export and import operations by creating a couple records, exporting them, deleting them, and re-importing it
        /// This also tests out creating multiple records at once and saving them (we create 2 and save)
        /// I use a Selenium 2.0 feature that allows me to bypass the File Dialog
        /// I do basic verification after the "import" operation to make sure we have imported the expected records
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ExcelExportAndImport()
        {
            const string FirstName1 = "Martin";
            const string LastName1 = "Drake";
            const string JerseyNum1 = "23";
            const string Email1 = "drake@west.com";

            const string FirstName2 = "Izzy";
            const string LastName2 = "Makonnen";
            const string JerseyNum2 = "44";
            const string Email2 = "makonnen@ga.com";

            string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string pathDownload = Path.Combine(pathUser, "Downloads");
            string pathFile = pathDownload + @"\2013_MensVarsityFootball_Roster.xlsx"; //hopefully there would be a way to programatically determine the proper file name
            
            try
            {
                //Just make sure the file doesn't exist so we don't have duplicates when downloading 
                File.Delete(pathFile);
            }
            catch
            {
                //if file failed to delete or wasn't found, let's just keep going, may not bother us
            }

            //Create some records to export
            BasicPersonCreate(FirstName1, LastName1, JerseyNum1, Email1);
            BasicPersonCreate(FirstName2, LastName2, JerseyNum2, Email2);

            //Now EXPORT the data we created
            hudlDriver.GetWebElement(FindBy.Id, "export_roster").Click();
            System.Threading.Thread.Sleep(2000); //give it time to download here

            //Delete all the records now
            CleanupRoster();

            //Now IMPORT the data we exported earlier
            hudlDriver.GetWebElement(FindBy.Id, "upload_new_roster").Click();

            //Give it the path of the file using the Selenium super secret trick to upload files
            IHUDLWebElement webElt = hudlDriver.GetWebElement(FindBy.Id, "RosterFileUpload");

            webElt.Upload(pathFile);

            //Finish it!
            hudlDriver.GetWebElement(FindBy.Id, "uploadNext").Click();
            System.Threading.Thread.Sleep(2000); //Wait a couple more seconds as importing can take a minute

            //Verify 2 records now display, and do basic verification of the names
            Assert.IsTrue(hudlDriver.GetWebElements(FindBy.Selector, Constants.Selectors.Athelete.last_name).Count.Equals(2));
            IHUDLWebElement firstoneLastName = hudlDriver.GetWebElements(FindBy.Selector, Constants.Selectors.Athelete.last_name)[0];
            IHUDLWebElement secondoneLastName = hudlDriver.GetWebElements(FindBy.Selector, Constants.Selectors.Athelete.last_name)[1];
            string lastName = firstoneLastName.GetValue();
            Assert.IsTrue(lastName.Equals(LastName1));
            lastName = secondoneLastName.GetValue();
            Assert.IsTrue(lastName.Equals(LastName2));

            //Blow away the excel file we downloaded
            File.Delete(pathFile);
        }
    }
}
