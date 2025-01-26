using System.Collections.Generic;
using Core.Database.Constants;
using Core.Network.SerializationObjects;
using Game.Scripts.BaseControls;
using Godot;

namespace Game.Scripts.MainScenes.MainMenu;

// Logic Management
public partial class CharacterWindowScript : WindowBase
{
    private ItemList? _playersList;
    private Button? _enterGameButton;

    private List<PlayerDto> _players = [];

    public List<PlayerDto> Players
    {
        get => _players;

        set
        {
            _players.Clear();
            _players.AddRange(value);

            CallDeferred(MethodName.PopulatePlayersList);
        }
    }

    public override void _Ready()
    {
        _playersList = GetNode<ItemList>("VBoxContainer/PlayersItemList");
        _enterGameButton = GetNode<Button>("VBoxContainer/EnterGameButton");
        
        PopulatePlayersList();
    }

    public void PopulatePlayersList()
    {
        _playersList?.Clear();

        const int maxPlayersSlots = DatabaseLimiter.MaxPlayersPerAccount;

        if (_players.Count > 0)
            foreach (var player in _players)
            {
                if (player.SlotNumber is >= maxPlayersSlots or < 0)
                {
                    GD.PrintErr("Slot number out of range");
                    return;
                }

                AddPlayerToList(player.SlotNumber, player.Name, player.Level);
            }
        else
            for (var i = 0; i < maxPlayersSlots; i++)
                _playersList?.AddItem("Empty Slot");
    }

    public void AddPlayerToList(int index, string name, int level)
    {
        if (index is >= DatabaseLimiter.MaxPlayersPerAccount or < 0)
        {
            GD.PrintErr("Index out of range");
            return;
        }

        _playersList?.SetItemText(index, name + " : " + level);
    }

    # region Signals

    private void SendLogoutSignal()
    {
        SendLogout();
    }

    # endregion
}