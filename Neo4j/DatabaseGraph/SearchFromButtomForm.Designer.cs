namespace DatabaseGraph
{
    partial class SearchFromButtomForm
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
            this.dbNameCombo = new System.Windows.Forms.ComboBox();
            this.searchButton = new System.Windows.Forms.Button();
            this.searchResultGridView = new System.Windows.Forms.DataGridView();
            this.dbTypeCombo = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.searchResultGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // dbNameCombo
            // 
            this.dbNameCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dbNameCombo.FormattingEnabled = true;
            this.dbNameCombo.Location = new System.Drawing.Point(144, 74);
            this.dbNameCombo.Name = "dbNameCombo";
            this.dbNameCombo.Size = new System.Drawing.Size(326, 21);
            this.dbNameCombo.TabIndex = 12;
            // 
            // searchButton
            // 
            this.searchButton.Location = new System.Drawing.Point(191, 111);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(75, 23);
            this.searchButton.TabIndex = 11;
            this.searchButton.Text = "Search";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // searchResultGridView
            // 
            this.searchResultGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.searchResultGridView.Location = new System.Drawing.Point(15, 151);
            this.searchResultGridView.MultiSelect = false;
            this.searchResultGridView.Name = "searchResultGridView";
            this.searchResultGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.searchResultGridView.Size = new System.Drawing.Size(455, 150);
            this.searchResultGridView.TabIndex = 10;
            this.searchResultGridView.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.searchResultGridView_CellContentDoubleClick);
            // 
            // dbTypeCombo
            // 
            this.dbTypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dbTypeCombo.FormattingEnabled = true;
            this.dbTypeCombo.Location = new System.Drawing.Point(144, 18);
            this.dbTypeCombo.Name = "dbTypeCombo";
            this.dbTypeCombo.Size = new System.Drawing.Size(326, 21);
            this.dbTypeCombo.TabIndex = 9;
            this.dbTypeCombo.SelectedIndexChanged += new System.EventHandler(this.dbTypeCombo_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Database Object Name:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Database Object Type";
            // 
            // SearchFromButtomForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 326);
            this.Controls.Add(this.dbNameCombo);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.searchResultGridView);
            this.Controls.Add(this.dbTypeCombo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "SearchFromButtomForm";
            this.Text = "SearchFromButtom";
            ((System.ComponentModel.ISupportInitialize)(this.searchResultGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox dbNameCombo;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.DataGridView searchResultGridView;
        private System.Windows.Forms.ComboBox dbTypeCombo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}