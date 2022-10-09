using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MacksInterestingMovement
{
    internal class GameObject : DrawableGameComponent
    {
        public Vector2 pos, dir;
        protected Game1 Game;

        public GameObject(Game1 game) : base(game)
        {
            this.Game = game;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
