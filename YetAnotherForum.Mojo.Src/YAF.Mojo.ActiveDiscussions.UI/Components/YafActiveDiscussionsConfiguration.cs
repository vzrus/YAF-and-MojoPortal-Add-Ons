﻿
using System;
using System.Collections;
using System.Configuration;
using System.Globalization;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;

namespace YAF.Mojo.ActiveDiscussions.UI
{
    public class YafActiveDiscussionsConfiguration
    {
        private int _numberToShow = 5;

        public int NumberToShow
        {
            get { return _numberToShow; }
        }

        private Guid _yafModuleDefinitionGuid;

        public Guid YafModuleDefinitionGuid
        {
            get { return _yafModuleDefinitionGuid; }
        }

        public YafActiveDiscussionsConfiguration()
        { }

        public YafActiveDiscussionsConfiguration(Hashtable settings)
        {
            LoadSettings(settings);

        }

        private void LoadSettings(Hashtable settings)
        {
            if (settings == null) { throw new ArgumentException("must pass in a hashtable of settings"); }
            _numberToShow = WebUtils.ParseInt32FromHashtable(settings, "NumberToShow", _numberToShow);
            _yafModuleDefinitionGuid = WebUtils.ParseGuidFromHashTable(settings, "YafModuleDefinitionGuid", _yafModuleDefinitionGuid);
        }
    }
}