/* ***************************************************************************************************
 * The MIT License (MIT)
 * Copyright (c) 2006-2009,2011 vzrus 2009,2010 Mek 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and 
 * associated documentation files (the "Software"), to deal in the Software without restriction, 
 * including without limitation the rights to use, copy, modify, merge, publish, distribute, 
 * sublicense, and/or sell copies of the Software, and to permit persons
 * to whom the Software is furnished to do so, subject to the following conditions:
 *The above copyright notice and this permission notice shall be included in all copies 
 *or substantial portions of the Software.
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE 
 * AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
 * DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 * ***************************************************************************************************
*/

using YAF.Classes;

namespace YAF.Mojo.UI.YAFModule
{
    #region Region

    using System;
    using System.Web;
    using YAF.Classes.Data;
    using YAF.Types.EventProxies;
    using mojoPortal.Business;
    using mojoPortal.Web;
    using mojoPortal.Web.Framework;
    using YAF.Core;
    using YAF.Types;
    using YAF.Types.Interfaces;
    using YAF.Utils;
 
    #endregion

    public partial class YafForum : SiteModuleControl
    {
        
        /// <summary>
        /// The page_ load.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Page_Load(object sender, EventArgs e)
        {
           
            Page.EnableViewState = true; // MojoPortal disables Viewstate by default and YAF requires it

            // Disable Jquery Register
            if (HttpContext.Current.Items["AddJQueryRegistedHandler"] == null)
            {
                HttpContext.Current.Items["AddJQueryRegistedHandler"] = true;
            }
            
            try
            {
                this.forum1.BoardID = WebUtils.ParseInt32FromHashtable(Settings, "BoardID", 1);
                this.forum1.CategoryID = WebUtils.ParseInt32FromHashtable(Settings, "CategoryID", 0);
                this.forum1.LockedForum = WebUtils.ParseInt32FromHashtable(Settings, "LockedForum", 0);
            }
            catch (Exception)
            {
                this.forum1.BoardID = 0;
            }

           
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            { 
              SiteUser su = SiteUtils.GetCurrentSiteUser();
              SyncUserProfile.UpdateTimeZone(su);  
              SyncUserProfile.UpdateProfile(su);
              int? newTimeZone = SyncUserProfile.UpdateTimeZone(su);

              if (newTimeZone != null || YafContext.Current.CurrentUserData.DisplayName != su.Name)
              {
                  SaveUser(su, newTimeZone);
              }
            }
            
        }
        
        /// <summary>
        ///  The method saves a YAF user data to database.
        ///  </summary>
        /// <param name="su">
        /// The MP SiteUser Method.
        /// </param>
        /// <param name="timeZone">
        /// The time zone offset as int in minutes.
        /// </param>
        private void SaveUser(SiteUser su, int? timeZone)
        {
            LegacyDb.user_save(
                     YafContext.Current.PageUserID,
                     YafContext.Current.PageBoardID,
                     null,
                     su.Name,
                     null,
                     timeZone ?? YafContext.Current.CurrentUserData.TimeZone,
                     YafContext.Current.LanguageFile.IsSet() ? YafContext.Current.LanguageFile.Trim() : null,
                     YafContext.Current.CultureUser.Trim(),
                     YafContext.Current.Get<ITheme>().ThemeFile,
                     false, YafContext.Current.TextEditor,
                     YafContext.Current.CurrentUserData.UseMobileTheme,
                     null,
                     null,
                     null,
                     YafContext.Current.CurrentUserData.DSTUser,
                     YafContext.Current.CurrentUserData.IsActiveExcluded,
                     null);
            // clear the cache for this user...)
            YafContext.Current.Get<IRaiseEvent>().Raise(new UpdateUserEvent(YafContext.Current.PageUserID));
            YafContext.Current.Get<IDataCache>().Clear();
        }
    }
}