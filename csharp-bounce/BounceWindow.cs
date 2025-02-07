using Terminal.Gui;

namespace Bounce;

public class BounceWindow : Window
{
	private readonly List<Ball> Balls = [];
	private int BallSpeed { get; set; } = 15;
	private Label Status { get; set; }
	private object TimeOutToken { get; set; }
	private const int MaxSpeed = 50;

	public BounceWindow()
	{
		Title = "Bouncing Ball (ESC to quit)";

		ColorScheme = new ColorScheme()
		{
			Normal = Terminal.Gui.Attribute.Make(Color.Gray, Color.Black)
		};

		Status = new Label()
		{
			X = Pos.Center(),
			Y = Pos.Center(),
			ColorScheme = new ColorScheme()
			{
				Normal = Terminal.Gui.Attribute.Make(Color.Cyan, Color.Black)
			}
		};

		// Add the event handlers
		KeyPress += HandleKeyPresses;
		Application.Resized += WindowResized;

		TimeOutToken = ResetTimer();

		// Add the first ball
		AddBall();
		Add(Status);
	}

	private bool UpdateBalls(MainLoop loop)
	{
		foreach (var ball in Balls)
		{
			ball.Update();
		}

		UpdateStatus();

		return true;
	}

	private void WindowResized(Application.ResizedEventArgs e)
	{
		foreach (var b in Balls)
		{
			b.UpdateBounds();
		}

		UpdateStatus();
	}

	private void UpdateStatus()
	{
		int currentWidth = Terminal.Gui.Application.Driver.Cols;
		int currentHeight = Terminal.Gui.Application.Driver.Rows;
		Status.Text = $"Width: {currentWidth} - Height: {currentHeight}  - Speed: {BallSpeed} - Balls: {Balls.Count} \n[↑] Decrease Speed [↓] Increase Speed, [←] Remove Ball [→] All Ball";
	}

	private void HandleKeyPresses(KeyEventEventArgs e)
	{
		switch (e.KeyEvent.Key)
		{
			case Key.Esc:
				Application.RequestStop();
				break;

			case Key.CursorUp:
				BallSpeed = Math.Min(MaxSpeed, BallSpeed + 1);
				TimeOutToken = ResetTimer();
				break;

			case Key.CursorDown:
				BallSpeed = Math.Max(1, BallSpeed - 1);
				TimeOutToken = ResetTimer();
				break;

			case Key.CursorRight:
				AddBall();
				break;

			case Key.CursorLeft:
				RemoveBall();
				break;
		}

		UpdateStatus();
	}

	private object ResetTimer()
	{
		Application.MainLoop.RemoveTimeout(TimeOutToken);
		return Application.MainLoop.AddTimeout(TimeSpan.FromMilliseconds(BallSpeed), UpdateBalls);
	}

	private void AddBall()
	{
		var ball = new Ball(this);
		Balls.Add(ball);

		Add(ball);
	}

	private void RemoveBall()
	{
		if (Balls.Count > 1)
		{
			Balls[Balls.Count - 1].RemoveTail();
			Remove(Balls[Balls.Count - 1]);
			Balls.RemoveAt(Balls.Count - 1);
		}
	}
}