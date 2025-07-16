using System.Text;

namespace AsciiBlitz.Core.Render.Buffer;

public class BufferedConsoleRenderer {
  private ScreenBuffer _front = new(1, 1);
  private ScreenBuffer _back = new(1, 1);

  public ScreenBuffer Buffer => _front;

  public void Resize(int width, int height) {
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
}
