namespace DatabaseGraph
{
    partial class DatabaseGraphForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DatabaseGraphForm));
            this.testButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.createToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createDatabaseObjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createRelationshipToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.queryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.queryFromTopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.queryFromButtomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // testButton
            // 
            this.testButton.Location = new System.Drawing.Point(12, 337);
            this.testButton.Name = "testButton";
            this.testButton.Size = new System.Drawing.Size(75, 23);
            this.testButton.TabIndex = 0;
            this.testButton.Text = "Test";
            this.testButton.UseVisualStyleBackColor = true;
            this.testButton.Click += new System.EventHandler(this.testButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(0, 311);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(368, 20);
            this.textBox1.TabIndex = 1;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createToolStripMenuItem,
            this.queryToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(392, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // createToolStripMenuItem
            // 
            this.createToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createDatabaseObjectToolStripMenuItem,
            this.createRelationshipToolStripMenuItem});
            this.createToolStripMenuItem.Name = "createToolStripMenuItem";
            this.createToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.createToolStripMenuItem.Text = "Create";
            // 
            // createDatabaseObjectToolStripMenuItem
            // 
            this.createDatabaseObjectToolStripMenuItem.Name = "createDatabaseObjectToolStripMenuItem";
            this.createDatabaseObjectToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.createDatabaseObjectToolStripMenuItem.Text = "Create Database Object";
            this.createDatabaseObjectToolStripMenuItem.Click += new System.EventHandler(this.createDatabaseObjectToolStripMenuItem_Click);
            // 
            // createRelationshipToolStripMenuItem
            // 
            this.createRelationshipToolStripMenuItem.Name = "createRelationshipToolStripMenuItem";
            this.createRelationshipToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.createRelationshipToolStripMenuItem.Text = "Create Relationship";
            this.createRelationshipToolStripMenuItem.Click += new System.EventHandler(this.createRelationshipToolStripMenuItem_Click);
            // 
            // queryToolStripMenuItem
            // 
            this.queryToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.queryFromTopToolStripMenuItem,
            this.queryFromButtomToolStripMenuItem});
            this.queryToolStripMenuItem.Name = "queryToolStripMenuItem";
            this.queryToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.queryToolStripMenuItem.Text = "Query";
            // 
            // queryFromTopToolStripMenuItem
            // 
            this.queryFromTopToolStripMenuItem.Name = "queryFromTopToolStripMenuItem";
            this.queryFromTopToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.queryFromTopToolStripMenuItem.Text = "Query From Top";
            this.queryFromTopToolStripMenuItem.Click += new System.EventHandler(this.queryFromTopToolStripMenuItem_Click);
            // 
            // queryFromButtomToolStripMenuItem
            // 
            this.queryFromButtomToolStripMenuItem.Name = "queryFromButtomToolStripMenuItem";
            this.queryFromButtomToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.queryFromButtomToolStripMenuItem.Text = "Query From Buttom";
            this.queryFromButtomToolStripMenuItem.Click += new System.EventHandler(this.queryFromButtomToolStripMenuItem_Click);
            // 
            // DatabaseGraphForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 384);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.testButton);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "DatabaseGraphForm";
            this.Text = "Database Graph";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button testButton;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem createToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createDatabaseObjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createRelationshipToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem queryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem queryFromTopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem queryFromButtomToolStripMenuItem;
    }
}

