using Neo4j.Driver.V1;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4jConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var driver = GraphDatabase.Driver("bolt://localhost", AuthTokens.Basic("neo4j", "v0cn115")))
            using (var session = driver.Session())
            {
                //session.Run("CALL dbms.changePassword('v0cn115')");
                //session.Run("create (t:table{dbname:'trtbBooking'})");
                var result = session.Run("match (t:table) return t");

                foreach (var record in result)
                {
                    Console.WriteLine("table name: " + record["t"].As<string>() + Environment.NewLine);
                }
            }
            Console.ReadKey();

        }
    }
}
