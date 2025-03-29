using Godot;
using System;

public partial class PauseScreen : RichTextLabel
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var keyPressed = Input.IsAnythingPressed();
		if (keyPressed)
		{
			// Unpause the game
			GetTree().Paused = false;
		}
	}
}
