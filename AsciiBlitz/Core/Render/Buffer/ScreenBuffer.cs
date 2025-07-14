using System.Text;

namespace AsciiBlitz.Core.Render.Buffer;

public class ScreenBuffer {
  public int Width { get; private set; }
  public int Height { get; private set; }

  private ScreenCell?[,] _current;
  private ScreenCell?[,] _previous;

  public ScreenBuffer() {
    Console.OutputEncoding = Encoding.UTF8;

    Width = 10;
    Height = 10;

    SetSize(Width, Height);
  }

  public void SetSize(int width, int height) {
    Width = width;
    Height = height;

    _current = new ScreenCell?[height, width];
    _previous = new ScreenCell?[height, width];
  }

  public void Clear() {
    for (int y = 0; y < Height; y++) {
      for (int x = 0; x < Width; x++) {
        _current[y, x] = null;
      }
    }
  }

  public void Set(int x, int y, char symbol, int fg, int bg) {
    if (x >= 0 && x < Width && y >= 0 && y < Height) {
      _current[y, x] = new ScreenCell(symbol, fg, bg);
    }
  }

  public void RenderChangesOnly() {
    for (int y = 0; y < Height; y++) {
      bool changed = false;

      for (int x = 0; x < Width; x++) {
        if (_current[y, x] != _previous[y, x]) {
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

      for (int x = 0; x < Width; x++) {
        var cell = _current[y, x];

        if (cell == null) {
          sb.Append(' ');
          continue;
        }

        if (cell.Value.Color != lastFg || cell.Value.BgColor != lastBg) {
          sb.Append($"\x1b[38;5;{cell.Value.Color};48;5;{cell.Value.BgColor}m");
          lastFg = cell.Value.Color;
          lastBg = cell.Value.BgColor;
        }

        sb.Append(cell.Value.Char);
      }

      sb.Append("\x1b[0m");
      Console.Write(sb.ToString());

      for (int x = 0; x < Width; x++) {
        _previous[y, x] = _current[y, x];
      }
    }
  }
}
