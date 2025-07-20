using AsciiBlitz.Core.Objects;
using AsciiBlitz.Core.Render.Buffer;
using AsciiBlitz.Game.Tiles;
using AsciiBlitz.Types;

namespace AsciiBlitz.Core.Render;

public readonly struct CharContext(GameObject? gameObject, double gameTime, Vec2Int charPos) {
  public GameObject? GameObject { get; } = gameObject;
  public double GameTime { get; } = gameTime;
  public Vec2Int CharPos { get; } = charPos;
}

public abstract class Sprite {
  public int Width { get; }
  public int Height { get; }

  protected readonly ScreenCell[,] Cells;

  protected Sprite(int width, int height) {
    Width = width;
    Height = height;
    Cells = new ScreenCell[width, height];
    
    // Init with spaces by default
    for (int y = 0; y < Height; y++) {
      for (int x = 0; x < Width; x++) {
        Cells[x, y].Char = ' ';
      }
    }
    
    SetColor(7);
  }

  public abstract void UpdateCell(in CharContext context, ref ScreenCell cell);
  
  public ScreenCell[,] UpdateAll(GameObject? obj, double gameTime) {
    for (int y = 0; y < Height; y++) {
      for (int x = 0; x < Width; x++) {
        var ctx = new CharContext(obj, gameTime, new Vec2Int(x, y));
        UpdateCell(in ctx, ref Cells[x, y]);
      }
    }
    
    return Cells;
  }

  public ScreenCell[,] GetCells() => Cells;

  protected void SetColor(int color) {
    for (int y = 0; y < Height; y++) {
      for (int x = 0; x < Width; x++) {
        Cells[x, y].Color = color;
      }
    }
  }
}