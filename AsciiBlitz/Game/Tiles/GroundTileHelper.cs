using AsciiBlitz.Core.Objects;

namespace AsciiBlitz.Game.Tiles;

public static class GroundTileHelper {
  public static int GetGrassColor(int x, int y, double t, double xOffsetScale) {
    // === CONFIGURATION CONSTANTS ===

    const double BaseFrequency = 0.5; // base wave frequency (radians per second)
    const double WaveSpeed = 0.8; // vertical wave movement speed (units per second)

    double PhaseOffsetX = 0.5 * xOffsetScale; // phase shift per X unit
    const double PhaseOffsetY = 0.7; // phase shift per Y unit

    const double NoiseScaleX = 0.21; // noise frequency scale on X
    const double NoiseScaleY = 0.37; // noise frequency scale on Y
    const double NoiseAmount = 0.7; // amplitude of positional noise

    const int ColorSteps = 3; // number of discrete grass states (e.g., 0 = left, 1 = center, 2 = right)

    // === SPATIAL PHASE OFFSET + NOISE ===
    double phase = (x * PhaseOffsetX + y * PhaseOffsetY); //  * windStrength;
    double noise = Math.Sin(x * NoiseScaleX + y * NoiseScaleY) * NoiseAmount;

    // === WAVE PHASE COMPUTATION ===
    double wavePhase = BaseFrequency * t - y * WaveSpeed + phase + noise;

    // === SINUSOIDAL WAVE VALUE IN [0..1] ===
    double wave = Math.Sin(wavePhase);
    double normalized = (wave + 1.0) * 0.5;

    // === FINAL DISCRETE COLOR STATE (0, 1, 2) ===
    return (int)(normalized * (ColorSteps - 0.001));
  }
  
  public static char? GetDamageChar(int x, int y, GroundTile tile, int damageThreshold) {
    if (tile.Sprite == null || tile.DamageLevel < damageThreshold) {
      return null;
    }

    int maxDamage = 4;
    int width = tile.Sprite.Width;
    int height = tile.Sprite.Height;
    
    int fragPosX = tile.Pos.X * width + x;
    int fragPosY = tile.Pos.Y * width + y;
    int id = fragPosX + fragPosY * 100;
    
    int vertDmg = tile.VertDamageLevel - damageThreshold;
    int horzDmg = tile.HorzDamageLevel - damageThreshold;

    if (vertDmg >= 0 && (x == 0 || x == width - 1)) {
      return RandInt(id, maxDamage) <= vertDmg ? '#' : null;
    }
    
    if (horzDmg >= 0 && (y == 0 || y == height - 1)) {
      return RandInt(id, maxDamage) <= horzDmg ? '#' : ' ';
    }

    return null;
  }
  
  private static double Rand(int value) {
    double v = 12345.234f * (Math.Sin(value * 4567.89f) * 0.5f + 0.5f);

    return v - Math.Truncate(v);
  }

  private static int RandInt(int value, int max) {
    return (int)Math.Floor(Rand(value) * max);
  }
}
