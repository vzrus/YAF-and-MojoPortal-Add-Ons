
namespace mojoPortal.Data
{
    using System;
    using System.IO;
    using System.Text;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Configuration;


    /// <summary>
    /// Author:				Joe Audette
    /// Created:			2007-06-22
    /// Last Modified:		2007-06-22
    /// 
    /// 
    /// The use and distribution terms for this software are covered by the 
    /// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
    /// which can be found in the file CPL.TXT at the root of this distribution.
    /// By using this software in any fashion, you are agreeing to be bound by 
    /// the terms of this license.
    ///
    /// You must not remove this notice, or any other, from this software.
    ///  
    /// </summary>
    public static class DBActiveDiscussions
    {
        private static string GetConnectionString()
        {
            string conStr = string.Empty;
            if (ConfigurationManager.AppSettings["YAF.ConnectionStringName"] != null)
            {
                conStr = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["YAF.ConnectionStringName"]];
            }
            if (string.IsNullOrEmpty(conStr))
            {
                return ConfigurationManager.AppSettings["MSSQLConnectionString"];
            }
            return conStr;
        }

        public static String DBPlatform()
        {
            return "MSSQL";
        }


        /// <summary>
        /// Gets an IDataReader with all rows in the mp_Currency table.
        /// </summary>
        public static IDataReader GetSpecificSettingAllModulesWithTheDefinition(Guid guid, string settingName)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "mp_ModulePagePath_SelectByGuid", 2);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@SettingName", SqlDbType.NVarChar, ParameterDirection.Input, settingName);
            return sph.ExecuteReader();
        }
        /// <summary>
        /// Gets an IDataReader with all rows in the mp_Currency table.
        /// </summary>
        public static IDataReader GetSpecificSettingAllModulesWithTheDefinition(string srcPath, string settingName)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "mp_ModulePagePath_SelectByControlSrcPathSnippet", 2);
            sph.DefineSqlParameter("@FeaturePath", SqlDbType.UniqueIdentifier, ParameterDirection.Input, srcPath);
            sph.DefineSqlParameter("@SettingName", SqlDbType.NVarChar, ParameterDirection.Input, settingName);
            return sph.ExecuteReader();
        }
    }
}