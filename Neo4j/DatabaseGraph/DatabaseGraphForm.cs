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
    public partial class DatabaseGraphForm : Form
    {
        public DatabaseGraphForm()
        {
            InitializeComponent();
        }

        private void testButton_Click(object sender, EventArgs e)
        {
            var client = Neo4jGraphDatabaseHelper.CreateNeo4jClient();
            var query = client.Cypher
                             .Match("(t:Table)")
                             .Return(t => t.As<DBTable>());
            var tables = query.Results;
            StringBuilder resultBuilder = new StringBuilder();
            foreach (var t in tables)
            {
                resultBuilder.Append(t.Schema).Append(".").Append(t.Name).Append(";");
            }
            textBox1.Text = resultBuilder.ToString();
        }

        private void createDatabaseObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNodeForm createNodeForm = new CreateNodeForm();
            createNodeForm.ShowDialog();
        }

        private void createRelationshipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LinkNodeForm createRelationshipForm = new LinkNodeForm();
            createRelationshipForm.ShowDialog();
        }

        private void queryFromTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchFromTopForm searchFromTopNodeForm = new SearchFromTopForm();
            searchFromTopNodeForm.ShowDialog();
        }

        private void queryFromButtomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchFromButtomForm searchFromButtomForm = new SearchFromButtomForm();
            searchFromButtomForm.ShowDialog();
        }
    }
}
