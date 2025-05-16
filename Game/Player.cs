using Godot;
using System;

public partial class Player : CharacterBody2D
{
	private const double TIME_BEFORE_START_FALLING = 1.5;
	private const float GRAVITY = 9.8f;
	private const double JUMP_VERTICAL_ACCELERATION = -2.5;

	private double _timeSinceStart = 0;

	private double _verticalAcceleration = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Player is ready");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_timeSinceStart += delta;

		if (_timeSinceStart <= TIME_BEFORE_START_FALLING)
		{
			// Do nothing
			return;
		}

		if (Input.IsActionPressed("jump"))
		{
			Jump();
		}

		Fall();
	}

	private void Jump()
	{
		_verticalAcceleration = JUMP_VERTICAL_ACCELERATION;
	}

	private void Fall()
	{
		var fallingAcceleration = 0.003f * GRAVITY;
		_verticalAcceleration += fallingAcceleration;

		Position += new Vector2(0, (float)_verticalAcceleration);
	}
}
