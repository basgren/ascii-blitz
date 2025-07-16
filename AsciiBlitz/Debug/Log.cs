namespace AsciiBlitz.Debug;

public static class Log {
  public static void Info(string s) {
    Console.Write(s);
  }
  
  public static void Info(int x, int y, string s) {
    Console.SetCursorPosition(x, y);
    Info(s);
  }
}
