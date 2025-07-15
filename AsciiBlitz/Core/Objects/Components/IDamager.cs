namespace AsciiBlitz.Core.Types;

public interface IDamager {
  IDamagerComponent Damager { get; }
}

public interface IDamagerComponent {
  float Damage { get; }
}

public class BaseDamagerComponent : IDamagerComponent {
  public float Damage { get; }

  public BaseDamagerComponent(float damage) {
    Damage = damage;
  }
}
