using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TanksIO.Sockets
{
    using Box2DX.Common;
    using Game;
    using Game.Math;

    class GameHub : Hub
    {
        public static IHubContext<GameHub> context = null;

        public override Task OnConnectedAsync()
        {
            Console.WriteLine("New connection");

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            (Player player, Room room) = Rooms.FindPlayer(Context.ConnectionId);

            if (room != null)
            {
                room.RemovePlayer(player.Id);
            }


            return base.OnDisconnectedAsync(exception);
        }

        public void CreateRoom()
        {
            Room room = Rooms.CreateRoom();

            // Use connetion id for as player id
            Player player = room.CreatePlayer(Context.ConnectionId);


            //Clients.Caller.SendCoreAsync("RoomCreated", new string[] { room.Id });
            Clients.Caller.SendCoreAsync("Join", new string[] { player.Id, room.Id });
        }

        public void Leave(string playerId, string roomCode)
        {
            Rooms.GetRoom(roomCode).RemovePlayer(playerId);
        }

        public void Join(string roomCode)
        {
            Room room = Rooms.GetRoom(roomCode);

            if(room == null)
            {
                Clients.Caller.SendAsync("WrongCode");
                return;
            }

            Clients.Caller.SendAsync("Join", Context.ConnectionId);

            room.CreatePlayer(Context.ConnectionId);
        }

        public void StartRotatingTurret(string dirString)
        {
            Vec2 dir = JsonConvert.DeserializeObject<Vec2>(dirString);

            (Player player, Room _) = Rooms.FindPlayer(Context.ConnectionId);

            if(player == null)
            {
                Console.WriteLine("Error: trying to rotate turret before the player joined a game");
                return;
            }

            if (dir.Length() < 1e-6)
            {
                Console.WriteLine("Warning: direction cannot be zero vector. Setting to default ([ 0, 1 ])");
                dir = new(0, 1);
            }

            dir.Normalize();

            player.Tank.Aim(dir);
        }

        public void StopRotatingTurret()
        {

        }

        public void Shoot()
        {
            (Player player, Room room) = Rooms.FindPlayer(Context.ConnectionId);

            room.Scene.PlayerAction(player).Shoot();
        }

        public void StartRotating(bool cw)
        {
            (Player player, Room _) = Rooms.FindPlayer(Context.ConnectionId);

            player.Tank.Body.SetAngularVelocity(1 * (cw ? -1 : 1));
        }

        public void StopRotating()
        {
            (Player player, Room _) = Rooms.FindPlayer(Context.ConnectionId);

            player.Tank.Body.SetAngularVelocity(0);
        }

        public void StartMoving(bool forward)
        {
            (Player player, Room _) = Rooms.FindPlayer(Context.ConnectionId);

            player.Tank.Speed = 5 * (forward ? 1 : -1);
        }

        public void StopMoving()
        {
            (Player player, Room _) = Rooms.FindPlayer(Context.ConnectionId);

            player.Tank.Speed = 0;

            //player.Tank.Body.SetLinearVelocity(new());
        }

    }
}