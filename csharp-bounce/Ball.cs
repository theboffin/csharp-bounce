using Terminal.Gui;

namespace Bounce;


public class Ball
{
    public Position Position { get; set; }
    public Position Direction { get; set; }
    public Bounds Bounds { get; set; }
    public List<Position> Tail { get; set; } = new List<Position>();

    public ColorScheme ColorScheme { get; set; }
    const int MaxTailLength = 10;


    public Ball(Bounds bounds)
    {
        var random = new Random();
        Bounds = bounds;

        // randomly set the starting position and direction
        Position = new Position(bounds.Left + random.Next(bounds.Right), bounds.Top + random.Next(bounds.Bottom));
        Direction = new Position(random.Next(2) == 0 ? -1 : 1, new Random().Next(2) == 0 ? -1 : 1);

        var colors = new List<Color> { Color.BrightRed, Color.BrightGreen, Color.BrightYellow, Color.BrightBlue, Color.BrightMagenta, Color.BrightCyan, Color.White, Color.Blue, Color.Red, Color.Green, Color.Brown, Color.Magenta, Color.Cyan };

        var randomColor = colors[random.Next(0, colors.Count)];

        ColorScheme = new ColorScheme()
        {
            Normal = Terminal.Gui.Attribute.Make(randomColor, Color.Black)
        };
    }

    public void Update()
    {
        UpdateTail();
        Position = new Position(Position.X + Direction.X, Position.Y + Direction.Y);

        if (!Bounds.IsInHorizontalBounds(Position))
        {
            Direction = new Position(Direction.X * -1, Direction.Y);
        }
        if (!Bounds.IsInVerticalBounds(Position))
        {
            Direction = new Position(Direction.X, Direction.Y * -1);
        }
    }

    private void UpdateTail()
    {
        Tail.Insert(0, Position);
        if (Tail.Count > MaxTailLength)
        {
            Tail.RemoveAt(Tail.Count - 1);
        }
    }

    public void UpdateBounds(Bounds bounds)
    {
        Bounds = bounds;

        if (Position.X > Bounds.Right)
        {
            Position = new Position(Bounds.Right, Position.Y);
        }
        if (Position.Y > Bounds.Bottom)
        {
            Position = new Position(Position.X, Bounds.Bottom);
        }
    }
}
