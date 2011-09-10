using System;
using YAF.Types.EventProxies;
using YAF.Types.Interfaces;
using mojoPortal.Business.WebHelpers;

namespace YAF.Mojo
{
    #region Using
    using YAF.Classes.Data;
    using mojoPortal.Business;
    using YAF.Core;
    using YAF.Utils;
    using mojoPortal.Web; 
    #endregion

    public class SyncUserProfile
    {
        public static void UpdateProfile(SiteUser su){
            
            var yup = YafUserProfile.GetProfile(su.Email);
           
            // No MP counterpart to sync
            // yup.LastUpdatedDate
            // using IsDirty Property to sync to MP
            if (YafContext.Current.IsDirty)
            {
                // sync to MP
                su.AIM = yup.AIM;
                su.ICQ = yup.ICQ;
                su.Yahoo = yup.YIM;
                su.Interests = yup.Interests;
                su.MSN = yup.MSN;
                su.Occupation = yup.Occupation;

                switch (yup.Gender)
                {
                    case 0:
                        su.Gender = "";
                        break;
                    case 1:
                        su.Gender = "M";
                        break;
                    case 2:
                        su.Gender = "F";
                        break;
                    default:
                        su.Gender = "";
                        break;
                }

                su.Save();

                LegacyDb.user_setnotdirty(YafContext.Current.PageBoardID,YafContext.Current.PageUserID);
                YafContext.Current.Get<IRaiseEvent>().Raise(new UpdateUserEvent(YafContext.Current.PageUserID));
             }
            else
            {
                // sync to yaf
                yup.AIM = su.AIM;
                yup.ICQ = su.ICQ;
                yup.YIM = su.Yahoo;
                yup.RealName = su.FirstName + " " + su.LastName;
                yup.Interests = su.Interests;
                yup.MSN = su.MSN;
                yup.Occupation = su.Occupation;
                switch (su.Gender)
                {
                    case "":
                        yup.Gender = 0;
                        break;
                    case "M":
                        yup.Gender = 1;
                        break;
                    case "F":
                        yup.Gender = 2;
                        break;
                    default:
                        yup.Gender = 0;
                        break;
                } 
            }
            // yup.Country = su.Country;
            yup.Save();
        }

        public static int? UpdateTimeZone(SiteUser su)
        {
            
            if (YafContext.Current.CurrentUserData.TimeZone != null)
            {
                if (!(((double)YafContext.Current.CurrentUserData.TimeZone) / 60).Equals(su.TimeOffsetHours))
                {
                    return Convert.ToInt32(su.TimeOffsetHours*60);
                }
            }

            return null;
        }
        
    }
}
