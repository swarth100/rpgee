namespace RPGEE
{
    partial class RpgEE
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.mapBtn = new System.Windows.Forms.Button();
            this.playersBtn = new System.Windows.Forms.Button();
            this.areasBtn = new System.Windows.Forms.Button();
            this.gameBtn = new System.Windows.Forms.Button();
            this.spritesBtn = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.mapBtn, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.playersBtn, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.areasBtn, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.gameBtn, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.spritesBtn, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.button4, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(928, 577);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // mapBtn
            // 
            this.mapBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapBtn.Location = new System.Drawing.Point(3, 3);
            this.mapBtn.Name = "mapBtn";
            this.mapBtn.Size = new System.Drawing.Size(303, 282);
            this.mapBtn.TabIndex = 0;
            this.mapBtn.Text = "MAP";
            this.mapBtn.UseVisualStyleBackColor = true;
            // 
            // playersBtn
            // 
            this.playersBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playersBtn.Location = new System.Drawing.Point(312, 3);
            this.playersBtn.Name = "playersBtn";
            this.playersBtn.Size = new System.Drawing.Size(303, 282);
            this.playersBtn.TabIndex = 1;
            this.playersBtn.Text = "Players";
            this.playersBtn.UseVisualStyleBackColor = true;
            // 
            // areasBtn
            // 
            this.areasBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.areasBtn.Location = new System.Drawing.Point(621, 3);
            this.areasBtn.Name = "areasBtn";
            this.areasBtn.Size = new System.Drawing.Size(304, 282);
            this.areasBtn.TabIndex = 2;
            this.areasBtn.Text = "Areas";
            this.areasBtn.UseVisualStyleBackColor = true;
            // 
            // gameBtn
            // 
            this.gameBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gameBtn.Location = new System.Drawing.Point(3, 291);
            this.gameBtn.Name = "gameBtn";
            this.gameBtn.Size = new System.Drawing.Size(303, 283);
            this.gameBtn.TabIndex = 3;
            this.gameBtn.Text = "Game";
            this.gameBtn.UseVisualStyleBackColor = true;
            // 
            // spritesBtn
            // 
            this.spritesBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spritesBtn.Location = new System.Drawing.Point(312, 291);
            this.spritesBtn.Name = "spritesBtn";
            this.spritesBtn.Size = new System.Drawing.Size(303, 283);
            this.spritesBtn.TabIndex = 4;
            this.spritesBtn.Text = "Sprites";
            this.spritesBtn.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(621, 291);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 5;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // RpgEE
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(928, 577);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "RpgEE";
            this.Text = "Form1";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button mapBtn;
        private System.Windows.Forms.Button playersBtn;
        private System.Windows.Forms.Button areasBtn;
        private System.Windows.Forms.Button gameBtn;
        private System.Windows.Forms.Button spritesBtn;
        private System.Windows.Forms.Button button4;
    }
}

