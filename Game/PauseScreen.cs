using Godot;

public partial class PauseScreen : RichTextLabel
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("PauseScreen is ready");
		GetTree().Paused = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("pause"))
		{
			// Pause the game
			GD.Print("Pausing the game");
			GetTree().SetDeferred("paused", true);
			Show();
		}

		if (GetTree().Paused && Input.IsActionJustPressed("unpause"))
		{
			// Unpause the game
			GD.Print("Unpausing the game");
			GetTree().SetDeferred("paused", false);
			EmitSignal(SignalName.Unpaused);
			Hide();
		}
	}

	[Signal]
	public delegate void UnpausedEventHandler();
}
