using Godot;

public partial class Player : CharacterBody2D
{
	private Vector2 initialPosition;
	private double initialVerticalAcceleration = 0;
	private const float GRAVITY = 9.8f;
	private const double JUMP_VERTICAL_ACCELERATION = -2.5;

	private bool isInGracePeriod = false;
	private double verticalAcceleration;

	public override void _Ready()
	{
		initialPosition = new Vector2(Position.X, Position.Y);
		verticalAcceleration = initialVerticalAcceleration;

		var gameManager = GetNode<GameManager>("/root/Main/GameManager");
		gameManager.Restarted += OnRestarted;
		gameManager.GracePeriodStarted += OnGracePeriodStarted;
		gameManager.GracePeriodEnded += OnGracePeriodEnded;

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
		verticalAcceleration = JUMP_VERTICAL_ACCELERATION;
	}

	private void Fall()
	{
		var fallingAcceleration = 0.003f * GRAVITY;
		verticalAcceleration += fallingAcceleration;

		Position += new Vector2(0, (float)verticalAcceleration);
	}

	private void OnRestarted()
	{
		Position = new Vector2(initialPosition.X, initialPosition.Y);
		verticalAcceleration = initialVerticalAcceleration;
	}

	private void OnGracePeriodStarted()
	{
		isInGracePeriod = true;
	}

	private void OnGracePeriodEnded()
	{
		isInGracePeriod = false;
	}

	private void OnArea2DBodyEntered(Node2D body)
	{
		if (body.IsInGroup("ScreenEdges"))
		{
			EmitSignal(SignalName.CollidedWithScreenEdge);
		}
	}

	[Signal]
	public delegate void CollidedWithScreenEdgeEventHandler();
}
