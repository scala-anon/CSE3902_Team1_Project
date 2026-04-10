using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Collision;
using HollowKnight.Interfaces;
using HollowKnight.Player;
using HollowKnight.Projectiles;
using HollowKnight.Abilities;
using HollowKnight.Pathfinding;

namespace HollowKnight.Graphics
{
    public class DebugOverlay
    {
        private readonly Texture2D _pixel;

        public DebugOverlay(GraphicsDevice graphicsDevice)
        {
            _pixel = new Texture2D(graphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });
        }

        public void Draw(
            SpriteBatch spriteBatch,
            TheKnight knight,
            List<IObject> platforms,
            List<IEnemy> enemies,
            List<Spirit> items,
            ProjectileManager projectileManager,
            NavigationGrid navigationGrid,
            Camera camera)
        {
            // Navigation grid
            if (NavigationGrid.GridEnabled)
                navigationGrid.Draw(spriteBatch);

            // Enemy debug
            foreach (IEnemy enemy in enemies)
            {
                DebugRenderer.DrawBounds(spriteBatch, enemy, DebugRenderer.ColorEnemy);
                DrawRectangleOutline(spriteBatch, enemy.GetHurtbox(), Color.DarkRed);
                DebugRenderer.DrawStateLabel(spriteBatch, enemy, enemy.GetStateName(), DebugRenderer.ColorEnemy);

                if (enemy.GetDetectionRadius() > 0)
                {
                    Vector2 center = enemy.GetBounds()[0].Center.ToVector2();
                    DebugRenderer.DrawRadius(spriteBatch, center, enemy.GetDetectionRadius(), DebugRenderer.ColorTrigger * 0.8f);
                }
                
                if (enemy.GetChaseRadius() > 0)
                {
                    Vector2 center = enemy.GetBounds()[0].Center.ToVector2();
                    DebugRenderer.DrawRadius(spriteBatch, center, enemy.GetChaseRadius(), Color.Yellow * 0.5f);
                }
            }

            // Projectile debug
            foreach (var p in projectileManager.All)
                DrawRectangleOutline(spriteBatch, p.Bounds, Color.Red);

            // Item debug
            foreach (Spirit item in items)
            {
                if (item.IsActive)
                    DebugRenderer.DrawBounds(spriteBatch, item, DebugRenderer.ColorEnvironment);
            }

            // Knight debug
            DebugRenderer.DrawBounds(spriteBatch, knight, DebugRenderer.ColorKnight);
            DebugRenderer.DrawPoint(spriteBatch, knight.GetBounds()[0].Center.ToVector2(), DebugRenderer.ColorMidpoint);
            DrawRectangleOutline(spriteBatch, knight.Bounds, Color.LimeGreen);
            DrawRectangleOutline(spriteBatch, knight.GetHurtbox(), Color.DarkRed);
            DebugRenderer.DrawStateLabel(spriteBatch, knight, knight.GetStateName(), DebugRenderer.ColorKnight);

            string atkText = $"Attack CoolDown: {knight.GetAttackCooldownRemaining():F2}s";
            string invText = $"Invincibility CoolDown: {knight.GetInvincibilityCooldownRemaining():F2}s";
            string dashText = $"Dash CoolDown: {knight.GetDashCooldownRemaining():F2}s";
            string healText = $"Heal CoolDown: {knight.GetHealCooldownRemaining():F2}s";
            string castText = $"Cast CoolDown: {knight.GetCastCooldownRemaining():F2}s";
            string posText = $"Knight Position: ({knight.position.X:F0}, {knight.position.Y:F0})";
            string hpText  = $"Health: {knight.GetHealth()}";
            string soulText = $"Soul: {knight.Soul}";

            //hud for knight stats in debug mode
            Vector2 hudBasePos = camera.Position + new Vector2(10, 10);
            DebugRenderer.DrawText(spriteBatch, atkText, hudBasePos, Color.White);
            DebugRenderer.DrawText(spriteBatch, invText, hudBasePos + new Vector2(0, 20), Color.White);
            DebugRenderer.DrawText(spriteBatch, dashText, hudBasePos + new Vector2(0, 40), Color.White);
            DebugRenderer.DrawText(spriteBatch, healText, hudBasePos + new Vector2(0, 60), Color.White);
            DebugRenderer.DrawText(spriteBatch, castText, hudBasePos + new Vector2(0, 80), Color.White);
            DebugRenderer.DrawText(spriteBatch, posText, hudBasePos + new Vector2(0, 100), Color.White);
            DebugRenderer.DrawText(spriteBatch, hpText,  hudBasePos + new Vector2(0, 120), Color.White);
            DebugRenderer.DrawText(spriteBatch, soulText, hudBasePos + new Vector2(0, 140), Color.White);

            // Sword hitbox debug
            SwordHitbox swordHitbox = knight.GetSwordHitbox();
            if (swordHitbox != null)
                DebugRenderer.DrawBounds(spriteBatch, swordHitbox, DebugRenderer.ColorSword);

            // Platform hitbox and label debug
            foreach (IObject obj in platforms)
            {
                if (obj != null)
                {
                    DebugRenderer.DrawBounds(spriteBatch, obj, DebugRenderer.ColorEnvironment);

                    // Draw block label above the hitbox
                    Rectangle[] objBounds = obj.GetBounds();
                    if (objBounds.Length > 0)
                    {
                        DebugRenderer.DrawText(spriteBatch, obj.Label, new Vector2(objBounds[0].Left, objBounds[0].Top - 20), Color.White);
                    }
                }
            }

            // Enemy pathfinding debug
            foreach (IEnemy enemy in enemies)
            {
                if (enemy == null || !enemy.IsActive) continue;
                DebugRenderer.DrawPath(spriteBatch, enemy, enemy.GetCurrentPath(), Color.Yellow, Color.Red);
            }
        }

        private void DrawRectangleOutline(SpriteBatch spriteBatch, Rectangle rect, Color color, int thickness = 2)
        {
            if (!DebugRenderer.hitboxEnabled) return;
            spriteBatch.Draw(_pixel, new Rectangle(rect.Left, rect.Top, rect.Width, thickness), color);
            spriteBatch.Draw(_pixel, new Rectangle(rect.Left, rect.Bottom - thickness, rect.Width, thickness), color);
            spriteBatch.Draw(_pixel, new Rectangle(rect.Left, rect.Top, thickness, rect.Height), color);
            spriteBatch.Draw(_pixel, new Rectangle(rect.Right - thickness, rect.Top, thickness, rect.Height), color);
        }
    }
}
