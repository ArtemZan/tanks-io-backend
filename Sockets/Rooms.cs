using System.Collections.Generic;

namespace TanksIO.Sockets
{
    using Game;

    class Rooms
    {
        private static List<Room> _rooms = new();

        private Rooms() { }

        private static string GenRoomCode()
        {
            return Utilities.GenId(code => (object)GetRoom(code) == null);
        }

        // Creates a new room and returns it
        public static Room CreateRoom()
        {
            Room room = new Room(GenRoomCode());
            _rooms.Add(room);
            return room;
        }

        public static Room GetRoom(string id)
        {
            return _rooms.Find(room => room.Id == id);
        }
    }
}
