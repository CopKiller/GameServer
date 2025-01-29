
using Core.Network.SerializationObjects.Enum;
using Game.Scripts.Components;
using Godot;

namespace Game.Scripts.BaseControls;

[Tool]
public partial class BodyEntity : CharacterBody2D
{
    [ExportCategory("Configuration")]
    
    [Export] private float _gridSnapSize = 32f;
    [Export] private AnimatedSprite2D? _animatedSprite;
    
    
    [ExportCategory("Tests")]
    
    [Export]
    private Vector2 _targetPosition = Vector2.Zero;
    
    [ExportToolButton("Move To Target")]
    private Callable MoveToTarget => Callable.From(() => _movementController?.MoveToPosition(_targetPosition));
    
    [ExportToolButton("Move Up")]
    private Callable MoveUp => Callable.From(() => _movementController?.MoveToDirection(Direction.Up));
    
    [ExportToolButton("Move Down")]
    private Callable MoveDown => Callable.From(() => _movementController?.MoveToDirection(Direction.Down));
    
    [ExportToolButton("Move Left")]
    private Callable MoveLeft => Callable.From(() => _movementController?.MoveToDirection(Direction.Left));
    
    [ExportToolButton("Move Right")]
    private Callable MoveRight => Callable.From(() => _movementController?.MoveToDirection(Direction.Right));
    
    [ExportToolButton("Move Up Right")]
    private Callable MoveUpRight => Callable.From(() => _movementController?.MoveToDirection(Direction.UpRight));
    
    [ExportToolButton("Move Up Left")]
    private Callable MoveUpLeft => Callable.From(() => _movementController?.MoveToDirection(Direction.UpLeft));
    
    [ExportToolButton("Move Down Right")]
    private Callable MoveDownRight => Callable.From(() => _movementController?.MoveToDirection(Direction.DownRight));
    
    [ExportToolButton("Move Down Left")]
    private Callable MoveDownLeft => Callable.From(() => _movementController?.MoveToDirection(Direction.DownLeft));
    
    [ExportToolButton("Attack")]
    private Callable Attack => Callable.From(() => _attackController?.Attack());

    private MovementController? _movementController;
    private AttackController? _attackController;
    private AnimationController? _animationController;
    private InputHandler? _inputHandler;

    public override void _Ready()
    {
        _animatedSprite ??= GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        
        _movementController = new MovementController(this, _gridSnapSize);
        
        _attackController = new AttackController(this);
        
        _animationController = new AnimationController(_animatedSprite);
        
        _inputHandler = new InputHandler(_movementController, _attackController);
        
        RegisterEvents();

        GD.Print("Player Ready");
    }
    
    private void RegisterEvents()
    {
        if (!_ValidateComponents()) return;
        
        _movementController!.MovementStateChanged += isMoving =>
            _animationController?.SetStepTimer(isMoving);
        
        _attackController!.AttackStateChanged += isAttacking =>
            _animationController?.SetStepTimer(isAttacking);
    }
    
    public override void _PhysicsProcess(double delta)
    {
        if (!_ValidateComponents()) return;

        _animationController?.PlayAnimation(
            _movementController!.Direction, 
            _movementController!.IsMoving, 
            _attackController!.IsAttacking);
    }
    
    private bool _ValidateComponents()
    {
        if (_movementController != null &&
            _attackController != null &&
            _animationController != null &&
            _inputHandler != null) return true;
        
        GD.PrintErr("One or more components are not initialized");
        return false;

    }


    public override void _Input(InputEvent @event)
    {
        _inputHandler?.ProcessInput();
    }
}