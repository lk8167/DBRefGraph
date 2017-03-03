using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseGraphWebsite.Models
{
    public class DPGraphRelationship
    {
        public string SourceNodeType { get; set; }
        public string SourceNodeName { get; set; }
        public string TargetNodeType { get; set; }
        public string TargetNodeName { get; set; }
    }
}