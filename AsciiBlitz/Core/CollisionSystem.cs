using AsciiBlitz.Core.Map.Layers;
using AsciiBlitz.Core.Map.Objects;
using AsciiBlitz.Core.Types;

namespace AsciiBlitz.Core;

public static class CollisionSystem {
  
  public static void CheckCollisionsWithTiles(IReadOnlyList<ICollidable> objects, TileLayer tileLayer) {
    foreach (var collidable in objects) {
      if (!collidable.IsActive) {
        continue;
      }

      var bounds = collidable.Bounds;
      int minX = (int)bounds.X;
      int maxX = (int)(bounds.X + bounds.Width);
      int minY = (int)bounds.Y;
      int maxY = (int)(bounds.Y + bounds.Height);

      for (int x = minX; x < maxX; x++) {
        for (int y = minY; y < maxY; y++) {
          if (!tileLayer.TryGetTileAt(x, y, out TileObject? tile)) {
            continue;
          }

          // First apply damage, as OnCollision may destroy object.
          // We assume that every tile is collidable. All non-collidable tiles should be added to another layers.
          if (collidable is IHasDamager damager && tile is IHasDamageable damageable) {
            damageable.Damageable.ApplyDamage(damager.Damager.Damage);

            if (damageable.Damageable.IsDead) {
              tileLayer.Remove(tile);
            }
            
            // Console.SetCursorPosition(2, 33);
            // Console.Write($"Applied damage {damager.Damager.Damage}");
          }
          
          // Console.SetCursorPosition(2, 34);
          // Console.Write($"Collision! {tile}");
          
          collidable.OnCollision(tile);
        }
      }
    }
  }
  
  /// <summary>
  /// Invoked after all moving objects have updated their coordinates.
  /// </summary>
  /// <param name="collidables"></param>
  public static void PostCollisionCheck(IReadOnlyList<ICollidable> collidables) {
    for (int i = 0; i < collidables.Count; i++) {
      var collidableA = collidables[i];

      if (!collidableA.IsActive) {
        continue;
      }

      for (int j = i + 1; j < collidables.Count; j++) {
        var collidableB = collidables[j];
        
        if (!collidableB.IsActive) {
          continue;
        }

        if (collidableA.Bounds.Intersects(collidableB.Bounds)) {
          ProcessCollision(collidableA, collidableB);
          ProcessCollision(collidableB, collidableA);
        }
      }
    }
  }

  private static void ProcessCollision(ICollidable source, ICollidable target) {
    if (source is IHasDamager damager && target is IHasDamageable damageable) {
      damageable.Damageable.ApplyDamage(damager.Damager.Damage);
    }
    
    source.OnCollision(target);
  }
}
