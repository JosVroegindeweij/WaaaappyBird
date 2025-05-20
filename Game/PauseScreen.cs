using Godot;

public partial class PauseScreen : RichTextLabel
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var gameManager = GetNode<GameManager>("/root/Main/GameManager");
		gameManager.Paused += OnPause;
		gameManager.Unpaused += OnUnpause;

		Show();
	}

	public void OnPause()
	{
		Show();
	}

	public void OnUnpause()
	{
		Hide();
	}
}
