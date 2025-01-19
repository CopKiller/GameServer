using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Network.SerializationObjects.Enum;
using Game.Scripts.GameState;
using Game.Scripts.GameState.Interface;
using Game.Scripts.MainScenes.MainMenu;
using Godot;

namespace Game.Scripts.Singletons;

public partial class GameStateManager : Node, IDisposable
{
    private IGameStateBase? _currentState;
    private readonly SemaphoreSlim _stateLock = new(1, 1);

    public override void _Ready()
    {
        GD.Print("GameStateManager ready!");
    }

    public async Task ChangeState<T>(IGameState<T> newState) where T : CanvasItem
    {
        await _stateLock.WaitAsync();
        try
        {
            // Saída do estado atual, se houver
            if (_currentState != null)
                await _currentState.ExitStateAsync();

            // Mudança para o novo estado
            _currentState = newState;
            await _currentState.EnterStateAsync();

            Log($"State changed to: {typeof(T).Name}");
        }
        catch (Exception e)
        {
            LogError($"Failed to change game state: {e.Message}");
        }
        finally
        {
            _stateLock.Release();
        }
    }

    public async void ReceiveChangeState(ClientState newState)
    {
        try
        {
            switch (newState)
            {
                case ClientState.MainMenu:
                case ClientState.CharacterSelection:
                    await HandleMainMenuState(newState);
                    break;

                case ClientState.InGame:
                    //await HandleInGameState();
                    break;

                case ClientState.Loading:
                    // Implementação do estado de carregamento
                    Log("Loading state is not implemented yet.");
                    break;

                default:
                    LogError($"Unknown state: {newState}");
                    break;
            }
        }
        catch (Exception e)
        {
            LogError($"Error processing state change: {e.Message}");
        }
    }

    private async Task HandleMainMenuState(ClientState newState)
    {
        if (newState == ClientState.CharacterSelection && _currentState is MainMenuState menuState)
        {
            menuState.ChangeStateToCharacterSelection();
            return;
        }

        var mainMenuState = ServiceManager.GetRequiredService<IGameState<MainMenuScript>>();
        await ChangeState(mainMenuState);

        if (newState == ClientState.CharacterSelection && _currentState is MainMenuState mainMenu)
        {
            mainMenu.ChangeStateToCharacterSelection();
        }
    }

    //private async Task HandleInGameState()
    //{
        // TODO: Implementar o estado de jogo
        //var inGameState = ServiceManager.GetRequiredService<IGameState<CanvasItem>>(); // Substitua CanvasItem pelo tipo correto
        //await ChangeState(inGameState);
    //}

    public IGameStateBase? GetCurrentState() => _currentState;

    public void ExitGame() => GetTree().Quit();

    public override void _ExitTree()
    {
        _currentState?.ExitStateAsync();
        _currentState = null;
    }

    private void Log(string message) => GD.Print($"[GameStateManager] {message}");

    private void LogError(string message) => GD.PrintErr($"[GameStateManager] {message}");

    public void Dispose() => _stateLock.Dispose();
}
