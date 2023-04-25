namespace WindowsFormsApp1
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtCEP = new System.Windows.Forms.TextBox();
            this.HttpClient = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtCEP
            // 
            this.txtCEP.Location = new System.Drawing.Point(280, 115);
            this.txtCEP.Name = "txtCEP";
            this.txtCEP.Size = new System.Drawing.Size(100, 20);
            this.txtCEP.TabIndex = 0;
            // 
            // HttpClient
            // 
            this.HttpClient.Location = new System.Drawing.Point(209, 149);
            this.HttpClient.Name = "HttpClient";
            this.HttpClient.Size = new System.Drawing.Size(110, 34);
            this.HttpClient.TabIndex = 2;
            this.HttpClient.Text = "Pesquisar";
            this.HttpClient.UseVisualStyleBackColor = true;
            this.HttpClient.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(206, 118);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Insira o CEP:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(896, 480);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.HttpClient);
            this.Controls.Add(this.txtCEP);
            this.Name = "Form1";
            this.Text = "Pesquisador de CEP";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtCEP;
        private System.Windows.Forms.Button HttpClient;
        private System.Windows.Forms.Label label1;
    }
}

