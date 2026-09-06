namespace computernayGrafika
{
    partial class DrawingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.PictureBox pictureBoxCanvas;
        private System.Windows.Forms.Panel panelControls;
        private System.Windows.Forms.TextBox txtAngle;
        private System.Windows.Forms.Button btnDraw;
        private System.Windows.Forms.Button btnRotate;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label lblAngle;

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
            panelControls = new Panel();
            lblAngle = new Label();
            txtAngle = new TextBox();
            btnDraw = new Button();
            btnRotate = new Button();
            btnClear = new Button();
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
            // panelControls
            // 
            panelControls.BackColor = SystemColors.Control;
            panelControls.Controls.Add(lblAngle);
            panelControls.Controls.Add(txtAngle);
            panelControls.Controls.Add(btnDraw);
            panelControls.Controls.Add(btnRotate);
            panelControls.Controls.Add(btnClear);
            panelControls.Dock = DockStyle.Left;
            panelControls.Location = new Point(0, 0);
            panelControls.Name = "panelControls";
            panelControls.Size = new Size(200, 450);
            panelControls.TabIndex = 1;
            // 
            // lblAngle
            // 
            lblAngle.AutoSize = true;
            lblAngle.Location = new Point(12, 12);
            lblAngle.Name = "lblAngle";
            lblAngle.Size = new Size(44, 20);
            lblAngle.TabIndex = 0;
            lblAngle.Text = "Угол:";
            // 
            // txtAngle
            // 
            txtAngle.Location = new Point(56, 9);
            txtAngle.Name = "txtAngle";
            txtAngle.PlaceholderText = "deg";
            txtAngle.Size = new Size(60, 27);
            txtAngle.TabIndex = 1;
            // 
            // btnDraw
            // 
            btnDraw.Location = new Point(15, 60);
            btnDraw.Name = "btnDraw";
            btnDraw.Size = new Size(170, 30);
            btnDraw.TabIndex = 2;
            btnDraw.Text = "Построить";
            btnDraw.UseVisualStyleBackColor = true;
            btnDraw.Click += BtnDraw_Click;
            // 
            // btnRotate
            // 
            btnRotate.Location = new Point(15, 100);
            btnRotate.Name = "btnRotate";
            btnRotate.Size = new Size(170, 55);
            btnRotate.TabIndex = 3;
            btnRotate.Text = "Повернуть (относительно 0,0)";
            btnRotate.UseVisualStyleBackColor = true;
            btnRotate.Click += BtnRotate_Click;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(15, 165);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(170, 30);
            btnClear.TabIndex = 4;
            btnClear.Text = "Очистить";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += BtnClear_Click;
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
    }
}