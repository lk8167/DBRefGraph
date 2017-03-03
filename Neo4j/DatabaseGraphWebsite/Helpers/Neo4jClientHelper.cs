using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseGraphWebsite.Helpers
{
    public static class Neo4jClientHelper
    {
        public static GraphClient CreateNeo4jClient()
        {
            GraphClient neo4jClient = new GraphClient(new Uri("http://localhost:7474/db/data"), "neo4j", "v0cn115");
            neo4jClient.Connect();
            return neo4jClient;
        }
    }
}