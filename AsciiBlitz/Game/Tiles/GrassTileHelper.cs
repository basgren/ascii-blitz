namespace AsciiBlitz.Game.Tiles;

public static class GrassTileHelper {
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
}
