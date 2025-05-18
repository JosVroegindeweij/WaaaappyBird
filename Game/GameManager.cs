using Godot;

public partial class GameManager : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetTree().Paused = true;

		var inputHandler = GetNode<InputHandler>("/root/Main/InputHandler");
		GD.Print("Found input handler? ", inputHandler != null);
		inputHandler.PausePressed += OnPausePressed;
		inputHandler.UnpausePressed += OnUnpausePressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// TODO Implement obstacles + score counting
	}

	public void OnPausePressed()
	{
		GetTree().SetDeferred("paused", true);
		EmitSignal(SignalName.Paused);
	}

	public void OnUnpausePressed()
	{
		GetTree().SetDeferred("paused", false);
		EmitSignal(SignalName.Unpaused);
	}

	[Signal]
	public delegate void PausedEventHandler();

	[Signal]
	public delegate void UnpausedEventHandler();
}
