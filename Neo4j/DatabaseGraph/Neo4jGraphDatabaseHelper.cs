using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseGraph
{
    public static class Neo4jGraphDatabaseHelper
    {
        public static GraphClient CreateNeo4jClient()
        {
            GraphClient neo4jClient = new GraphClient(new Uri("http://10.234.56.26:7474/db/data"), "neo4j", "v0cn115");
            neo4jClient.Connect();
            return neo4jClient;
        }
    }
}
