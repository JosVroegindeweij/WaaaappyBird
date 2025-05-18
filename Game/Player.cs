using Godot;

public partial class Player : CharacterBody2D
{
	private const float GRAVITY = 9.8f;
	private const double JUMP_VERTICAL_ACCELERATION = -2.5;

	private bool isInGracePeriod = false;
	private double _verticalAcceleration = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (isInGracePeriod)
		{
			// Do nothing
			return;
		}

		if (Input.IsActionJustPressed("jump"))
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

	public void OnUnpaused()
	{
		isInGracePeriod = true;
		var timer = GetNode<Timer>("GracePeriodTimer");
		timer.Start();

		EmitSignal(SignalName.GracePeriodStarted);
	}

	public void OnGracePeriodEnded()
	{
		isInGracePeriod = false;

		EmitSignal(SignalName.GracePeriodEnded);
	}

	[Signal]
	public delegate void GracePeriodStartedEventHandler();

	[Signal]
	public delegate void GracePeriodEndedEventHandler();
}
