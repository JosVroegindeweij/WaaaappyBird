using Godot;

public partial class Player : CharacterBody2D
{
	private const float GRAVITY = 9.8f;
	private const double JUMP_VERTICAL_ACCELERATION = -2.5;

	private bool isInGracePeriod = false;
	private double _verticalAcceleration = 0;

	private Timer gracePeriodTimer;

	public override void _Ready()
	{
		var gameManager = GetNode<GameManager>("/root/Main/GameManager");
		gameManager.Unpaused += OnUnpaused;
		gracePeriodTimer = GetNode<Timer>("GracePeriodTimer");
		gracePeriodTimer.Timeout += OnGracePeriodEnded;

		var playerCollisionArea = GetNode<Area2D>("Hitbox");
		playerCollisionArea.BodyEntered += OnArea2DBodyEntered;
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
		gracePeriodTimer.Start();

		EmitSignal(SignalName.GracePeriodStarted);
	}

	public void OnGracePeriodEnded()
	{
		isInGracePeriod = false;

		EmitSignal(SignalName.GracePeriodEnded);
	}

	private void OnArea2DBodyEntered(Node2D body)
	{
		if (body.IsInGroup("ScreenEdges"))
		{
			GD.Print("Died, unlucky");
		}
	}

	[Signal]
	public delegate void GracePeriodStartedEventHandler();

	[Signal]
	public delegate void GracePeriodEndedEventHandler();
}
