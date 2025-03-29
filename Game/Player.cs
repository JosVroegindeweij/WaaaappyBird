using Godot;
using System;

public partial class Player : CharacterBody2D
{
	private const double TIME_BEFORE_START_FALLING = 3;
	private const float GRAVITY = 9.8f;
	private const double JUMP_VERTICAL_ACCELERATION = -2.5;

	private double _timeSinceStart = 0;

	private double _verticalAcceleration = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var escPressed = Input.IsActionPressed("ui_cancel");
		if (escPressed)
		{
			// Pause the game
			GetTree().Paused = true;
			return;
		}
		_timeSinceStart += delta;

		if (_timeSinceStart <= TIME_BEFORE_START_FALLING)
		{
			// Do nothing
			return;
		}

		var acceptPressed = Input.IsActionPressed("ui_accept");
		var upPressed = Input.IsActionPressed("ui_up");
		var downPressed = Input.IsActionPressed("ui_down");
		var leftPressed = Input.IsActionPressed("ui_left");
		var rightPressed = Input.IsActionPressed("ui_right");

		var jumped = acceptPressed || upPressed || downPressed || leftPressed || rightPressed;

		if (jumped)
		{
			Jump(delta);
		}

		Fall(delta);
	}

	private void Jump(double delta)
	{
		_verticalAcceleration = JUMP_VERTICAL_ACCELERATION;
	}

	private void Fall(double delta)
	{
		var fallingAcceleration = 0.003f * GRAVITY;
		_verticalAcceleration += fallingAcceleration;

		Position += new Vector2(0, (float)_verticalAcceleration);
	}
}
