namespace AsciiBlitz.Core.Objects.Components;

public interface IDamageable {
  IDamageableComponent Damageable { get; }
}

public interface IDamageableComponent {
  float Health { get; }
  float MaxHealth { get; }
  void ApplyDamage(float amount);
  bool IsDead { get; }
}

public class BaseDamageableComponent : IDamageableComponent {
  private float _health;
  private float _maxHealth;

  public float Health => _health;
  public float MaxHealth => _maxHealth;
  public bool IsDead => _health <= 0;

  public BaseDamageableComponent(float health) {
    _health = health;
    _maxHealth = health;
  }

  public void ApplyDamage(float amount) {
    _health = Math.Clamp(_health - amount, 0, _maxHealth);
    
    Console.SetCursorPosition(0, 1);
    Console.WriteLine($"Damage {amount} to {this}");
  }
}
