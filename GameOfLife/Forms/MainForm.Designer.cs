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
        // MainForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1195, 689);
        Controls.Add(panelMundo);
        Controls.Add(buttonGenerarGrupo);
        MaximizeBox = false;
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        Text = "MainForm";
        ResumeLayout(false);
    }

    private System.Windows.Forms.ToolTip toolTipCoso;

    private System.Windows.Forms.Panel panelMundo;

    private System.Windows.Forms.Button buttonGenerarGrupo;

    #endregion
}