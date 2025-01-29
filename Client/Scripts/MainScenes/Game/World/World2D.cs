using Game.Scripts.Local;
using Godot;

namespace Game.Scripts.MainScenes.Game.World;

public partial class World2D : Node2D
{   
    public override void _Ready()
    {
        // Inicializa o jogador
        var player = new MyPlayer();
        
        AddChild(player);
    }
}