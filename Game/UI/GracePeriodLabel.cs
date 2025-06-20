using Godot;

public partial class GracePeriodLabel : RichTextLabel
{
	public override void _Ready()
	{
		var gameManager = GetNode<GameManager>("/root/Main/GameManager");

		gameManager.GracePeriodStarted += OnGracePeriodStarted;
		gameManager.GracePeriodEnded += OnGracePeriodEnded;

		Hide();
	}

	public void OnGracePeriodStarted()
	{
		Show();
	}

	public void OnGracePeriodEnded()
	{
		Hide();
	}
}
