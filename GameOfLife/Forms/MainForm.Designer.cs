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
        buttonAvanzarTiempo = new System.Windows.Forms.Button();
        buttonDestruccionGlobal = new System.Windows.Forms.Button();
        buttonResucitar = new System.Windows.Forms.Button();
        button1 = new System.Windows.Forms.Button();
        button2 = new System.Windows.Forms.Button();
        buttonExportarJSON = new System.Windows.Forms.Button();
        buttonTransferirCoso = new System.Windows.Forms.Button();
        SuspendLayout();
        // 
        // buttonGenerarGrupo
        // 
        buttonGenerarGrupo.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
        buttonGenerarGrupo.Location = new System.Drawing.Point(946, 12);
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
        buttonGenerarParejas.Location = new System.Drawing.Point(946, 91);
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
        buttonGenerarHijos.Location = new System.Drawing.Point(946, 167);
        buttonGenerarHijos.Name = "buttonGenerarHijos";
        buttonGenerarHijos.Size = new System.Drawing.Size(223, 67);
        buttonGenerarHijos.TabIndex = 4;
        buttonGenerarHijos.Text = "Generar Hijos";
        buttonGenerarHijos.UseVisualStyleBackColor = true;
        buttonGenerarHijos.Click += buttonGenerarHijos_Click;
        // 
        // buttonCovid
        // 
        buttonCovid.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
        buttonCovid.Location = new System.Drawing.Point(946, 240);
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
        buttonCine.Location = new System.Drawing.Point(946, 315);
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
        buttonGuerra.Location = new System.Drawing.Point(946, 389);
        buttonGuerra.Name = "buttonGuerra";
        buttonGuerra.Size = new System.Drawing.Size(223, 68);
        buttonGuerra.TabIndex = 7;
        buttonGuerra.Text = "GUERRA";
        buttonGuerra.UseVisualStyleBackColor = true;
        buttonGuerra.Click += buttonGuerra_Click;
        // 
        // buttonReset
        // 
        buttonReset.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
        buttonReset.Location = new System.Drawing.Point(27, 637);
        buttonReset.Name = "buttonReset";
        buttonReset.Size = new System.Drawing.Size(221, 41);
        buttonReset.TabIndex = 8;
        buttonReset.Text = "RESET";
        buttonReset.UseVisualStyleBackColor = true;
        buttonReset.Click += buttonReset_Click;
        // 
        // buttonAvanzarTiempo
        // 
        buttonAvanzarTiempo.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
        buttonAvanzarTiempo.Location = new System.Drawing.Point(266, 638);
        buttonAvanzarTiempo.Name = "buttonAvanzarTiempo";
        buttonAvanzarTiempo.Size = new System.Drawing.Size(212, 40);
        buttonAvanzarTiempo.TabIndex = 9;
        buttonAvanzarTiempo.Text = "Avanzar Tiempo";
        buttonAvanzarTiempo.UseVisualStyleBackColor = true;
        buttonAvanzarTiempo.Click += buttonAvanzarTiempo_Click;
        // 
        // buttonDestruccionGlobal
        // 
        buttonDestruccionGlobal.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
        buttonDestruccionGlobal.Location = new System.Drawing.Point(499, 637);
        buttonDestruccionGlobal.Name = "buttonDestruccionGlobal";
        buttonDestruccionGlobal.Size = new System.Drawing.Size(206, 41);
        buttonDestruccionGlobal.TabIndex = 10;
        buttonDestruccionGlobal.Text = "Destruccion Global";
        buttonDestruccionGlobal.UseVisualStyleBackColor = true;
        buttonDestruccionGlobal.Click += buttonDestruccionGlobal_Click;
        // 
        // buttonResucitar
        // 
        buttonResucitar.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
        buttonResucitar.Location = new System.Drawing.Point(723, 636);
        buttonResucitar.Name = "buttonResucitar";
        buttonResucitar.Size = new System.Drawing.Size(221, 41);
        buttonResucitar.TabIndex = 11;
        buttonResucitar.Text = "RESUCITAR";
        buttonResucitar.UseVisualStyleBackColor = true;
        buttonResucitar.Click += buttonResucitar_Click;
        // 
        // button1
        // 
        button1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
        button1.Location = new System.Drawing.Point(946, 556);
        button1.Name = "button1";
        button1.Size = new System.Drawing.Size(223, 68);
        button1.TabIndex = 12;
        button1.Text = "CARGAR DB";
        button1.UseVisualStyleBackColor = true;
        button1.Click += buttonCargarDB_Click;
        // 
        // button2
        // 
        button2.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
        button2.Location = new System.Drawing.Point(946, 482);
        button2.Name = "button2";
        button2.Size = new System.Drawing.Size(223, 68);
        button2.TabIndex = 13;
        button2.Text = "GUARDAR DB";
        button2.UseVisualStyleBackColor = true;
        button2.Click += buttonGuardarDB_Click;
        // 
        // buttonExportarJSON
        // 
        buttonExportarJSON.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
        buttonExportarJSON.Location = new System.Drawing.Point(1089, 637);
        buttonExportarJSON.Name = "buttonExportarJSON";
        buttonExportarJSON.Size = new System.Drawing.Size(94, 41);
        buttonExportarJSON.TabIndex = 14;
        buttonExportarJSON.Text = "JSON";
        buttonExportarJSON.UseVisualStyleBackColor = true;
        buttonExportarJSON.Click += buttonExportarJSON_Click;
        // 
        // buttonTransferirCoso
        // 
        buttonTransferirCoso.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
        buttonTransferirCoso.Location = new System.Drawing.Point(974, 636);
        buttonTransferirCoso.Name = "buttonTransferirCoso";
        buttonTransferirCoso.Size = new System.Drawing.Size(94, 41);
        buttonTransferirCoso.TabIndex = 15;
        buttonTransferirCoso.Text = "WIFI";
        buttonTransferirCoso.UseVisualStyleBackColor = true;
        buttonTransferirCoso.Click += buttonTransferirCoso_Click;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1195, 689);
        Controls.Add(buttonTransferirCoso);
        Controls.Add(buttonExportarJSON);
        Controls.Add(button2);
        Controls.Add(button1);
        Controls.Add(buttonResucitar);
        Controls.Add(buttonDestruccionGlobal);
        Controls.Add(buttonAvanzarTiempo);
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

    private System.Windows.Forms.Button buttonTransferirCoso;

    private System.Windows.Forms.Button buttonExportarJSON;

    private System.Windows.Forms.Button button1;

    private System.Windows.Forms.Button buttonResucitar;

    private System.Windows.Forms.Button buttonDestruccionGlobal;

    private System.Windows.Forms.Button buttonAvanzarTiempo;

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