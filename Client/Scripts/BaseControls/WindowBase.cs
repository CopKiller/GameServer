using Godot;

namespace Game.Scripts.BaseControls;

public partial class WindowBase : Window
{
	[Export] public bool CanClose { get; set; } = true;
	
	[Export] public Window? OpenWindowWhenClosed { get; set; }
	
	private void OnCloseRequest()
	{
		if (CanClose)
			this.Hide();
		
		if (OpenWindowWhenClosed != null)
			OpenWindowWhenClosed.Show();
	}
}