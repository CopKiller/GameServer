using System;
using System.Threading.Tasks;
using Core.Client.Network.Interface;
using Core.Network.Interface;
using Core.Network.Interface.Event;
using Core.Service.Interfaces.Types;
using Game.Scripts.GameState.Interface;
using Game.Scripts.MainScenes.MainMenu;
using Game.Scripts.Network;
using Godot;

namespace Game.Scripts.Singletons;

public partial class NetworkManager : Node
{
    private ISingleService? _clientNetworkManager;
    private IClientConnectionManager? _clientConnectionManager;
    private INetworkEventsListener? _networkEventsListener;
    
    private ulong _networkUpdateTimer;

    [Signal]
    public delegate void NetworkLatencyUpdatedEventHandler(int latency);

    [Signal]
    public delegate void NetworkDisconnectedEventHandler();
    
    private bool _isConnected;

    public override void _Ready()
    {
        GD.Print("NetworkManager ready!");

        var loadingManager = ServiceManager.GetRequiredService<LoadingManager>();

        loadingManager.AddTask(ConfigureNetwork, "Configurando rede...");
        loadingManager.AddTask(StartNetwork, "Iniciando rede...");
    }
    
    public override void _Process(double delta)
    {
         if (_isConnected)
         {
             var time = Time.GetTicksMsec();
             if (_networkUpdateTimer <= time)
             {
                 //GD.Print($"Update network {time - _networkUpdateTimer}");;
                 _clientNetworkManager?.Update((long)_networkUpdateTimer);
                 _networkUpdateTimer = time + 15;
             }
         }
    }

    public override void _EnterTree()
    {
        GD.Print("NetworkManager entered tree!");
    }

    public override void _ExitTree()
    {
        _clientNetworkManager = null;
        _clientConnectionManager = null;
    }

    public void Reconnect(string reason)
    {
        var loadingManager = ServiceManager.GetRequiredService<LoadingManager>();

        loadingManager.AddTask(StartNetwork, "Desconectado, motivo: " + reason + " [Tentando reconectar...]");
        
        var mainMenuState = ServiceManager.GetRequiredService<IGameState<MainMenuScript>>();

        var gameStateManager = ServiceManager.GetRequiredService<GameStateManager>();
        
        gameStateManager.ChangeState(mainMenuState);
    }

    private Task ConfigureNetwork()
    {
        _clientNetworkManager = ServiceManager.GetRequiredService<ISingleService>();
        _clientConnectionManager = ServiceManager.GetRequiredService<IClientConnectionManager>();
        _networkEventsListener = ServiceManager.GetRequiredService<INetworkEventsListener>();
        
        var clientRegisterHandler = ServiceManager.GetRequiredService<ClientRegisterHandler>();
        
        clientRegisterHandler.RegisterHandlers();

        return Task.CompletedTask;
    }

    private async Task StartNetwork()
    {
        _clientNetworkManager?.Stop();
        _clientNetworkManager?.Start();
        
        await TryConnect();

        GD.Print("Conectado ao servidor!");

        ConnectNetworkEvents();
    }
    
    private async Task TryConnect()
    {
        var connectionManager = ServiceManager.GetRequiredService<IClientConnectionManager>();

        while (!connectionManager.IsConnected)
        {
            try
            {
                _clientConnectionManager?.ConnectToServer();
            }
            catch (Exception ex)
            {
                // Log a exceção e continue tentando
                Console.WriteLine($"Erro ao tentar conectar: {ex.Message}");
            }

            await Task.Delay(2000);
        }
        
        _isConnected = true;
    }
    
    private void ConnectNetworkEvents()
    {
        if (_networkEventsListener == null) return;

        _networkEventsListener.OnNetworkLatencyUpdate += UpdateNetworkLatency;
        _networkEventsListener.OnPeerDisconnected += PeerDisconnectedEvent;
    }

    private void ClearNetworkEvents()
    {
        if (_networkEventsListener == null) return;

        _networkEventsListener.OnNetworkLatencyUpdate -= UpdateNetworkLatency;
        _networkEventsListener.OnPeerDisconnected -= PeerDisconnectedEvent;
    }

    private void UpdateNetworkLatency(IAdapterNetPeer peer, int latency)
    {
        EmitSignal(SignalName.NetworkLatencyUpdated, latency);
    }

    private void PeerDisconnectedEvent(IAdapterNetPeer peer, IAdapterDisconnectInfo disconnectInfo)
    {
        if (_isConnected)
        {
            _isConnected = false;
            Reconnect(disconnectInfo.Reason.ToString());
        }
    }
}