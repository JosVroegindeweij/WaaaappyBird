using System;
using System.Collections.Generic;
using Godot;

public partial class GameManager : Node
{
	private bool isPaused;
	private bool isGameOver;
	private bool isInGracePeriod = false;

	private Timer gracePeriodTimer;

	private LinkedList<Node2D> obstacles = new();
	private PackedScene obstacleScene;
	private Node2D obstacleContainer;

	public override void _Ready()
	{
		isPaused = true;
		GetTree().Paused = true;

		var inputHandler = GetNode<InputHandler>("/root/Main/InputHandler");
		inputHandler.PausePressed += OnPausePressed;
		inputHandler.UnpausePressed += OnUnpausePressed;

		var player = GetNode<Player>("/root/Main/Game/Player");
		player.CollidedWithScreenEdge += OnCollidedWithScreenEdge;
		player.CollidedWithObstacle += OnCollidedWithObstacle;

		gracePeriodTimer = GetNode<Timer>("GracePeriodTimer");
		gracePeriodTimer.Timeout += OnGracePeriodTimerTimeout;

		obstacleScene = GD.Load<PackedScene>("res://Game/Elements/Obstacle/obstacle.tscn");

		obstacleContainer = GetNode<Node2D>("/root/Main/Game/Obstacles");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 screenSize = GetViewport().GetVisibleRect().Size;

		if (!isInGracePeriod)
		{
			if (obstacles.Count == 0 || (screenSize.X - obstacles.Last.Value.Position.X) > screenSize.X / 2)
			{
				// Create a new obstacle if there are none or if the last one is far enough
				// from the right edge of the screen.
				CreateObstacle();
			}

			GetTree().CallGroup("ObstacleContainers", "MoveLeft", (float)delta);
		}
	}

	private void CreateObstacle()
	{
		Vector2 screenSize = GetViewport().GetVisibleRect().Size;
		var random = new Random();

		var obstacle = obstacleScene.Instantiate<ObstacleContainer>();

		var minimalObstacleSize = 40; // Minimum size of the obstacle (in pixels). This is because the visuals of the obstacle are not scaled down to fit smaller sizes.

		var bottomLimit = (int)Math.Round(0.1 * screenSize.Y); // Minimum gap offset from the bottom of the screen
		var topLimit = (int)Math.Round(0.9f * screenSize.Y); // Maximum gap offset from the bottom of the screen

		var width = 0.2f * screenSize.X; // Width of the obstacle container
		var height = screenSize.Y; // Height of the obstacle container

		var rightOfScreen = screenSize.X; // Right edge of the screen
		var topOfScreen = 0; // Top edge of the screen

		var gapSize = (int)Math.Round(0.3f * screenSize.Y); // Size of the gap between obstacles
		var halfGapSize = (int)(0.5 * gapSize); // Half of the gap size

		var gapMiddle = random.NextInt64(bottomLimit + minimalObstacleSize + halfGapSize, topLimit - minimalObstacleSize - halfGapSize);
		var gapOffset = gapMiddle - halfGapSize; // Offset of the gap from the top of the screen (also size of top obstacle)

		obstacle.Setup(
			position: new Vector2(rightOfScreen, topOfScreen),
			size: new Vector2(width, height),
			gapSize: gapSize,
			gapOffset: gapOffset
		);
		obstacles.AddLast(obstacle);
		obstacleContainer.AddChild(obstacle);
		obstacle.ScreenExited += OnObstacleScreenExited;
	}

	private void ResetObstacles()
	{
		foreach (var obstacle in obstacles)
		{
			obstacle.QueueFree();
		}
		obstacles.Clear();
	}

	private void OnObstacleScreenExited()
	{
		obstacles.RemoveFirst();
	}

	private void OnPausePressed()
	{
		if (!isPaused && !isGameOver)
		{
			if (isInGracePeriod)
			{
				gracePeriodTimer.Stop();
				EndGracePeriod();
			}
			isPaused = true;
			GetTree().SetDeferred("paused", true);
			EmitSignal(SignalName.Paused);
		}
	}

	private void OnUnpausePressed()
	{
		if (isGameOver)
		{
			isGameOver = false;
			GetTree().SetDeferred("paused", false);
			EmitSignal(SignalName.Restarted);
			StartGracePeriod();
		}
		else if (isPaused)
		{
			isPaused = false;
			GetTree().SetDeferred("paused", false);
			EmitSignal(SignalName.Unpaused);
			StartGracePeriod();
		}
	}

	private void OnGracePeriodTimerTimeout()
	{
		EndGracePeriod();
	}

	private void EndGracePeriod()
	{
		isInGracePeriod = false;
		EmitSignal(SignalName.GracePeriodEnded);
	}

	private void StartGracePeriod()
	{
		isInGracePeriod = true;
		gracePeriodTimer.Start();
		EmitSignal(SignalName.GracePeriodStarted);
	}

	private void OnCollidedWithScreenEdge()
	{
		Die();
	}

	private void OnCollidedWithObstacle()
	{
		Die();
	}

	private void Die()
	{
		if (isGameOver || isPaused) return;
		isGameOver = true;
		GetTree().SetDeferred("paused", true);
		EmitSignal(SignalName.GameOver);

		// ResetObstacles();
	}

	[Signal]
	public delegate void PausedEventHandler();

	[Signal]
	public delegate void UnpausedEventHandler();

	[Signal]
	public delegate void RestartedEventHandler();

	[Signal]
	public delegate void GameOverEventHandler();

	[Signal]
	public delegate void GracePeriodStartedEventHandler();

	[Signal]
	public delegate void GracePeriodEndedEventHandler();
}
