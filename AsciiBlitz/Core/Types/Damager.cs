namespace AsciiBlitz.Core.Types;

public interface IDamager {
  float Damage { get; }
}

public interface IHasDamager {
  IDamager Damager { get; }
}

public class BaseDamager : IDamager {
  public float Damage { get; }

  public BaseDamager(float damage) {
    Damage = damage;
  }
}
