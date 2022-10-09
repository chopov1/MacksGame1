using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MacksInterestingMovement
{
    

    internal class InputHandler
    {
        private KeyboardState KeyboardState;
        KeyboardState prevKeyboardState;
        GamePadState GamePadState;
        GamePadState prevGamePadState;

        //figure out how to make key names reassignable for each player so that in the player class
        //right can mean rightarrow if its p1 or d if its p2 etc..
        public Dictionary<string, Keys[]> inputKeys;
        public Dictionary<string, Buttons[]> inputButtons;

        public InputHandler()
        {
            setupKeyDic();
            setupButtonDic();
        }
        private void setupButtonDic()
        {
            inputButtons = new Dictionary<string, Buttons[]>();
            inputButtons.Add("Up", new Buttons[] { Buttons.A, Buttons.X });
        }
        private void setupKeyDic()
        {
            inputKeys = new Dictionary<string, Keys[]>();
            inputKeys.Add("Right", new Keys[] { Keys.Right, Keys.D });
            inputKeys.Add("Left", new Keys[] { Keys.Left, Keys.A });
            inputKeys.Add("Up", new Keys[] { Keys.Up, Keys.W });
        }
        public void Update()
        {
            prevKeyboardState = KeyboardState;
            prevGamePadState = GamePadState;
            KeyboardState = Keyboard.GetState();
            GamePadState = GamePad.GetState(PlayerIndex.One);
        }

        public bool isUsingGamepad()
        {
            return GamePad.GetState(PlayerIndex.One).IsConnected;
        }

        public Vector2 thumbstick(int player)
        {
            GamePadThumbSticks sticks = GamePadState.ThumbSticks;
            if(player == 1)
            {
                return sticks.Left;
            }
            else
            {
                return sticks.Right;
            }
            
        }


        public bool IsButtonPressed(Buttons button)
        {
            return GamePadState.IsButtonDown(button);
        }

        public bool IsHoldingButton(Buttons button)
        {
            if (prevGamePadState.IsButtonDown(button) && GamePadState.IsButtonDown(button))
            {
                return true;
            }
            return false;
        }

        public bool ReleasedButton(Buttons button)
        {
            if (prevGamePadState.IsButtonDown(button) && !GamePadState.IsButtonDown(button))
            {
                return true;
            }
            return false;
        }

        public bool IsKeyPressed(Keys key)
        {
            return KeyboardState.IsKeyDown(key);
        }

        public bool IsHoldingKey(Keys key)
        {
            if(prevKeyboardState.IsKeyDown(key) && KeyboardState.IsKeyDown(key))
            {
                return true;
            }
            return false;
        }

        public bool ReleasedKey(Keys key)
        {
            if(prevKeyboardState.IsKeyDown(key) && !KeyboardState.IsKeyDown(key))
            {
                return true;
            }
            return false;
        }

        public bool IsPressingAnyKey()
        {
            if(KeyboardState.GetPressedKeyCount() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
