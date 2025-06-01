using Godot;

public partial class GameManager : Node
{
	// Called when the node enters the scene tree for the first time.

	private bool isPaused;
	private bool isGameOver;
	private bool isInGracePeriod = false;

	private Timer gracePeriodTimer;

	public override void _Ready()
	{
		isPaused = true;
		GetTree().Paused = true;

		var inputHandler = GetNode<InputHandler>("/root/Main/InputHandler");
		inputHandler.PausePressed += OnPausePressed;
		inputHandler.UnpausePressed += OnUnpausePressed;

		var player = GetNode<Player>("/root/Main/Game/Player");
		player.CollidedWithScreenEdge += OnCollidedWithScreenEdge;

		gracePeriodTimer = GetNode<Timer>("GracePeriodTimer");
		gracePeriodTimer.Timeout += OnGracePeriodTimerTimeout;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// TODO Implement obstacles + score counting
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
		if (isGameOver || isPaused) return;
		isGameOver = true;
		GetTree().SetDeferred("paused", true);
		EmitSignal(SignalName.GameOver);
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
