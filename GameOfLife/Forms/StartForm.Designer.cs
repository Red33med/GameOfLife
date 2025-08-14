using System.ComponentModel;

namespace GameOfLife.Forms;

partial class StartForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

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
        btnJugar = new System.Windows.Forms.Button();
        lblTitulo = new System.Windows.Forms.Label();
        label1 = new System.Windows.Forms.Label();
        SuspendLayout();
        // 
        // btnJugar
        // 
        btnJugar.BackColor = System.Drawing.Color.Ivory;
        btnJugar.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
        btnJugar.Location = new System.Drawing.Point(297, 266);
        btnJugar.Name = "btnJugar";
        btnJugar.Size = new System.Drawing.Size(227, 72);
        btnJugar.TabIndex = 0;
        btnJugar.Text = "JUGAR";
        btnJugar.UseVisualStyleBackColor = false;
        btnJugar.Click += btnJugar_Click;
        // 
        // lblTitulo
        // 
        lblTitulo.AutoSize = true;
        lblTitulo.BackColor = System.Drawing.SystemColors.ButtonHighlight;
        lblTitulo.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Bold);
        lblTitulo.Location = new System.Drawing.Point(250, 100);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new System.Drawing.Size(363, 37);
        lblTitulo.TabIndex = 1;
        lblTitulo.Text = "EL JUEGO DE LA VIDA";
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
        label1.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Bold);
        label1.Location = new System.Drawing.Point(140, 191);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(599, 37);
        label1.TabIndex = 2;
        label1.Text = "EDDY LEONARDO VASQUEZ MORENO";
        // 
        // StartForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        BackColor = System.Drawing.Color.DarkSeaGreen;
        ClientSize = new System.Drawing.Size(841, 502);
        Controls.Add(label1);
        Controls.Add(btnJugar);
        Controls.Add(lblTitulo);
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        Text = "StartForm";
        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button btnJugar;
    private System.Windows.Forms.Label lblTitulo;

    #endregion
}