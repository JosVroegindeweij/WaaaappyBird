using Godot;
using System;

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
		GD.Print("Pause pressed: " + Input.IsActionPressed("pause"));
		if (Input.IsActionPressed("pause"))
		{
			// Pause the game
			GD.Print("Pausing the game");
			GetTree().SetDeferred("paused", true);
			Show();
		}

		if (GetTree().Paused && Input.IsActionPressed("unpause"))
		{
			// Unpause the game
			GD.Print("Unpausing the game");
			GetTree().SetDeferred("paused", false);
			Hide();
		}
	}
}
