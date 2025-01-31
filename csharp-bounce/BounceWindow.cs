using Terminal.Gui;

namespace Bounce;

public class BounceWindow : Window
{
	readonly List<Ball> Balls = [];
	readonly List<Label> BallLabels = [];
	readonly List<List<Label>> TailLabels = [];
	int BallSpeed { get; set; } = 15;
	Label Status { get; set; }
	object TimeOutToken { get; set; }
	const int MaxSpeed = 50;
	readonly string[] BallCharacters = { "〇", "◯", "○", "○", "◌", "◌", "◌", "◌", "◌", "◌", "⋅" };

	public BounceWindow()
	{
		Title = "Bouncing Ball (ESC to quit)";

		ColorScheme = new ColorScheme()
		{
			Normal = Terminal.Gui.Attribute.Make(Color.White, Color.Black)
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


		KeyPress += HandleKeyPresses;
		Application.Resized += WindowResized;

		TimeOutToken = ResetTimer();

		AddBall();
		Add(Status);
	}

	private bool UpdateBalls(MainLoop loop)
	{
		for (int i = 0; i < Balls.Count; i++)
		{
			Balls[i].Update();
			BallLabels[i].X = Balls[i].Position.X;
			BallLabels[i].Y = Balls[i].Position.Y;
			UpdateTails(i);
		}
		UpdateStatus();
		return true;
	}

	private void UpdateTails(int ball)
	{
		for (int i = 0; i < Balls[ball].Tail.Count; i++)
		{
			if (TailLabels[ball].Count <= i)
			{
				var tailLabel = new Label()
				{
					Text = BallCharacters[TailLabels[ball].Count],
					X = Balls[ball].Tail[i].X,
					Y = Balls[ball].Tail[i].Y,
					ColorScheme = Balls[ball].ColorScheme
				};
				TailLabels[ball].Add(tailLabel);
				Add(tailLabel);
			}
			else
			{
				TailLabels[ball][i].X = Balls[ball].Tail[i].X;
				TailLabels[ball][i].Y = Balls[ball].Tail[i].Y;
			}
		}
	}

	private void WindowResized(Application.ResizedEventArgs e)
	{
		int currentWidth = Application.Driver.Cols;
		int currentHeight = Application.Driver.Rows;

		foreach (var b in Balls)
		{
			b.UpdateBounds(new Bounds(0, 0, currentHeight - 3, currentWidth - 3));
		}

		UpdateStatus();
	}

	private void UpdateStatus()
	{
		int currentWidth = Terminal.Gui.Application.Driver.Cols;
		int currentHeight = Terminal.Gui.Application.Driver.Rows;
		Status.Text = $"Width: {currentWidth} - Height: {currentHeight}  - Speed: {BallSpeed} - Balls: {Balls.Count} \n[↑/↓ to change speed, ←/→ to add/remove balls]";
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
		int currentWidth = Application.Driver.Cols;
		int currentHeight = Application.Driver.Rows;
		var ball = new Ball(new Bounds(0, 0, currentHeight - 3, currentWidth - 3));
		Balls.Add(ball);

		var ballLabel = new Label()
		{
			Text = BallCharacters[0],
			X = ball.Position.X,
			Y = ball.Position.Y,
			ColorScheme = ball.ColorScheme
		};
		BallLabels.Add(ballLabel);
		TailLabels.Add([]);
		Add(ballLabel);
	}

	private void RemoveBall()
	{
		if (Balls.Count > 1)
		{
			foreach (var tailLabel in TailLabels[Balls.Count - 1])
			{
				Remove(tailLabel);
			}

			Remove(BallLabels[Balls.Count - 1]);
			BallLabels.RemoveAt(Balls.Count - 1);
			TailLabels.RemoveAt(Balls.Count - 1);
			Balls.RemoveAt(Balls.Count - 1);
		}
	}
}