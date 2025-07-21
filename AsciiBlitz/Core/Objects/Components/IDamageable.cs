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

public class BaseDamageableComponent(float health, Action<float>? onDamage = null) : IDamageableComponent {
  public float Health => _health;
  public float MaxHealth => _maxHealth;
  public bool IsDead => _health <= 0;
  
  private float _health = health;
  private float _maxHealth = health;

  public void ApplyDamage(float amount) {
    _health = Math.Clamp(_health - amount, 0, _maxHealth);
    onDamage?.Invoke(amount);
  }
  
  public void SetMaxHealth(float maxHealth) {
    _maxHealth = maxHealth;
  }
  
  public void SetHealth(float health) {
    _health = Math.Clamp(health, 0, _maxHealth);
  }
}
