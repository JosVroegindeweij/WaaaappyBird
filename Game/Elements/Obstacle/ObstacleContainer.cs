using System;
using Godot;

public partial class ObstacleContainer : Node2D
{
	public override void _Ready()
	{
		var onScreenNotifier = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
		onScreenNotifier.ScreenExited += OnScreenExited;
	}

	public void Setup(Vector2 position, Vector2 size, float gapSize, float gapOffset)
	{
		// Setup actual obstacles
		SetupObstacles(position, size, gapSize, gapOffset);

		// Setup notifier to detect when the obstacles are off-screen
		var onScreenNotifier = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
		onScreenNotifier.Position = new Vector2(size.X, 0);

		// Setup sprites of obstacles

	}

	private void SetupObstacles(Vector2 position, Vector2 size, float gapSize, float gapOffset)
	{
		Position = position;
		// Position of RectangleShape2D is the middle of the rectangle
		var middleWidth = size.X / 2f;
		var topMiddleHeight = gapOffset / 2f;
		// The middle of the bottom gap is the topleft of the bottom rectangle + half the remaining height for that rectangle
		var bottomMiddleHeight = gapOffset + gapSize + (size.Y - gapOffset - gapSize) / 2f;

		var topObstacle = GetNode<Area2D>("TopObstacle");
		var bottomObstacle = GetNode<Area2D>("BottomObstacle");
		if (bottomObstacle.GetNode<CollisionShape2D>("CollisionShape2D").Shape is RectangleShape2D bottomShape)
		{
			bottomObstacle.Position = new Vector2(middleWidth, bottomMiddleHeight);
			bottomShape.Size = new Vector2(size.X, size.Y - gapOffset - gapSize);
		}
		if (topObstacle.GetNode<CollisionShape2D>("CollisionShape2D").Shape is RectangleShape2D topShape)
		{
			topObstacle.Position = new Vector2(middleWidth, topMiddleHeight);
			topShape.Size = new Vector2(size.X, gapOffset);
		}
	}

	public void MoveLeft(float delta)
	{
		Position -= new Vector2(100 * delta, 0);
	}

	private void OnScreenExited()
	{
		EmitSignal(SignalName.ScreenExited);
		QueueFree();
	}

	[Signal]
	public delegate void ScreenExitedEventHandler();
}
