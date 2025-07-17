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

  public void SetSize(int width, int height) {
    SetConsoleSize(width, height);
    _front.SetSize(width, height);
    _back.SetSize(width, height);
  }

  public void Render() {
    var curr = _front.Surface;
    var prev = _back.Surface;
    int height = _front.Height;
    int width = _front.Width;

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

      var sb = new StringBuilder();
      int? lastFg = null;
      int? lastBg = null;

      for (int x = 0; x < width; x++) {
        var cell = curr[y, x];

        if (cell == null) {
          sb.Append(' ');
          continue;
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
    }

    // Swap buffers
    (_front, _back) = (_back, _front);
    _front.Clear();
  }

  private static void SetConsoleSize(int width, int height) {
    // Set window size using escape sequence.
    Console.Write($"\x1b[8;{height};{width}t");

    // To prevent exception in Linux.
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
      Console.SetBufferSize(width, height);
      Console.SetWindowSize(width, height);
    }
  }
}
