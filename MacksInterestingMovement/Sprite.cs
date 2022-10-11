using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MacksInterestingMovement
{
    internal class Sprite : GameObject
    {
        public Texture2D texture;
        string textureName;
        Vector2 Origin;
        protected float Rotate;
        SpriteBatch spriteBatch;
        Rectangle locationRect;

        protected SpriteEffects currentEffect = SpriteEffects.None;
        public Sprite(Game1 game, string texturename) : base(game)
        {
            textureName = texturename;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
            texture = Game.Content.Load<Texture2D>(textureName);
            
            locationRect = new Rectangle(0, 0, texture.Width, texture.Height);
            this.Origin = new Vector2(texture.Width / 2, texture.Height / 2);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            locationRect = new Rectangle((int)pos.X, (int)pos.Y, texture.Width, texture.Height);
        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            spriteBatch.Draw(texture, pos, null, Color.White, MathHelper.ToRadians(Rotate), Origin, 1, currentEffect, 0);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        #region jeffsCollision
        public bool Intersects(Sprite OtherSprite)
        {
            return Sprite.Intersects(this.locationRect, OtherSprite.locationRect);
        }

        /// <summary>
        /// Checks if this sprites pixels intersect with another sprite
        /// This is more painfull than checking rectangles
        /// </summary>
        /// <param name="OtherSprite"></param>
        /// <returns></returns>
        public virtual bool PerPixelCollision(Sprite OtherSprite)
        {

            Color[] OtherSpriteColors;
            Color[] SpriteColors;

            //GraphicsDevice.Textures[0] = null;          //Bug 
            /*
             * Exception thrown
             * The operation was aborted. You may not modify a resource that has been set on a 
             * device, or after it has been used within a tiling bracket.
             */

            OtherSpriteColors = new Color[OtherSprite.texture.Width *
                OtherSprite.texture.Height];
            SpriteColors = new Color[this.texture.Width * this.texture.Height];

            this.texture.GetData<Color>(SpriteColors);

            OtherSprite.texture.GetData<Color>(OtherSpriteColors);

            return IntersectPixels(this.locationRect, SpriteColors,
                OtherSprite.locationRect, OtherSpriteColors);
        }

        /// <summary>
        /// Checks if this sprites pixels intersect with another sprite
        /// This is more painful than checking rectangles
        /// </summary>
        /// <param name="OtherSprite"></param>
        /// <returns></returns>

        /// <summary>
        /// Checks if two rectangles intersect
        /// </summary>
        /// <param name="a">Rectangle A</param>
        /// <param name="b">Rectangle B</param>
        /// <returns>bool</returns>
        public static bool Intersects(Rectangle a, Rectangle b)
        {
            // check if two Rectangles intersect
            return (a.Right > b.Left && a.Left < b.Right &&
                    a.Bottom > b.Top && a.Top < b.Bottom);
        }

        /// <summary>
        /// Checks if two rectanges intersect
        /// </summary>
        /// <param name="rectangle1"></param>
        /// <param name="rectangle2"></param>
        /// <returns>Rectangle of intersection of rectangle1 and rectangle2</returns>
        public static Rectangle Intersection(Rectangle rectangle1, Rectangle rectangle2)
        {
            int x1 = Math.Max(rectangle1.Left, rectangle2.Left);
            int y1 = Math.Max(rectangle1.Top, rectangle2.Top);
            int x2 = Math.Min(rectangle1.Right, rectangle2.Right);
            int y2 = Math.Min(rectangle1.Bottom, rectangle2.Bottom);

            if ((x2 >= x1) && (y2 >= y1))
            {
                return new Rectangle(x1, y1, x2 - x1, y2 - y1);
            }
            return Rectangle.Empty;
        }

        /// <summary>
        /// Determines if there is overlap of the non-transparent pixels
        /// between two sprites.
        /// </summary>
        /// <param name="rectangleA">Bounding rectangle of the first sprite</param>
        /// <param name="dataA">Pixel data of the first sprite</param>
        /// <param name="rectangleB">Bouding rectangle of the second sprite</param>
        /// <param name="dataB">Pixel data of the second sprite</param>
        /// <returns>True if non-transparent pixels overlap; false otherwise</returns>
        private static bool IntersectPixels(Rectangle rectangleA, Color[] dataA,
                                    Rectangle rectangleB, Color[] dataB)
        {
            // Find the bounds of the rectangle intersection
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            // Check every point within the intersection bounds
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {

                    // Get the color of both pixels at this point

                    Color colorA = dataA[(x - rectangleA.Left) +
                                         (y - rectangleA.Top) * rectangleA.Width];
                    Color colorB = dataB[(x - rectangleB.Left) +
                                         (y - rectangleB.Top) * rectangleB.Width];

                    // If both pixels are not completely transparent,
                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        // then an intersection has been found
                        return true;
                    }
                }
            }

            // No intersection found
            return false;
        }

        /// <summary>
        /// Determines if there is overlap of the non-transparent pixels between two
        /// sprites.
        /// </summary>
        /// <param name="transformA">World transform of the first sprite.</param>
        /// <param name="widthA">Width of the first sprite's texture.</param>
        /// <param name="heightA">Height of the first sprite's texture.</param>
        /// <param name="dataA">Pixel color data of the first sprite.</param>
        /// <param name="transformB">World transform of the second sprite.</param>
        /// <param name="widthB">Width of the second sprite's texture.</param>
        /// <param name="heightB">Height of the second sprite's texture.</param>
        /// <param name="dataB">Pixel color data of the second sprite.</param>
        /// <returns>True if non-transparent pixels overlap; false otherwise</returns>
        private static bool IntersectPixels(
                            Matrix transformA, int widthA, int heightA, Color[] dataA,
                            Matrix transformB, int widthB, int heightB, Color[] dataB)
        {
            // Calculate a matrix which transforms from A's local space into
            // world space and then into B's local space
            Matrix transformAToB = transformA * Matrix.Invert(transformB);

            // When a point moves in A's local space, it moves in B's local space with a
            // fixed direction and distance proportional to the movement in A.
            // This algorithm steps through A one pixel at a time along A's X and Y axes
            // Calculate the analogous steps in B:
            Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
            Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

            // Calculate the top left corner of A in B's local space
            // This variable will be reused to keep track of the start of each row
            Vector2 yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);

            // For each row of pixels in A
            for (int yA = 0; yA < heightA; yA++)
            {
                // Start at the beginning of the row
                Vector2 posInB = yPosInB;

                // For each pixel in this row
                for (int xA = 0; xA < widthA; xA++)
                {
                    // Round to the nearest pixel
                    int xB = (int)Math.Round(posInB.X);
                    int yB = (int)Math.Round(posInB.Y);

                    // If the pixel lies within the bounds of B
                    if (0 <= xB && xB < widthB &&
                        0 <= yB && yB < heightB)
                    {
                        try
                        {

                            // Get the colors of the overlapping pixels
                            Color colorA = dataA[xA + yA * widthA];
                            Color colorB = dataB[xB + yB * widthB];
                            // If both pixels are not completely transparent,
                            if (colorA.A != 0 && colorB.A != 0)
                            {
                                // then an intersection has been found
                                return true;
                            }
                        }
                        catch
                        {
                            //HUH?
                            //throw ex;
                            return false;
                        }

                    }

                    // Move to the next pixel in the row
                    posInB += stepX;
                }

                // Move to the next row
                yPosInB += stepY;
            }

            // No intersection found
            return false;
        }

        #endregion
    }
}
