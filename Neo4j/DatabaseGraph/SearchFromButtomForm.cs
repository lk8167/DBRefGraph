using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace DatabaseGraph
{
    public partial class SearchFromButtomForm : Form
    {
        private GraphClient neo4jClient;
        private List<DBTable> tables;
        private List<DBView> views;
        private List<DBFunction> functions;
        private List<DBStoredProcedure> storedProcedures;
        private string selectedDBType;
        private string selectedDBSchema;
        private string selectedDBName;

        public SearchFromButtomForm()
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
            DataTable databaseObjectDataTable = SearchDatabaseObjectsFromBottom(selectedDBType, selectedDBSchema, selectedDBName);
            searchResultGridView.AutoGenerateColumns = true;
            searchResultGridView.DataSource = databaseObjectDataTable;
        }

        private DataTable SearchDatabaseObjectsFromBottom(string sourceDataType, string sourceObjectSchema, string sourceObjectName)
        {
            DataTable databaseObjectDataTable = new DataTable();
            databaseObjectDataTable.Columns.Add("Type", typeof(string));
            databaseObjectDataTable.Columns.Add("Schema", typeof(string));
            databaseObjectDataTable.Columns.Add("Name", typeof(string));
            switch (sourceDataType)
            {
                case "Table":
                    List<DBView> linkedFromViewsOfTable = neo4jClient.Cypher
                                                              .Match("(t:Table)<-[:Reference]-(v:View)")
                                                              .Where((DBTable t) => t.Schema == sourceObjectSchema && t.Name == sourceObjectName)
                                                              .Return(v => v.As<DBView>())
                                                              .Results
                                                              .ToList();

                    if (linkedFromViewsOfTable != null && linkedFromViewsOfTable.Count > 0)
                    {
                        foreach (var v in linkedFromViewsOfTable)
                        {
                            databaseObjectDataTable.Rows.Add("View", v.Schema, v.Name);
                        }
                    }
                    List<DBFunction> linkedFromFunctionsOfTable = neo4jClient.Cypher
                                                              .Match("(t:Table)<-[:Reference]-(f:Function)")
                                                              .Where((DBTable t) => t.Schema == sourceObjectSchema && t.Name == sourceObjectName)
                                                              .Return(f => f.As<DBFunction>())
                                                              .Results
                                                              .ToList();

                    if (linkedFromFunctionsOfTable != null && linkedFromFunctionsOfTable.Count > 0)
                    {
                        foreach (var f in linkedFromFunctionsOfTable)
                        {
                            databaseObjectDataTable.Rows.Add("Function", f.Schema, f.Name);
                        }
                    }
                    List<DBStoredProcedure> linkedFromSPsOfTable = neo4jClient.Cypher
                                                              .Match("(t:Table)<-[:Reference]-(sp:StoredProcedure)")
                                                              .Where((DBTable t) => t.Schema == sourceObjectSchema && t.Name == sourceObjectName)
                                                              .Return(sp => sp.As<DBStoredProcedure>())
                                                              .Results
                                                              .ToList();

                    if (linkedFromSPsOfTable != null && linkedFromSPsOfTable.Count > 0)
                    {
                        foreach (var sp in linkedFromSPsOfTable)
                        {
                            databaseObjectDataTable.Rows.Add("StoredProcedure", sp.Schema, sp.Name);
                        }
                    }
                    break;
                case "View":
                    List<DBView> linkedFromViewsOfView = neo4jClient.Cypher
                                                               .Match("(v1:View)<-[:Reference]-(v2:View)")
                                                               .Where((DBView v1) => v1.Schema == sourceObjectSchema && v1.Name == sourceObjectName)
                                                               .Return(v2 => v2.As<DBView>())
                                                               .Results
                                                               .ToList();

                    if (linkedFromViewsOfView != null && linkedFromViewsOfView.Count > 0)
                    {
                        foreach (var f in linkedFromViewsOfView)
                        {
                            databaseObjectDataTable.Rows.Add("View", f.Schema, f.Name);
                        }
                    }
                    List<DBFunction> linkedFromFunctionOfView = neo4jClient.Cypher
                                                               .Match("(v:View)<-[:Reference]-(f:Function)")
                                                               .Where((DBView v) => v.Schema == sourceObjectSchema && v.Name == sourceObjectName)
                                                               .Return(f => f.As<DBFunction>())
                                                               .Results
                                                               .ToList();
                    if (linkedFromFunctionOfView != null && linkedFromFunctionOfView.Count > 0)
                    {
                        foreach (var f in linkedFromFunctionOfView)
                        {
                            databaseObjectDataTable.Rows.Add("Function", f.Schema, f.Name);
                        }
                    }
                    List<DBStoredProcedure> linkedFromSPsOfView = neo4jClient.Cypher
                                                               .Match("(v:View)<-[:Reference]-(sp:StoredProcedure)")
                                                               .Where((DBView v) => v.Schema == sourceObjectSchema && v.Name == sourceObjectName)
                                                               .Return(sp => sp.As<DBStoredProcedure>())
                                                               .Results
                                                               .ToList();
                    if (linkedFromSPsOfView != null && linkedFromSPsOfView.Count > 0)
                    {
                        foreach (var sp in linkedFromSPsOfView)
                        {
                            databaseObjectDataTable.Rows.Add("StoredProcedure", sp.Schema, sp.Name);
                        }
                    }
                    break;
                case "Function":
                    List<DBView> linkedFromViewsOfFunction = neo4jClient.Cypher
                                                               .Match("(f:Function)<-[:Call]-(v:View)")
                                                               .Where((DBFunction f) => f.Schema == sourceObjectSchema && f.Name == sourceObjectName)
                                                               .Return(v => v.As<DBView>())
                                                               .Results
                                                               .ToList();

                    if (linkedFromViewsOfFunction != null && linkedFromViewsOfFunction.Count > 0)
                    {
                        foreach (var v in linkedFromViewsOfFunction)
                        {
                            databaseObjectDataTable.Rows.Add("View", v.Schema, v.Name);
                        }
                    }
                    List<DBFunction> linkedFromFunctionOfFunction = neo4jClient.Cypher
                                                               .Match("(f1:Function)<-[:Call]-(f2:Function)")
                                                               .Where((DBFunction f1) => f1.Schema == sourceObjectSchema && f1.Name == sourceObjectName)
                                                               .Return(f2 => f2.As<DBFunction>())
                                                               .Results
                                                               .ToList();
                    if (linkedFromFunctionOfFunction != null && linkedFromFunctionOfFunction.Count > 0)
                    {
                        foreach (var f in linkedFromFunctionOfFunction)
                        {
                            databaseObjectDataTable.Rows.Add("Function", f.Schema, f.Name);
                        }
                    }
                    List<DBStoredProcedure> linkedFromSPsOfFunction = neo4jClient.Cypher
                                                               .Match("(f:Function)<-[:Call]-(sp:StoredProcedure)")
                                                               .Where((DBFunction f) => f.Schema == sourceObjectSchema && f.Name == sourceObjectName)
                                                               .Return(sp => sp.As<DBStoredProcedure>())
                                                               .Results
                                                               .ToList();
                    if (linkedFromSPsOfFunction != null && linkedFromSPsOfFunction.Count > 0)
                    {
                        foreach (var sp in linkedFromSPsOfFunction)
                        {
                            databaseObjectDataTable.Rows.Add("StoredProcedure", sp.Schema, sp.Name);
                        }
                    }
                    break;
                case "StoredProcedure":
                    List<DBStoredProcedure> linkedFromSPsOfSP = neo4jClient.Cypher
                                                               .Match("(sp1:StoredProcedure)<-[:Call]-(sp2:StoredProcedure)")
                                                               .Where((DBStoredProcedure sp1) => sp1.Schema == sourceObjectSchema && sp1.Name == sourceObjectName)
                                                               .Return(sp2 => sp2.As<DBStoredProcedure>())
                                                               .Results
                                                               .ToList();

                    if (linkedFromSPsOfSP != null && linkedFromSPsOfSP.Count > 0)
                    {
                        foreach (var sp in linkedFromSPsOfSP)
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
            dbTypeCombo.Items.Add("Table");
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
            DataTable databaseObjectDataTable = SearchDatabaseObjectsFromBottom(selectedDBType, selectedDBSchema, selectedDBName);
            searchResultGridView.AutoGenerateColumns = true;
            searchResultGridView.DataSource = databaseObjectDataTable;
        }
    }
}
