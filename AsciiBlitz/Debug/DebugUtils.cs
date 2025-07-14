using System.Text;

namespace AsciiBlitz.Debug;

public class DebugUtils {
  /// <summary>
  /// Renders characters with each of 256 available ANSI colors.
  /// </summary>
  /// <param name="step"></param>
  public static void PrintAInAllColors(int step = 1) {
    int perLine = 36;
    Console.OutputEncoding = Encoding.UTF8;
    Console.Clear();
    StringBuilder sb = new();

    Console.WriteLine("    012345678901234567890123456789012345");
    
    for (int i = 0; i < 256; i += step) {
      sb.Append($"\x1b[38;5;{i}mA");
      
      if ((i / step + 1) % perLine == 0) {
        Console.WriteLine($"{(i + 1 - perLine).ToString().PadLeft(3)} {sb}");
        sb.Clear();
      }
    }
    
    Console.Write("\x1b[0m\n"); // Escape
  }
}
