using Godot;

public partial class GracePeriodLabel : RichTextLabel
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var player = GetNode<Player>("/root/Main/Game/Player");

		player.GracePeriodStarted += OnGracePeriodStarted;
		player.GracePeriodEnded += OnGracePeriodEnded;

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
