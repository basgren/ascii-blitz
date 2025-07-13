using System.Text;

namespace AsciiBlitz.Debug;

public class DebugUtils {
  /// <summary>
  /// Renders characters with each of 256 available ANSI colors.
  /// </summary>
  /// <param name="step"></param>
  public static void PrintAInAllColors(int step = 4) {
    Console.OutputEncoding = System.Text.Encoding.UTF8;
    Console.Clear();
    StringBuilder sb = new();
    
    for (int i = 0; i < 256; i += step) {
      sb.Append($"\x1b[38;5;{i}mA");
      
      if ((i / step + 1) % 36 == 0) {
        Console.Write(sb.ToString());
        sb.Clear();
        Console.WriteLine();
      }
    }
    
    Console.Write("\x1b[0m\n"); // Escape
  }
}
