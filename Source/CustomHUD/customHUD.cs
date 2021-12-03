using System;
using HarmonyLib;
using StardewModdingAPI;
using StardewValley;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using StardewValley.Locations;
using StardewValley.Menus;
using System.Collections.Generic;
using SDObject = StardewValley.Object;

namespace CustomHUD
{
    public class customHUD : Mod
    {
        public override void Entry(IModHelper helper)
        {

            Harmony oHarmony = new Harmony(helper.ModRegistry.ModID);

            oHarmony.Patch(
           original: AccessTools.Method(typeof(Game1), "drawHUD"),
           prefix: new HarmonyMethod(typeof(customHUD), nameof(customHUD.drawHUD_prefix))
        );
        }

        private static bool drawHUD_prefix()
        {
            //
            //  this is a straight clone of the Game1 code
            //  updated to be used outside of the Game1 class
            //
            if (Game1.eventUp ||Game1.farmEvent != null)
            {
                return false;
            }
            float modifier = 0.625f;
            Vector2 topOfBar = new Vector2(Game1.graphics.GraphicsDevice.Viewport.GetTitleSafeArea().Right - 48 - 8,Game1.graphics.GraphicsDevice.Viewport.GetTitleSafeArea().Bottom - 224 - 16 - (int)((float)(Game1.player.MaxStamina - 270) * modifier));
            if (Game1.isOutdoorMapSmallerThanViewport())
            {
                topOfBar.X = Math.Min(topOfBar.X, -Game1.viewport.X + Game1.currentLocation.map.Layers[0].LayerWidth * 64 - 48);
            }
            if (Game1.staminaShakeTimer > 0)
            {
                topOfBar.X += Game1.random.Next(-3, 4);
                topOfBar.Y += Game1.random.Next(-3, 4);
            }
            Color c = Utility.getRedToGreenLerpColor(Game1.player.stamina / (float)(int)Game1.player.maxStamina);
            //
            // draw energy bar
            if (true)
            {
                Game1.spriteBatch.Draw(Game1.mouseCursors, topOfBar, new Microsoft.Xna.Framework.Rectangle(256, 408, 12, 16), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
                Game1.spriteBatch.Draw(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle((int)topOfBar.X, (int)(topOfBar.Y + 64f), 48, Game1.graphics.GraphicsDevice.Viewport.GetTitleSafeArea().Bottom - 64 - 16 - (int)(topOfBar.Y + 64f - 8f)), new Microsoft.Xna.Framework.Rectangle(256, 424, 12, 16), Color.White);
                Game1.spriteBatch.Draw(Game1.mouseCursors, new Vector2(topOfBar.X, topOfBar.Y + 224f + (float)(int)((float)(Game1.player.MaxStamina - 270) * modifier) - 64f), new Microsoft.Xna.Framework.Rectangle(256, 448, 12, 16), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
                Microsoft.Xna.Framework.Rectangle r = new Microsoft.Xna.Framework.Rectangle((int)topOfBar.X + 12, (int)topOfBar.Y + 16 + 32 + (int)((float)(Game1.player.MaxStamina - (int)Math.Max(0f, Game1.player.Stamina)) * modifier), 24, (int)(Game1.player.Stamina * modifier));
                if ((float)Game1.getOldMouseX() >= topOfBar.X && (float)Game1.getOldMouseY() >= topOfBar.Y)
                {
                    Game1.drawWithBorder((int)Math.Max(0f, Game1.player.Stamina) + "/" + Game1.player.MaxStamina, Color.Black * 0f, Color.White, topOfBar + new Vector2(0f - Game1.dialogueFont.MeasureString("999/999").X - 16f - (float)(Game1.showingHealth ? 64 : 0), 64f));
                }
                Game1.spriteBatch.Draw(Game1.staminaRect, r, Color.AliceBlue);
                r.Height = 4;
                c.R = (byte)Math.Max(0, c.R - 50);
                c.G = (byte)Math.Max(0, c.G - 50);
                Game1.spriteBatch.Draw(Game1.staminaRect, r, c);
                if ((bool)Game1.player.exhausted)
                {
                    Game1.spriteBatch.Draw(Game1.mouseCursors, topOfBar - new Vector2(0f, 11f) * 4f, new Microsoft.Xna.Framework.Rectangle(191, 406, 12, 11), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
                    if ((float)Game1.getOldMouseX() >= topOfBar.X && (float)Game1.getOldMouseY() >= topOfBar.Y - 44f)
                    {
                        Game1.drawWithBorder(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3747"), Color.Black * 0f, Color.White, topOfBar + new Vector2(0f - Game1.dialogueFont.MeasureString(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3747")).X - 16f - (float)(Game1.showingHealth ? 64 : 0), 96f));
                    }
                }
            }
            //
            //	draw health bar
            //
            if (Game1.currentLocation is MineShaft || Game1.currentLocation is Woods || Game1.currentLocation is SlimeHutch || Game1.currentLocation is VolcanoDungeon || Game1.player.health < Game1.player.maxHealth)
            {
                Game1.showingHealthBar = true;
                Game1.showingHealth = true;
                int bar_full_height = 168 + (Game1.player.maxHealth - 100);
                int height = (int)((float)Game1.player.health / (float)Game1.player.maxHealth * (float)bar_full_height);
                topOfBar.X -= 56 + ((Game1.hitShakeTimer > 0) ? Game1.random.Next(-3, 4) : 0);
                topOfBar.Y = Game1.graphics.GraphicsDevice.Viewport.GetTitleSafeArea().Bottom - 224 - 16 - (Game1.player.maxHealth - 100);
                Game1.spriteBatch.Draw(Game1.mouseCursors, topOfBar, new Microsoft.Xna.Framework.Rectangle(268, 408, 12, 16), (Game1.player.health < 20) ? (Color.Pink * ((float)Math.Sin(Game1.currentGameTime.TotalGameTime.TotalMilliseconds / (double)((float)Game1.player.health * 50f)) / 4f + 0.9f)) : Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
                Game1.spriteBatch.Draw(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle((int)topOfBar.X, (int)(topOfBar.Y + 64f), 48, Game1.graphics.GraphicsDevice.Viewport.GetTitleSafeArea().Bottom - 64 - 16 - (int)(topOfBar.Y + 64f)), new Microsoft.Xna.Framework.Rectangle(268, 424, 12, 16), (Game1.player.health < 20) ? (Color.Pink * ((float)Math.Sin(Game1.currentGameTime.TotalGameTime.TotalMilliseconds / (double)((float)Game1.player.health * 50f)) / 4f + 0.9f)) : Color.White);
                Game1.spriteBatch.Draw(Game1.mouseCursors, new Vector2(topOfBar.X, topOfBar.Y + 224f + (float)(Game1.player.maxHealth - 100) - 64f), new Microsoft.Xna.Framework.Rectangle(268, 448, 12, 16), (Game1.player.health < 20) ? (Color.Pink * ((float)Math.Sin(Game1.currentGameTime.TotalGameTime.TotalMilliseconds / (double)((float)Game1.player.health * 50f)) / 4f + 0.9f)) : Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
                Microsoft.Xna.Framework.Rectangle health_bar_rect = new Microsoft.Xna.Framework.Rectangle((int)topOfBar.X + 12, (int)topOfBar.Y + 16 + 32 + bar_full_height - height, 24, height);
                c = Utility.getRedToGreenLerpColor((float)Game1.player.health / (float)Game1.player.maxHealth);
                Game1.spriteBatch.Draw(Game1.staminaRect, health_bar_rect, Game1.staminaRect.Bounds, c, 0f, Vector2.Zero, SpriteEffects.None, 1f);
                c.R = (byte)Math.Max(0, c.R - 50);
                c.G = (byte)Math.Max(0, c.G - 50);
                if ((float)Game1.getOldMouseX() >= topOfBar.X && (float)Game1.getOldMouseY() >= topOfBar.Y && (float)Game1.getOldMouseX() < topOfBar.X + 32f)
                {
                    Game1.drawWithBorder(Math.Max(0, Game1.player.health) + "/" + Game1.player.maxHealth, Color.Black * 0f, Color.Red, topOfBar + new Vector2(0f - Game1.dialogueFont.MeasureString("999/999").X - 32f, 64f));
                }
                health_bar_rect.Height = 4;
                Game1.spriteBatch.Draw(Game1.staminaRect, health_bar_rect, Game1.staminaRect.Bounds, c, 0f, Vector2.Zero, SpriteEffects.None, 1f);
            }
            else
            {
                Game1.showingHealth = false;
            }
            _ = Game1.player.ActiveObject;
            foreach (IClickableMenu menu in Game1.onScreenMenus)
            {
                if (menu != Game1.chatBox)
                {
                    menu.update(Game1.currentGameTime);
                    menu.draw(Game1.spriteBatch);
                }
            }
            if (!Game1.player.professions.Contains(17) || !Game1.currentLocation.IsOutdoors)
            {
                return false;
            }


            foreach (KeyValuePair<Vector2, SDObject> v in Game1.currentLocation.objects.Pairs)
            {
                if (((bool)v.Value.isSpawnedObject || v.Value.ParentSheetIndex == 590) && !Utility.isOnScreen(v.Key * 64f + new Vector2(32f, 32f), 64))
                {
                    Microsoft.Xna.Framework.Rectangle vpbounds = Game1.graphics.GraphicsDevice.Viewport.Bounds;
                    Vector2 onScreenPosition2 = default(Vector2);
                    float rotation2 = 0f;
                    if (v.Key.X * 64f > (float)(Game1.viewport.MaxCorner.X - 64))
                    {
                        onScreenPosition2.X = vpbounds.Right - 8;
                        rotation2 = (float)Math.PI / 2f;
                    }
                    else if (v.Key.X * 64f < (float)Game1.viewport.X)
                    {
                        onScreenPosition2.X = 8f;
                        rotation2 = -(float)Math.PI / 2f;
                    }
                    else
                    {
                        onScreenPosition2.X = v.Key.X * 64f - (float)Game1.viewport.X;
                    }
                    if (v.Key.Y * 64f > (float)(Game1.viewport.MaxCorner.Y - 64))
                    {
                        onScreenPosition2.Y = vpbounds.Bottom - 8;
                        rotation2 = (float)Math.PI;
                    }
                    else if (v.Key.Y * 64f < (float)Game1.viewport.Y)
                    {
                        onScreenPosition2.Y = 8f;
                    }
                    else
                    {
                        onScreenPosition2.Y = v.Key.Y * 64f - (float)Game1.viewport.Y;
                    }
                    if (onScreenPosition2.X == 8f && onScreenPosition2.Y == 8f)
                    {
                        rotation2 += (float)Math.PI / 4f;
                    }
                    if (onScreenPosition2.X == 8f && onScreenPosition2.Y == (float)(vpbounds.Bottom - 8))
                    {
                        rotation2 += (float)Math.PI / 4f;
                    }
                    if (onScreenPosition2.X == (float)(vpbounds.Right - 8) && onScreenPosition2.Y == 8f)
                    {
                        rotation2 -= (float)Math.PI / 4f;
                    }
                    if (onScreenPosition2.X == (float)(vpbounds.Right - 8) && onScreenPosition2.Y == (float)(vpbounds.Bottom - 8))
                    {
                        rotation2 -= (float)Math.PI / 4f;
                    }
                    Microsoft.Xna.Framework.Rectangle srcRect = new Microsoft.Xna.Framework.Rectangle(412, 495, 5, 4);
                    float renderScale = 4f;
                    Vector2 safePos = Utility.makeSafe(renderSize: new Vector2((float)srcRect.Width * renderScale, (float)srcRect.Height * renderScale), renderPos: onScreenPosition2);
                    Game1.spriteBatch.Draw(Game1.mouseCursors, safePos, srcRect, Color.White, rotation2, new Vector2(2f, 2f), renderScale, SpriteEffects.None, 1f);
                }
            }
            if (!Game1.currentLocation.orePanPoint.Equals(Point.Zero) && !Utility.isOnScreen(Utility.PointToVector2(Game1.currentLocation.orePanPoint) * 64f + new Vector2(32f, 32f), 64))
            {
                Vector2 onScreenPosition = default(Vector2);
                float rotation = 0f;
                if (Game1.currentLocation.orePanPoint.X * 64 > Game1.viewport.MaxCorner.X - 64)
                {
                    onScreenPosition.X = Game1.graphics.GraphicsDevice.Viewport.Bounds.Right - 8;
                    rotation = (float)Math.PI / 2f;
                }
                else if (Game1.currentLocation.orePanPoint.X * 64 < Game1.viewport.X)
                {
                    onScreenPosition.X = 8f;
                    rotation = -(float)Math.PI / 2f;
                }
                else
                {
                    onScreenPosition.X = Game1.currentLocation.orePanPoint.X * 64 - Game1.viewport.X;
                }
                if (Game1.currentLocation.orePanPoint.Y * 64 > Game1.viewport.MaxCorner.Y - 64)
                {
                    onScreenPosition.Y = Game1.graphics.GraphicsDevice.Viewport.Bounds.Bottom - 8;
                    rotation = (float)Math.PI;
                }
                else if (Game1.currentLocation.orePanPoint.Y * 64 < Game1.viewport.Y)
                {
                    onScreenPosition.Y = 8f;
                }
                else
                {
                    onScreenPosition.Y = Game1.currentLocation.orePanPoint.Y * 64 - Game1.viewport.Y;
                }
                if (onScreenPosition.X == 8f && onScreenPosition.Y == 8f)
                {
                    rotation += (float)Math.PI / 4f;
                }
                if (onScreenPosition.X == 8f && onScreenPosition.Y == (float)(Game1.graphics.GraphicsDevice.Viewport.Bounds.Bottom - 8))
                {
                    rotation += (float)Math.PI / 4f;
                }
                if (onScreenPosition.X == (float)(Game1.graphics.GraphicsDevice.Viewport.Bounds.Right - 8) && onScreenPosition.Y == 8f)
                {
                    rotation -= (float)Math.PI / 4f;
                }
                if (onScreenPosition.X == (float)(Game1.graphics.GraphicsDevice.Viewport.Bounds.Right - 8) && onScreenPosition.Y == (float)(Game1.graphics.GraphicsDevice.Viewport.Bounds.Bottom - 8))
                {
                    rotation -= (float)Math.PI / 4f;
                }
                Game1.spriteBatch.Draw(Game1.mouseCursors, onScreenPosition, new Microsoft.Xna.Framework.Rectangle(412, 495, 5, 4), Color.Cyan, rotation, new Vector2(2f, 2f), 4f, SpriteEffects.None, 1f);
            }

            return false;
        }
    }
}
