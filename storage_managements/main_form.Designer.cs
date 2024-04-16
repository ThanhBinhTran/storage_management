
namespace storage_managements
{
	partial class main_form
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
            System.Windows.Forms.TableLayoutPanel layout_L1;
            this.layout_L2_bottom = new System.Windows.Forms.TableLayoutPanel();
            this.textbox_display = new System.Windows.Forms.RichTextBox();
            this.label_debug = new System.Windows.Forms.Label();
            this.tab_view = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.item_layout_L2_top = new System.Windows.Forms.TableLayoutPanel();
            this.table_item = new System.Windows.Forms.DataGridView();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.item_textbox_search_ID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.item_textbox_search_name = new System.Windows.Forms.TextBox();
            this.but_test = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.item_textbox_new_ID = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.item_textbox_new_name = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.item_textbox_new_unit = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            layout_L1 = new System.Windows.Forms.TableLayoutPanel();
            layout_L1.SuspendLayout();
            this.layout_L2_bottom.SuspendLayout();
            this.tab_view.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.item_layout_L2_top.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.table_item)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // layout_L1
            // 
            layout_L1.AutoSize = true;
            layout_L1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            layout_L1.ColumnCount = 1;
            layout_L1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            layout_L1.Controls.Add(this.layout_L2_bottom, 0, 1);
            layout_L1.Controls.Add(this.tab_view, 0, 0);
            layout_L1.Dock = System.Windows.Forms.DockStyle.Fill;
            layout_L1.Location = new System.Drawing.Point(0, 0);
            layout_L1.Name = "layout_L1";
            layout_L1.RowCount = 2;
            layout_L1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 91.93246F));
            layout_L1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.067542F));
            layout_L1.Size = new System.Drawing.Size(1186, 545);
            layout_L1.TabIndex = 7;
            // 
            // layout_L2_bottom
            // 
            this.layout_L2_bottom.ColumnCount = 2;
            this.layout_L2_bottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layout_L2_bottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layout_L2_bottom.Controls.Add(this.textbox_display, 0, 0);
            this.layout_L2_bottom.Controls.Add(this.label_debug, 1, 0);
            this.layout_L2_bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.layout_L2_bottom.Location = new System.Drawing.Point(3, 507);
            this.layout_L2_bottom.Name = "layout_L2_bottom";
            this.layout_L2_bottom.RowCount = 1;
            this.layout_L2_bottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layout_L2_bottom.Size = new System.Drawing.Size(1180, 35);
            this.layout_L2_bottom.TabIndex = 8;
            // 
            // textbox_display
            // 
            this.textbox_display.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textbox_display.Location = new System.Drawing.Point(3, 3);
            this.textbox_display.Name = "textbox_display";
            this.textbox_display.Size = new System.Drawing.Size(319, 29);
            this.textbox_display.TabIndex = 3;
            this.textbox_display.Text = "";
            // 
            // label_debug
            // 
            this.label_debug.AutoSize = true;
            this.label_debug.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_debug.Location = new System.Drawing.Point(593, 0);
            this.label_debug.Name = "label_debug";
            this.label_debug.Size = new System.Drawing.Size(93, 32);
            this.label_debug.TabIndex = 2;
            this.label_debug.Text = "label1";
            // 
            // tab_view
            // 
            this.tab_view.Controls.Add(this.tabPage1);
            this.tab_view.Controls.Add(this.tabPage2);
            this.tab_view.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tab_view.Location = new System.Drawing.Point(3, 3);
            this.tab_view.Name = "tab_view";
            this.tab_view.SelectedIndex = 0;
            this.tab_view.Size = new System.Drawing.Size(1171, 483);
            this.tab_view.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.item_layout_L2_top);
            this.tabPage1.Location = new System.Drawing.Point(4, 40);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1163, 439);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "hang_hoa";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // item_layout_L2_top
            // 
            this.item_layout_L2_top.ColumnCount = 1;
            this.item_layout_L2_top.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.item_layout_L2_top.Controls.Add(this.table_item, 0, 1);
            this.item_layout_L2_top.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.item_layout_L2_top.Controls.Add(this.flowLayoutPanel2, 0, 2);
            this.item_layout_L2_top.Location = new System.Drawing.Point(6, 6);
            this.item_layout_L2_top.Name = "item_layout_L2_top";
            this.item_layout_L2_top.RowCount = 3;
            this.item_layout_L2_top.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.item_layout_L2_top.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.item_layout_L2_top.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.item_layout_L2_top.Size = new System.Drawing.Size(1151, 427);
            this.item_layout_L2_top.TabIndex = 6;
            // 
            // table_item
            // 
            this.table_item.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.table_item.Dock = System.Windows.Forms.DockStyle.Fill;
            this.table_item.Location = new System.Drawing.Point(3, 173);
            this.table_item.Name = "table_item";
            this.table_item.RowHeadersWidth = 51;
            this.table_item.RowTemplate.Height = 24;
            this.table_item.Size = new System.Drawing.Size(1145, 164);
            this.table_item.TabIndex = 4;
            this.table_item.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.label2);
            this.flowLayoutPanel1.Controls.Add(this.item_textbox_search_ID);
            this.flowLayoutPanel1.Controls.Add(this.label3);
            this.flowLayoutPanel1.Controls.Add(this.item_textbox_search_name);
            this.flowLayoutPanel1.Controls.Add(this.but_test);
            this.flowLayoutPanel1.Controls.Add(this.button1);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1017, 123);
            this.flowLayoutPanel1.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 32);
            this.label2.TabIndex = 2;
            this.label2.Text = "Tìm mã:";
            // 
            // item_textbox_search_ID
            // 
            this.item_textbox_search_ID.Location = new System.Drawing.Point(125, 3);
            this.item_textbox_search_ID.Name = "item_textbox_search_ID";
            this.item_textbox_search_ID.Size = new System.Drawing.Size(100, 38);
            this.item_textbox_search_ID.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(231, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 32);
            this.label3.TabIndex = 3;
            this.label3.Text = "Tên";
            // 
            // item_textbox_search_name
            // 
            this.item_textbox_search_name.Location = new System.Drawing.Point(301, 3);
            this.item_textbox_search_name.Name = "item_textbox_search_name";
            this.item_textbox_search_name.Size = new System.Drawing.Size(100, 38);
            this.item_textbox_search_name.TabIndex = 4;
            // 
            // but_test
            // 
            this.but_test.Location = new System.Drawing.Point(407, 3);
            this.but_test.Name = "but_test";
            this.but_test.Size = new System.Drawing.Size(98, 69);
            this.but_test.TabIndex = 1;
            this.but_test.Text = "test";
            this.but_test.UseVisualStyleBackColor = true;
            this.but_test.Click += new System.EventHandler(this.button1_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(511, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(141, 70);
            this.button1.TabIndex = 5;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.label1);
            this.flowLayoutPanel2.Controls.Add(this.item_textbox_new_ID);
            this.flowLayoutPanel2.Controls.Add(this.label4);
            this.flowLayoutPanel2.Controls.Add(this.item_textbox_new_name);
            this.flowLayoutPanel2.Controls.Add(this.label5);
            this.flowLayoutPanel2.Controls.Add(this.item_textbox_new_unit);
            this.flowLayoutPanel2.Controls.Add(this.button2);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 343);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(1145, 81);
            this.flowLayoutPanel2.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 32);
            this.label1.TabIndex = 3;
            this.label1.Text = "Mã";
            // 
            // item_textbox_new_ID
            // 
            this.item_textbox_new_ID.Location = new System.Drawing.Point(63, 3);
            this.item_textbox_new_ID.Name = "item_textbox_new_ID";
            this.item_textbox_new_ID.Size = new System.Drawing.Size(100, 38);
            this.item_textbox_new_ID.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(169, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 32);
            this.label4.TabIndex = 5;
            this.label4.Text = "Tên";
            // 
            // item_textbox_new_name
            // 
            this.item_textbox_new_name.Location = new System.Drawing.Point(239, 3);
            this.item_textbox_new_name.Name = "item_textbox_new_name";
            this.item_textbox_new_name.Size = new System.Drawing.Size(100, 38);
            this.item_textbox_new_name.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(345, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 32);
            this.label5.TabIndex = 7;
            this.label5.Text = "Đơn vị";
            // 
            // item_textbox_new_unit
            // 
            this.item_textbox_new_unit.Location = new System.Drawing.Point(446, 3);
            this.item_textbox_new_unit.Name = "item_textbox_new_unit";
            this.item_textbox_new_unit.Size = new System.Drawing.Size(100, 38);
            this.item_textbox_new_unit.TabIndex = 8;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(552, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(98, 69);
            this.button2.TabIndex = 9;
            this.button2.Text = "test";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 40);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1163, 439);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // main_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1186, 545);
            this.Controls.Add(layout_L1);
            this.Name = "main_form";
            this.Text = "Storage managements";
            layout_L1.ResumeLayout(false);
            this.layout_L2_bottom.ResumeLayout(false);
            this.layout_L2_bottom.PerformLayout();
            this.tab_view.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.item_layout_L2_top.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.table_item)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Button but_test;
		private System.Windows.Forms.Label label_debug;
        private System.Windows.Forms.RichTextBox textbox_display;
        private System.Windows.Forms.DataGridView table_item;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabControl tab_view;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TableLayoutPanel layout_L2_bottom;
        private System.Windows.Forms.TableLayoutPanel item_layout_L2_top;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox item_textbox_search_ID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox item_textbox_search_name;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox item_textbox_new_ID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox item_textbox_new_name;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox item_textbox_new_unit;
        private System.Windows.Forms.Button button2;
    }
}

