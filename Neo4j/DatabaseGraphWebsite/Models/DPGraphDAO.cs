using DatabaseGraphWebsite.Helpers;
using Neo4jClient;
using System.Collections.Generic;
using System.Linq;

namespace DatabaseGraphWebsite.Models
{
    public static class DPGraphDAO
    {
        public static void CreateDPGraphNode(DPGraphNode newNode)
        {
            GraphClient neo4jClient = Neo4jClientHelper.CreateNeo4jClient();
            switch (newNode.NodeType)
            {
                case DPGraphNodeType.Table:
                    DBTable table = new DBTable() { Schema = newNode.DBSchema, Name = newNode.DBName };
                    neo4jClient.Cypher
                                .Merge("(t:Table { Name: {name} })")
                                .OnCreate()
                                .Set("t = {newTable}")
                                .WithParams(new
                                {
                                    name = table.Name,
                                    newTable = table
                                })
                                .ExecuteWithoutResults();
                    break;
                case DPGraphNodeType.View:
                    DBView view = new DBView() { Schema = newNode.DBSchema, Name = newNode.DBName };
                    neo4jClient.Cypher
                                .Merge("(v:View { Name: {name} })")
                                .OnCreate()
                                .Set("v = {newView}")
                                .WithParams(new
                                {
                                    name = view.Name,
                                    newView = view
                                })
                                .ExecuteWithoutResults();
                    break;
                case DPGraphNodeType.Function:
                    DBFunction function = new DBFunction() { Schema = newNode.DBSchema, Name = newNode.DBName };
                    neo4jClient.Cypher
                               .Merge("(f:Function { Name: {name} })")
                               .OnCreate()
                               .Set("f = {newFunction}")
                               .WithParams(new
                               {
                                   name = function.Name,
                                   newFunction = function
                               })
                               .ExecuteWithoutResults();
                    break;
                case DPGraphNodeType.StoredProcedure:
                    DBStoredProcedure sp = new DBStoredProcedure() { Schema = newNode.DBSchema, Name = newNode.DBName };
                    neo4jClient.Cypher
                               .Merge("(sp:StoredProcedure { Name: {name} })")
                               .OnCreate()
                               .Set("sp = {newSP}")
                               .WithParams(new
                               {
                                   name = sp.Name,
                                   newSP = sp
                               })
                               .ExecuteWithoutResults();
                    break;
                case DPGraphNodeType.Page:
                    Page page = new Page() { Module = newNode.PageModule, Name = newNode.PageName };
                    neo4jClient.Cypher
                               .Merge("(p:Page { Name: {name} })")
                               .OnCreate()
                               .Set("p = {newPage}")
                               .WithParams(new
                               {
                                   name = page.Name,
                                   newPage = page
                               })
                               .ExecuteWithoutResults();
                    break;
            }
        }
        public static List<DPGraphNode> GetNodesByNodeType(string nodeType)
        {
            List<DPGraphNode> nodeList = new List<DPGraphNode>();
            GraphClient neo4jClient = Neo4jClientHelper.CreateNeo4jClient();
            switch (nodeType)
            {
                case DPGraphNodeType.Table:
                    List<DBTable> tables = neo4jClient.Cypher.Match("(t:Table)").Return(t => t.As<DBTable>()).Results.ToList();
                    foreach (var t in tables)
                    {
                        nodeList.Add(new DPGraphNode { NodeType = DPGraphNodeType.Table,DBSchema=t.Schema,DBName=t.Name});
                    }
                    break;
                case DPGraphNodeType.View:
                    List<DBView> views = neo4jClient.Cypher.Match("(v:View)").Return(v => v.As<DBView>()).Results.ToList();
                    foreach (var v in views)
                    {
                        nodeList.Add(new DPGraphNode { NodeType = DPGraphNodeType.View, DBSchema=v.Schema,DBName=v.Name});
                    }
                    break;
                case DPGraphNodeType.Function:
                    List<DBFunction> functions = neo4jClient.Cypher.Match("(f:Function)").Return(f => f.As<DBFunction>()).Results.ToList();
                    foreach (var f in functions)
                    {
                        nodeList.Add(new DPGraphNode { NodeType = DPGraphNodeType.Function, DBSchema = f.Schema, DBName = f.Name });
                    }
                    break;
                case DPGraphNodeType.StoredProcedure:
                    List<DBStoredProcedure> storedProcedures = neo4jClient.Cypher.Match("(sp:StoredProcedure)").Return(sp => sp.As<DBStoredProcedure>()).Results.ToList();
                    foreach (var sp in storedProcedures)
                    {
                        nodeList.Add(new DPGraphNode { NodeType = DPGraphNodeType.Function, DBSchema = sp.Schema, DBName = sp.Name });
                    }
                    break;
                case DPGraphNodeType.Page:
                    List<Page> pages = neo4jClient.Cypher.Match("(p:Page)").Return(p => p.As<Page>()).Results.ToList();
                    foreach (var p in pages)
                    {
                        nodeList.Add(new DPGraphNode { NodeType = DPGraphNodeType.Page, PageModule = p.Module, PageName = p.Name });
                    }
                    break;
            }
            return nodeList;
        }
        public static bool CreateDPGraphRelationship(DPGraphRelationship newRelationship)
        {
            bool isValidRelationship = true;
            string sourceDBName, sourceDBSchema, targetDBName, targetDBSchema, sourcePageModule, sourcePageName;
            GraphClient neo4jClient = Neo4jClientHelper.CreateNeo4jClient();
            switch (newRelationship.SourceNodeType)
            {
                case DPGraphNodeType.Table:
                    isValidRelationship = false;
                    break;
                case DPGraphNodeType.View:
                    sourceDBName = ParseDBNameFromDisplayName(newRelationship.SourceNodeName);
                    sourceDBSchema = ParseDBSchemaFromDisplayName(newRelationship.SourceNodeName);
                    targetDBName = ParseDBNameFromDisplayName(newRelationship.TargetNodeName);
                    targetDBSchema = ParseDBSchemaFromDisplayName(newRelationship.TargetNodeName);
                    if (newRelationship.TargetNodeType == DPGraphNodeType.Table)
                    {
                        neo4jClient.Cypher
                                    .Match("(v:View)", "(t:Table)")
                                    .Where((DBView v) => v.Name == sourceDBName && v.Schema == sourceDBSchema)
                                    .AndWhere((DBTable t) => t.Name == targetDBName && t.Schema == targetDBSchema)
                                    .CreateUnique("(v)-[:Reference]->(t)")
                                    .ExecuteWithoutResults();
                    }
                    else if (newRelationship.TargetNodeType == DPGraphNodeType.View)
                    {
                        neo4jClient.Cypher
                                   .Match("(v1:View)", "(v2:View)")
                                   .Where((DBView v1) => v1.Name == sourceDBName && v1.Schema == sourceDBSchema)
                                   .AndWhere((DBView v2) => v2.Name == targetDBName && v2.Schema == targetDBSchema)
                                   .CreateUnique("(v1)-[:Reference]->(v2)")
                                   .ExecuteWithoutResults();
                    }
                    else if (newRelationship.TargetNodeType == DPGraphNodeType.Function)
                    {
                        neo4jClient.Cypher
                                   .Match("(v:View)", "(f:Function)")
                                   .Where((DBView v) => v.Name == sourceDBName && v.Schema == sourceDBSchema)
                                   .AndWhere((DBFunction f) => f.Name == targetDBName && f.Schema == targetDBSchema)
                                   .CreateUnique("(v)-[:Call]->(f)")
                                   .ExecuteWithoutResults();
                    }
                    else
                    {
                        isValidRelationship = false;
                    }
                    break;
                case DPGraphNodeType.Function:
                    sourceDBName = ParseDBNameFromDisplayName(newRelationship.SourceNodeName);
                    sourceDBSchema = ParseDBSchemaFromDisplayName(newRelationship.SourceNodeName);
                    targetDBName = ParseDBNameFromDisplayName(newRelationship.TargetNodeName);
                    targetDBSchema = ParseDBSchemaFromDisplayName(newRelationship.TargetNodeName);
                    if (newRelationship.TargetNodeType == DPGraphNodeType.Table)
                    {
                        neo4jClient.Cypher
                                    .Match("(f:Function)", "(t:Table)")
                                    .Where((DBFunction f) => f.Name == sourceDBName && f.Schema == sourceDBSchema)
                                    .AndWhere((DBTable t) => t.Name == targetDBName && t.Schema == targetDBSchema)
                                    .CreateUnique("(f)-[:Reference]->(t)")
                                    .ExecuteWithoutResults();
                    }
                    else if (newRelationship.TargetNodeType == DPGraphNodeType.View)
                    {
                        neo4jClient.Cypher
                                   .Match("(f:Function)", "(v:View)")
                                   .Where((DBFunction f) => f.Name == sourceDBName && f.Schema == sourceDBSchema)
                                   .AndWhere((DBView v) => v.Name == targetDBName && v.Schema == targetDBSchema)
                                   .CreateUnique("(f)-[:Reference]->(v)")
                                   .ExecuteWithoutResults();
                    }
                    else if (newRelationship.TargetNodeType == DPGraphNodeType.Function)
                    {
                        neo4jClient.Cypher
                                   .Match("(f1:Function)", "(f2:Function)")
                                   .Where((DBFunction f1) => f1.Name == sourceDBName && f1.Schema == sourceDBSchema)
                                   .AndWhere((DBFunction f2) => f2.Name == targetDBName && f2.Schema == targetDBSchema)
                                   .CreateUnique("(f1)-[:Call]->(f2)")
                                   .ExecuteWithoutResults();
                    }
                    else
                    {
                        isValidRelationship = false;
                    }
                    break;
                case DPGraphNodeType.StoredProcedure:
                    sourceDBName = ParseDBNameFromDisplayName(newRelationship.SourceNodeName);
                    sourceDBSchema = ParseDBSchemaFromDisplayName(newRelationship.SourceNodeName);
                    targetDBName = ParseDBNameFromDisplayName(newRelationship.TargetNodeName);
                    targetDBSchema = ParseDBSchemaFromDisplayName(newRelationship.TargetNodeName);
                    if (newRelationship.TargetNodeType == DPGraphNodeType.Table)
                    {
                        neo4jClient.Cypher
                                    .Match("(sp:StoredProcedure)", "(t:Table)")
                                    .Where((DBStoredProcedure sp) => sp.Name == sourceDBName && sp.Schema == sourceDBSchema)
                                    .AndWhere((DBTable t) => t.Name == targetDBName && t.Schema == targetDBSchema)
                                    .CreateUnique("(sp)-[:Reference]->(t)")
                                    .ExecuteWithoutResults();
                    }
                    else if (newRelationship.TargetNodeType == DPGraphNodeType.View)
                    {
                        neo4jClient.Cypher
                                   .Match("(sp:StoredProcedure)", "(v:View)")
                                   .Where((DBStoredProcedure sp) => sp.Name == sourceDBName && sp.Schema == sourceDBSchema)
                                   .AndWhere((DBView v) => v.Name == targetDBName && v.Schema == targetDBSchema)
                                   .CreateUnique("(sp)-[:Reference]->(v)")
                                   .ExecuteWithoutResults();
                    }
                    else if (newRelationship.TargetNodeType == DPGraphNodeType.Function)
                    {
                        neo4jClient.Cypher
                                   .Match("(sp:StoredProcedure)", "(f:Function)")
                                   .Where((DBStoredProcedure sp) => sp.Name == sourceDBName && sp.Schema == sourceDBSchema)
                                   .AndWhere((DBFunction f) => f.Name == targetDBName && f.Schema == targetDBSchema)
                                   .CreateUnique("(sp)-[:Call]->(f)")
                                   .ExecuteWithoutResults();
                    }
                    else if (newRelationship.TargetNodeType == DPGraphNodeType.StoredProcedure)
                    {
                        neo4jClient.Cypher
                                  .Match("(sp1:StoredProcedure)", "(sp2:StoredProcedure)")
                                  .Where((DBStoredProcedure sp1) => sp1.Name == sourceDBName && sp1.Schema == sourceDBSchema)
                                  .AndWhere((DBStoredProcedure sp2) => sp2.Name == targetDBName && sp2.Schema == targetDBSchema)
                                  .CreateUnique("(sp1)-[:Call]->(sp2)")
                                  .ExecuteWithoutResults();
                    }
                    else
                    {
                        isValidRelationship = false;
                    }
                    break;
                case DPGraphNodeType.Page:
                    sourcePageModule = ParsePageModuleFromDisplayName(newRelationship.SourceNodeName);
                    sourcePageName = ParsePageNameFromDisplayName(newRelationship.SourceNodeName);
                    targetDBName = ParseDBNameFromDisplayName(newRelationship.TargetNodeName);
                    targetDBSchema = ParseDBSchemaFromDisplayName(newRelationship.TargetNodeName);
                    if (newRelationship.TargetNodeType == DPGraphNodeType.StoredProcedure)
                    {
                        neo4jClient.Cypher
                                  .Match("(p:Page)", "(sp:StoredProcedure)")
                                  .Where((Page p) => p.Name == sourcePageName && p.Module == sourcePageModule)
                                  .AndWhere((DBStoredProcedure sp) => sp.Name == targetDBName && sp.Schema == targetDBSchema)
                                  .CreateUnique("(p)-[:Call]->(sp)")
                                  .ExecuteWithoutResults();
                    }
                    else
                    {
                        isValidRelationship = false;
                    }
                    break;
            }
            return isValidRelationship;
        }
        public static List<DPGraphNode> GetNodesFromSourceNode(string sourceNodeType,string sourceNodeName)
        {
            List<DPGraphNode> targetNodes = new List<DPGraphNode>();
            GraphClient neo4jClient = Neo4jClientHelper.CreateNeo4jClient();
            string sourceDBSchema = string.Empty;
            string sourceDBName = string.Empty;
            string sourcePageModule = string.Empty;
            string sourcePageName = string.Empty;
            if (sourceNodeType == DPGraphNodeType.Page)
            {
                sourcePageModule = ParsePageModuleFromDisplayName(sourceNodeName);
                sourcePageName = ParsePageNameFromDisplayName(sourceNodeName);
            }
            else
            {
                sourceDBSchema = ParseDBSchemaFromDisplayName(sourceNodeName);
                sourceDBName = ParseDBNameFromDisplayName(sourceNodeName);
            }
            switch (sourceNodeType)
            {
                case DPGraphNodeType.View:
                    List<DBTable> linkedDBTablesFromView = neo4jClient.Cypher
                                                               .Match("(v:View)-[:Reference]->(t:Table)")
                                                               .Where((DBView v) => v.Schema == sourceDBSchema && v.Name == sourceDBName)
                                                               .Return(t => t.As<DBTable>())
                                                               .Results
                                                               .ToList();

                    if (linkedDBTablesFromView != null && linkedDBTablesFromView.Count > 0)
                    {
                        foreach (var t in linkedDBTablesFromView)
                        {
                            targetNodes.Add(new DPGraphNode { NodeType = DPGraphNodeType.Table, DBSchema = t.Schema, DBName = t.Name });
                        }
                    }
                    List<DBView> linkedDBViewsFromView = neo4jClient.Cypher
                                                               .Match("(v1:View)-[:Reference]->(v2:View)")
                                                               .Where((DBView v1) => v1.Schema == sourceDBSchema && v1.Name == sourceDBName)
                                                               .Return(v2 => v2.As<DBView>())
                                                               .Results
                                                               .ToList();
                    if (linkedDBViewsFromView != null && linkedDBViewsFromView.Count > 0)
                    {
                        foreach (var v in linkedDBViewsFromView)
                        {
                            targetNodes.Add(new DPGraphNode { NodeType = DPGraphNodeType.View, DBSchema = v.Schema, DBName = v.Name });
                        }
                    }
                    List<DBFunction> linkedDBFunctionsFromView = neo4jClient.Cypher
                                                               .Match("(v:View)-[:Call]->(f:Function)")
                                                               .Where((DBView v) => v.Schema == sourceDBSchema && v.Name == sourceDBName)
                                                               .Return(f => f.As<DBFunction>())
                                                               .Results
                                                               .ToList();
                    if (linkedDBFunctionsFromView != null && linkedDBFunctionsFromView.Count > 0)
                    {
                        foreach (var f in linkedDBFunctionsFromView)
                        {
                            targetNodes.Add(new DPGraphNode { NodeType = DPGraphNodeType.Function, DBSchema = f.Schema, DBName = f.Name });
                        }
                    }
                    break;
                case DPGraphNodeType.Function:
                    List<DBTable> linkedDBTablesFromFunction = neo4jClient.Cypher
                                                               .Match("(f:Function)-[:Reference]->(t:Table)")
                                                               .Where((DBFunction f) => f.Schema == sourceDBSchema && f.Name == sourceDBName)
                                                               .Return(t => t.As<DBTable>())
                                                               .Results
                                                               .ToList();

                    if (linkedDBTablesFromFunction != null && linkedDBTablesFromFunction.Count > 0)
                    {
                        foreach (var t in linkedDBTablesFromFunction)
                        {
                            targetNodes.Add(new DPGraphNode { NodeType = DPGraphNodeType.Table, DBSchema = t.Schema, DBName = t.Name });
                        }
                    }
                    List<DBView> linkedDBViewsFromFunction = neo4jClient.Cypher
                                                               .Match("(f:Function)-[:Reference]->(v:View)")
                                                               .Where((DBFunction f) => f.Schema == sourceDBSchema && f.Name == sourceDBName)
                                                               .Return(v => v.As<DBView>())
                                                               .Results
                                                               .ToList();
                    if (linkedDBViewsFromFunction != null && linkedDBViewsFromFunction.Count > 0)
                    {
                        foreach (var v in linkedDBViewsFromFunction)
                        {
                            targetNodes.Add(new DPGraphNode { NodeType = DPGraphNodeType.View, DBSchema = v.Schema, DBName = v.Name });
                        }
                    }
                    List<DBFunction> linkedDBFunctionsFromFunction = neo4jClient.Cypher
                                                               .Match("(f1:Function)-[:Call]->(f2:Function)")
                                                               .Where((DBFunction f1) => f1.Schema == sourceDBSchema && f1.Name == sourceDBName)
                                                               .Return(f2 => f2.As<DBFunction>())
                                                               .Results
                                                               .ToList();
                    if (linkedDBFunctionsFromFunction != null && linkedDBFunctionsFromFunction.Count > 0)
                    {
                        foreach (var f in linkedDBFunctionsFromFunction)
                        {
                            targetNodes.Add(new DPGraphNode { NodeType = DPGraphNodeType.Function, DBSchema = f.Schema, DBName = f.Name });
                        }
                    }
                    break;
                case DPGraphNodeType.StoredProcedure:
                    List<DBTable> linkedDBTablesFromSP = neo4jClient.Cypher
                                                               .Match("(sp:StoredProcedure)-[r:Reference]->(t:Table)")
                                                               .Where((DBStoredProcedure sp) => sp.Schema == sourceDBSchema && sp.Name == sourceDBName)
                                                               .Return(t => t.As<DBTable>())
                                                               .Results
                                                               .ToList();

                    if (linkedDBTablesFromSP != null && linkedDBTablesFromSP.Count > 0)
                    {
                        foreach (var t in linkedDBTablesFromSP)
                        {
                            targetNodes.Add(new DPGraphNode { NodeType = DPGraphNodeType.Table, DBSchema = t.Schema, DBName = t.Name });
                        }
                    }
                    List<DBView> linkedDBViewsFromSP = neo4jClient.Cypher
                                                               .Match("(sp:StoredProcedure)-[r:Reference]->(v:View)")
                                                               .Where((DBStoredProcedure sp) => sp.Schema == sourceDBSchema && sp.Name == sourceDBName)
                                                               .Return(v => v.As<DBView>())
                                                               .Results
                                                               .ToList();
                    if (linkedDBViewsFromSP != null && linkedDBViewsFromSP.Count > 0)
                    {
                        foreach (var v in linkedDBViewsFromSP)
                        {
                            targetNodes.Add(new DPGraphNode { NodeType = DPGraphNodeType.View, DBSchema = v.Schema, DBName = v.Name });
                        }
                    }
                    List<DBFunction> linkedDBFunctionsFromSP = neo4jClient.Cypher
                                                               .Match("(sp:StoredProcedure)-[r:Call]->(f:Function)")
                                                               .Where((DBStoredProcedure sp) => sp.Schema == sourceDBSchema && sp.Name == sourceDBName)
                                                               .Return(f => f.As<DBFunction>())
                                                               .Results
                                                               .ToList();
                    if (linkedDBFunctionsFromSP != null && linkedDBFunctionsFromSP.Count > 0)
                    {
                        foreach (var f in linkedDBFunctionsFromSP)
                        {
                            targetNodes.Add(new DPGraphNode { NodeType = DPGraphNodeType.Function, DBSchema = f.Schema, DBName = f.Name });
                        }
                    }
                    List<DBStoredProcedure> linkedDBSPsFromSP = neo4jClient.Cypher
                                                               .Match("(sp1:StoredProcedure)-[r:Call]->(sp2:StoredProcedure)")
                                                               .Where((DBStoredProcedure sp1) => sp1.Schema == sourceDBSchema && sp1.Name == sourceDBName)
                                                               .Return(sp2 => sp2.As<DBStoredProcedure>())
                                                               .Results
                                                               .ToList();
                    if (linkedDBSPsFromSP != null && linkedDBSPsFromSP.Count > 0)
                    {
                        foreach (var sp in linkedDBSPsFromSP)
                        {
                            targetNodes.Add(new DPGraphNode { NodeType = DPGraphNodeType.StoredProcedure, DBSchema = sp.Schema, DBName = sp.Name });
                        }
                    }
                    break;
                case DPGraphNodeType.Page:
                    List<DBStoredProcedure> linkedDBSPsFromPage = neo4jClient.Cypher
                                                               .Match("(p:Page)-[r:Call]->(sp:StoredProcedure)")
                                                               .Where((Page p) => p.Module == sourcePageModule && p.Name == sourcePageName)
                                                               .Return(sp => sp.As<DBStoredProcedure>())
                                                               .Results
                                                               .ToList();
                    if (linkedDBSPsFromPage != null && linkedDBSPsFromPage.Count > 0)
                    {
                        foreach (var sp in linkedDBSPsFromPage)
                        {
                            targetNodes.Add(new DPGraphNode { NodeType = DPGraphNodeType.StoredProcedure, DBSchema = sp.Schema, DBName = sp.Name });
                        }
                    }
                    break;
            }
            return targetNodes;
        }
        public static List<DPGraphNode> GetNodesFromTargetNode(string targetNodeType, string targetNodeName)
        {
            List<DPGraphNode> sourceNodes = new List<DPGraphNode>();
            GraphClient neo4jClient = Neo4jClientHelper.CreateNeo4jClient();
            string targetDBSchema = string.Empty;
            string targetDBName = string.Empty;
            string targetPageModule = string.Empty;
            string targetPageName = string.Empty;
            if (targetNodeType == DPGraphNodeType.Page)
            {
                targetPageModule = ParsePageModuleFromDisplayName(targetNodeName);
                targetPageName = ParsePageNameFromDisplayName(targetNodeName);
            }
            else
            {
                targetDBSchema = ParseDBSchemaFromDisplayName(targetNodeName);
                targetDBName = ParseDBNameFromDisplayName(targetNodeName);
            }
            switch (targetNodeType)
            {
                case DPGraphNodeType.Table:
                    List<DBView> linkedFromViewsOfTable = neo4jClient.Cypher
                                                              .Match("(t:Table)<-[:Reference]-(v:View)")
                                                              .Where((DBTable t) => t.Schema == targetDBSchema && t.Name == targetDBName)
                                                              .Return(v => v.As<DBView>())
                                                              .Results
                                                              .ToList();

                    if (linkedFromViewsOfTable != null && linkedFromViewsOfTable.Count > 0)
                    {
                        foreach (var v in linkedFromViewsOfTable)
                        {
                            sourceNodes.Add(new DPGraphNode { NodeType = DPGraphNodeType.View, DBSchema = v.Schema, DBName = v.Name });
                        }
                    }
                    List<DBFunction> linkedFromFunctionsOfTable = neo4jClient.Cypher
                                                              .Match("(t:Table)<-[:Reference]-(f:Function)")
                                                              .Where((DBTable t) => t.Schema == targetDBSchema && t.Name == targetDBName)
                                                              .Return(f => f.As<DBFunction>())
                                                              .Results
                                                              .ToList();

                    if (linkedFromFunctionsOfTable != null && linkedFromFunctionsOfTable.Count > 0)
                    {
                        foreach (var f in linkedFromFunctionsOfTable)
                        {
                            sourceNodes.Add(new DPGraphNode { NodeType = DPGraphNodeType.Function, DBSchema = f.Schema, DBName = f.Name });
                        }
                    }
                    List<DBStoredProcedure> linkedFromSPsOfTable = neo4jClient.Cypher
                                                              .Match("(t:Table)<-[:Reference]-(sp:StoredProcedure)")
                                                              .Where((DBTable t) => t.Schema == targetDBSchema && t.Name == targetDBName)
                                                              .Return(sp => sp.As<DBStoredProcedure>())
                                                              .Results
                                                              .ToList();

                    if (linkedFromSPsOfTable != null && linkedFromSPsOfTable.Count > 0)
                    {
                        foreach (var sp in linkedFromSPsOfTable)
                        {
                            sourceNodes.Add(new DPGraphNode { NodeType = DPGraphNodeType.StoredProcedure, DBSchema = sp.Schema, DBName = sp.Name });
                        }
                    }
                    break;
                case DPGraphNodeType.View:
                    List<DBView> linkedFromViewsOfView = neo4jClient.Cypher
                                                               .Match("(v1:View)<-[:Reference]-(v2:View)")
                                                               .Where((DBView v1) => v1.Schema == targetDBSchema && v1.Name == targetDBName)
                                                               .Return(v2 => v2.As<DBView>())
                                                               .Results
                                                               .ToList();

                    if (linkedFromViewsOfView != null && linkedFromViewsOfView.Count > 0)
                    {
                        foreach (var v in linkedFromViewsOfView)
                        {
                            sourceNodes.Add(new DPGraphNode { NodeType = DPGraphNodeType.View, DBSchema = v.Schema, DBName = v.Name });
                        }
                    }
                    List<DBFunction> linkedFromFunctionOfView = neo4jClient.Cypher
                                                               .Match("(v:View)<-[:Reference]-(f:Function)")
                                                               .Where((DBView v) => v.Schema == targetDBSchema && v.Name == targetDBName)
                                                               .Return(f => f.As<DBFunction>())
                                                               .Results
                                                               .ToList();
                    if (linkedFromFunctionOfView != null && linkedFromFunctionOfView.Count > 0)
                    {
                        foreach (var f in linkedFromFunctionOfView)
                        {
                            sourceNodes.Add(new DPGraphNode { NodeType = DPGraphNodeType.Function, DBSchema = f.Schema, DBName = f.Name });
                        }
                    }
                    List<DBStoredProcedure> linkedFromSPsOfView = neo4jClient.Cypher
                                                               .Match("(v:View)<-[:Reference]-(sp:StoredProcedure)")
                                                               .Where((DBView v) => v.Schema == targetDBSchema && v.Name == targetDBName)
                                                               .Return(sp => sp.As<DBStoredProcedure>())
                                                               .Results
                                                               .ToList();
                    if (linkedFromSPsOfView != null && linkedFromSPsOfView.Count > 0)
                    {
                        foreach (var sp in linkedFromSPsOfView)
                        {
                            sourceNodes.Add(new DPGraphNode { NodeType = DPGraphNodeType.StoredProcedure, DBSchema = sp.Schema, DBName = sp.Name });
                        }
                    }
                    break;
                case DPGraphNodeType.Function:
                    List<DBView> linkedFromViewsOfFunction = neo4jClient.Cypher
                                                               .Match("(f:Function)<-[:Call]-(v:View)")
                                                               .Where((DBFunction f) => f.Schema == targetDBSchema && f.Name == targetDBName)
                                                               .Return(v => v.As<DBView>())
                                                               .Results
                                                               .ToList();

                    if (linkedFromViewsOfFunction != null && linkedFromViewsOfFunction.Count > 0)
                    {
                        foreach (var v in linkedFromViewsOfFunction)
                        {
                            sourceNodes.Add(new DPGraphNode { NodeType = DPGraphNodeType.View, DBSchema = v.Schema, DBName = v.Name });
                        }
                    }
                    List<DBFunction> linkedFromFunctionOfFunction = neo4jClient.Cypher
                                                               .Match("(f1:Function)<-[:Call]-(f2:Function)")
                                                               .Where((DBFunction f1) => f1.Schema == targetDBSchema && f1.Name == targetDBName)
                                                               .Return(f2 => f2.As<DBFunction>())
                                                               .Results
                                                               .ToList();
                    if (linkedFromFunctionOfFunction != null && linkedFromFunctionOfFunction.Count > 0)
                    {
                        foreach (var f in linkedFromFunctionOfFunction)
                        {
                            sourceNodes.Add(new DPGraphNode { NodeType = DPGraphNodeType.Function, DBSchema = f.Schema, DBName = f.Name });
                        }
                    }
                    List<DBStoredProcedure> linkedFromSPsOfFunction = neo4jClient.Cypher
                                                               .Match("(f:Function)<-[:Call]-(sp:StoredProcedure)")
                                                               .Where((DBFunction f) => f.Schema == targetDBSchema && f.Name == targetDBName)
                                                               .Return(sp => sp.As<DBStoredProcedure>())
                                                               .Results
                                                               .ToList();
                    if (linkedFromSPsOfFunction != null && linkedFromSPsOfFunction.Count > 0)
                    {
                        foreach (var sp in linkedFromSPsOfFunction)
                        {
                            sourceNodes.Add(new DPGraphNode { NodeType = DPGraphNodeType.StoredProcedure, DBSchema = sp.Schema, DBName = sp.Name });
                        }
                    }
                    break;
                case DPGraphNodeType.StoredProcedure:
                    List<DBStoredProcedure> linkedFromSPsOfSP = neo4jClient.Cypher
                                                               .Match("(sp1:StoredProcedure)<-[:Call]-(sp2:StoredProcedure)")
                                                               .Where((DBStoredProcedure sp1) => sp1.Schema == targetDBSchema && sp1.Name == targetDBName)
                                                               .Return(sp2 => sp2.As<DBStoredProcedure>())
                                                               .Results
                                                               .ToList();

                    if (linkedFromSPsOfSP != null && linkedFromSPsOfSP.Count > 0)
                    {
                        foreach (var sp in linkedFromSPsOfSP)
                        {
                            sourceNodes.Add(new DPGraphNode { NodeType = DPGraphNodeType.StoredProcedure, DBSchema = sp.Schema, DBName = sp.Name });
                        }
                    }
                    List<Page> linkedFromPagesOfSP = neo4jClient.Cypher
                                                               .Match("(sp:StoredProcedure)<-[:Call]-(p:Page)")
                                                               .Where((DBStoredProcedure sp) => sp.Schema == targetDBSchema && sp.Name == targetDBName)
                                                               .Return(p => p.As<Page>())
                                                               .Results
                                                               .ToList();

                    if (linkedFromPagesOfSP != null && linkedFromPagesOfSP.Count > 0)
                    {
                        foreach (var page in linkedFromPagesOfSP)
                        {
                            sourceNodes.Add(new DPGraphNode { NodeType = DPGraphNodeType.Page, PageModule = page.Module, PageName = page.Name });
                        }
                    }
                    break;
            }
            return sourceNodes;
            
        }
        private static string ParseDBSchemaFromDisplayName(string displayName)
        {
            string dbSchema = string.Empty;
            if (!string.IsNullOrEmpty(displayName) && displayName.Contains("."))
            {
                dbSchema = displayName.Split(new char[] { '.' })[0];
            }
            return dbSchema;
        }
        private static string ParseDBNameFromDisplayName(string displayName)
        {
            string dbName = string.Empty;
            if (!string.IsNullOrEmpty(displayName) && displayName.Contains("."))
            {
                dbName = displayName.Split(new char[] { '.' })[1];
            }
            return dbName;
        }
        private static string ParsePageModuleFromDisplayName(string displayName)
        {
            string pageModule = string.Empty;
            if (!string.IsNullOrEmpty(displayName) && displayName.Contains("-"))
            {
                pageModule = displayName.Split(new char[] { '-' })[0];
            }
            return pageModule;
        }
        private static string ParsePageNameFromDisplayName(string displayName)
        {
            string pageName = string.Empty;
            if (!string.IsNullOrEmpty(displayName) && displayName.Contains("-"))
            {
                pageName = displayName.Split(new char[] { '-' })[1];
            }
            return pageName;
        }
    }
}