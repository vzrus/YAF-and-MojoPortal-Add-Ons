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

using System.Collections.Generic;
using System.Linq;
using YAF.Types.Objects;
using YAF.Utils;
using mojoPortal.Business;

namespace YAF.Mojo.UI.YAFModule.Controls
{
    #region Using

    using System;
    using System.Data;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using YAF.Classes;
    using YAF.Types.Interfaces;
    using mojoPortal.Web;
    using mojoPortal.Web.Framework;
    using mojoPortal.Web.UI;
    using YAF.Core;
    using YAF.Classes.Data; 

    #endregion

    public partial class YafBoardSettings : UserControl, ISettingControl
    {
        #region Fields

        private int _boardId = 0; 

        #endregion

        #region Properties

        private int BoardId
        {
            get { return _boardId; }
        }

        private int ModuleId
        {
            get { return WebUtils.ParseInt32FromQueryString("mid", 0); }
        }

        private int PageId
        {
            get { return WebUtils.ParseInt32FromQueryString("pageid", 0); }
        } 

        #endregion

        #region MojoPortal Settings Interface

        public void SetValue(string val)
        {
            ListItem item = BoardDropDownList.Items.FindByValue(val);
            if (item != null)
            {
                BoardDropDownList.ClearSelection();
                item.Selected = true;
            }
            _boardId = Convert.ToInt32(val);
        }

        public string GetValue()
        {
            return BoardDropDownList.SelectedValue;
        } 

        #endregion

        #region Events

        /// <summary>
        /// The standard Page OnInit event. 
        /// </summary>
        /// <param name="e">
        /// The e - argument of the event.
        /// </param>
        protected override void OnInit(EventArgs e)
        {
            SaveBtn.Click += new EventHandler(Save_Click);
            base.OnInit(e);
        }

        /// <summary>
        /// The new board save button click event.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e - argument of the event.
        /// </param>
        protected void Save_Click(object sender, EventArgs e)
        {
            CreateBoard();
            BindBoardList();
        } 

        /// <summary>
        /// The standard PageLoad event.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        ///  The e - argument of the event.
        /// </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // this keeps the action from changing during ajax postback in folder based sites
                SiteUtils.SetFormAction(Page, Request.RawUrl);
            }
            catch (MissingMethodException)
            {
                //this method was introduced in .NET 3.5 SP1
            }

            if (!Page.IsPostBack)
            {
                // If YAF is not install it'll case troubles
                try
                {
                    // Get interface to localization and populate labels with localized data
                    var iloc = YafContext.Current.Get<ILocalization>();

                    this.NewBoardLbl.Text = iloc.GetText("ADMIN_BOARDS", "NEW_BOARD");
                    this.SettingsLbl.Text = iloc.GetText("ADMIN_EDITBOARD", "NAME");
                    this.BoardCultureLbl.Text = iloc.GetText("ADMIN_EDITBOARD", "CULTURE");
                    this.AllowThreadedLabel.Text = iloc.GetText("ADMIN_EDITBOARD", "THREADED");
                    this.SaveBtn.Text = iloc.GetText("ADMIN_BOARDS", "NEW_BOARD");
                    this.SelectBoardLbl.Text = iloc.GetText("ADMIN_EDITGROUP", "BOARD");

                    BindBoardList();
                    BindCultureList();
                }
                catch(Exception)
                {
                    Response.Redirect(string.Format("{0}/install", Config.AppRoot));
                }
                
            }

        }

        #endregion

        #region Private Methods

        /// <summary>
        /// The method binds gets YAF board lists and binds it to respective DropDown control.
        /// </summary>
        private void BindBoardList()
        {
            BoardDropDownList.Items.Clear();
            DataTable dt = null;

            try
            {
                dt = LegacyDb.board_list(null);
            }
            catch (Exception)
            {
                Response.Redirect(string.Format("{0}/install", Config.AppRoot));
            }
            BoardDropDownList.DataSource = dt; // TODO: Change this to a list unique for the Portal
            BoardDropDownList.DataTextField = "Name";
            BoardDropDownList.DataValueField = "BoardID";
            BoardDropDownList.DataBind();

            ListItem item = BoardDropDownList.Items.FindByValue(BoardId.ToString());

            if (item != null)
            {
                BoardDropDownList.ClearSelection();
                item.Selected = true;
            }

        }

        /// <summary>
        /// The method gets a list of available cultures and sets them as dropdown list data source.
        /// </summary>
        private void BindCultureList()
        {
            Culture.Items.Clear();
            Culture.DataSource = StaticDataHelper.Cultures();
            Culture.DataValueField = "CultureTag";
            Culture.DataTextField = "CultureNativeName";
            Culture.DataBind();
        }

        /// <summary>
        /// The method creates a new board and sets required YAF registry values.
        /// </summary>
        private void CreateBoard()
        {
            MembershipUser boardAdmin = UserMembershipHelper.GetUser();

            System.Data.DataTable cult = StaticDataHelper.Cultures();
            string langFile = "english.xml";
            foreach (System.Data.DataRow drow in cult.Rows)
            {
                if (drow["CultureTag"].ToString() == this.Culture.SelectedValue)
                {
                    langFile = (string)drow["CultureFile"];
                }
            }

            _boardId = LegacyDb.board_create(boardAdmin.UserName, boardAdmin.Email, boardAdmin.ProviderUserKey, BoardName.Text, this.Culture.SelectedItem.Value, langFile, "", "", "YAF");
           
            this.PrepareBoard();

            // MojoBoardHelper.Current.CreateBoardReference(ModuleId, BoardId);
        }

        private void PrepareBoard()
        {

            // Make sure that display names are enabled and can't be modified by default
            LegacyDb.registry_save("enabledisplayname", 1);
            LegacyDb.registry_save("allowdisplaynamemodification", 0);

            // Save the current portal editor as the site admin user.
            SiteUser su = SiteUtils.GetCurrentSiteUser();

            // Link user with Membership and add him to user table
            LegacyDb.user_aspnet(_boardId, su.Email, su.Name, su.Email, su.UserGuid, true);

            // Find the created user
            var tuf = LegacyDb.UserFind(_boardId, false, su.Email, su.Email, su.Name, null, null);

            // The user already should be in the DB if not - something went wrong. 
            // Add the module editor as a host admin in YAF
            LegacyDb.user_adminsave(_boardId, tuf.FirstOrDefault().UserID, su.Email, su.Name, su.Email, 3,
                                    tuf.FirstOrDefault().RankID);

            // Loop through group list and delete a useless fake group if it's here
            var dt = LegacyDb.group_list(_boardId, null);
            foreach (
                DataRow drow in
                    dt.Rows.Cast<DataRow>().Where(
                        drow => drow["Name"].ToString().IsNotSet() || drow["Name"].ToString().Trim() == ","))
            {
                LegacyDb.group_delete(drow["GroupID"]);
                Roles.DeleteRole(drow["Name"].ToString());
            }
        }


        private void PrepareNewBoard()
        {
            PrepareBoard();
        }



 
        #endregion
    }
}