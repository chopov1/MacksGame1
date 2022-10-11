using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacksInterestingMovement
{
    internal class FallingObject : Sprite
    {
        float maxSpeed = 300;
        float accel = 50;
        Vector2 GravityDir = new Vector2(0, 1);

        public bool isActive;

        OneButtonPlayer Player;

        public FallingObject(Game1 game, string texturename, OneButtonPlayer player) : base(game, texturename)
        {
            //make falling obj add itself as a component
            game.Components.Add(this);
            isActive = false;
            dir.Y = 1;
            Player = player;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (isActive)
            {
                Fall();
                UpdatePos(gameTime.ElapsedGameTime.Milliseconds);
            }
            checkPlayerCollision();
        }

        void checkPlayerCollision()
        {
            if (Intersects(Player))
            {
                if (PerPixelCollision(Player))
                {
                    //Player.currentStatus = OneButtonPlayer.PlayerStatus.dead;
                    Player.Death();
                }
            }
        }

        private void UpdatePos(float time)
        {
            pos = pos + (dir * time / 1000);
            dir.Y = dir.Y + (GravityDir.X * accel) * (time / 1000);
        }

        public void Fall()
        {
            dir.Y = Math.Max((maxSpeed), dir.Y - accel);
        }
    }
}
