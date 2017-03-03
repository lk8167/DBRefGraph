namespace DatabaseGraph
{
    partial class LinkNodeForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.sourceDBNameCombo = new System.Windows.Forms.ComboBox();
            this.sourceDBTypeCombo = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.targetDBNameCombo = new System.Windows.Forms.ComboBox();
            this.targetDBTypeCombo = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.createRelationshipButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.sourceDBNameCombo);
            this.groupBox1.Controls.Add(this.sourceDBTypeCombo);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(412, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Source";
            // 
            // sourceDBNameCombo
            // 
            this.sourceDBNameCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sourceDBNameCombo.FormattingEnabled = true;
            this.sourceDBNameCombo.Location = new System.Drawing.Point(83, 61);
            this.sourceDBNameCombo.Name = "sourceDBNameCombo";
            this.sourceDBNameCombo.Size = new System.Drawing.Size(285, 21);
            this.sourceDBNameCombo.TabIndex = 3;
            this.sourceDBNameCombo.SelectedValueChanged += new System.EventHandler(this.sourceDBNameCombo_SelectedValueChanged);
            // 
            // sourceDBTypeCombo
            // 
            this.sourceDBTypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sourceDBTypeCombo.FormattingEnabled = true;
            this.sourceDBTypeCombo.Location = new System.Drawing.Point(83, 26);
            this.sourceDBTypeCombo.Name = "sourceDBTypeCombo";
            this.sourceDBTypeCombo.Size = new System.Drawing.Size(285, 21);
            this.sourceDBTypeCombo.TabIndex = 2;
            this.sourceDBTypeCombo.SelectedIndexChanged += new System.EventHandler(this.sourceDBTypeCombo_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Type";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.targetDBNameCombo);
            this.groupBox2.Controls.Add(this.targetDBTypeCombo);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(13, 119);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(412, 100);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Target";
            // 
            // targetDBNameCombo
            // 
            this.targetDBNameCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.targetDBNameCombo.FormattingEnabled = true;
            this.targetDBNameCombo.Location = new System.Drawing.Point(83, 68);
            this.targetDBNameCombo.Name = "targetDBNameCombo";
            this.targetDBNameCombo.Size = new System.Drawing.Size(285, 21);
            this.targetDBNameCombo.TabIndex = 3;
            this.targetDBNameCombo.SelectedIndexChanged += new System.EventHandler(this.targetDBNameCombo_SelectedIndexChanged);
            // 
            // targetDBTypeCombo
            // 
            this.targetDBTypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.targetDBTypeCombo.FormattingEnabled = true;
            this.targetDBTypeCombo.Location = new System.Drawing.Point(83, 22);
            this.targetDBTypeCombo.Name = "targetDBTypeCombo";
            this.targetDBTypeCombo.Size = new System.Drawing.Size(285, 21);
            this.targetDBTypeCombo.TabIndex = 2;
            this.targetDBTypeCombo.SelectedIndexChanged += new System.EventHandler(this.targetDBTypeCombo_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Type";
            // 
            // createRelationshipButton
            // 
            this.createRelationshipButton.Location = new System.Drawing.Point(140, 224);
            this.createRelationshipButton.Name = "createRelationshipButton";
            this.createRelationshipButton.Size = new System.Drawing.Size(138, 23);
            this.createRelationshipButton.TabIndex = 4;
            this.createRelationshipButton.Text = "Create Relationship";
            this.createRelationshipButton.UseVisualStyleBackColor = true;
            this.createRelationshipButton.Click += new System.EventHandler(this.createRelationshipButton_Click);
            // 
            // LinkNodeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 259);
            this.Controls.Add(this.createRelationshipButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "LinkNodeForm";
            this.Text = "LinkNodeForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox sourceDBNameCombo;
        private System.Windows.Forms.ComboBox sourceDBTypeCombo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox targetDBNameCombo;
        private System.Windows.Forms.ComboBox targetDBTypeCombo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button createRelationshipButton;
    }
}