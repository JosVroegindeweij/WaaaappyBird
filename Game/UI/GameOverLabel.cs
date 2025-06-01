using Godot;

public partial class GameOverLabel : RichTextLabel
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var gameManager = GetNode<GameManager>("/root/Main/GameManager");
		gameManager.GameOver += OnGameOver;
		gameManager.Restarted += OnRestarted;

		Hide();
	}

	public void OnGameOver()
	{
		Show();
	}

	public void OnRestarted()
	{
		Hide();
	}
}
