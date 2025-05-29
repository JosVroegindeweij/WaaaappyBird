using Godot;

public partial class ScreenEdge : StaticBody2D
{
	public void Setup(Vector2 size, float angle, Vector2 position)
	{
		if (GetNode<CollisionShape2D>("CollisionShape2D")?.Shape is RectangleShape2D shape)
		{
			shape.Size = size;
		}
		RotationDegrees = angle;
		Position = position;
	}
}
