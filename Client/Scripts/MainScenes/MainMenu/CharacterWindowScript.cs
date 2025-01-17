using Core.Database.Constants;
using Game.Scripts.BaseControls;
using Godot;

namespace Game.Scripts.MainScenes.MainMenu;

// Logic Management
public partial class CharacterWindowScript : WindowBase
{
    private ItemList? _playersList;
    private Button? _enterGameButton;
    
    public override void _Ready()
    {
        _playersList = GetNode<ItemList>("VBoxContainer/PlayersItemList");
        _enterGameButton = GetNode<Button>("VBoxContainer/EnterGameButton");
        
        PopulatePlayersList();
    }

    private void PopulatePlayersList()
    {
        _playersList?.Clear();
        
        const int maxPlayersSlots = DatabaseLimiter.MaxPlayersPerAccount;

        for (var i = 0; i < maxPlayersSlots; i++)
        {
            _playersList?.AddItem("Empty");
            _playersList?.SetItemDisabled(i, true);
        }
    }

    public void AddPlayerToList(int index, Texture2D icon, string name, int level)
    {
        if (index is >= DatabaseLimiter.MaxPlayersPerAccount or < 0)
        {
            GD.PrintErr("Index out of range");
            return;
        }
        
        _playersList?.SetItemText(index, name + " : " + level);
        _playersList?.SetItemIcon(index, icon);
    }

    
}