namespace computernayGrafika
{
    partial class ProjectionForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            tableLayoutPanel = new TableLayoutPanel();
            panelFront = new Panel();
            pictureBoxFront = new PictureBox();
            lblFront = new Label();
            panelTop = new Panel();
            pictureBoxTop = new PictureBox();
            lblTop = new Label();
            panelSide = new Panel();
            pictureBoxSide = new PictureBox();
            lblSide = new Label();
            tableLayoutPanel.SuspendLayout();
            panelFront.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxFront).BeginInit();
            panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxTop).BeginInit();
            panelSide.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSide).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnCount = 3;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34f));
            tableLayoutPanel.Controls.Add(panelFront, 0, 0);
            tableLayoutPanel.Controls.Add(panelTop, 1, 0);
            tableLayoutPanel.Controls.Add(panelSide, 2, 0);
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.Location = new Point(0, 0);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 1;
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            tableLayoutPanel.Size = new Size(900, 350);
            tableLayoutPanel.TabIndex = 0;
            // 
            // panelFront
            // 
            panelFront.Controls.Add(pictureBoxFront);
            panelFront.Controls.Add(lblFront);
            panelFront.Dock = DockStyle.Fill;
            panelFront.Location = new Point(3, 3);
            panelFront.Name = "panelFront";
            panelFront.Size = new Size(294, 344);
            panelFront.TabIndex = 0;
            // 
            // pictureBoxFront
            // 
            pictureBoxFront.BackColor = Color.White;
            pictureBoxFront.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxFront.Dock = DockStyle.Fill;
            pictureBoxFront.Location = new Point(0, 24);
            pictureBoxFront.Name = "pictureBoxFront";
            pictureBoxFront.Size = new Size(294, 320);
            pictureBoxFront.TabIndex = 1;
            pictureBoxFront.TabStop = false;
            // 
            // lblFront
            // 
            lblFront.Dock = DockStyle.Top;
            lblFront.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblFront.Location = new Point(0, 0);
            lblFront.Name = "lblFront";
            lblFront.Size = new Size(294, 24);
            lblFront.TabIndex = 0;
            lblFront.Text = "Вид спереди (XY)";
            lblFront.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelTop
            // 
            panelTop.Controls.Add(pictureBoxTop);
            panelTop.Controls.Add(lblTop);
            panelTop.Dock = DockStyle.Fill;
            panelTop.Location = new Point(303, 3);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(294, 344);
            panelTop.TabIndex = 1;
            // 
            // pictureBoxTop
            // 
            pictureBoxTop.BackColor = Color.White;
            pictureBoxTop.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxTop.Dock = DockStyle.Fill;
            pictureBoxTop.Location = new Point(0, 24);
            pictureBoxTop.Name = "pictureBoxTop";
            pictureBoxTop.Size = new Size(294, 320);
            pictureBoxTop.TabIndex = 1;
            pictureBoxTop.TabStop = false;
            // 
            // lblTop
            // 
            lblTop.Dock = DockStyle.Top;
            lblTop.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTop.Location = new Point(0, 0);
            lblTop.Name = "lblTop";
            lblTop.Size = new Size(294, 24);
            lblTop.TabIndex = 0;
            lblTop.Text = "Вид сверху (XZ)";
            lblTop.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelSide
            // 
            panelSide.Controls.Add(pictureBoxSide);
            panelSide.Controls.Add(lblSide);
            panelSide.Dock = DockStyle.Fill;
            panelSide.Location = new Point(603, 3);
            panelSide.Name = "panelSide";
            panelSide.Size = new Size(294, 344);
            panelSide.TabIndex = 2;
            // 
            // pictureBoxSide
            // 
            pictureBoxSide.BackColor = Color.White;
            pictureBoxSide.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxSide.Dock = DockStyle.Fill;
            pictureBoxSide.Location = new Point(0, 24);
            pictureBoxSide.Name = "pictureBoxSide";
            pictureBoxSide.Size = new Size(294, 320);
            pictureBoxSide.TabIndex = 1;
            pictureBoxSide.TabStop = false;
            // 
            // lblSide
            // 
            lblSide.Dock = DockStyle.Top;
            lblSide.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSide.Location = new Point(0, 0);
            lblSide.Name = "lblSide";
            lblSide.Size = new Size(294, 24);
            lblSide.TabIndex = 0;
            lblSide.Text = "Вид сбоку (YZ)";
            lblSide.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ProjectionForm
            // 
            ClientSize = new Size(900, 350);
            Controls.Add(tableLayoutPanel);
            MinimumSize = new Size(700, 300);
            Name = "ProjectionForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Проекция кабине — три вида";
            tableLayoutPanel.ResumeLayout(false);
            panelFront.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxFront).EndInit();
            panelTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxTop).EndInit();
            panelSide.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxSide).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel;
        private Panel panelFront;
        private PictureBox pictureBoxFront;
        private Label lblFront;
        private Panel panelTop;
        private PictureBox pictureBoxTop;
        private Label lblTop;
        private Panel panelSide;
        private PictureBox pictureBoxSide;
        private Label lblSide;
    }
}