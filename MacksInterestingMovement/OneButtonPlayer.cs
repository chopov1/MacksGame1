using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text.Encodings.Web;
using System.ComponentModel;

namespace MacksInterestingMovement
{
    internal class OneButtonPlayer : Sprite
    {
        //learn how to put player and falling objects on their own layer, so there
        //can be a background that wont interfere with collision checking.
        
        Vector2 GravityDir = new Vector2(0, 1);
        float maxSpeed = 300;
        float accel = 50;
        float friction = 12;
        public Rectangle rect;
        float speed = 5;
        InputHandler inputHandler;
        int dirFlip = 1;

        Game1 game1Ref;
        enum Direction { right, left};

        StringSprite scoreSprite;
        StringSprite highScoreSprite;
        StringSprite howToPlay;

        public enum PlayerStatus { alive, dead};
        public PlayerStatus currentStatus = PlayerStatus.alive;

        public int Score;
        public int HighScore;

        Direction currentDirection = Direction.right;

        public delegate void PlayerEventHandler();
        public event PlayerEventHandler onDeath;
        public OneButtonPlayer(Game1 game, string texturename) : base(game, texturename)
        {
            game1Ref = game;
            inputHandler = new InputHandler();
            pos = new Vector2(40, 400);
            dir = new Vector2(1, 0);
            currentEffect = SpriteEffects.FlipHorizontally;
            scoreSprite = new StringSprite(game, "ScoreFont", new Vector2(20,40));
            highScoreSprite = new StringSprite(game, "ScoreFont", new Vector2(20, 20));
            howToPlay = new StringSprite(game, "ScoreFont", new Vector2(20, 60));
            howToPlay.StringToDraw = "Hold SPACE To Stop Moving";
            scoreSprite.StringToDraw = "Score: " + Score;
            highScoreSprite.StringToDraw = "HighScore: " + HighScore;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            inputHandler.Update();
            UpdatePlayer(gameTime.ElapsedGameTime.Milliseconds);
        }

        private void UpdatePlayer(float Time)
        {

            Move();
            UpdatePos(Time);
        }

        public void Death()
        {
            Score = 0;
            maxSpeed = 300;
            pos = new Vector2(40, 400);
            updateScore();
            onDeath();
        }
        private void updateScore()
        {
            maxSpeed += (Score*2);
            if (Score >= HighScore)
            {
                HighScore = Score;
            }
            scoreSprite.StringToDraw = "Score: " + Score;
            highScoreSprite.StringToDraw = "HighScore: " + HighScore;
            if(Score >=4)
            {
                howToPlay.StringToDraw = "";
            }
            else
            {
                howToPlay.StringToDraw = "Hold SPACE To Stop Moving";
            }
        }
        private void UpdatePos(float time)
        {
            pos = pos + (dir * time / 1000);
            dir.X = dir.X + (GravityDir.X * accel) * (time / 1000);
        }

        private void Move()
        {
                if (!inputHandler.IsKeyPressed(Keys.Space))
                {
                    dir.X = Math.Max((maxSpeed * dirFlip), dir.X - accel);
                    
                }
            if ((inputHandler.IsKeyPressed(Keys.Space)))
            {
                addFriction();
            }
            if(pos.X >= game1Ref.RightBound && currentDirection == Direction.right)
            {
                dirFlip *= -1;
                currentDirection = Direction.left;
                currentEffect = SpriteEffects.None;
                Score++;
                updateScore();

            }
            if(pos.X <= game1Ref.LeftBound && currentDirection == Direction.left)
            {
                dirFlip *= -1;
                currentDirection = Direction.right;
                currentEffect = SpriteEffects.FlipHorizontally;
                Score++;
                updateScore();
            }

        }

        public void addFriction()
        {
            if (dir.X > 0)
            {
                dir.X = Math.Max(0, dir.X - friction);
            }
            else
            {
                dir.X = Math.Min(0, dir.X + friction);
            }
        }

    }
}
