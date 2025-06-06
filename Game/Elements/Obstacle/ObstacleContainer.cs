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
	}

	private void SetupObstacles(Vector2 position, Vector2 size, float gapSize, float gapOffset)
	{
		Position = position;
		// Position of RectangleShape2D is the middle of the rectangle
		var width = size.X;
		var height = size.Y;
		var middleWidth = width / 2f;
		var middleHeight = height / 2f;
		var topObstacleHeight = gapOffset;
		var topMiddleHeight = topObstacleHeight / 2f;
		var bottomObstacleHeight = height - topObstacleHeight - gapSize;
		// The middle of the bottom gap is the topleft of the bottom rectangle + half the remaining height for that rectangle
		var bottomMiddleHeight = topObstacleHeight + gapSize + bottomObstacleHeight / 2f;

		var topObstacle = GetNode<Area2D>("TopObstacle");
		var bottomObstacle = GetNode<Area2D>("BottomObstacle");
		if (bottomObstacle.GetNode<CollisionShape2D>("CollisionShape2D").Shape is RectangleShape2D bottomShape)
		{
			bottomObstacle.Position = new Vector2(middleWidth, bottomMiddleHeight);
			bottomShape.Size = new Vector2(width, bottomObstacleHeight);
		}
		if (topObstacle.GetNode<CollisionShape2D>("CollisionShape2D").Shape is RectangleShape2D topShape)
		{
			topObstacle.Position = new Vector2(middleWidth, topMiddleHeight);
			topShape.Size = new Vector2(width, topObstacleHeight);
		}

		// Setup scoring zone
		var scoringZone = GetNode<Area2D>("ScoringZone");
		if (scoringZone.GetNode<CollisionShape2D>("CollisionShape2D").Shape is RectangleShape2D scoringZoneShape)
		{
			scoringZone.Position = new Vector2(middleWidth, middleHeight);
			scoringZoneShape.Size = new Vector2(width, height);
		}

		var pipeTopHeight = 40;
		var pipeMiddleHeight = 4;

		var visualHolderTop = topObstacle.GetNode<Node2D>("VisualHolder");
		var pipeTop = GD.Load<Texture2D>("res://Assets/Elements/Obstacle/WaaappyBirdPipeTop.png");
		var pipeMiddle = GD.Load<Texture2D>("res://Assets/Elements/Obstacle/WaaappyBirdPipeMiddle.png");

		// Set sprites of the top obstacle
		var numberOfMiddlesTopObstacle = (topObstacleHeight - pipeTopHeight) / pipeMiddleHeight;
		for (int i = 0; i < numberOfMiddlesTopObstacle; i++)
		{
			visualHolderTop.AddChild(
				new Sprite2D
				{
					Position = new Vector2(0, -topMiddleHeight + (i + 0.5f) * pipeMiddleHeight),
					Texture = pipeMiddle,
				}
			);
		}
		visualHolderTop.AddChild(
			new Sprite2D
			{
				Position = new Vector2(0, -topMiddleHeight + numberOfMiddlesTopObstacle * pipeMiddleHeight + pipeTopHeight / 2f),
				Texture = pipeTop,
				FlipV = true
			}
		);

		// Set sprites of the bottom obstacle
		var visualHolderBottom = bottomObstacle.GetNode<Node2D>("VisualHolder");
		var numberOfMiddlesBottomObstacle = (bottomObstacleHeight - pipeTopHeight) / pipeMiddleHeight;
		visualHolderBottom.AddChild(
			new Sprite2D
			{
				Position = new Vector2(0, -bottomMiddleHeight + topObstacleHeight + gapSize + pipeTopHeight / 2f),
				Texture = pipeTop
			});
		for (int i = 0; i < numberOfMiddlesBottomObstacle; i++)
		{
			visualHolderBottom.AddChild(
				new Sprite2D
				{
					Position = new Vector2(0, -bottomMiddleHeight + topObstacleHeight + gapSize + pipeTopHeight + (i + 0.5f) * pipeMiddleHeight),
					Texture = pipeMiddle
				}
			);
		}
	}

	private static Sprite2D CreateObstacleSprite(Vector2 position, Vector2 size, Texture2D texture)
	{
		var sprite = new Sprite2D
		{
			Position = position,
			Scale = size,
			Texture = texture
		};
		return sprite;
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
