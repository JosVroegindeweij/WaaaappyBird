using Godot;

public partial class ScoreLabel : RichTextLabel
{
	public override void _Ready()
	{
		var gameManager = GetNode<GameManager>("/root/Main/GameManager");

		gameManager.SetScore += OnSetScore;
		gameManager.Restarted += OnRestarted;
		gameManager.GameOver += OnGameOver;
		gameManager.GracePeriodEnded += OnGracePeriodEnded;

		Hide();
	}

	private void OnSetScore(int score)
	{
		SetScore(score);
		if (!Visible && score > 0)
		{
			Show();
		}
	}

	private void OnRestarted()
	{
		SetScore(0);
		Hide();
	}

	private void SetScore(int score)
	{
		Text = $"[center]{score}[/center]";
	}

	private void OnGameOver(int score)
	{
		Text = $"[center][color=red]{score}[/color][/center]";
	}

	private void OnGracePeriodEnded()
	{
		Show();
	}
}
