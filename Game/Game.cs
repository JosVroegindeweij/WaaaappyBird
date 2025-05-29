using Godot;

public partial class Game : Node2D
{
	public override void _Ready()
	{
		Vector2 screenSize = GetViewport().GetVisibleRect().Size;
		float screenEdgeThickness = 10;

		GetNode<ScreenEdge>("TopScreenEdge")
			.Setup(
				size: new Vector2(x: screenSize.X, y: screenEdgeThickness),
				angle: 0,
				position: new Vector2(screenSize.X / 2f, 0)
			);
		// GetNode<ScreenEdge>("BottomScreenEdge")
		// 	.Setup(
		// 		size: new Vector2(x: screenSize.X, y: screenEdgeThickness),
		// 		angle: 0,
		// 		position: new Vector2(screenSize.X / 2f, screenSize.Y)
		// 	);
	}
}
