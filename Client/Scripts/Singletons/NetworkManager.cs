using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Client.Network;
using Core.Client.Network.Interface;
using Core.Network.Interface;
using Core.Service.Interfaces.Types;
using Game.Scripts.Extensions;
using Game.Scripts.GameState.Interface;
using Game.Scripts.MainScenes.MainMenu;
using Game.Scripts.Transitions;
using Godot;

namespace Game.Scripts.Singletons;

public partial class NetworkManager : Node
{
    private ISingleService? _clientNetworkManager;
    private IClientConnectionManager? _clientConnectionManager;
    private IClientPacketProcessor? _clientPacketProcessor;
    private INetworkEventsListener? _networkEventsListener;

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

    public override void _EnterTree()
    {
        GD.Print("NetworkManager entered tree!");
    }

    public override void _ExitTree()
    {
        _clientNetworkManager = null;
        _clientConnectionManager = null;
        _clientPacketProcessor = null;
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
        _clientPacketProcessor = ServiceManager.GetRequiredService<IClientPacketProcessor>();
        _clientConnectionManager = ServiceManager.GetRequiredService<IClientConnectionManager>();
        _networkEventsListener = ServiceManager.GetRequiredService<INetworkEventsListener>();

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

    private void UpdateNetworkLatency(ICustomNetPeer peer, int latency)
    {
        CallDeferred(GodotObject.MethodName.EmitSignal, SignalName.NetworkLatencyUpdated, latency);
    }

    private void PeerDisconnectedEvent(ICustomNetPeer peer, ICustomDisconnectInfo disconnectInfo)
    {
        if (_isConnected)
        {
            _isConnected = false;
            CallDeferred(nameof(Reconnect), disconnectInfo.Reason.ToString());
        }
    }
}