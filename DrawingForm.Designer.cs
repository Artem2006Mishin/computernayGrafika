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
            BtnRotate = new Button();
            btnDraw = new Button();
            txtAngle = new TextBox();
            lblAngle = new Label();
            panelControls = new Panel();
            ScaleButton = new Button();
            MoveButton = new Button();
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
            btnClear.Location = new Point(15, 384);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(170, 30);
            btnClear.TabIndex = 4;
            btnClear.Text = "Очистить";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += BtnClear_Click;
            // 
            // BtnRotate
            // 
            BtnRotate.Location = new Point(15, 163);
            BtnRotate.Name = "BtnRotate";
            BtnRotate.Size = new Size(170, 27);
            BtnRotate.TabIndex = 3;
            BtnRotate.Text = "Повернуть ";
            BtnRotate.UseVisualStyleBackColor = true;
            BtnRotate.Click += BtnRotate_Click;
            // 
            // btnDraw
            // 
            btnDraw.Location = new Point(15, 38);
            btnDraw.Name = "btnDraw";
            btnDraw.Size = new Size(170, 30);
            btnDraw.TabIndex = 2;
            btnDraw.Text = "Построить";
            btnDraw.UseVisualStyleBackColor = true;
            btnDraw.Click += BtnDraw_Click;
            // 
            // txtAngle
            // 
            txtAngle.Location = new Point(65, 130);
            txtAngle.Name = "txtAngle";
            txtAngle.PlaceholderText = "deg";
            txtAngle.Size = new Size(60, 27);
            txtAngle.TabIndex = 1;
            // 
            // lblAngle
            // 
            lblAngle.AutoSize = true;
            lblAngle.Location = new Point(15, 133);
            lblAngle.Name = "lblAngle";
            lblAngle.Size = new Size(44, 20);
            lblAngle.TabIndex = 0;
            lblAngle.Text = "Угол:";
            // 
            // panelControls
            // 
            panelControls.BackColor = SystemColors.Control;
            panelControls.Controls.Add(ScaleButton);
            panelControls.Controls.Add(MoveButton);
            panelControls.Controls.Add(lblAngle);
            panelControls.Controls.Add(txtAngle);
            panelControls.Controls.Add(btnDraw);
            panelControls.Controls.Add(BtnRotate);
            panelControls.Controls.Add(btnClear);
            panelControls.Dock = DockStyle.Left;
            panelControls.Location = new Point(0, 0);
            panelControls.Name = "panelControls";
            panelControls.Size = new Size(200, 450);
            panelControls.TabIndex = 1;
            // 
            // ScaleButton
            // 
            ScaleButton.Location = new Point(15, 231);
            ScaleButton.Name = "ScaleButton";
            ScaleButton.Size = new Size(170, 29);
            ScaleButton.TabIndex = 6;
            ScaleButton.Text = "Масштабировать";
            ScaleButton.UseVisualStyleBackColor = true;
            ScaleButton.Click += ScaleButton_Click;
            // 
            // MoveButton
            // 
            MoveButton.Location = new Point(15, 196);
            MoveButton.Name = "MoveButton";
            MoveButton.Size = new Size(170, 29);
            MoveButton.TabIndex = 5;
            MoveButton.Text = "Переместить";
            MoveButton.UseVisualStyleBackColor = true;
            MoveButton.Click += MoveButton_Click;
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
        private Button BtnRotate;
        private Button btnDraw;
        private TextBox txtAngle;
        private Label lblAngle;
        private Panel panelControls;
        private Button ScaleButton;
        private Button MoveButton;
    }
}