using GameOfLife.Models;
using GameOfLife.Utils;

namespace GameOfLife.Forms;
// TransferForm.cs - Solo el código, SIN creación de controles
public partial class TransferForm : Form
{
    private CosoTransferManager _transferManager;
    private List<Coso> _cosos;
    
    public event Action<Coso>? OnCosoReceived;

    public TransferForm(List<Coso> cosos)
    {
        InitializeComponent(); // Este método se genera automáticamente
        
        _cosos = cosos ?? new List<Coso>();
        _transferManager = new CosoTransferManager();
        
        ConfigurarFormularioDespuesDelDesigner();
        ConfigurarEventos();
    }

    private void ConfigurarFormularioDespuesDelDesigner()
    {
        // Configurar propiedades que no se pueden hacer en el Designer
        labelMyIP.Text = $"Mi IP: {_transferManager.GetLocalIP()}";
        
        // Configurar placeholder del textbox (si no se puede en Designer)
        textBoxIP.PlaceholderText = "192.168.1.100";
        
        // Cargar cosos vivos en el ListBox
        var cososVivos = _cosos.Where(c => c.Estado == Estado.Vivo).ToList();
        listBoxCosos.Items.Clear();
        foreach (var coso in cososVivos)
        {
            listBoxCosos.Items.Add(coso);
        }
        
        // Configurar el ListBox para mostrar nombres completos
        listBoxCosos.DisplayMember = "NombreCompleto";
        
        // Configurar textbox de log
        textBoxLog.BackColor = Color.Black;
        textBoxLog.ForeColor = Color.LimeGreen;
        textBoxLog.Font = new Font("Consolas", 9);
        
        // Configurar estado inicial de botones
        buttonStopListening.Enabled = false;
    }

    private void ConfigurarEventos()
    {
        // Eventos del TransferManager
        _transferManager.OnMessageReceived += (message) =>
        {
            this.Invoke(() =>
            {
                textBoxLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\r\n");
                textBoxLog.SelectionStart = textBoxLog.Text.Length;
                textBoxLog.ScrollToCaret();
            });
        };

        _transferManager.OnCosoReceived += (coso) =>
        {
            this.Invoke(() =>
            {
                var result = MessageBox.Show($"Has recibido un coso:\n\n" +
                                           $"Nombre: {coso.NombreCompleto}\n" +
                                           $"Edad: {coso.Edad}\n" +
                                           $"Sexo: {coso.Sexo}\n" +
                                           $"Estado: {coso.EstadoAnimo}\n\n" +
                                           $"¿Deseas aceptarlo?",
                                           "Coso Recibido", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    coso.Codigo = Guid.NewGuid();
                    coso.Posicion = new Point(50, 50);
                    
                    OnCosoReceived?.Invoke(coso);
                    MessageBox.Show("Coso agregado a tu población!", "Éxito", 
                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            });
        };

        // Eventos de controles
        buttonScanNetwork.Click += async (s, e) =>
        {
            buttonScanNetwork.Enabled = false;
            listBoxPlayers.Items.Clear();
            
            var players = await _transferManager.ScanLocalNetwork();
            foreach (var ip in players)
            {
                listBoxPlayers.Items.Add(ip);
            }
            
            buttonScanNetwork.Enabled = true;
        };

        listBoxPlayers.SelectedIndexChanged += (s, e) =>
        {
            if (listBoxPlayers.SelectedItem != null)
            {
                textBoxIP.Text = listBoxPlayers.SelectedItem.ToString();
            }
        };

        buttonSendCoso.Click += async (s, e) =>
        {
            if (listBoxCosos.SelectedItem == null)
            {
                MessageBox.Show("Selecciona un coso para enviar", "Error", 
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxIP.Text))
            {
                MessageBox.Show("Ingresa una IP de destino", "Error", 
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var coso = (Coso)listBoxCosos.SelectedItem;
            var result = MessageBox.Show($"¿Enviar {coso.NombreCompleto} a {textBoxIP.Text}?", 
                                       "Confirmar envío", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                buttonSendCoso.Enabled = false;
                var success = await _transferManager.SendCoso(coso, textBoxIP.Text);
                
                if (success)
                {
                    MessageBox.Show("Coso enviado exitosamente!", "Éxito", 
                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
                buttonSendCoso.Enabled = true;
            }
        };

        buttonStartListening.Click += async (s, e) =>
        {
            buttonStartListening.Enabled = false;
            buttonStopListening.Enabled = true;
            
            _ = Task.Run(() => _transferManager.StartListening());
        };

        buttonStopListening.Click += (s, e) =>
        {
            _transferManager.StopListening();
            buttonStartListening.Enabled = true;
            buttonStopListening.Enabled = false;
        };

        this.FormClosing += (s, e) =>
        {
            _transferManager.StopListening();
        };
    }
}
