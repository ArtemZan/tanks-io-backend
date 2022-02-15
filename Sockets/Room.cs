using System.Collections;
using System.Collections.Generic;
using System;

namespace TanksIO.Sockets
{
    using Game;
    using Game.Objects.Tanks;
    using Game.Math;
    using Microsoft.AspNetCore.SignalR;

    class Room
    {
        public readonly string Id;

        private Game _game;

        public Room(string id)
        {
            Id = id;
            _game = new(this);
        }

        public static bool operator==(Room r1, Room r2)
        {
            if ((object)r1 == null && (object)r2 == null)
                return true;

            if ((object)r1 == null || (object)r2 == null)
                return false;

            return r1.Id == r2.Id;
        }

        public static bool operator!=(Room r1, Room r2)
        {
            return !(r1 == r2);
        }

        public override bool Equals(object obj)
        {
            return this == (Room)obj;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        private delegate bool Validator();
        private string GenPlayerId()
        {
            return Utilities.GenId(id => (object)GetPlayer(id) == null);
        }

        public Player CreatePlayer()
        {
            return CreatePlayer(GenPlayerId());
        }

        /// <summary>
        /// Creates a player with the given id and emits 'PlayerJoined' event. Under some circumstances starts the game
        /// </summary>
        /// <returns>The created player</returns>
        public Player CreatePlayer(string id)
        {
            Player player = new Player(id);
            player.Tank = new DefaultTank(id);

            _game.Players.Add(player.Id, player);
            Send(GameEventType.PlayerJoined, id);

            if(_game.Players.Count >= 2)
            {
                StartGame();
            }
            else
            {
                // ? Clients.Caller.SendAsync("GameStarts");
            }

            return player;
        }

        /// <summary>
        /// Creates the player with the given id and emits 'PlayerLeft' event. Under some circumstances ends the game
        /// </summary>
        /// <returns>The created player</returns>
        public Player RemovePlayer(string id)
        {
            _game.Players.Remove(id, out Player player);

            Send(GameEventType.PlayerLeft, id);

            if (_game.Players.Count <= 1 /* For now always 1 */)
            {
                StopGame();
            }

            return player;
        }

        public Player GetPlayer(string id)
        {
            Player player;
            _game.Players.TryGetValue(id, out player);
            return player;
        }

        /// <summary>
        /// Emits "GameStars" event, sends initial update and starts the game loop
        /// </summary>
        public void StartGame()
        {
            Send(new PayloadEvent(GameEventType.GameStarted));
            _game.Start();
            _game.UpdateAll();
        }

        public void StopGame()
        {
            Send(GameEventType.GameEnded);
            _game.Stop();
        }


        public void Send(GameEvent @event)
        {
            foreach (KeyValuePair<string, Player> pair in _game.Players)
            {
                GameHub.context.Clients.Client(pair.Key).SendAsync(@event.TypeToString(), @event.GetPayload(pair.Key));
                Console.WriteLine("Sent message to " + pair.Key + ": " + @event.TypeToString() + "[ " + @event.GetPayload(pair.Key) + " ]");
            }
        }

        public void Send(GameEventType type, params object[] payload)
        {
            Send(new PayloadEvent(type, payload));
        }

        public void Send(GameEventType type, EventGenerator.PayloadGenerator payloadGenerator)
        {
            Send(new EventGenerator(type, payloadGenerator));
        }

        /* public delegate object[] PayloadArrayGenerator(string playerId);
        public void Send(string method, PayloadArrayGenerator payloadGenerator)
        {
            foreach (KeyValuePair<string, Player> pair in Game.Players)
            {
                GameHub.context.Clients.Client(pair.Key).SendAsync(method, payloadGenerator(pair.Key));
            }
        } */

        public void SendUpdate(Update update)
        {
            if(update == null)
            {
                return;
            }

            Send(GameEventType.Update, (string id) =>
            {
                Player player = GetPlayer(id);
                update.Player = new PlayerUpdate(player.Dir, player.Pos);

                return update.ToString();
            });
        }
    }
}