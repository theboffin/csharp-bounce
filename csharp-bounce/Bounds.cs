namespace Bounce;

public class Bounds
{
    public int Top { get; }
    public int Left { get; }
    public int Bottom { get; }
    public int Right { get; }

    public Bounds(int top, int left, int bottom, int right)
    {
        Top = top;
        Left = left;
        Bottom = bottom;
        Right = right;
    }

    public bool IsInBounds(Position position)
    {
        return position.X >= Left && position.X <= Right && position.Y >= Top && position.Y <= Bottom;
    }

    public bool IsInHorizontalBounds(Position position)
    {
        return position.X >= Left && position.X <= Right;
    }

    public bool IsInVerticalBounds(Position position)
    {
        return position.Y >= Top && position.Y <= Bottom;
    }
}