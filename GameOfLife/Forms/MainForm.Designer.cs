namespace GameOfLife.Forms;

partial class MainForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
        components = new System.ComponentModel.Container();
        buttonGenerarGrupo = new System.Windows.Forms.Button();
        panelMundo = new System.Windows.Forms.Panel();
        toolTipCoso = new System.Windows.Forms.ToolTip(components);
        buttonGenerarParejas = new System.Windows.Forms.Button();
        buttonGenerarHijos = new System.Windows.Forms.Button();
        buttonCovid = new System.Windows.Forms.Button();
        buttonCine = new System.Windows.Forms.Button();
        buttonGuerra = new System.Windows.Forms.Button();
        buttonReset = new System.Windows.Forms.Button();
        SuspendLayout();
        // 
        // buttonGenerarGrupo
        // 
        buttonGenerarGrupo.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
        buttonGenerarGrupo.Location = new System.Drawing.Point(946, 39);
        buttonGenerarGrupo.Name = "buttonGenerarGrupo";
        buttonGenerarGrupo.Size = new System.Drawing.Size(223, 73);
        buttonGenerarGrupo.TabIndex = 1;
        buttonGenerarGrupo.Text = "Generar Grupo";
        buttonGenerarGrupo.UseVisualStyleBackColor = true;
        buttonGenerarGrupo.Click += buttonGenerarGrupo_Click;
        // 
        // panelMundo
        // 
        panelMundo.Location = new System.Drawing.Point(12, 12);
        panelMundo.Name = "panelMundo";
        panelMundo.Size = new System.Drawing.Size(900, 600);
        panelMundo.TabIndex = 2;
        panelMundo.Paint += panelMundo_Paint;
        panelMundo.MouseClick += panelMundo_MouseClick;
        panelMundo.MouseMove += panelMundo_MouseMove;
        // 
        // buttonGenerarParejas
        // 
        buttonGenerarParejas.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
        buttonGenerarParejas.Location = new System.Drawing.Point(946, 144);
        buttonGenerarParejas.Name = "buttonGenerarParejas";
        buttonGenerarParejas.Size = new System.Drawing.Size(223, 70);
        buttonGenerarParejas.TabIndex = 3;
        buttonGenerarParejas.Text = "Generar Pareja";
        buttonGenerarParejas.UseVisualStyleBackColor = true;
        buttonGenerarParejas.Click += buttonGenerarParejas_Click;
        // 
        // buttonGenerarHijos
        // 
        buttonGenerarHijos.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
        buttonGenerarHijos.Location = new System.Drawing.Point(946, 240);
        buttonGenerarHijos.Name = "buttonGenerarHijos";
        buttonGenerarHijos.Size = new System.Drawing.Size(223, 67);
        buttonGenerarHijos.TabIndex = 4;
        buttonGenerarHijos.Text = "Generar Hijos";
        buttonGenerarHijos.UseVisualStyleBackColor = true;
        // 
        // buttonCovid
        // 
        buttonCovid.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
        buttonCovid.Location = new System.Drawing.Point(946, 335);
        buttonCovid.Name = "buttonCovid";
        buttonCovid.Size = new System.Drawing.Size(223, 69);
        buttonCovid.TabIndex = 5;
        buttonCovid.Text = "COVID";
        buttonCovid.UseVisualStyleBackColor = true;
        buttonCovid.Click += buttonCovid_Click;
        // 
        // buttonCine
        // 
        buttonCine.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
        buttonCine.Location = new System.Drawing.Point(946, 437);
        buttonCine.Name = "buttonCine";
        buttonCine.Size = new System.Drawing.Size(223, 68);
        buttonCine.TabIndex = 6;
        buttonCine.Text = "CINE";
        buttonCine.UseVisualStyleBackColor = true;
        buttonCine.Click += buttonCine_Click;
        // 
        // buttonGuerra
        // 
        buttonGuerra.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
        buttonGuerra.Location = new System.Drawing.Point(946, 531);
        buttonGuerra.Name = "buttonGuerra";
        buttonGuerra.Size = new System.Drawing.Size(223, 68);
        buttonGuerra.TabIndex = 7;
        buttonGuerra.Text = "GUERRA";
        buttonGuerra.UseVisualStyleBackColor = true;
        buttonGuerra.Click += buttonGuerra_Click;
        // 
        // buttonReset
        // 
        buttonReset.Location = new System.Drawing.Point(57, 636);
        buttonReset.Name = "buttonReset";
        buttonReset.Size = new System.Drawing.Size(221, 41);
        buttonReset.TabIndex = 8;
        buttonReset.Text = "RESET";
        buttonReset.UseVisualStyleBackColor = true;
        buttonReset.Click += buttonReset_Click;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1195, 689);
        Controls.Add(buttonReset);
        Controls.Add(buttonGuerra);
        Controls.Add(buttonCine);
        Controls.Add(buttonCovid);
        Controls.Add(buttonGenerarHijos);
        Controls.Add(buttonGenerarParejas);
        Controls.Add(panelMundo);
        Controls.Add(buttonGenerarGrupo);
        MaximizeBox = false;
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        Text = "MainForm";
        ResumeLayout(false);
    }

    private System.Windows.Forms.Button buttonReset;

    private System.Windows.Forms.Button buttonGuerra;

    private System.Windows.Forms.Button buttonGenerarHijos;

    private System.Windows.Forms.Button ss;

    private System.Windows.Forms.Button buttonGenerarParejas;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.Button buttonCovid;
    private System.Windows.Forms.Button buttonCine;

    private System.Windows.Forms.ToolTip toolTipCoso;

    private System.Windows.Forms.Panel panelMundo;

    private System.Windows.Forms.Button buttonGenerarGrupo;

    #endregion
}