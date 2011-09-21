
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

using System;
using System.Collections;
using YAF.Types.Constants;
using YAF.Utils;
using mojoPortal.Web;
using mojoPortal.Web.Framework;

namespace YAF.Mojo
{
    #region Using

    using System.Web;
    using mojoPortal.Business;
    using mojoPortal.Business.WebHelpers;
    using YAF.Classes;
    using YAF.Core;
    using System.Linq; 

    #endregion

    public class MojoPortalUrlBuilder
        :  RewriteUrlBuilder
    {
        /// <summary>
        ///  The method forms yaf url. 
        /// </summary>
        /// <param name="url">
        /// The url to override.
        /// </param>
        /// <returns>
        /// The new url string.
        /// </returns>
        public override string BuildUrl(string url)
        {
            int moduleId = -1;
            PageSettings currentPage = CacheHelper.GetCurrentPage();
            
            string scriptName = currentPage == null ? HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"] : currentPage.Url.Replace("~/", "");
           
            var ar = currentPage.Modules;

            int boardId = -1;
            // Loop through modules on the page
            ///TODO: Single out a required module from all page modules.
            foreach (var m in ar.Cast<Module>())
            {
               ///TODO: We should get here a YAF module from all page modules. How to get an exact feature instance?
               // Else we'll  
               if ( m.ControlSource.Contains("YAFModule"))
               {
                  // Module.GetModulesForSite(currentPage.SiteId, System.Guid.ParseExact("c5584bb4-e42f-4c7d-81b7-037176d562df"));

                   Hashtable moduleSettings = ModuleSettings.GetModuleSettings(m.ModuleId);
                  int moduleIId = WebUtils.ParseInt32FromHashtable(
                  moduleSettings, "BoardID", -1);
                  if (YafContext.Current.Settings.BoardID == moduleIId)
                  {
                      moduleId = m.ModuleId;
                      boardId = moduleIId;
                      break;
                  }
               }
            }

            // ConfigHelper.GetStringProperty("BoardID", "forums");
            // Redirect to MP login page instead of YAF one.
            // TODO: ajust for site folders
            if (url.Contains("g=login"))
            {
                return "~/Secure/Login.aspx";
            }
            
            // Get YAF page token from server variables.
            scriptName = HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"];

            // Replace it for a correct rewriting
            url = url.Replace("&", "&amp;");

            // Redirect to YAF install page.
            if (scriptName.Contains("install"))
            {
                return Config.AppRoot.Replace("~", "").Trim() + scriptName;
            }

           // scriptName = scriptName.Replace("Default.aspx", currentPage.Url.TrimStart('~', '/'));
           // scriptName = scriptName.Replace("default.aspx", currentPage.Url.TrimStart('~', '/'));

            // If Url Rewriting is disabled we simply add MP module path before yaf path, else we use a custom rewriting. 
            if (!Config.EnableURLRewriting)
            {
                    return string.Format("{0}?pageid={1}&mid={2}&{3}", scriptName, currentPage.PageId, moduleId, url);
            }
            
                string baseEl = string.Empty;
                string addEl = string.Empty;
                string[] urlBase = url.Split(new string[]{"&amp;"}, StringSplitOptions.RemoveEmptyEntries);
               
                for (int i = 0; i < urlBase.GetLength(0);i++)
                {
                    if (i >= 1)
                    {
                        addEl +=  "&" + urlBase[i];
                    }
                    else
                    {
                        if (urlBase[0].Contains("g="))
                        {
                            baseEl = urlBase[0];
                        }
                    }
                }

                ///TODO: Get rid of it.
                if (addEl.Contains("pg=posts"))
                {
                    addEl = addEl.Replace("pg=posts", "pg=5");
                }

                return this.FriendlyRewriter("{0}&pageid={1}&mid={2}{3}".Trim('&').FormatWith(baseEl, currentPage.PageId, moduleId, addEl), boardId, currentPage);
        }

        /// <summary>
        /// The custom Url rewriter
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string FriendlyRewriter(string url, int boardId, PageSettings currentPage)
        {
            string rewriteDelimiter = "-";

            string newUrl = "{0}{1}?{2}".FormatWith(AppPath, Config.ForceScriptName ?? ScriptName, url);

            // create scriptName
            string scriptName = "{0}{1}".FormatWith(AppPath, Config.ForceScriptName ?? ScriptName);

            // get the base script file from the config -- defaults to, well, default.aspx :)
            string scriptFile = Config.BaseScriptFile;

            if (scriptName.EndsWith(scriptFile))
            {
                string before = scriptName.Remove(scriptName.LastIndexOf(scriptFile));

                var parser = new SimpleURLParameterParser(url);

                // create "rewritten" url...
                newUrl = before + Config.UrlRewritingPrefix;

                string useKey = string.Empty;
                string useKey2 = string.Empty;
                string description = string.Empty;
                string pageName = parser["g"];
                bool isFeed = false;
                // const bool showKey = false;
                bool handlePage = false;

                switch (parser["g"])
                {
                    case "topics":
                        useKey = "f";
                        description = this.GetForumName(parser[useKey].ToType<int>());
                        handlePage = true;
                        break;
                    case "posts":
                        if (parser["t"].IsSet())
                        {
                            useKey = "t";
                            pageName += "t";
                            description = this.GetTopicName(parser[useKey].ToType<int>());
                        }
                        else if (parser["m"].IsSet())
                        {
                            useKey = "m";
                            pageName += "m";

                            try
                            {
                                description = this.GetTopicNameFromMessage(parser[useKey].ToType<int>());
                            }
                            catch (Exception)
                            {
                                description = "posts";
                            }
                        }
                        handlePage = true;
                        break;
                    case "profile":
                        useKey = "u";

                        // description = GetProfileName( Convert.ToInt32( parser [useKey] ) );
                        break;
                    case "forum":
                        if (parser["c"].IsSet())
                        {
                            useKey = "c";
                            description = this.GetCategoryName(parser[useKey].ToType<int>());
                        }
                        break;
                    case "rsstopic":
                        if (parser["pg"].IsSet())
                        {
                            useKey = "pg";
                            // pageName += "pg";
                            if (parser[useKey].ToType<int>() == YafRssFeeds.Active.ToInt())
                            {
                                description = "active";
                            }
                            if (parser[useKey].ToType<int>() == YafRssFeeds.Favorite.ToInt())
                            {
                                description = "favorite";
                            }
                            if (parser[useKey].ToType<int>() == YafRssFeeds.Forum.ToInt())
                            {
                                description = "forum";
                            }
                            if (parser[useKey].ToType<int>() == YafRssFeeds.LatestAnnouncements.ToInt())
                            {
                                description = "latestannouncements";
                            }
                            if (parser[useKey].ToType<int>() == YafRssFeeds.LatestPosts.ToInt())
                            {
                                description = "latestposts";
                            }
                            if (parser[useKey].ToType<int>() == YafRssFeeds.Posts.ToInt())
                            {
                                description = "posts";
                            }
                            if (parser[useKey].ToType<int>() == YafRssFeeds.Topics.ToInt())
                            {
                                description = "topics";
                            }
                        }
                        if (parser["f"].IsSet())
                        {
                            useKey2 = "f";
                            description += this.GetForumName(parser[useKey2].ToType<int>());
                        }
                        if (parser["t"].IsSet())
                        {
                            useKey2 = "t";
                            description += this.GetTopicName(parser[useKey2].ToType<int>());
                        }
                        if (parser["ft"].IsSet())
                        {
                            useKey2 = "ft";
                            if ((parser[useKey2].ToType<int>()) == YafSyndicationFormats.Atom.ToInt())
                            {
                                description += "{0}atom".FormatWith(rewriteDelimiter);
                            }
                            else
                            {
                                description += "{0}rss".FormatWith(rewriteDelimiter);
                            }
                        }
                        handlePage = true;
                        isFeed = true;
                        break;
                }

                if (parser["pageid"] != null)
                {
                    int page = parser["pageid"].ToType<int>();
                    newUrl += "pageid{0}".FormatWith(page);
                    parser.Parameters.Remove("pageid");
                }

                if (parser["mid"] != null)
                {
                    int page = parser["mid"].ToType<int>();
                    newUrl += "{1}mid{0}{1}".FormatWith(page,rewriteDelimiter);
                    parser.Parameters.Remove("mid");
                }

                newUrl += pageName;

                if (useKey.Length > 0)
                {
                    newUrl += parser[useKey];
                }

                if (handlePage && parser["p"] != null && !isFeed)
                {
                    int page = parser["p"].ToType<int>();
                    if (page != 1)
                    {
                        newUrl += "p{0}".FormatWith(page);
                    }

                    parser.Parameters.Remove("p");
                }
                if (isFeed)
                {
                    if (parser["ft"] != null)
                    {
                        int page = parser["ft"].ToType<int>();
                        newUrl += "ft{0}".FormatWith(page);
                        parser.Parameters.Remove("ft");
                    }
                    if (parser["f"] != null)
                    {
                        int page = parser["f"].ToType<int>();
                        newUrl += "f{0}".FormatWith(page);
                        parser.Parameters.Remove("f");
                    }
                    if (parser["t"] != null)
                    {
                        int page = parser["t"].ToType<int>();
                        newUrl += "t{0}".FormatWith(page);
                        parser.Parameters.Remove("t");
                    }
                }

                if (parser["find"] != null)
                {
                    newUrl += "find{0}".FormatWith(parser["find"].Trim());
                    parser.Parameters.Remove("find");
                }

                if (description.Length > 0)
                {
                    if (description.EndsWith("-"))
                    {
                        description = description.Remove(description.Length - 1, 1);
                    }

                    newUrl += "-{0}".FormatWith(description);
                }

                if (!isFeed)
                {
                    newUrl += ".aspx";
                }
                else
                {
                    newUrl += ".xml";
                }

                string restURL = parser.CreateQueryString(new[] { "g", useKey });

                // append to the url if there are additional (unsupported) parameters
                if (restURL.Length > 0)
                {
                    newUrl += "?{0}".FormatWith(restURL);
                }

                // see if we can just use the default (/)
                if (newUrl.EndsWith("yaf_forum.aspx"))
                {
                    // remove in favor of just slash...
                    newUrl = newUrl.Remove(newUrl.LastIndexOf("yaf_forum.aspx"), "yaf_forum.aspx".Length);
                }

                // add anchor
                if (parser.HasAnchor)
                {
                    newUrl += "#{0}".FormatWith(parser.Anchor);
                }
            }

            // just make sure & is &amp; ...
            newUrl = newUrl.Replace("&amp;", "&").Replace("&", "&amp;");

            // It doesn't work - a problem in MP when rewirting is disabled
            if (currentPage.UrlHasBeenAdjustedForFolderSites)
            {
                int trimIndex = currentPage.Url.IndexOf(currentPage.UnmodifiedUrl.Trim('~'));
                string md = currentPage.Url.Remove(trimIndex).Trim('/');
                md = md.Substring(md.LastIndexOf('/')+1);
                string folderName = md;
                newUrl = newUrl.Replace(newUrl, "/{0}{1}".FormatWith(folderName, newUrl));
            }
            return newUrl;
        }
    }
}
