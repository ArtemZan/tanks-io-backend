using System.Diagnostics;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TanksIO.Game
{
    using Sockets;

    class Game
    {
        private bool _gameRuns = false;
        private readonly Room _room = null;
        public Scene Scene;


        /// <summary>
        /// Creates a new game (but doesn't start it. Call Start())
        /// </summary>
        /// <param name="room">A room where the updates will be dispatched</param>
        public Game(Room room)
        {
            Scene = new Maps.Map1();
            _room = room;
        }

        ~Game()
        {
            Stop();
        }

        public void Start()
        {
            if (_gameRuns)
            {
                Console.WriteLine("Warning: trying to start a game that already runs");
                return;
            }

            new Thread(GameLoop).Start();
            _gameRuns = true;
        }

        /// <summary> 
        /// Stops the game. Maximum one update might happen after calling this method
        /// </summary>
        public void Stop()
        {
            _gameRuns = false;
        }

        private void GameLoop()
        {
            const int fps = 100;
            const int mspf = 1000 / fps;

            Stopwatch timer = new();
            double DeltaTime = 1;

            while (_gameRuns)
            {
                timer.Restart();

                Thread timerThread = new(() => { Thread.Sleep(mspf); });
                timerThread.Start();

                Scene.OnUpdate(DeltaTime);


                Update update = Scene.GetUpdate();

                lock (update.Obj)
                {
                    if (update.Obj.Count != 0)
                    {
                        _room.SendUpdate(update);
                        Scene.ClearUpdate();
                    }
                }

                update.Player = new();

                timerThread.Join();

                timer.Stop();

                DeltaTime = timer.ElapsedMilliseconds * 1e-3;
            }
        }
    }
}