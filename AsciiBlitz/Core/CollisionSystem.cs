using AsciiBlitz.Core.Map.Layers;
using AsciiBlitz.Core.Objects;
using AsciiBlitz.Core.Objects.Components;
using AsciiBlitz.Core.Types;
using AsciiBlitz.Game.Objects;

namespace AsciiBlitz.Core;

public static class CollisionSystem {
  
  public static void CheckCollisionsWithTiles(IReadOnlyList<ICollidable> objects, TileLayer tileLayer) {
    foreach (var collidable in objects) {
      if (!collidable.IsActive) {
        continue;
      }

      if (collidable is Projectile) {
        int a = 2;
      }

      var bounds = collidable.Bounds;
      int minX = (int)bounds.X;
      int maxX = (int)(bounds.X + bounds.Width);
      int minY = (int)bounds.Y;
      int maxY = (int)(bounds.Y + bounds.Height);

      for (int x = minX; x <= maxX; x++) {
        for (int y = minY; y <= maxY; y++) {
          if (!tileLayer.TryGetTileAt(x, y, out TileObject? tile)) {
            continue;
          }

          // First, apply damage, as OnCollision may destroy object.
          // We assume that every tile is collidable. All non-collidable tiles should be added to another layers.
          if (collidable is IDamager damager && tile is IDamageable damageable) {
            damageable.Damageable.ApplyDamage(damager.Damager.Damage);

            if (damageable.Damageable.IsDead) {
              tileLayer.Remove(tile);
            }
          }
          
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
    if (source is IDamager damager && target is IDamageable damageable) {
      damageable.Damageable.ApplyDamage(damager.Damager.Damage);
    }
    
    source.OnCollision(target);
  }
}
