namespace SimplexUI
{
   partial class Form1
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
         this.btn_Procesar = new System.Windows.Forms.Button();
         this.BorrarModelo = new System.Windows.Forms.Button();
         this.SuspendLayout();
         // 
         // btn_Procesar
         // 
         this.btn_Procesar.Location = new System.Drawing.Point(12, 12);
         this.btn_Procesar.Name = "btn_Procesar";
         this.btn_Procesar.Size = new System.Drawing.Size(75, 23);
         this.btn_Procesar.TabIndex = 0;
         this.btn_Procesar.Text = "Procesar";
         this.btn_Procesar.UseVisualStyleBackColor = true;
         this.btn_Procesar.Click += new System.EventHandler(this.Btn_Procesar_Click);
         // 
         // BorrarModelo
         // 
         this.BorrarModelo.Location = new System.Drawing.Point(13, 42);
         this.BorrarModelo.Name = "BorrarModelo";
         this.BorrarModelo.Size = new System.Drawing.Size(75, 23);
         this.BorrarModelo.TabIndex = 1;
         this.BorrarModelo.Text = "BorrarModelos";
         this.BorrarModelo.UseVisualStyleBackColor = true;
         this.BorrarModelo.Click += new System.EventHandler(this.button1_Click);
         // 
         // Form1
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(417, 313);
         this.Controls.Add(this.BorrarModelo);
         this.Controls.Add(this.btn_Procesar);
         this.Name = "Form1";
         this.Text = "Form1";
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Button btn_Procesar;
      private System.Windows.Forms.Button BorrarModelo;
   }
}

