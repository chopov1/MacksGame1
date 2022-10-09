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
        protected SpriteEffects currentEffect = SpriteEffects.None;
        public Sprite(Game1 game, string texturename) : base(game)
        {
            textureName = texturename;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
            texture = Game.Content.Load<Texture2D>(textureName);
            this.Origin = new Vector2(texture.Width / 2, texture.Height / 2);
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            spriteBatch.Draw(texture, pos, null, Color.White, MathHelper.ToRadians(Rotate), Origin, 2, currentEffect, 0);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
