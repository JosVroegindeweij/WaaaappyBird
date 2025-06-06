using System;
using Godot;

public partial class Player : CharacterBody2D
{
	private Vector2 initialPosition;
	private double initialVerticalAcceleration = 0;
	private const float GRAVITY = 9.8f;
	private const double JUMP_VERTICAL_ACCELERATION = -6;

	private bool isInGracePeriod = false;
	private bool isHandlingInput = true;
	private double verticalAcceleration;

	public override void _Ready()
	{
		initialPosition = new Vector2(Position.X, Position.Y);
		verticalAcceleration = initialVerticalAcceleration;

		var gameManager = GetNode<GameManager>("/root/Main/GameManager");
		gameManager.GameOver += OnGameOver;
		gameManager.Restarted += OnRestarted;
		gameManager.GracePeriodStarted += OnGracePeriodStarted;
		gameManager.GracePeriodEnded += OnGracePeriodEnded;

		var playerCollisionArea = GetNode<Area2D>("Hitbox");
		playerCollisionArea.BodyEntered += OnArea2DBodyEntered;
		playerCollisionArea.AreaShapeEntered += OnArea2DAreaShapeEntered;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (isInGracePeriod)
		{
			// Do nothing
			return;
		}

		if (isHandlingInput && Input.IsActionJustPressed("jump"))
		{
			Jump();
		}

		Fall(delta);
	}

	private void Jump()
	{
		verticalAcceleration = JUMP_VERTICAL_ACCELERATION;
	}

	private void Fall(double delta)
	{
		var fallingAcceleration = delta * GRAVITY;
		verticalAcceleration += fallingAcceleration;

		Position += new Vector2(0, (float)verticalAcceleration);
	}

	private void OnGameOver()
	{
		isHandlingInput = false;
	}

	private void OnRestarted()
	{
		isHandlingInput = true;
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

	private void OnArea2DAreaShapeEntered(Rid areaRid, Area2D area, long areaShapeIndex, long localShapeIndex)
	{
		if (area.IsInGroup("Obstacles"))
		{
			EmitSignal(SignalName.CollidedWithObstacle);
		}
	}

	[Signal]
	public delegate void CollidedWithScreenEdgeEventHandler();

	[Signal]
	public delegate void CollidedWithObstacleEventHandler();
}
