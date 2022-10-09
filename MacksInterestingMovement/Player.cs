using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacksInterestingMovement
{
    internal class Player : Sprite
    {
        float GravityAccel = 300f;
        float fallMultiplier = 2.8f;
        float lowJumpMultiplier = 2.5f;
        Vector2 GravityDir = new Vector2(0, 1);
        float maxSpeed = 300;
        float accel = 50;
        float friction = 12;
        float airFriction = 16;
        float jumpForce = 300;
        float jumpHeight = 40;
        float landMoveBuffer = 10;
        float leftGroundPos;
        public Rectangle rect;

        InputHandler input;
        Vector2 gamePadStick;

        private enum horizontalState {idle, moving};
        horizontalState currentHoriState;
        private enum verticalState { ascending, descending, grounded};
        verticalState currentVertState;

        delegate void hMovement();
        hMovement horizontalMovement;

        int playerNum;

        public Player(Game1 game, int playerNumber, string spriteName) : base(game, spriteName)
        {
            pos = new Microsoft.Xna.Framework.Vector2(2, 2);
            input = new InputHandler();
            if (input.isUsingGamepad())
            {
                horizontalMovement = horizontalGamepadMovement;
            }
            else
            {
                horizontalMovement = horizontalKeyboardMovement;
            }
            playerNum = playerNumber;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            PlayerUpdate(this.Game.TIME);
            base.Update(gameTime);
        }
        public void PlayerUpdate(float time)
        {
            UpdatePos(time);
            input.Update();
            movement();
        }

        private void UpdatePos(float time)
        {
            if (isOnGround() && currentVertState == verticalState.grounded)
            {
                pos.Y = 400;
                dir.Y = 0;
            }
            pos = pos + (dir * time / 1000);
            dir.X = dir.X + (GravityDir.X * GravityAccel) * (time / 1000);
            if(currentVertState == verticalState.descending)
            {
                dir.Y = dir.Y + (GravityDir.Y * GravityAccel) * fallMultiplier * lowJumpMultiplier * (time / 1000);
            }
            else if(dir.Y > 0f)
            {
                dir.Y = dir.Y + (GravityDir.Y * GravityAccel) * fallMultiplier * (time / 1000);
            }
            else
            {
                dir.Y = dir.Y + (GravityDir.Y * GravityAccel) * (time / 1000);
            }

        }

        private void horizontalGamepadMovement()
        {
            gamePadStick = input.thumbstick(playerNum);
            Debug.WriteLine(gamePadStick);
            if(gamePadStick.X < -.1f)
            {
                dir.X = Math.Max((maxSpeed * -1.0f), dir.X - accel);
                currentEffect = SpriteEffects.None;
            }
            if(gamePadStick.X > .1f)
            {
                dir.X = Math.Min(maxSpeed, dir.X + accel);
                currentEffect = SpriteEffects.FlipHorizontally;
            }
            if(gamePadStick.X == 0)
            {
                addFriction();
            }
        }

        private void addFriction()
        {
            switch (currentVertState)
            {
                case verticalState.ascending:  
                case verticalState.descending:
                    if (dir.X > 0)
                    {
                        dir.X = Math.Max(0, dir.X - airFriction);
                    }
                    else
                    {
                        dir.X = Math.Min(0, dir.X + airFriction);
                    }
                    break;
                case verticalState.grounded:
                    if (dir.X > 0)
                    {
                        dir.X = Math.Max(0, dir.X - friction);
                    }
                    else
                    {
                        dir.X = Math.Min(0, dir.X + friction);
                    }
                    break;

            }
        }


        private void horizontalKeyboardMovement()
        {
            
            if(currentVertState != verticalState.ascending)
            {
                if (input.IsKeyPressed(input.inputKeys["Left"][playerNum]))
                {
                    dir.X = Math.Max((maxSpeed * -1.0f), dir.X - accel);
                    currentEffect = SpriteEffects.None;
                }
                if (input.IsKeyPressed(input.inputKeys["Right"][playerNum]))
                {
                    dir.X = Math.Min(maxSpeed, dir.X + accel);
                    currentEffect = SpriteEffects.FlipHorizontally;
                }
            }
            if ((!input.IsKeyPressed(input.inputKeys["Right"][playerNum]) && dir.X > 0) || (!input.IsKeyPressed(input.inputKeys["Left"][playerNum]) && dir.X < 0))
            {
                addFriction();
            }
            
        }

        private void verticalMovement()
        {
            if ((input.IsKeyPressed(input.inputKeys["Up"][playerNum]) || input.IsButtonPressed(input.inputButtons["Up"][playerNum])) && currentVertState == verticalState.grounded)
            {
                if (currentVertState != verticalState.ascending)
                {
                    pos.Y--;
                    currentVertState = verticalState.ascending;
                    dir.Y = Math.Min(50, (dir.Y - jumpForce));
                    //Debug.WriteLine(currentVertState);

                }
            }
            else if (input.ReleasedKey(input.inputKeys["Up"][playerNum]))
            {
                if(currentVertState != verticalState.descending)
                {
                    currentVertState = verticalState.descending;
                }
            }
            Debug.WriteLine(currentVertState);
            //TRYING to make jump go higher if button is held and add in a landMoveBuffer
            //so the as the player approaches the ground they can adjust x position slightly
            /*if (input.IsHoldingKey(input.inputKeys["Up"][playerNum]))
            {

                if (currentVertState != verticalState.descending)
                {
                    if (currentVertState != verticalState.ascending)
                    {
                        leftGroundPos = pos.Y;
                        pos.Y--;
                        currentVertState = verticalState.ascending;
                    }
                    dir.Y = Math.Min(10, (dir.Y - jumpForce));

                    if (pos.Y <= (leftGroundPos - jumpHeight) || input.ReleasedKey(input.inputKeys["Up"][playerNum]))
                    {
                        if (currentVertState != verticalState.descending)
                        {
                            currentVertState = verticalState.descending;
                        }
                    }
                }
                Debug.WriteLine(currentVertState);
            }*/
        }

        private void movement()
        {
            horizontalMovement();
            verticalMovement();
        }

        private bool isOnGround()
        {
            //change this to check if bottom of sprite rectangle is colliding with another sprite rectangle
            if(pos.Y < 400)
            {
                return false;
            }
            else
            {
                currentVertState = verticalState.grounded;
                return true;
            }
        }
    }
}
