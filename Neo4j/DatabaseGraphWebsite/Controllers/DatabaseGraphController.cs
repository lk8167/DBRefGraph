using DatabaseGraphWebsite.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Http;

namespace DatabaseGraphWebsite.Controllers
{
    public class DatabaseGraphController : ApiController
    {
        [HttpPost]
        public IHttpActionResult CreateNode(DPGraphNode node)
        {
            DPGraphDAO.CreateDPGraphNode(node);
            return Ok();
        }

        public IEnumerable<DPGraphNode> GetNodesByType(string nodeType)
        {
            IEnumerable<DPGraphNode> nodes = DPGraphDAO.GetNodesByNodeType(nodeType);
            return nodes;
        }

        [HttpPost]
        public IHttpActionResult CreateRelationship(DPGraphRelationship relationship)
        {
            bool isValid = DPGraphDAO.CreateDPGraphRelationship(relationship);
            if (isValid)
            {
                return Ok();
            }
            else
            {
                return InternalServerError(new Exception("Invalid relationship between source type and target type"));
            }
        }

        public IEnumerable<DPGraphNode> GetNodesFromSource(string sourceNodeType, string sourceNodeName)
        {
            IEnumerable<DPGraphNode> targetNodes = DPGraphDAO.GetNodesFromSourceNode(sourceNodeType, sourceNodeName);
            return targetNodes;
        }

        public IEnumerable<DPGraphNode> GetNodesFromTarget(string targetNodeType, string targetNodeName)
        {
            IEnumerable<DPGraphNode> targetNodes = DPGraphDAO.GetNodesFromTargetNode(targetNodeType, targetNodeName);
            return targetNodes;
        }
    }
}
