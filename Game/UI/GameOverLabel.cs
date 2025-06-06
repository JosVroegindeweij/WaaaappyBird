using Godot;

public partial class GameOverLabel : RichTextLabel
{
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
