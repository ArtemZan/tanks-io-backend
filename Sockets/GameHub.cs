using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;

namespace TanksIO.Sockets
{
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
            
            Clients.Caller.SendAsync("Join", Context.ConnectionId);

            room.CreatePlayer(Context.ConnectionId);
        }

        public void StartRotatingTurret(Vec2 dir)
        {

        }

        public void StopRotatingTurret()
        {

        }

        public void Shoot()
        {

        }

        public void StartRotating(bool ccw)
        {

        }

        public void StopRotating()
        {

        }

        public void StartMoving(bool forward)
        {

        }

        public void StopMoving()
        {

        }

    }
}