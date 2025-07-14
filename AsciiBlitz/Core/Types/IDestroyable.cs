namespace AsciiBlitz.Core.Types;

public interface IDestroyable
{
  event Action<IDestroyable> OnDestroyed;
  void Destroy();
}
