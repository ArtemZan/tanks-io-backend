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
        private Update _update = new();
        private static Mutex mutex = new Mutex();
        public Dictionary<string, Player> Players = new();


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

        public void UpdateObject(ObjectUpdate update)
        {
            _update.Obj.Add(update);
        }

        public void UpdateAll()
        {
            _update.Obj = Scene.GameObjects.Select(pair => new ObjectUpdate(pair.Key, new VerticesUpdatePayload(pair.Value.GetVertices()))).ToList();
            _update.Obj.AddRange(Players.Select(pair => new ObjectUpdate(pair.Key, new VerticesUpdatePayload(pair.Value.Tank.GetVertices()))));
        }

        private void GameLoop()
        {
            const int fps = 30;
            const int mspf = 1000 / fps;

            Stopwatch timer = new();
            double DeltaTime = 1;

            while (_gameRuns)
            {
                timer.Restart();

                Thread timerThread = new(() => { Thread.Sleep(mspf); });
                timerThread.Start();

                OnUpdate(DeltaTime);

                mutex.WaitOne();
                if (_update.Obj.Count != 0)
                {
                    _room.SendUpdate(_update);
                    _update.Obj.Clear();
                }
                mutex.ReleaseMutex();

                _update.Player = new();

                timerThread.Join();

                timer.Stop();

                DeltaTime = timer.ElapsedMilliseconds;
            }
        }

        private void OnUpdate(double dTime)
        {
            //if (_update.Obj.Count == 0)
            //{
            //    return;
            //}

            foreach((_, Player player) in Players)
            {
                UpdatePayload payload = player.Tank.Update(dTime);
                if(payload != null)
                {
                    _update.Obj.Add(new(player.Id, payload));
                }
            }


        }
    }
}