namespace computernayGrafika
{
    partial class DrawingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.PictureBox pictureBoxCanvas;

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

        private void InitializeComponent()
        {
            pictureBoxCanvas = new PictureBox();
            btnClear = new Button();
            btnDraw = new Button();
            panelControls = new Panel();
            lblDepth = new Label();
            txtDepth = new TextBox();
            lblAngleX = new Label();
            txtAngleX = new TextBox();
            BtnRotateX = new Button();
            lblAngleY = new Label();
            txtAngleY = new TextBox();
            BtnRotateY = new Button();
            lblAngleZ = new Label();
            txtAngleZ = new TextBox();
            BtnRotateZ = new Button();
            MoveButton = new Button();
            ScaleButton = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBoxCanvas).BeginInit();
            panelControls.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBoxCanvas
            // 
            pictureBoxCanvas.BackColor = Color.White;
            pictureBoxCanvas.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxCanvas.Dock = DockStyle.Fill;
            pictureBoxCanvas.Location = new Point(200, 0);
            pictureBoxCanvas.Name = "pictureBoxCanvas";
            pictureBoxCanvas.Size = new Size(600, 450);
            pictureBoxCanvas.TabIndex = 0;
            pictureBoxCanvas.TabStop = false;
            pictureBoxCanvas.Paint += PictureBoxCanvas_Paint;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(15, 400);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(170, 30);
            btnClear.TabIndex = 10;
            btnClear.Text = "Очистить";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += BtnClear_Click;
            // 
            // btnDraw
            // 
            btnDraw.Location = new Point(15, 15);
            btnDraw.Name = "btnDraw";
            btnDraw.Size = new Size(170, 30);
            btnDraw.TabIndex = 0;
            btnDraw.Text = "Построить";
            btnDraw.UseVisualStyleBackColor = true;
            btnDraw.Click += BtnDraw_Click;
            // 
            // panelControls
            // 
            panelControls.BackColor = SystemColors.Control;
            panelControls.Controls.Add(lblDepth);
            panelControls.Controls.Add(txtDepth);
            panelControls.Controls.Add(lblAngleX);
            panelControls.Controls.Add(txtAngleX);
            panelControls.Controls.Add(BtnRotateX);
            panelControls.Controls.Add(lblAngleY);
            panelControls.Controls.Add(txtAngleY);
            panelControls.Controls.Add(BtnRotateY);
            panelControls.Controls.Add(lblAngleZ);
            panelControls.Controls.Add(txtAngleZ);
            panelControls.Controls.Add(BtnRotateZ);
            panelControls.Controls.Add(MoveButton);
            panelControls.Controls.Add(ScaleButton);
            panelControls.Controls.Add(btnDraw);
            panelControls.Controls.Add(btnClear);
            panelControls.Dock = DockStyle.Left;
            panelControls.Location = new Point(0, 0);
            panelControls.Name = "panelControls";
            panelControls.Size = new Size(200, 450);
            panelControls.TabIndex = 1;
            // 
            // lblDepth
            // 
            lblDepth.AutoSize = true;
            lblDepth.Location = new Point(15, 58);
            lblDepth.Name = "lblDepth";
            lblDepth.Size = new Size(64, 20);
            lblDepth.TabIndex = 1;
            lblDepth.Text = "Глубина:";
            // 
            // txtDepth
            // 
            txtDepth.Location = new Point(85, 55);
            txtDepth.Name = "txtDepth";
            txtDepth.PlaceholderText = "depth";
            txtDepth.Size = new Size(60, 27);
            txtDepth.TabIndex = 2;
            // 
            // lblAngleX
            // 
            lblAngleX.AutoSize = true;
            lblAngleX.Location = new Point(15, 96);
            lblAngleX.Name = "lblAngleX";
            lblAngleX.Size = new Size(20, 20);
            lblAngleX.TabIndex = 3;
            lblAngleX.Text = "X:";
            // 
            // txtAngleX
            // 
            txtAngleX.Location = new Point(38, 93);
            txtAngleX.Name = "txtAngleX";
            txtAngleX.PlaceholderText = "deg";
            txtAngleX.Size = new Size(45, 27);
            txtAngleX.TabIndex = 4;
            // 
            // BtnRotateX
            // 
            BtnRotateX.Location = new Point(90, 92);
            BtnRotateX.Name = "BtnRotateX";
            BtnRotateX.Size = new Size(95, 29);
            BtnRotateX.TabIndex = 5;
            BtnRotateX.Text = "Повернуть X";
            BtnRotateX.UseVisualStyleBackColor = true;
            BtnRotateX.Click += BtnRotateX_Click;
            // 
            // lblAngleY
            // 
            lblAngleY.AutoSize = true;
            lblAngleY.Location = new Point(15, 132);
            lblAngleY.Name = "lblAngleY";
            lblAngleY.Size = new Size(20, 20);
            lblAngleY.TabIndex = 6;
            lblAngleY.Text = "Y:";
            // 
            // txtAngleY
            // 
            txtAngleY.Location = new Point(38, 129);
            txtAngleY.Name = "txtAngleY";
            txtAngleY.PlaceholderText = "deg";
            txtAngleY.Size = new Size(45, 27);
            txtAngleY.TabIndex = 7;
            // 
            // BtnRotateY
            // 
            BtnRotateY.Location = new Point(90, 128);
            BtnRotateY.Name = "BtnRotateY";
            BtnRotateY.Size = new Size(95, 29);
            BtnRotateY.TabIndex = 8;
            BtnRotateY.Text = "Повернуть Y";
            BtnRotateY.UseVisualStyleBackColor = true;
            BtnRotateY.Click += BtnRotateY_Click;
            // 
            // lblAngleZ
            // 
            lblAngleZ.AutoSize = true;
            lblAngleZ.Location = new Point(15, 168);
            lblAngleZ.Name = "lblAngleZ";
            lblAngleZ.Size = new Size(20, 20);
            lblAngleZ.TabIndex = 9;
            lblAngleZ.Text = "Z:";
            // 
            // txtAngleZ
            // 
            txtAngleZ.Location = new Point(38, 165);
            txtAngleZ.Name = "txtAngleZ";
            txtAngleZ.PlaceholderText = "deg";
            txtAngleZ.Size = new Size(45, 27);
            txtAngleZ.TabIndex = 10;
            // 
            // BtnRotateZ
            // 
            BtnRotateZ.Location = new Point(90, 164);
            BtnRotateZ.Name = "BtnRotateZ";
            BtnRotateZ.Size = new Size(95, 29);
            BtnRotateZ.TabIndex = 11;
            BtnRotateZ.Text = "Повернуть Z";
            BtnRotateZ.UseVisualStyleBackColor = true;
            BtnRotateZ.Click += BtnRotateZ_Click;
            // 
            // MoveButton
            // 
            MoveButton.Location = new Point(15, 210);
            MoveButton.Name = "MoveButton";
            MoveButton.Size = new Size(170, 29);
            MoveButton.TabIndex = 12;
            MoveButton.Text = "Переместить";
            MoveButton.UseVisualStyleBackColor = true;
            MoveButton.Click += MoveButton_Click;
            // 
            // ScaleButton
            // 
            ScaleButton.Location = new Point(15, 246);
            ScaleButton.Name = "ScaleButton";
            ScaleButton.Size = new Size(170, 29);
            ScaleButton.TabIndex = 13;
            ScaleButton.Text = "Масштабировать";
            ScaleButton.UseVisualStyleBackColor = true;
            ScaleButton.Click += ScaleButton_Click;
            // 
            // DrawingForm
            // 
            ClientSize = new Size(800, 450);
            Controls.Add(pictureBoxCanvas);
            Controls.Add(panelControls);
            Name = "DrawingForm";
            Text = "Бригада №3: Бояркин, Мишин, Толстоухов";
            ((System.ComponentModel.ISupportInitialize)pictureBoxCanvas).EndInit();
            panelControls.ResumeLayout(false);
            panelControls.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private Button btnClear;
        private Button btnDraw;
        private Panel panelControls;
        private Label lblDepth;
        private TextBox txtDepth;
        private Label lblAngleX;
        private TextBox txtAngleX;
        private Button BtnRotateX;
        private Label lblAngleY;
        private TextBox txtAngleY;
        private Button BtnRotateY;
        private Label lblAngleZ;
        private TextBox txtAngleZ;
        private Button BtnRotateZ;
        private Button MoveButton;
        private Button ScaleButton;
    }
}