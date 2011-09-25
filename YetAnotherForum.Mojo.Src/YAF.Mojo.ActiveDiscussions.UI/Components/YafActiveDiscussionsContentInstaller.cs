// Author:					Joe Audette
// Created:				    2011-03-23
// Last Modified:			2011-03-26
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System.Globalization;
using System.IO;
using System.Text;
using System.Web.Hosting;
using System.Xml;
using YAF.Classes.Pattern;
using mojoPortal.Business;
using mojoPortal.Web;
using mojoPortal.Web.Framework;

namespace mojoPortal.Features.UI 
{
    public class YafActiveDiscussionContentInstaller : IContentInstaller
    {
        private ThreadSafeDictionary<string, string> mysettings;
        public void InstallContent(Module module, string configInfo)
        {
            foreach (var node in mysettings)
            {
                ModuleSettings.UpdateModuleSetting(module.ModuleGuid, module.ModuleId, node.Key, node.Value);
            }
        }

    }
}