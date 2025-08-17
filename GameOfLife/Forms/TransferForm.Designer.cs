using System.ComponentModel;

namespace GameOfLife.Forms;
partial class TransferForm
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
        labelMyIP = new System.Windows.Forms.Label();
        label1 = new System.Windows.Forms.Label();
        listBoxCosos = new System.Windows.Forms.ListBox();
        label2 = new System.Windows.Forms.Label();
        textBoxIP = new System.Windows.Forms.TextBox();
        label3 = new System.Windows.Forms.Label();
        listBoxPlayers = new System.Windows.Forms.ListBox();
        buttonScanNetwork = new System.Windows.Forms.Button();
        buttonSendCoso = new System.Windows.Forms.Button();
        buttonStartListening = new System.Windows.Forms.Button();
        buttonStopListening = new System.Windows.Forms.Button();
        label4 = new System.Windows.Forms.Label();
        textBoxLog = new System.Windows.Forms.TextBox();
        SuspendLayout();
        // 
        // labelMyIP
        // 
        labelMyIP.AutoSize = true;
        labelMyIP.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        labelMyIP.ForeColor = System.Drawing.Color.DarkBlue;
        labelMyIP.Location = new System.Drawing.Point(20, 20);
        labelMyIP.Name = "labelMyIP";
        labelMyIP.Size = new System.Drawing.Size(150, 16);
        labelMyIP.TabIndex = 0;
        labelMyIP.Text = "Mi IP: Obteniendo...";
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new System.Drawing.Point(20, 60);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(85, 15);
        label1.TabIndex = 1;
        label1.Text = "👥 Mis Cosos:";
        // 
        // listBoxCosos
        // 
        listBoxCosos.FormattingEnabled = true;
        listBoxCosos.ItemHeight = 15;
        listBoxCosos.Location = new System.Drawing.Point(20, 90);
        listBoxCosos.Name = "listBoxCosos";
        listBoxCosos.Size = new System.Drawing.Size(350, 199);
        listBoxCosos.TabIndex = 2;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new System.Drawing.Point(400, 60);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(82, 15);
        label2.TabIndex = 3;
        label2.Text = "IP de destino:";
        // 
        // textBoxIP
        // 
        textBoxIP.Location = new System.Drawing.Point(400, 90);
        textBoxIP.Name = "textBoxIP";
        textBoxIP.PlaceholderText = "192.168.1.100";
        textBoxIP.Size = new System.Drawing.Size(150, 23);
        textBoxIP.TabIndex = 4;
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Location = new System.Drawing.Point(400, 130);
        label3.Name = "label3";
        label3.Size = new System.Drawing.Size(134, 15);
        label3.TabIndex = 5;
        label3.Text = "Jugadores encontrados:";
        // 
        // listBoxPlayers
        // 
        listBoxPlayers.FormattingEnabled = true;
        listBoxPlayers.ItemHeight = 15;
        listBoxPlayers.Location = new System.Drawing.Point(400, 160);
        listBoxPlayers.Name = "listBoxPlayers";
        listBoxPlayers.Size = new System.Drawing.Size(200, 124);
        listBoxPlayers.TabIndex = 6;
        // 
        // buttonScanNetwork
        // 
        buttonScanNetwork.BackColor = System.Drawing.Color.DarkCyan;
        buttonScanNetwork.ForeColor = System.Drawing.Color.White;
        buttonScanNetwork.Location = new System.Drawing.Point(620, 160);
        buttonScanNetwork.Name = "buttonScanNetwork";
        buttonScanNetwork.Size = new System.Drawing.Size(120, 35);
        buttonScanNetwork.TabIndex = 7;
        buttonScanNetwork.Text = "Buscar Jugadores";
        buttonScanNetwork.UseVisualStyleBackColor = false;
        // 
        // buttonSendCoso
        // 
        buttonSendCoso.BackColor = System.Drawing.Color.DarkGreen;
        buttonSendCoso.ForeColor = System.Drawing.Color.White;
        buttonSendCoso.Location = new System.Drawing.Point(620, 200);
        buttonSendCoso.Name = "buttonSendCoso";
        buttonSendCoso.Size = new System.Drawing.Size(120, 35);
        buttonSendCoso.TabIndex = 8;
        buttonSendCoso.Text = "Enviar Coso";
        buttonSendCoso.UseVisualStyleBackColor = false;
        // 
        // buttonStartListening
        // 
        buttonStartListening.BackColor = System.Drawing.Color.DarkBlue;
        buttonStartListening.ForeColor = System.Drawing.Color.White;
        buttonStartListening.Location = new System.Drawing.Point(620, 240);
        buttonStartListening.Name = "buttonStartListening";
        buttonStartListening.Size = new System.Drawing.Size(120, 35);
        buttonStartListening.TabIndex = 9;
        buttonStartListening.Text = "Empezar a Escuchar";
        buttonStartListening.UseVisualStyleBackColor = false;
        // 
        // buttonStopListening
        // 
        buttonStopListening.BackColor = System.Drawing.Color.DarkRed;
        buttonStopListening.Enabled = false;
        buttonStopListening.ForeColor = System.Drawing.Color.White;
        buttonStopListening.Location = new System.Drawing.Point(620, 280);
        buttonStopListening.Name = "buttonStopListening";
        buttonStopListening.Size = new System.Drawing.Size(120, 35);
        buttonStopListening.TabIndex = 10;
        buttonStopListening.Text = "Parar de Escuchar";
        buttonStopListening.UseVisualStyleBackColor = false;
        // 
        // label4
        // 
        label4.AutoSize = true;
        label4.Location = new System.Drawing.Point(20, 310);
        label4.Name = "label4";
        label4.Size = new System.Drawing.Size(98, 15);
        label4.TabIndex = 11;
        label4.Text = "Log de actividad:";
        // 
        // textBoxLog
        // 
        textBoxLog.BackColor = System.Drawing.Color.Black;
        textBoxLog.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        textBoxLog.ForeColor = System.Drawing.Color.LimeGreen;
        textBoxLog.Location = new System.Drawing.Point(20, 340);
        textBoxLog.Multiline = true;
        textBoxLog.Name = "textBoxLog";
        textBoxLog.ReadOnly = true;
        textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        textBoxLog.Size = new System.Drawing.Size(720, 200);
        textBoxLog.TabIndex = 12;
        // 
        // TransferForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(784, 561);
        Controls.Add(this.textBoxLog);
        Controls.Add(this.label4);
        Controls.Add(this.buttonStopListening);
        Controls.Add(this.buttonStartListening);
        Controls.Add(this.buttonSendCoso);
        Controls.Add(this.buttonScanNetwork);
        Controls.Add(this.listBoxPlayers);
        Controls.Add(this.label3);
        Controls.Add(this.textBoxIP);
        Controls.Add(this.label2);
        Controls.Add(this.listBoxCosos);
        Controls.Add(this.label1);
        Controls.Add(this.labelMyIP);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        Name = "TransferForm";
        StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        Text = "Transferir Cosos por WiFi";
        ResumeLayout(false);
        PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label labelMyIP;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ListBox listBoxCosos;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox textBoxIP;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.ListBox listBoxPlayers;
    private System.Windows.Forms.Button buttonScanNetwork;
    private System.Windows.Forms.Button buttonSendCoso;
    private System.Windows.Forms.Button buttonStartListening;
    private System.Windows.Forms.Button buttonStopListening;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox textBoxLog;
}
