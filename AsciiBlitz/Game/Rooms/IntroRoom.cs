using AsciiBlitz.Core.Input;
using AsciiBlitz.Core.Render;
using AsciiBlitz.Core.Render.Buffer;
using AsciiBlitz.Core.Services;
using AsciiBlitz.Core.Utils;

namespace AsciiBlitz.Game.Rooms;

public class IntroRoom : AbstractRoom {
  private readonly IGameInput _input = Services.Get<IGameInput>();
  private readonly IConsoleRenderer _renderer = Services.Get<IConsoleRenderer>();
  private readonly IRoomService _roomService = Services.Get<IRoomService>();
  private readonly DrawUtils _drawUtils;
  
  private string[] _lines = [];
  private int _imageWidth;

  public IntroRoom() {
    _drawUtils = new DrawUtils(_renderer.Buffer);
  }

  public override void OnRoomEnter() {
    _renderer.Clear();
    _lines = Assets.LoadLines("Images/Intro.txt");
    _imageWidth = _lines.Max(x => x.Length);
  }

  public override bool ProcessFrame(IFrameContext frameContext) {
    var buf = _renderer.Buffer;

    _drawUtils
      .SetTarget(buf) // Important to set, as Buffer is swapped each frame.
      .DrawTextLines((buf.Width - _imageWidth) / 2, 1, _lines)
      .DrawTextCentered(buf.Width / 2, buf.Height - 5, "Please do not resize game window")
      .DrawTextCentered(buf.Width / 2, buf.Height - 4, "Press any key to continue");

    if (_input.GetKey() != null) {
      _roomService.GoToRoom<MenuRoom>();
      _input.Consume();
    }

    return true;
  }
}
