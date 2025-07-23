using System.Runtime.InteropServices;
using System.Text;

namespace AsciiBlitz.Core.Render.Buffer;

/// <summary>
/// Tightly linked with Console. Renders contents of the buffer right on the screen. 
/// </summary>
public class BufferedConsoleRenderer {
  private ScreenBuffer _front = new(1, 1);
  private ScreenBuffer _back = new(1, 1);

  public ScreenBuffer Buffer => _front;

  public BufferedConsoleRenderer() {
    Console.OutputEncoding = Encoding.UTF8;
  }

  public void Init() {
    int width = Console.WindowWidth;
    int height = Console.WindowHeight;
    
    _front.SetSize(width, height);
    _back.SetSize(width, height);
  }
  
  private readonly ScreenCell _emptyCell = new(' ', AnsiColor.White, AnsiColor.Black);

  public void Render() {
    var curr = _front.Surface;
    var prev = _back.Surface;
    int height = _front.Height;
    int width = _front.Width;

    var sb = new StringBuilder();
    
    for (int y = 0; y < height; y++) {
      bool changed = false;

      for (int x = 0; x < width; x++) {
        if (curr[y, x] != prev[y, x]) {
          changed = true;
          break;
        }
      }

      if (!changed) {
        continue;
      }

      Console.SetCursorPosition(0, y);

      int? lastFg = null;
      int? lastBg = null;

      for (int x = 0; x < width; x++) {
        var cell = curr[y, x];

        if (cell == null) {
          cell = _emptyCell;
        }

        if (cell.Value.Color != lastFg || cell.Value.BgColor != lastBg) {
          sb.Append(AnsiColor.GetCode(cell.Value.Color, cell.Value.BgColor));
          lastFg = cell.Value.Color;
          lastBg = cell.Value.BgColor;
        }

        sb.Append(cell.Value.Char);
      }

      sb.Append(AnsiColor.Reset);
      Console.Write(sb.ToString());
      sb.Clear();
    }

    // Swap buffers
    (_front, _back) = (_back, _front);
    _front.Clear();
  }

  public void Clear() {
    _front.Clear();
    _back.Clear();
  }
}
