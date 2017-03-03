using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseGraphWebsite.Models
{
    public class DPGraphNode
    {
        public string NodeType { get; set; }
        public string DBSchema { get; set; }
        public string DBName { get; set; }
        public string PageModule { get; set; }
        public string PageName { get; set; }

        public string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(NodeType))
                {
                    return string.Empty;
                }
                else if (NodeType == DPGraphNodeType.Page)
                {
                    return PageModule + "-" + PageName;
                }
                else
                {
                    return DBSchema + "." + DBName;
                }
            }
        }
    }

    public class DPGraphNodeType
    {
        public const string Table = "Table";
        public const string View = "View";
        public const string Function = "Function";
        public const string StoredProcedure = "StoredProcedure";
        public const string Page = "Page";
    }
}