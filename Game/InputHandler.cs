using Godot;

public partial class InputHandler : Node
{
	public override void _Input(InputEvent @event)
	{
		if (@event.IsAction("pause"))
			EmitSignal(SignalName.PausePressed);
		if (@event.IsAction("unpause"))
			EmitSignal(SignalName.UnpausePressed);
	}

	[Signal]
	public delegate void PausePressedEventHandler();
	[Signal]
	public delegate void UnpausePressedEventHandler();
}
