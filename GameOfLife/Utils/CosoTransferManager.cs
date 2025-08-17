using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using GameOfLife.Models;

namespace GameOfLife.Utils;

// 1. CLASE PARA MANEJAR LA TRANSFERENCIA
public class CosoTransferManager
{
    private TcpListener? _listener;
    private bool _isListening = false;
    private const int Port = 8080;

    public event Action<string>? OnMessageReceived;
    public event Action<Coso>? OnCosoReceived;

    // Obtener IP local
    public string GetLocalIP()
    {
        try
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip.ToString();
            }
        }
        catch { }
        return "No encontrada";
    }

    // Iniciar escucha para recibir cosos
    public async Task StartListening()
    {
        try
        {
            _listener = new TcpListener(IPAddress.Any, Port);
            _listener.Start();
            _isListening = true;
            
            OnMessageReceived?.Invoke($"Escuchando en puerto {Port}...");
            
            while (_isListening)
            {
                var client = await _listener.AcceptTcpClientAsync();
                _ = Task.Run(() => HandleClient(client));
            }
        }
        catch (ObjectDisposedException)
        {
            // Normal cuando se detiene el listener
        }
        catch (Exception ex)
        {
            OnMessageReceived?.Invoke($"Error en listener: {ex.Message}");
        }
    }

    private async Task HandleClient(TcpClient client)
    {
        try
        {
            using (client)
            using (var stream = client.GetStream())
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                var jsonData = await reader.ReadToEndAsync();
                
                if (!string.IsNullOrEmpty(jsonData))
                {
                    var coso = JsonSerializer.Deserialize<Coso>(jsonData);
                    if (coso != null)
                    {
                        OnMessageReceived?.Invoke($"Coso recibido: {coso.NombreCompleto}");
                        OnCosoReceived?.Invoke(coso);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            OnMessageReceived?.Invoke($"Error al recibir: {ex.Message}");
        }
    }

    // Enviar coso a otra IP
    public async Task<bool> SendCoso(Coso coso, string targetIP)
    {
        try
        {
            using var client = new TcpClient();
            await client.ConnectAsync(targetIP, Port);
            
            using var stream = client.GetStream();
            using var writer = new StreamWriter(stream, Encoding.UTF8);
            
            var json = JsonSerializer.Serialize(coso, new JsonSerializerOptions { WriteIndented = false });
            await writer.WriteAsync(json);
            await writer.FlushAsync();
            
            OnMessageReceived?.Invoke($"Coso enviado a {targetIP}");
            return true;
        }
        catch (Exception ex)
        {
            OnMessageReceived?.Invoke($"Error al enviar: {ex.Message}");
            return false;
        }
    }

    public void StopListening()
    {
        _isListening = false;
        _listener?.Stop();
        OnMessageReceived?.Invoke("Dejando de escuchar...");
    }

    // Escanear red local para encontrar otros jugadores
    public async Task<List<string>> ScanLocalNetwork()
    {
        var availableIPs = new List<string>();
        var localIP = GetLocalIP();
        
        if (localIP == "No encontrada") return availableIPs;
        
        var networkBase = localIP.Substring(0, localIP.LastIndexOf('.') + 1);
        var tasks = new List<Task>();
        
        OnMessageReceived?.Invoke("Escaneando red local...");
        
        for (int i = 1; i <= 254; i++)
        {
            var ip = networkBase + i;
            if (ip == localIP) continue;
            
            tasks.Add(Task.Run(async () =>
            {
                try
                {
                    using var client = new TcpClient();
                    var connectTask = client.ConnectAsync(ip, Port);
                    if (await Task.WhenAny(connectTask, Task.Delay(1000)) == connectTask)
                    {
                        if (client.Connected)
                        {
                            lock (availableIPs)
                            {
                                availableIPs.Add(ip);
                            }
                        }
                    }
                }
                catch { }
            }));
        }
        
        await Task.WhenAll(tasks);
        OnMessageReceived?.Invoke($"Escaneo completado. Encontrados: {availableIPs.Count}");
        return availableIPs;
    }
}
