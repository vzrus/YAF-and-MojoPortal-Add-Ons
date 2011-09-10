
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
        /// The method forms yaf url. 
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
           
          
            // Loop through modules on the page
            ///TODO: Single out a required module from all page modules.
            foreach (var m in ar.Cast<Module>())
            {
               ///TODO: We should get here a YAF module from all page modules. How to get an exact feature instance?
               // Else we'll  
               if ( m.ControlSource.Contains("YAFModule"))
               {
                  // Hashtable moduleSettings = ModuleSettings.GetModuleSettings(m.ModuleId);
                   moduleId = m.ModuleId;
               }
            
            }

            // Redirect to MP login page instead of YAF one.
            // TODO: ajust for site folders
            if (url.Contains("g=login"))
            {
                //  WebUtils.SetupRedirect(this, "Login.aspx");
                return "~/Secure/Login.aspx";
            }
            
            // Get YAF page token from server variables.
            scriptName = HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"];

            // Replace it for acorrect rewriting
            url = url.Replace("&", "&amp;");

            // Redirect to YAF install page.
            if (scriptName.Contains("install"))
            {
                return Config.AppRoot.Replace("~", "").Trim() + scriptName;
            }
            
            // If Url Rewriting is disabled we simply add MP module path before yaf path, else we use a custom rewriting. 
            if (!Config.EnableURLRewriting)
            {
                    return string.Format("{0}?pageid={1}&mid={2}&{3}", scriptName, currentPage.PageId, moduleId, url);
            }
            else
            {
                FriendlyRewriter(string.Format("{0}?pageid={1}&mid={2}&{3}", scriptName, currentPage.PageId,
                                                   moduleId, url));
            }
            
            // return to the MP degault page
            return scriptName.Replace(Config.AppRoot.Trim(), "");
            
        }

        /// <summary>
        /// The custom Url rewriter
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string FriendlyRewriter(string url)
        {
            return url;
        } 
    }
}
