using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MacksInterestingMovement
{
    internal class fallingObjectManager : GameComponent
    {
        Game1 game1Ref;
        GameComponentCollection gameComponents;
        int NumOfObjects = 8;
        Queue<FallingObject> fallingObjects;
        Random random = new Random();
        ATimer spawnTimer;
        OneButtonPlayer Player;

        public fallingObjectManager(Game game, GameComponentCollection components, Game1 game1, OneButtonPlayer player) : base(game)
        {
            gameComponents = components;
            game1Ref = game1;
            Player = player;
            spawnTimer = new ATimer(game1Ref, 2, 5);
            spawnTimer.onReset += SpawnObject;
            gameComponents.Add(spawnTimer);
        }

        public override void Initialize()
        {
            fallingObjects = createObjects(NumOfObjects);
            base.Initialize();
            Player.onDeath += resetObjs;
        }
        public Queue<FallingObject> createObjects(int size)
        {
            Queue<FallingObject> objs = new Queue<FallingObject>();
            for(int i = 0; i < size; i++)
            {
                objs.Enqueue(new FallingObject(game1Ref, "MushroomGuyPurple", Player));
            }
            return objs;
        }

        private void resetObjs()
        {
            foreach(FallingObject f in fallingObjects)
            {
                f.pos = new Vector2(-20, -20);
                if(f.isActive == true)
                {
                    f.isActive = false;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        private void SpawnObject()
        {
            FallingObject objToSpawn = fallingObjects.Dequeue();
            fallingObjects.Enqueue(objToSpawn);
            objToSpawn.pos = getRandomPos();
            objToSpawn.isActive = true;
        }
        
        private Vector2 getRandomPos()
        {
            return new Vector2(random.Next(game1Ref.LeftBound +4, game1Ref.RightBound+4), random.Next(-100,-20));
        }
        
        //implement object pooling for falling objects
        //create a timer for score
        //decide how collision checking works

    }
}
