using Game.Scripts.BaseControls;
using Game.Scripts.Cache;
using Game.Scripts.Components;
using Game.Scripts.Singletons;
using Godot;

namespace Game.Scripts.Local;

public partial class MyPlayer : Node
{
    private InputHandler? _inputHandler;
    
    private PackedScene? _bodyEntityScene;
    
    private BodyEntity? _bodyEntity;
    
    public override void _Ready()   
    {
        var scenePathCache = ServiceManager.GetRequiredService<ScenePathCache>();
        
        _bodyEntityScene = GD.Load<PackedScene>(scenePathCache.GetScenePath<BodyEntity>());
        
        _bodyEntity = _bodyEntityScene?.Instantiate<BodyEntity>();
        
        if (_bodyEntity != null)
        {
            AddChild(_bodyEntity);
        }
        
        _inputHandler = new InputHandler(_bodyEntity!.GetMovementController(), _bodyEntity.GetAttackController());
        
        InitializePlayerData();
    }

    private void InitializePlayerData()
    {
        var movementController = _bodyEntity?.GetMovementController();
        
        movementController?.MoveToPosition(new Vector2(8, 15));
    }
    
    public override void _Input(InputEvent @event)
    {
        _inputHandler?.ProcessInput();
    }
}