namespace AsciiBlitz.Core.Utils;

public static class Assets {
  private static readonly string AssetsRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets");

  public static string[] LoadLines(string relativePath) {
    string fullPath = GetPath(relativePath);

    if (!File.Exists(fullPath)) {
      throw new FileNotFoundException($"Asset file not found: {fullPath}");
    }

    return File.ReadAllLines(fullPath);
  }

  private static string GetPath(string relativePath) {
    string combined = Path.Combine(AssetsRoot, relativePath);
    return Path.GetFullPath(combined);
  }
}
