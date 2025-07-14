using AsciiBlitz.Core.Map.Layers;
using AsciiBlitz.Core.Map.Objects;
using AsciiBlitz.Core.Types;

namespace AsciiBlitz.Core;

public static class CollisionSystem {
  /// <summary>
  /// Invoked after all moving objects have updated their coordinates.
  /// </summary>
  /// <param name="objects"></param>
  public static void PostCollisionCheck(List<ICollidable> objects) {
    for (int i = 0; i < objects.Count; i++) {
      var a = objects[i];

      if (!a.IsActive) {
        continue;
      }

      for (int j = i + 1; j < objects.Count; j++) {
        var b = objects[j];
        if (!b.IsActive) continue;

        if (a.Bounds.Intersects(b.Bounds)) {
          ProcessCollision(a, b);
          ProcessCollision(b, a);
        }
      }
    }
  }

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

          Console.SetCursorPosition(2, 33);
          Console.Write($"Collision! {tile}");

          collidable.OnCollision(tile);

          // We assume that every tile is collidable. All non-collidable tiles should be added to another layers.
          // if (obj is IHasDamager damager && tile is IHasDamageable damageable) {
          //   // damageable.Damageable.ApplyDamage(damager.Damager.Damage);
          //
          //   if (obj is Bullet bullet)
          //     bullet.Destroy();
          // }
        }
      }
    }
  }

  private static void ProcessCollision(ICollidable source, ICollidable target) {
    if (source is IHasDamager damager && target is IHasDamageable damageable) {
      damageable.Damageable.ApplyDamage(damager.Damager.Damage);
      // todo add event to Damager
    }
  }
}
