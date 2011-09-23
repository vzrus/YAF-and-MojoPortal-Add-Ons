
using System.Linq;
using System.Web.UI.HtmlControls;
using Resources;
using YAF.Mojo.ActiveDiscussions.UI;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;

namespace YAF.Mojo.ActiveDiscussions.UI
{
    #region Using
    using System;
    using System.Data;
    using System.Web.UI.WebControls;

    using mojoPortal.Web;
    using log4net;

    using YAF.Classes;
    using YAF.Classes.Data;
    using YAF.Controls;
    using YAF.Core;
    using YAF.Types;
    using YAF.Types.Constants;
    using YAF.Types.Interfaces;
    using YAF.Utils;
    using mojoPortal.Web.Controls;
    using System;
    using System.Configuration;
    using System.Data;
    using System.Globalization;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using mojoPortal.Business;
    using mojoPortal.Web.Framework;
    using mojoPortal.Web.UI;
    using Resources;

    #endregion

    public partial class ActiveDiscussion : SiteModuleControl
    {
        #region Constants and Fields

        /// <summary>
        ///  The last post tooltip string.
        /// </summary>
        private string lastPostToolTip;

        private ILocalization iloc;
       
        public int BoardId { get; set; }

        private static readonly ILog log
           = LogManager.GetLogger(typeof(ActiveDiscussion));

        /// <summary>
        ///  The first Unread post tooltip string
        /// </summary>
        private string firstUnreadPostToolTip;

        int mid = -1;
        int pageid = -1;
        int siteid = -1;

        protected YafActiveDiscussionsConfiguration config = new YafActiveDiscussionsConfiguration();

        #endregion

        #region Methods

        /// <summary>
        /// The latest posts_ item data bound.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void LatestPosts_ItemDataBound([NotNull] object sender, [NotNull] RepeaterItemEventArgs e)
        {
            // populate the controls here...
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
            {
                return;
            }

            var currentRow = (DataRowView)e.Item.DataItem;
           
            // make message url...
            string messageUrl = GetRealRelativeUrl(pageid,mid, "g=posts&m={0}&find=lastpost".FormatWith(currentRow["LastMessageID"]));

            // get the controls
            var newPostIcon = (Image)e.Item.FindControl("NewPostIcon");
            var textMessageLink = (HyperLink)e.Item.FindControl("TextMessageLink");
            var imageMessageLink = (HyperLink)e.Item.FindControl("ImageMessageLink");
            var lastPostedImage = (ThemeImage)e.Item.FindControl("LastPostedImage");
            var imageLastUnreadMessageLink = (HyperLink)e.Item.FindControl("ImageLastUnreadMessageLink");
            var lastUnreadImage = (ThemeImage)e.Item.FindControl("LastUnreadImage");

            var lastUserLink = (HtmlAnchor)e.Item.FindControl("LastUserLink");
          
            var lastPostedDateLabel = (DisplayDateTime)e.Item.FindControl("LastPostDate");
            var forumLink = (HyperLink)e.Item.FindControl("ForumLink");
            imageLastUnreadMessageLink.Visible = YafContext.Current.Get<YafBoardSettings>().ShowLastUnreadPost;

            // populate them...
            newPostIcon.AlternateText = YafContext.Current.Get<ILocalization>().GetText("NEW_POSTS");
            newPostIcon.ToolTip = YafContext.Current.Get<ILocalization>().GetText("NEW_POSTS");
            newPostIcon.ImageUrl = YafContext.Current.Get<ITheme>().GetItem("ICONS", "TOPIC_NEW");

            if (currentRow["Status"].ToString().IsSet() && YafContext.Current.Get<YafBoardSettings>().EnableTopicStatus)
            {
                var topicStatusIcon = YafContext.Current.Get<ITheme>().GetItem("TOPIC_STATUS", currentRow["Status"].ToString());

                if (topicStatusIcon.IsSet() &&
                    !topicStatusIcon.Contains("[TOPIC_STATUS."))
                {
                    textMessageLink.Text =
                    "<img src=\"{0}\" alt=\"{1}\" title=\"{1}\" style=\"border: 0;width:16px;height:16px\" />&nbsp;{2}".FormatWith(
                    YafContext.Current.Get<ITheme>().GetItem("TOPIC_STATUS", currentRow["Status"].ToString()),
                        YafContext.Current.Get<ILocalization>().GetText("TOPIC_STATUS", currentRow["Status"].ToString()),
                        YafContext.Current.Get<IBadWordReplace>().Replace(this.HtmlEncode(currentRow["TOPIC"])));
                }
                else
                {
                    textMessageLink.Text =
                    "[{0}]&nbsp;{1}".FormatWith(
                        YafContext.Current.Get<ILocalization>().GetText("TOPIC_STATUS", currentRow["Status"].ToString()),
                        YafContext.Current.Get<IBadWordReplace>().Replace(this.HtmlEncode(currentRow["TOPIC"])));
                }
            }
            else
            {
                textMessageLink.Text =
                    YafContext.Current.Get<IBadWordReplace>().Replace(this.HtmlEncode(currentRow["Topic"].ToString()));
            }

            if (!YafContext.Current.IsMobileDevice)
            {
                textMessageLink.ToolTip = YafContext.Current.Get<ILocalization>().GetText("COMMON", "VIEW_TOPIC");
                textMessageLink.NavigateUrl = GetRealRelativeUrl(pageid,mid, "g=posts&t={0}".FormatWith(currentRow["TopicID"]));
                  
            }
            else
            {
                textMessageLink.ToolTip = YafContext.Current.Get<ILocalization>().GetText("DEFAULT", "GO_LASTUNREAD_POST");
                textMessageLink.NavigateUrl = GetRealRelativeUrl(pageid,mid,"g=posts&m={0}&find=unread".FormatWith(currentRow["TopicID"]));
            }

            imageMessageLink.NavigateUrl = messageUrl;
            lastPostedImage.LocalizedTitle = this.lastPostToolTip;

            if (imageLastUnreadMessageLink.Visible)
            {
                imageLastUnreadMessageLink.NavigateUrl = GetRealRelativeUrl(pageid, mid, "g=posts&t={0}&find=unread".FormatWith(currentRow["TopicID"]));

                lastUnreadImage.LocalizedTitle = this.firstUnreadPostToolTip;
            }

            // Just in case...
            if (currentRow["LastUserID"] != DBNull.Value)
            {
                //lastUserLink. = currentRow["LastUserID"].ToType<int>();
                lastUserLink.Title = YafContext.Current.Get<IUserDisplayName>().GetName(currentRow["LastUserID"].ToType<int>());
                lastUserLink.Attributes.Remove("alt");
                lastUserLink.Attributes.Add("alt", lastUserLink.Title);
                lastUserLink.Attributes.Remove("Style");
                lastUserLink.Name = lastUserLink.Title;
                lastUserLink.InnerText= lastUserLink.Title;
                lastUserLink.Attributes.Add("Style",currentRow["LastUserStyle"].ToString() );
                lastUserLink.HRef = GetRealRelativeUrl(pageid, mid, "g=profile&u={0}".FormatWith(currentRow["LastUserID"]));

            }

            if (currentRow["LastPosted"] != DBNull.Value)
            {
                lastPostedDateLabel.DateTime = currentRow["LastPosted"];
                DateTime lastRead;
                DateTime lastReadForum;

                if (YafContext.Current.Get<YafBoardSettings>().UseReadTrackingByDatabase)
                {
                    try
                    {
                        lastRead = currentRow["LastTopicAccess"] != DBNull.Value
                                       ? currentRow["LastTopicAccess"].ToType<DateTime>()
                                       : DateTime.MinValue.AddYears(1902);
                    }
                    catch (Exception)
                    {
                        lastRead = YafContext.Current.Get<IReadTracking>().GetTopicRead(
                            YafContext.Current.PageUserID, currentRow["LastTopicID"].ToType<int>());
                    }

                    try
                    {
                        lastReadForum = currentRow["LastForumAccess"] != DBNull.Value
                                       ? currentRow["LastForumAccess"].ToType<DateTime>()
                                       : DateTime.MinValue.AddYears(1902);
                    }
                    catch (Exception)
                    {
                        lastReadForum = YafContext.Current.Get<IReadTracking>().GetForumRead(
                            YafContext.Current.PageUserID, currentRow["ForumID"].ToType<int>());
                    }
                }
                else
                {
                    lastRead = YafContext.Current.Get<IYafSession>().GetTopicRead(currentRow["TopicID"].ToType<int>());
                    lastReadForum = YafContext.Current.Get<IYafSession>().GetForumRead(currentRow["ForumID"].ToType<int>());
                }

                if (lastReadForum > lastRead)
                {
                    lastRead = lastReadForum;
                }

                lastUnreadImage.ThemeTag = (DateTime.Parse(currentRow["LastPosted"].ToString()) > lastRead)
                                               ? "ICON_NEWEST_UNREAD"
                                               : "ICON_LATEST_UNREAD";
                lastPostedImage.ThemeTag = (DateTime.Parse(currentRow["LastPosted"].ToString()) > lastRead)
                                               ? "ICON_NEWEST"
                                               : "ICON_LATEST";
            }

            forumLink.Text = this.Page.HtmlEncode(currentRow["Forum"].ToString());
            forumLink.ToolTip = YafContext.Current.Get<ILocalization>().GetText("COMMON", "VIEW_FORUM");
            forumLink.NavigateUrl = GetRealRelativeUrl(pageid, mid, "g=topics&f={0}".FormatWith(currentRow["ForumID"]));
        }

        /// <summary>
        /// The page_ load.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Page_Load([NotNull] object sender, [NotNull] EventArgs e)
        {
            PageSettings ps = CacheHelper.GetCurrentPage();

            iloc = YafContext.Current.Get<ILocalization>();
            // Latest forum posts
            // Shows the latest n number of posts on the main forum list page
            const string CacheKey = Constants.Cache.ForumActiveDiscussions;

            DataTable activeTopics = null;

            if (YafContext.Current.IsGuest)
            {
                // allow caching since this is a guest...
                activeTopics = YafContext.Current.Get<IDataCache>()[CacheKey] as DataTable;
            }
            mid =  WebUtils.ParseInt32FromHashtable(Settings, "ModuleID", -999);
            pageid = -1;
            siteid = -1;
            try
            {
                DataTable dt =  ActiveDiscussions.GetAll(new Guid("c5584bb4-e42f-4c7d-81b7-037176d562df"),"BoardID");
                if (dt.Rows.Count > 0)
                {

                    foreach (DataRow forumactivediscussion in dt.Rows)
                    {
                        if ((Convert.ToInt32(forumactivediscussion["SiteID"]) == ps.SiteId && mid <= 0) || mid > 0)
                        {
                            pageid = Convert.ToInt32(forumactivediscussion["PageID"]);
                            BoardId = Convert.ToInt32(forumactivediscussion["SettingValue"]);
                            siteid = Convert.ToInt32(forumactivediscussion["SiteID"]);

                            // The module is found, else retuned latest board. 
                            if (mid == Convert.ToInt32(forumactivediscussion["ModuleID"]))
                            {
                                break;
                            }
                            mid = Convert.ToInt32(forumactivediscussion["ModuleID"]);
                            if (pageid > 0)
                            {
                                break;
                            }
                        }

                    }
                    foreach (DataRow forumactivediscussion in dt.Rows)
                    {
                        pageid = Convert.ToInt32(forumactivediscussion["PageID"]);
                        BoardId = Convert.ToInt32(forumactivediscussion["SettingValue"]);
                        siteid = Convert.ToInt32(forumactivediscussion["SiteID"]);

                        // The module is found, else retuned latest board. 
                        if (mid == Convert.ToInt32(forumactivediscussion["ModuleID"]))
                        {
                            break;
                        }
                        mid = Convert.ToInt32(forumactivediscussion["ModuleID"]);

                    }
                }
                else
                {
                    log.Debug("No data available to fill in the YAF Activedicsussions control");
                }
            
            }
            catch (Exception)
            {
                BoardId = 1;
            }
            if (!Page.IsPostBack)
            {
                this.LatestPostsHeader.Text = YAFActiveDiscussions.LatestPostsHeader;
                if (activeTopics == null)
                {

                    //  try
                    //  {
                    int userId = -1;
                    SiteUser su = SiteUtils.GetCurrentSiteUser();
                    
                    if (su != null)
                    {
                        userId = LegacyDb.user_get(BoardId, su.UserGuid);
                    }
                    else
                    {
                        userId = LegacyDb.user_guest(BoardId).ToType<int>();
                    }

                    activeTopics = LegacyDb.topic_latest(
                        BoardId,
                        config.NumberToShow,
                        userId,
                        YafContext.Current.Get<YafBoardSettings>().UseStyledNicks,
                        YafContext.Current.Get<YafBoardSettings>().NoCountForumsInActiveDiscussions,
                        YafContext.Current.Get<YafBoardSettings>().UseReadTrackingByDatabase);

                    // Set colorOnly parameter to true, as we get all but color from css in the place
                    if (YafContext.Current.Get<YafBoardSettings>().UseStyledNicks)
                    {
                        YafContext.Current.Get<IStyleTransform>().DecodeStyleByTable(ref activeTopics, true,
                                                                                     "LastUserStyle");
                    }

                    if (YafContext.Current.IsGuest)
                    {
                        YafContext.Current.Get<IDataCache>().Set(
                            CacheKey, activeTopics,
                            TimeSpan.FromMinutes(
                                YafContext.Current.Get<YafBoardSettings>().ActiveDiscussionsCacheTimeout));
                    }
                    /*   }
                       catch (Exception)
                       {
                           log.Debug("No data available to fill in the YAF Activedicsussions control");
                            NoDataReceivedLbl.Text = YAFActiveDiscussions.ActiveDiscussionNoDataAvailable;
                           noifo_Tr.Visible = true;
                       } */
                }

                if (activeTopics != null)
                {
                    bool groupAccess =
                        YafContext.Current.Get<IPermissions>().Check(
                            YafContext.Current.Get<YafBoardSettings>().PostLatestFeedAccess);

                    this.lastPostToolTip = iloc.GetText("DEFAULT", "GO_LAST_POST");
                    this.firstUnreadPostToolTip = iloc.GetText("DEFAULT", "GO_LASTUNREAD_POST");
                    this.LatestPosts.DataSource = activeTopics;
                    this.LatestPosts.DataBind();
                }
            }

        }

        private  string GetRealRelativeUrl(int pageid, int mid, string yafPath)
        {
            if (!Config.EnableURLRewriting)
            {
               return string.Format("{0}?pageid={1}&mid={2}&{3}", "~/Default.aspx", pageid, mid, yafPath);
            }

            return new MojoPortalUrlBuilder().FriendlyRewriter("pageid={0}&mid={1}&{2}".FormatWith(pageid, mid, yafPath),BoardId);
        }

        #endregion
    }
}