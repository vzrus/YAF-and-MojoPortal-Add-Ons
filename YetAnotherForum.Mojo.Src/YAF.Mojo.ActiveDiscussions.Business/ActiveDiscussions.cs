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


        public ActiveDiscussions(Guid guid)
        {
            GetCurrency(guid);
        }

        #endregion

        #region Private Properties

        private Guid guid = Guid.Empty;
        private string title = string.Empty;
        private string code = "USD";
        private string symbolLeft = "$";
        private string symbolRight = string.Empty;
        private string decimalPointChar = ".";
        private string thousandsPointChar = ",";
        private string decimalPlaces = string.Empty;
        private decimal value = 1;
        private DateTime lastModified = DateTime.UtcNow;
        private DateTime created = DateTime.UtcNow;

        #endregion

        #region Public Properties

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public string Code
        {
            get { return code; }
            set { code = value; }
        }
        public string SymbolLeft
        {
            get { return symbolLeft; }
            set { symbolLeft = value; }
        }
        public string SymbolRight
        {
            get { return symbolRight; }
            set { symbolRight = value; }
        }
        public string DecimalPointChar
        {
            get { return decimalPointChar; }
            set { decimalPointChar = value; }
        }
        public string ThousandsPointChar
        {
            get { return thousandsPointChar; }
            set { thousandsPointChar = value; }
        }
        public string DecimalPlaces
        {
            get { return decimalPlaces; }
            set { decimalPlaces = value; }
        }
        public decimal Value
        {
            get { return value; }
            set { this.value = value; }
        }
        public DateTime LastModified
        {
            get { return lastModified; }
            set { lastModified = value; }
        }
        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }

        #endregion

        #region Private Methods

        private void GetCurrency(Guid guid)
        {
            using (IDataReader reader = DBActiveDiscussions.GetOne(guid))
            {
                if (reader.Read())
                {
                    this.guid = new Guid(reader["Guid"].ToString());
                    this.title = reader["Title"].ToString();
                    this.code = reader["Code"].ToString();
                    this.symbolLeft = reader["SymbolLeft"].ToString();
                    this.symbolRight = reader["SymbolRight"].ToString();
                    this.decimalPointChar = reader["DecimalPointChar"].ToString();
                    this.thousandsPointChar = reader["ThousandsPointChar"].ToString();
                    this.decimalPlaces = reader["DecimalPlaces"].ToString();
                    this.value = Convert.ToDecimal(reader["Value"]);
                    this.lastModified = Convert.ToDateTime(reader["LastModified"]);
                    this.created = Convert.ToDateTime(reader["Created"]);

                }

            }

        }

        private bool Create()
        {
            Guid newID = Guid.NewGuid();

            this.guid = newID;

            int rowsAffected = DBActiveDiscussions.Create(
                this.guid,
                this.title,
                this.code,
                this.symbolLeft,
                this.symbolRight,
                this.decimalPointChar,
                this.thousandsPointChar,
                this.decimalPlaces,
                this.value,
                this.lastModified,
                this.created);

            return (rowsAffected > 0);

        }



        private bool Update()
        {

            return DBActiveDiscussions.Update(
                this.guid,
                this.title,
                this.code,
                this.symbolLeft,
                this.symbolRight,
                this.decimalPointChar,
                this.thousandsPointChar,
                this.decimalPlaces,
                this.value,
                this.lastModified);

        }


        #endregion

        #region Public Methods


        public bool Save()
        {
            if (this.guid != Guid.Empty)
            {
                return Update();
            }
            else
            {
                return Create();
            }
        }




        #endregion

        #region Static Methods

        public static bool Delete(Guid guid)
        {
            return DBActiveDiscussions.Delete(guid);
        }


        public static DataTable GetAll(Guid guid, string settingName)
        { 
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("PageID", typeof(int));
            dataTable.Columns.Add("ModuleID", typeof(int));
            dataTable.Columns.Add("ModuleTitle", typeof(string));
            dataTable.Columns.Add("SettingValue", typeof(string));
            dataTable.Columns.Add("SiteID", typeof(int));
            using (IDataReader reader = DBActiveDiscussions.GetAll(guid, settingName))
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

        public static DataTable GetAll(string srcPath, string settingName)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("PageID", typeof(int));
            dataTable.Columns.Add("ModuleID", typeof(int));
            dataTable.Columns.Add("ModuleTitle", typeof(string));
            dataTable.Columns.Add("SettingValue", typeof(string));
            dataTable.Columns.Add("SiteID", typeof(int));

            using (IDataReader reader = DBActiveDiscussions.GetAll(srcPath, settingName))
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