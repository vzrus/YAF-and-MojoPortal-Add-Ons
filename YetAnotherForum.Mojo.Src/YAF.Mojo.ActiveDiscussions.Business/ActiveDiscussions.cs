using System;
using System.Data;
using mojoPortal.Data;

namespace mojoPortal.Business
{
    /// <summary>
    ///
    /// </summary>
    public class ActiveDiscussions
    {

        #region Constructors

        public ActiveDiscussions()
        { }

        #endregion

        #region Private Properties

        private Guid guid = Guid.Empty;
       

        #endregion

        #region Public Properties

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }


        #endregion


        #region Static Methods


        public static DataTable GetSpecificSettingAllModulesWithTheDefinition(Guid guid, string settingName)
        { 
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("PageID", typeof(int));
            dataTable.Columns.Add("ModuleID", typeof(int));
            dataTable.Columns.Add("ModuleTitle", typeof(string));
            dataTable.Columns.Add("SettingValue", typeof(string));
            dataTable.Columns.Add("SiteID", typeof(int));
            using (IDataReader reader = DBActiveDiscussions.GetSpecificSettingAllModulesWithTheDefinition(guid, settingName))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["PageID"] = reader["PageID"];
                    row["ModuleID"] = reader["ModuleID"];
                    row["ModuleTitle"] = reader["ModuleTitle"];
                    row["SettingValue"] = reader["SettingValue"];
                    row["SiteID"] = reader["SiteID"];

                    dataTable.Rows.Add(row);
                }

            }

            return dataTable;
        }

        public static DataTable GetSpecificSettingAllModulesWithTheDefinition(string srcPath, string settingName)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("PageID", typeof(int));
            dataTable.Columns.Add("ModuleID", typeof(int));
            dataTable.Columns.Add("ModuleTitle", typeof(string));
            dataTable.Columns.Add("SettingValue", typeof(string));
            dataTable.Columns.Add("SiteID", typeof(int));

            using (IDataReader reader = DBActiveDiscussions.GetSpecificSettingAllModulesWithTheDefinition(srcPath, settingName))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["PageID"] = reader["PageID"];
                    row["ModuleID"] = reader["ModuleID"];
                    row["ModuleTitle"] = reader["ModuleTitle"];
                    row["SettingValue"] = reader["SettingValue"];
                    row["SiteID"] = reader["SiteID"];

                    dataTable.Rows.Add(row);
                }

            }

            return dataTable;
        }

        #endregion


    }

}