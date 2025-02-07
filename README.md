# csharp-bounce
Bouncing Ball demo in C#.Net

One of my favourite things to write in any new language I come across is program to bounce a ball around the screen.  I've been programming with C#.Net for quite a number of year but oddly have never written a bouncing-ball program in C#.

This Console Mode demonstration of a Bouncing Ball in C#.Net uses https://github.com/gui-cs/Terminal.Gui/tree/v1_release for controlling the console I/O.  Terminal.Gui will handle console output on terminal windows in MS/Windows, Linux and MacOS.

The Demo allows you to add numerous balls (or remove them down to 1), and control the speed.   Each ball is randomly positioned, and has a random bounce direction as well as a random colour.
Each ball has a tail tracking the last 10 places the ball was.  The bouncing of the ball should auto-correct itself for any change in terminal window size

![Sample Output](./csharp-bounce/csharp-bounce.gif)

Controls are shown to:
* [ESC] exit the demo application
* [+] to add a ball (maximum of 10)
* [-] to remove a ball (down to 1)
* [CursorUp] Increase the speed of all balls
* [CursorDown] Decrease the speed of all balls

Each ball is made up of a ball 'head' and a 'tail'.  Each new ball is started in a random position, a random direction and a random colour.

I've chosen characters to represent a ball from a Unicode character set, with the characters of the tail getting smaller.  

As can be seen in the demo, when there are a number of balls bouncing around, resizing the window edges can make the balls aggregate into some interesting patterns.

