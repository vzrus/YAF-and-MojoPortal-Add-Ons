


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
        private int _moduleId = 0;
        private static readonly ILog log
         = LogManager.GetLogger(typeof(ActiveDiscussionsSettings));

        protected YafActiveDiscussionsConfiguration config = new YafActiveDiscussionsConfiguration();
        #endregion

        #region Properties
        private int ModuleID
        {
            get { return _moduleId; }
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
                // Get interface to localization
                var iloc = YafContext.Current.Get<ILocalization>();
                this.BoardDropDownList.Text = iloc.GetText("ADMIN_EDITBOARD", "NAME");
                BindBoardList();
                NumberToShow.Text = config.NumberToShow.ToString();
                YAFFeatureGuide.Text = config.YafFeatureGuide.ToString();
            }

        }

        protected void BindBoardList()
        {
            DataTable dt = null;
            try
            {
                dt = ActiveDiscussions.GetAll(config.YafFeatureGuide, "BoardID");
                if (dt != null && dt.Rows.Count > 0)
                {
                    _moduleId = Convert.ToInt32(dt.Rows[0]["ModuleID"]);
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
                BoardDropDownList.Items.Clear();
                BoardDropDownList.AppendDataBoundItems = true;
                BoardDropDownList.DataSource = dt; // TODO: Change this to a list unique for the Portal
                BoardDropDownList.DataTextField = "ModuleTitle";
                BoardDropDownList.DataValueField = "ModuleID";
                BoardDropDownList.SelectedValue = ModuleID.ToString();
                BoardDropDownList.DataBind();

                ListItem item = BoardDropDownList.Items.FindByValue(ModuleID.ToString());

                if (item != null)
                {
                    BoardDropDownList.ClearSelection();
                    item.Selected = true;
                }

        } 
        #endregion

        #region Public Methods (MojoPortal Interface)
        public void SetValue(string val)
        {
            ListItem item = BoardDropDownList.Items.FindByValue(val);
            if (item != null)
            {
                BoardDropDownList.ClearSelection();
                item.Selected = true;
            }
            _moduleId = Convert.ToInt32(val);
        }

        public string GetValue()
        {
            return BoardDropDownList.SelectedValue;
        } 
        #endregion
    }
}