using System.Runtime.InteropServices;

namespace AsciiBlitz;

public static class ConsoleUtils {
  public static int Width { get; private set; } = Console.WindowWidth;
  public static int Height { get; private set; } = Console.WindowHeight;

  public static void SetConsoleSize(int width, int height) {
    // Set window size using escape sequence.
    Console.Write($"\x1b[8;{height};{width}t");

    // To prevent exception in Linux.
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
      Console.SetBufferSize(width, height);
      Console.SetWindowSize(width, height);
    }

    // Also store in var as in linux Console.WindowHeight/Width is not synchronized.
    Width = width;
    Height = height;
  }
}
