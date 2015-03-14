using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCatchUpSelenium.Tests
{
    public static class Constants
    {
        public const string HudlUrl = @"https://www.hudl.com/";
        public const string LoginUrl = @"https://www.hudl.com/login/";
        public const string RosterUrl = @"http://www.hudl.com/manage/roster/122325";
        public const string Login = @"";
        public const string PW = @"";

        public static class Selectors
        {
            public static class Athelete
            {
                public const string first_name = ".first_name";
                public const string last_name = ".last_name";
                public const string email = ".email";
            }
        }

        public static class IDs
        {
            public static class Athelete
            {
                public const string add_player_link = "add_player_link";
                public const string first_name = "first_name";
                public const string last_name = "last_name";
                public const string classDropDown = "class";
                public const string jersey = "jersey";
                public const string position = "position";
                public const string positionSB = "new_pos_SB";
                public const string positionFB = "new_pos_FB";
                public const string email = "email";
                public const string add_more_details = "add_more_details";
                public const string add_less_details = "add_less_details";

                public const string cellnumber = "cell_number";
                public const string cellcarrier = "cell_carrier";
                public const string parents_name = "parents_name";
                public const string parents_number = "parents_number";
                public const string height = "height";
                public const string inches = "inches";
                public const string weight = "weight";
                public const string additional_notes = "additional_notes";

                public const string street = "street";
                public const string city = "city";
                public const string state = "state";
                public const string zipcode = "postal_code";

            }
        }
    }
}
