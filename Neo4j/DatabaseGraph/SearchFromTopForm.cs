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
    public partial class SearchFromTopForm : Form
    {
        private GraphClient neo4jClient;
        private List<DBTable> tables;
        private List<DBView> views;
        private List<DBFunction> functions;
        private List<DBStoredProcedure> storedProcedures;
        private string selectedDBType;
        private string selectedDBSchema;
        private string selectedDBName;

        public SearchFromTopForm()
        {
            InitializeComponent();
            InitDBObjectTypeCombo();
            neo4jClient = Neo4jGraphDatabaseHelper.CreateNeo4jClient();
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            if (dbTypeCombo.SelectedIndex < 0)
            {
                MessageBox.Show("Please select database object type before search", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (dbNameCombo.SelectedIndex < 0)
            {
                MessageBox.Show("Please select database object name before search", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            selectedDBType = dbTypeCombo.Items[dbTypeCombo.SelectedIndex].ToString();
            string dbFullname = dbNameCombo.Items[dbNameCombo.SelectedIndex].ToString();
            selectedDBName = dbFullname.Substring(dbFullname.LastIndexOf(".") + 1, dbFullname.Length - dbFullname.LastIndexOf(".") - 1);
            selectedDBSchema = dbFullname.Substring(0, dbFullname.LastIndexOf("."));
            DataTable databaseObjectDataTable = SearchDatabaseObjectsFromTop(selectedDBType, selectedDBSchema, selectedDBName);
            searchResultGridView.AutoGenerateColumns = true;
            searchResultGridView.DataSource = databaseObjectDataTable;
        }

        private DataTable SearchDatabaseObjectsFromTop(string sourceDataType, string sourceObjectSchema, string sourceObjectName)
        {
            DataTable databaseObjectDataTable = new DataTable();
            databaseObjectDataTable.Columns.Add("Type", typeof(string));
            databaseObjectDataTable.Columns.Add("Schema", typeof(string));
            databaseObjectDataTable.Columns.Add("Name", typeof(string));
            switch (sourceDataType)
            {
                case "View":
                    List<DBTable> linkedDBTablesFromView = neo4jClient.Cypher
                                                               .Match("(v:View)-[:Reference]->(t:Table)")
                                                               .Where((DBView v) => v.Schema == sourceObjectSchema && v.Name == sourceObjectName)
                                                               .Return(t => t.As<DBTable>())
                                                               .Results
                                                               .ToList();

                    if (linkedDBTablesFromView != null && linkedDBTablesFromView.Count > 0)
                    {
                        foreach (var t in linkedDBTablesFromView)
                        {
                            databaseObjectDataTable.Rows.Add("Table", t.Schema, t.Name);
                        }
                    }
                    List<DBView> linkedDBViewsFromView = neo4jClient.Cypher
                                                               .Match("(v1:View)-[:Reference]->(v2:View)")
                                                               .Where((DBView v1) => v1.Schema == sourceObjectSchema && v1.Name == sourceObjectName)
                                                               .Return(v2 => v2.As<DBView>())
                                                               .Results
                                                               .ToList();
                    if (linkedDBViewsFromView != null && linkedDBViewsFromView.Count > 0)
                    {
                        foreach (var v in linkedDBViewsFromView)
                        {
                            databaseObjectDataTable.Rows.Add("View", v.Schema, v.Name);
                        }
                    }
                    List<DBFunction> linkedDBFunctionsFromView = neo4jClient.Cypher
                                                               .Match("(v:View)-[:Call]->(f:Function)")
                                                               .Where((DBView v) => v.Schema == sourceObjectSchema && v.Name == sourceObjectName)
                                                               .Return(f => f.As<DBFunction>())
                                                               .Results
                                                               .ToList();
                    if (linkedDBFunctionsFromView != null && linkedDBFunctionsFromView.Count > 0)
                    {
                        foreach (var f in linkedDBFunctionsFromView)
                        {
                            databaseObjectDataTable.Rows.Add("Function", f.Schema, f.Name);
                        }
                    }
                    break;
                case "Function":
                    List<DBTable> linkedDBTablesFromFunction = neo4jClient.Cypher
                                                               .Match("(f:Function)-[:Reference]->(t:Table)")
                                                               .Where((DBFunction f) => f.Schema == sourceObjectSchema && f.Name == sourceObjectName)
                                                               .Return(t => t.As<DBTable>())
                                                               .Results
                                                               .ToList();

                    if (linkedDBTablesFromFunction != null && linkedDBTablesFromFunction.Count > 0)
                    {
                        foreach (var t in linkedDBTablesFromFunction)
                        {
                            databaseObjectDataTable.Rows.Add("Table", t.Schema, t.Name);
                        }
                    }
                    List<DBView> linkedDBViewsFromFunction = neo4jClient.Cypher
                                                               .Match("(f:Function)-[:Reference]->(v:View)")
                                                               .Where((DBFunction f) => f.Schema == sourceObjectSchema && f.Name == sourceObjectName)
                                                               .Return(v => v.As<DBView>())
                                                               .Results
                                                               .ToList();
                    if (linkedDBViewsFromFunction != null && linkedDBViewsFromFunction.Count > 0)
                    {
                        foreach (var v in linkedDBViewsFromFunction)
                        {
                            databaseObjectDataTable.Rows.Add("View", v.Schema, v.Name);
                        }
                    }
                    List<DBFunction> linkedDBFunctionsFromFunction = neo4jClient.Cypher
                                                               .Match("(f1:Function)-[:Call]->(f2:Function)")
                                                               .Where((DBFunction f1) => f1.Schema == sourceObjectSchema && f1.Name == sourceObjectName)
                                                               .Return(f2 => f2.As<DBFunction>())
                                                               .Results
                                                               .ToList();
                    if (linkedDBFunctionsFromFunction != null && linkedDBFunctionsFromFunction.Count > 0)
                    {
                        foreach (var f in linkedDBFunctionsFromFunction)
                        {
                            databaseObjectDataTable.Rows.Add("Function", f.Schema, f.Name);
                        }
                    }
                    break;
                case "StoredProcedure":
                    List<DBTable> linkedDBTablesFromSP = neo4jClient.Cypher
                                                               .Match("(sp:StoredProcedure)-[r:Reference]->(t:Table)")
                                                               .Where((DBStoredProcedure sp) => sp.Schema == sourceObjectSchema && sp.Name == sourceObjectName)
                                                               .Return(t => t.As<DBTable>())
                                                               .Results
                                                               .ToList();

                    if (linkedDBTablesFromSP != null && linkedDBTablesFromSP.Count > 0)
                    {
                        foreach (var t in linkedDBTablesFromSP)
                        {
                            databaseObjectDataTable.Rows.Add("Table", t.Schema, t.Name);
                        }
                    }
                    List<DBView> linkedDBViewsFromSP = neo4jClient.Cypher
                                                               .Match("(sp:StoredProcedure)-[r:Reference]->(v:View)")
                                                               .Where((DBStoredProcedure sp) => sp.Schema == sourceObjectSchema && sp.Name == sourceObjectName)
                                                               .Return(v => v.As<DBView>())
                                                               .Results
                                                               .ToList();
                    if (linkedDBViewsFromSP != null && linkedDBViewsFromSP.Count > 0)
                    {
                        foreach (var v in linkedDBViewsFromSP)
                        {
                            databaseObjectDataTable.Rows.Add("View", v.Schema, v.Name);
                        }
                    }
                    List<DBFunction> linkedDBFunctionsFromSP = neo4jClient.Cypher
                                                               .Match("(sp:StoredProcedure)-[r:Call]->(f:Function)")
                                                               .Where((DBStoredProcedure sp) => sp.Schema == sourceObjectSchema && sp.Name == sourceObjectName)
                                                               .Return(f => f.As<DBFunction>())
                                                               .Results
                                                               .ToList();
                    if (linkedDBFunctionsFromSP != null && linkedDBFunctionsFromSP.Count > 0)
                    {
                        foreach (var f in linkedDBFunctionsFromSP)
                        {
                            databaseObjectDataTable.Rows.Add("Function", f.Schema, f.Name);
                        }
                    }
                    List<DBStoredProcedure> linkedDBSPsFromSP = neo4jClient.Cypher
                                                               .Match("(sp1:StoredProcedure)-[r:Call]->(sp2:StoredProcedure)")
                                                               .Where((DBStoredProcedure sp1) => sp1.Schema == sourceObjectSchema && sp1.Name == sourceObjectName)
                                                               .Return(sp2 => sp2.As<DBStoredProcedure>())
                                                               .Results
                                                               .ToList();
                    if (linkedDBSPsFromSP != null && linkedDBSPsFromSP.Count > 0)
                    {
                        foreach (var sp in linkedDBSPsFromSP)
                        {
                            databaseObjectDataTable.Rows.Add("StoredProcedure", sp.Schema, sp.Name);
                        }
                    }
                    break;
            }
            return databaseObjectDataTable;
        }


        private void InitDBObjectTypeCombo()
        {
            dbTypeCombo.Items.Clear();
            dbTypeCombo.Items.Add("View");
            dbTypeCombo.Items.Add("Function");
            dbTypeCombo.Items.Add("StoredProcedure");
            dbTypeCombo.SelectedIndex = -1;
        }

        private void dbTypeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillDBNameComboByDBType(dbTypeCombo, dbNameCombo);
            if (!string.IsNullOrEmpty(selectedDBType) && !string.IsNullOrEmpty(selectedDBSchema) && !string.IsNullOrEmpty(selectedDBName))
            {
                if (dbNameCombo.FindStringExact(selectedDBSchema + "." + selectedDBName) >= 0)
                {
                    dbNameCombo.SelectedIndex = dbNameCombo.FindStringExact(selectedDBSchema + "." + selectedDBName);
                }
                else
                {
                    dbNameCombo.SelectedIndex = -1;
                }
            }
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

        private void searchResultGridView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (searchResultGridView.SelectedCells == null || searchResultGridView.SelectedCells.Count <= 0)
            {
                MessageBox.Show("Must select one database object before search", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int rowIndex = searchResultGridView.SelectedCells[0].RowIndex;
            DataGridViewRow selectedRow = searchResultGridView.Rows[rowIndex];
            selectedDBType = Convert.ToString(selectedRow.Cells["Type"].Value);
            selectedDBSchema = Convert.ToString(selectedRow.Cells["Schema"].Value);
            selectedDBName = Convert.ToString(selectedRow.Cells["Name"].Value);
            dbTypeCombo.SelectedIndex = dbTypeCombo.FindStringExact(selectedDBType);
            dbNameCombo.SelectedIndex = dbNameCombo.FindStringExact(selectedDBSchema + "." + selectedDBName);
            DataTable databaseObjectDataTable = SearchDatabaseObjectsFromTop(selectedDBType, selectedDBSchema, selectedDBName);
            searchResultGridView.AutoGenerateColumns = true;
            searchResultGridView.DataSource = databaseObjectDataTable;
        }
    }
}
