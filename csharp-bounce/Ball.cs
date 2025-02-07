using Terminal.Gui;

namespace Bounce;

public class Ball : Label
{
    public Position Position { get; private set; }
    public Position Direction { get; private set; }
    private Window ParentWindow { get; init; }
    private List<Label> Tail { get; } = [];
    private const int MaxTailLength = 10;
    private readonly string[] BallCharacters = ["〇", "◯", "○", "○", "◌", "◌", "◌", "◌", "◌", "◌", "⋅"];

    public Ball(Window parentWindow)
    {
        var random = new Random();
        ParentWindow = parentWindow;
        Text = BallCharacters[0];

        // randomly set the starting position and direction
        Position = new Position(1 + random.Next(ParentWindow.Bounds.Right), ParentWindow.Bounds.Top + 1 + random.Next(ParentWindow.Bounds.Bottom));
        Direction = new Position(random.Next(2) == 0 ? -1 : 1, new Random().Next(2) == 0 ? -1 : 1);

        Color[] colors = [Color.BrightRed, Color.BrightGreen, Color.BrightYellow, Color.BrightBlue, Color.BrightMagenta, Color.BrightCyan, Color.White, Color.Blue, Color.Red, Color.Green, Color.Brown, Color.Magenta, Color.Cyan];

        var randomColor = colors[random.Next(0, colors.Length)];

        ColorScheme = new ColorScheme()
        {
            Normal = Terminal.Gui.Attribute.Make(randomColor, Color.Black)
        };
    }

    public void Update()
    {
        UpdateTail(); // Tail now occupies ball position

        Position = new Position(Position.X + Direction.X, Position.Y + Direction.Y);

        // update label coordinates
        X = Position.X;
        Y = Position.Y;

        // change direction if ball is at an edge
        if (Position.X <= ParentWindow.Bounds.Left || Position.X >= ParentWindow.Bounds.Right - 2)
        {
            Direction = new Position(Direction.X * -1, Direction.Y);
        }
        if (Position.Y <= ParentWindow.Bounds.Top || Position.Y >= ParentWindow.Bounds.Bottom - 2)
        {
            Direction = new Position(Direction.X, Direction.Y * -1);
        }
    }

    private void UpdateTail()
    {
        // Grow tail to ultimate length
        if (Tail.Count < MaxTailLength)
        {
            var tailLabel = new Label()
            {
                X = Position.X,
                Y = Position.Y,
                Text = BallCharacters[Tail.Count + 1],
                ColorScheme = ColorScheme
            };
            ParentWindow.Add(tailLabel);
            Tail.Add(tailLabel);
        }

        // Move tail
        for (int i = Tail.Count - 1; i > 0; i--)
        {
            Tail[i].X = Tail[i - 1].X;
            Tail[i].Y = Tail[i - 1].Y;
        }

        // set start of the tail to where the ball currently is
        Tail[0].X = Position.X;
        Tail[0].Y = Position.Y;
    }

    public void UpdateBounds()
    {
        if (Position.X <= ParentWindow.Bounds.Left)
        {
            Position = new Position(ParentWindow.Bounds.Left, Position.Y);
            X = Position.X;
            Y = Position.Y;
        }
        if (Position.Y <= ParentWindow.Bounds.Top)
        {
            Position = new Position(Position.X, ParentWindow.Bounds.Top);
            X = Position.X;
            Y = Position.Y;
        }
        if (Position.X > ParentWindow.Bounds.Right - 3)
        {
            Position = new Position(ParentWindow.Bounds.Right - 3, Position.Y);
            X = Position.X;
            Y = Position.Y;
        }
        if (Position.Y > ParentWindow.Bounds.Bottom - 3)
        {
            Position = new Position(Position.X, ParentWindow.Bounds.Bottom - 3);
            X = Position.X;
            Y = Position.Y;
        }
    }

    public void RemoveTail()
    {
        foreach (var tail in Tail)
        {
            ParentWindow.Remove(tail);
        }
        Tail.Clear();
    }
}
