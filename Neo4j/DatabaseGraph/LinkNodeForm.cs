using Neo4jClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DatabaseGraph
{
    public partial class LinkNodeForm : Form
    {
        private GraphClient neo4jClient;
        private List<DBTable> tables;
        private List<DBView> views;
        private List<DBFunction> functions;
        private List<DBStoredProcedure> storedProcedures;
        private DBTable selectedSourceTable;
        private DBView selectedSourceView;
        private DBFunction selectedSourceFunction;
        private DBStoredProcedure selectedSourceStoredProcedure;
        private DBTable selectedTargetTable;
        private DBView selectedTargetView;
        private DBFunction selectedTargetFunction;
        private DBStoredProcedure selectedTargetStoredProcedure;
        private string selectedSourceDBType;
        private string selectedTargetDBType;

        public LinkNodeForm()
        {
            InitializeComponent();
            InitDBObjectTypeCombo();
            neo4jClient = Neo4jGraphDatabaseHelper.CreateNeo4jClient();
        }

        private void createRelationshipButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedSourceDBType))
            {
                MessageBox.Show("Please select source database object type before create relationship", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(selectedTargetDBType))
            {
                MessageBox.Show("Please select target database object type before create relationship", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!ValidateSourceAndTargetDBType(selectedSourceDBType, selectedTargetDBType))
            {
                MessageBox.Show("Cannot create relationship from " + selectedSourceDBType + " to " + selectedTargetDBType, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            switch (selectedSourceDBType)
            {
                case "Table":
                    break;
                case "View":
                    if (selectedTargetDBType == "Table")
                    {
                        neo4jClient.Cypher
                                    .Match("(v:View)", "(t:Table)")
                                    .Where((DBView v) => v.Name == selectedSourceView.Name && v.Schema == selectedSourceView.Schema)
                                    .AndWhere((DBTable t) => t.Name == selectedTargetTable.Name && t.Schema == selectedTargetTable.Schema)
                                    .CreateUnique("(v)-[:Reference]->(t)")
                                    .ExecuteWithoutResults();
                    }
                    else if (selectedTargetDBType == "View")
                    {
                        neo4jClient.Cypher
                                   .Match("(v1:View)", "(v2:View)")
                                   .Where((DBView v1) => v1.Name == selectedSourceView.Name && v1.Schema == selectedSourceView.Schema)
                                   .AndWhere((DBView v2) => v2.Name == selectedTargetView.Name && v2.Schema == selectedTargetView.Schema)
                                   .CreateUnique("(v1)-[:Reference]->(v2)")
                                   .ExecuteWithoutResults();
                    }
                    else if (selectedTargetDBType == "Function")
                    {
                        neo4jClient.Cypher
                                   .Match("(v:View)", "(f:Function)")
                                   .Where((DBView v) => v.Name == selectedSourceView.Name && v.Schema == selectedSourceView.Schema)
                                   .AndWhere((DBFunction f) => f.Name == selectedTargetFunction.Name && f.Schema == selectedTargetFunction.Schema)
                                   .CreateUnique("(v)-[:Call]->(f)")
                                   .ExecuteWithoutResults();
                    }
                    break;
                case "Function":
                    if (selectedTargetDBType == "Table")
                    {
                        neo4jClient.Cypher
                                    .Match("(f:Function)", "(t:Table)")
                                    .Where((DBFunction f) => f.Name == selectedSourceFunction.Name && f.Schema == selectedSourceFunction.Schema)
                                    .AndWhere((DBTable t) => t.Name == selectedTargetTable.Name && t.Schema == selectedTargetTable.Schema)
                                    .CreateUnique("(f)-[:Reference]->(t)")
                                    .ExecuteWithoutResults();
                    }
                    else if (selectedTargetDBType == "View")
                    {
                        neo4jClient.Cypher
                                   .Match("(f:Function)", "(v:View)")
                                   .Where((DBFunction f) => f.Name == selectedSourceFunction.Name && f.Schema == selectedSourceFunction.Schema)
                                   .AndWhere((DBView v) => v.Name == selectedTargetView.Name && v.Schema == selectedTargetView.Schema)
                                   .CreateUnique("(f)-[:Reference]->(v)")
                                   .ExecuteWithoutResults();
                    }
                    else if (selectedTargetDBType == "Function")
                    {
                        neo4jClient.Cypher
                                   .Match("(f1:Function)", "(f2:Function)")
                                   .Where((DBFunction f1) => f1.Name == selectedSourceFunction.Name && f1.Schema == selectedSourceFunction.Schema)
                                   .AndWhere((DBFunction f2) => f2.Name == selectedTargetFunction.Name && f2.Schema == selectedTargetFunction.Schema)
                                   .CreateUnique("(f1)-[:Call]->(f2)")
                                   .ExecuteWithoutResults();
                    }
                    break;
                case "StoredProcedure":
                    if (selectedTargetDBType == "Table")
                    {
                        neo4jClient.Cypher
                                    .Match("(sp:StoredProcedure)", "(t:Table)")
                                    .Where((DBStoredProcedure sp) => sp.Name == selectedSourceStoredProcedure.Name && sp.Schema == selectedSourceStoredProcedure.Schema)
                                    .AndWhere((DBTable t) => t.Name == selectedTargetTable.Name && t.Schema == selectedTargetTable.Schema)
                                    .CreateUnique("(sp)-[:Reference]->(t)")
                                    .ExecuteWithoutResults();
                    }
                    else if (selectedTargetDBType == "View")
                    {
                        neo4jClient.Cypher
                                   .Match("(sp:StoredProcedure)", "(v:View)")
                                   .Where((DBStoredProcedure sp) => sp.Name == selectedSourceStoredProcedure.Name && sp.Schema == selectedSourceStoredProcedure.Schema)
                                   .AndWhere((DBView v) => v.Name == selectedTargetView.Name && v.Schema == selectedTargetView.Schema)
                                   .CreateUnique("(sp)-[:Reference]->(v)")
                                   .ExecuteWithoutResults();
                    }
                    else if (selectedTargetDBType == "Function")
                    {
                        neo4jClient.Cypher
                                   .Match("(sp:StoredProcedure)", "(f:Function)")
                                   .Where((DBStoredProcedure sp) => sp.Name == selectedSourceStoredProcedure.Name && sp.Schema == selectedSourceStoredProcedure.Schema)
                                   .AndWhere((DBFunction f) => f.Name == selectedTargetFunction.Name && f.Schema == selectedTargetFunction.Schema)
                                   .CreateUnique("(sp)-[:Call]->(f)")
                                   .ExecuteWithoutResults();
                    }
                    else if (selectedTargetDBType == "StoredProcedure")
                    {
                        neo4jClient.Cypher
                                  .Match("(sp1:StoredProcedure)", "(sp2:StoredProcedure)")
                                  .Where((DBStoredProcedure sp1) => sp1.Name == selectedSourceStoredProcedure.Name && sp1.Schema == selectedSourceStoredProcedure.Schema)
                                  .AndWhere((DBStoredProcedure sp2) => sp2.Name == selectedTargetStoredProcedure.Name && sp2.Schema == selectedTargetStoredProcedure.Schema)
                                  .CreateUnique("(sp1)-[:Call]->(sp2)")
                                  .ExecuteWithoutResults();
                    }
                    break;
            }
            MessageBox.Show("Create database object relationship succeed!");
        }

        private void InitDBObjectTypeCombo()
        {
            sourceDBTypeCombo.Items.Clear();
            sourceDBTypeCombo.Items.Add("Table");
            sourceDBTypeCombo.Items.Add("View");
            sourceDBTypeCombo.Items.Add("Function");
            sourceDBTypeCombo.Items.Add("StoredProcedure");
            sourceDBTypeCombo.SelectedIndex = -1;

            targetDBTypeCombo.Items.Clear();
            targetDBTypeCombo.Items.Add("Table");
            targetDBTypeCombo.Items.Add("View");
            targetDBTypeCombo.Items.Add("Function");
            targetDBTypeCombo.Items.Add("StoredProcedure");
            targetDBTypeCombo.SelectedIndex = -1;
        }

        private void sourceDBTypeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillDBNameComboByDBType(sourceDBTypeCombo,sourceDBNameCombo);
        }

        private void targetDBTypeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillDBNameComboByDBType(targetDBTypeCombo, targetDBNameCombo);
        }

        private void sourceDBNameCombo_SelectedValueChanged(object sender, EventArgs e)
        {
            GetSelectedDBObject(sourceDBTypeCombo,sourceDBNameCombo,true);
        }


        private void targetDBNameCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetSelectedDBObject(targetDBTypeCombo, targetDBNameCombo,false);
        }

        private void FillDBNameComboByDBType(ComboBox dbTypeCombo, ComboBox dbNameCombo)
        {
            if (dbTypeCombo.SelectedIndex >= 0)
            {
                string dbType = dbTypeCombo.Items[dbTypeCombo.SelectedIndex].ToString();
                switch (dbType)
                {
                    case "Table":
                        if (tables == null || tables.Count <= 0)
                        {
                            tables = neo4jClient.Cypher.Match("(t:Table)").Return(t => t.As<DBTable>()).Results.ToList();
                        }
                        if (tables != null && tables.Count > 0)
                        {
                            dbNameCombo.Items.Clear();
                            foreach (var t in tables)
                            {
                                dbNameCombo.Items.Add(t.Schema + "." + t.Name);
                            }
                        }
                        break;
                    case "View":
                        if (views == null || views.Count <= 0)
                        {
                            views = neo4jClient.Cypher.Match("(v:View)").Return(v => v.As<DBView>()).Results.ToList();
                        }
                        if (views != null && views.Count > 0)
                        {
                            dbNameCombo.Items.Clear();
                            foreach (var v in views)
                            {
                                dbNameCombo.Items.Add(v.Schema + "." + v.Name);
                            }
                        }
                        break;
                    case "Function":
                        if (functions == null || functions.Count <= 0)
                        {
                            functions = neo4jClient.Cypher.Match("(f:Function)").Return(f => f.As<DBFunction>()).Results.ToList();
                        }
                        if (functions != null && functions.Count > 0)
                        {
                            dbNameCombo.Items.Clear();
                            foreach (var f in functions)
                            {
                                dbNameCombo.Items.Add(f.Schema + "." + f.Name);
                            }
                        }
                        break;
                    case "StoredProcedure":
                        if (storedProcedures == null || storedProcedures.Count <= 0)
                        {
                            storedProcedures = neo4jClient.Cypher.Match("(sp:StoredProcedure)").Return(sp => sp.As<DBStoredProcedure>()).Results.ToList();
                        }
                        if (storedProcedures != null && storedProcedures.Count > 0)
                        {
                            dbNameCombo.Items.Clear();
                            foreach (var sp in storedProcedures)
                            {
                                dbNameCombo.Items.Add(sp.Schema + "." + sp.Name);
                            }
                        }
                        break;
                }
            }
        }

        private void GetSelectedDBObject(ComboBox dbTypeCombo,ComboBox dbNameCombo,bool isSource)
        {
            if (dbNameCombo.SelectedIndex >= 0 && dbTypeCombo.SelectedIndex >= 0)
            {
                string dbtype = dbTypeCombo.Items[dbTypeCombo.SelectedIndex].ToString();
                string dbname = dbNameCombo.Items[dbNameCombo.SelectedIndex].ToString();
                switch (dbtype)
                {
                    case "Table":
                        if (isSource)
                        {
                            selectedSourceTable = tables.Where(t => t.Schema + "." + t.Name == dbname).FirstOrDefault();
                            selectedSourceDBType = "Table";
                        }
                        else
                        {
                            selectedTargetTable = tables.Where(t => t.Schema + "." + t.Name == dbname).FirstOrDefault();
                            selectedTargetDBType = "Table";
                        }
                        break;
                    case "View":
                        if (isSource)
                        {
                            selectedSourceView = views.Where(v => v.Schema + "." + v.Name == dbname).FirstOrDefault();
                            selectedSourceDBType = "View";
                        }
                        else
                        {
                            selectedTargetView = views.Where(v => v.Schema + "." + v.Name == dbname).FirstOrDefault();
                            selectedTargetDBType = "View";
                        }
                        break;
                    case "Function":
                        if (isSource)
                        {
                            selectedSourceFunction = functions.Where(f => f.Schema + "." + f.Name == dbname).FirstOrDefault();
                            selectedSourceDBType = "Function";
                        }
                        else
                        {
                            selectedTargetFunction = functions.Where(f => f.Schema + "." + f.Name == dbname).FirstOrDefault();
                            selectedTargetDBType = "Function";
                        }
                        break;
                    case "StoredProcedure":
                        if (isSource)
                        {
                            selectedSourceStoredProcedure = storedProcedures.Where(sp => sp.Schema + "." + sp.Name == dbname).FirstOrDefault();
                            selectedSourceDBType = "StoredProcedure";
                        }
                        else
                        {
                            selectedTargetStoredProcedure = storedProcedures.Where(sp => sp.Schema + "." + sp.Name == dbname).FirstOrDefault();
                            selectedTargetDBType = "StoredProcedure";
                        }
                        break;
                }
            }
        }

        private bool ValidateSourceAndTargetDBType(string selectedSourceDBType, string selectedTargetDBType)
        {
            bool isValid = false;
            switch (selectedSourceDBType)
            {
                case "Table":
                    isValid = false;
                    break;
                case "View":
                    if (selectedTargetDBType == "Table"
                        || selectedTargetDBType == "View"
                        || selectedTargetDBType == "Function")
                    {
                        isValid = true;
                    }
                    else
                    {
                        isValid = false;
                    }
                    break;
                case "Function":
                    if (selectedTargetDBType == "Table"
                        || selectedTargetDBType == "View"
                        || selectedTargetDBType == "Function")
                    {
                        isValid = true;
                    }
                    else
                    {
                        isValid = false;
                    }
                    break;
                case "StoredProcedure":
                    isValid = true;
                    break;
            }
            return isValid;
        }
    }
}
