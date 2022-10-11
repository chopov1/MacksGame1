using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacksInterestingMovement
{
    internal class ATimer : GameComponent
    {
        public delegate void TimerResetEventHandler();
        public event TimerResetEventHandler onReset;

        //re-learn how to setup callbacks in c#
        float currentTime;
        float timerMax;
        int rMin;
        int rMax;
        Random random = new Random();
        public ATimer(Game game, int rangeMin, int rangeMax) : base(game)
        {
            this.timerMax = rangeMax;
            rMin = rangeMin * 10000;
            rMax = rangeMax * 10000;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            currentTime += (gameTime.TotalGameTime.Milliseconds);
            if(currentTime > timerMax)
            {
                currentTime = 0;
                timerMax = random.Next(rMin, rMax);
                for(int i =0; i < random.Next(1, 4); i++)
                {
                    onReset();
                }
            }
        }

        

    }
}
