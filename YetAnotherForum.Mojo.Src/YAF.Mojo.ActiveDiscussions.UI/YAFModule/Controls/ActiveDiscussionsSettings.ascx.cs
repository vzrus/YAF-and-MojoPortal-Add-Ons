


namespace YAF.Mojo.ActiveDiscussions.UI.Controls
{
    #region Using

    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using YAF.Classes;
    using YAF.Types.Interfaces;
    using mojoPortal.Web;
    using mojoPortal.Web.Framework;
    using mojoPortal.Web.UI;
    using YAF.Core;
    using YAF.Classes.Data;
    using mojoPortal.Business;
    using log4net;

    #endregion

    public partial class ActiveDiscussionsSettings : UserControl, ISettingControl
    {
        #region Fields
        private int _yafForumModuleInstanceId = 0;
        private static readonly ILog log
         = LogManager.GetLogger(typeof(ActiveDiscussionsSettings));

   
        #endregion

        #region Properties

        private int YafForumModuleInstanceId
        {
            get { return _yafForumModuleInstanceId; }
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

        #region Protected Methods

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

                try
                {
                    // Get interface to localization
                    var iloc = YafContext.Current.Get<ILocalization>();
                    this.ModuleDropDownList.Text = iloc.GetText("ADMIN_EDITBOARD", "NAME");
                    BindControls();
                }
                catch (Exception)
                {
                    // Yaf was not initialized
                    log.Debug("You should install a YAF module first.");
                    Response.Redirect(string.Format("{0}/install", Config.AppRoot ?? Config.ClientFileRoot));
                }
            }

        }

        protected void BindControls()
        {
            // The YAF Forum main control guid.
            Guid dd1 = Guid.Parse("c5584bb4-e42f-4c7d-81b7-037176d562df");
            DataTable dt = null;
            try
            {
                dt = ActiveDiscussions.GetSpecificSettingAllModulesWithTheDefinition(dd1, "BoardID");
                if (dt != null && dt.Rows.Count > 0)
                {
                    int ss = -1;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (!int.TryParse(dr["SettingValue"].ToString(), out ss))
                        {
                            _yafForumModuleInstanceId = Convert.ToInt32(dr["ModuleID"]);
                        }
                        else
                        {
                            dr["ModuleID"] = _yafForumModuleInstanceId = Convert.ToInt32(dr["ModuleID"]);
                        }
                    }
                 
                    dt.AcceptChanges();
                }
                else
                {
                    log.Debug("No data returned from database.");
                    return;
                }
            }
            catch (Exception)
            {
                log.Debug("No data available to fill in the YAF Activedicsussions control");
                Response.Redirect(string.Format("{0}/install", Config.AppRoot));
            }
                ModuleDropDownList.Items.Clear();
                ModuleDropDownList.AppendDataBoundItems = true;
                ModuleDropDownList.DataSource = dt; // TODO: Change this to a list unique for the Portal
                ModuleDropDownList.DataTextField = "ModuleTitle";
                ModuleDropDownList.DataValueField = "ModuleID";
                ModuleDropDownList.SelectedValue = YafForumModuleInstanceId.ToString();
                ModuleDropDownList.DataBind();

                ListItem item = ModuleDropDownList.Items.FindByValue(YafForumModuleInstanceId.ToString());

                if (item != null)
                {
                    ModuleDropDownList.ClearSelection();
                    item.Selected = true;
                }

        } 
        #endregion

        #region Public Methods (MojoPortal Interface)
        public void SetValue(string val)
        {
            ListItem item = ModuleDropDownList.Items.FindByValue(val);
            if (item != null)
            {
                ModuleDropDownList.ClearSelection();
                item.Selected = true;
            }
            _yafForumModuleInstanceId = Convert.ToInt32(val);
        }

        public string GetValue()
        {
            return ModuleDropDownList.SelectedValue;
        } 
        #endregion
    }
}