
namespace AsciiBlitz.Core.Types;

public interface IDamageable {
  float Health { get; }
  float MaxHealth { get; }
  void ApplyDamage(float amount);
  bool IsDead { get; }
}

public interface IHasDamageable {
  IDamageable Damageable { get; }
}

public class BaseDamageable : IDamageable {
  private float _health;
  private float _maxHealth;

  public float Health => _health;
  public float MaxHealth => _maxHealth;
  public bool IsDead => _health <= 0;

  public BaseDamageable(float health) {
    _health = health;
    _maxHealth = health;
  }

  public void ApplyDamage(float amount) {
    _health = Math.Clamp(_health - amount, 0, _maxHealth);
    
    Console.SetCursorPosition(0, 1);
    Console.WriteLine($"Damage {amount} to {this}");
  }
}
