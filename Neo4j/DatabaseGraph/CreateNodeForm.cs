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
    public partial class CreateNodeForm : Form
    {
        private GraphClient neo4jClient;
        public CreateNodeForm()
        {
            InitializeComponent();
            InitDBObjectTypeCombo();
            neo4jClient = Neo4jGraphDatabaseHelper.CreateNeo4jClient();
        }

        private void createNodeButton_Click(object sender, EventArgs e)
        {
            if (dbTypeCombo.SelectedIndex < 0)
            {
                MessageBox.Show("Please select database object type before create", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(schemaTextBox.Text.Trim()))
            {
                MessageBox.Show("Please input database object schema name before create", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(nameTextBox.Text.Trim()))
            {
                MessageBox.Show("Please input database object name before create", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string dbtype = dbTypeCombo.Items[dbTypeCombo.SelectedIndex].ToString();
            string schemaName = schemaTextBox.Text.Trim();
            string name = nameTextBox.Text.Trim();
            switch (dbtype)
            {
                case "Table":
                    DBTable table = new DBTable() { Schema = schemaName, Name = name };
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
                case "View":
                    DBView view = new DBView() { Schema = schemaName, Name = name };
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
                case "Function":
                    DBFunction function = new DBFunction() { Schema = schemaName, Name = name };
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
                case "StoredProcedure":
                    DBStoredProcedure sp = new DBStoredProcedure() { Schema = schemaName, Name = name };
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
            }
            MessageBox.Show("Create database object succeed!");
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
    }
}
