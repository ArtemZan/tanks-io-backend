using System;
using System.Collections.Generic;
using System.Linq;

namespace TanksIO.Game
{
    using Objects.Bullets;
    using Math;
    using ObjectsContainer = Dictionary<string, Objects.Mesh>;

    class Scene
    {
        public ObjectsContainer GameObjects = new();
        public Dictionary<string, Player> Players = new();
        public Dictionary<string, Bullet> Bullets = new();

        private Update _update = new();

        public Scene()
        {

        }

        public void UpdateAll()
        {
            _update.Obj = GameObjects.Select(pair => new ObjectUpdate(pair.Key, new VerticesUpdatePayload(pair.Value.GetOwnVertices()))).ToList();
            _update.Obj.AddRange(Players.Select(pair => new ObjectUpdate(pair.Key, new VerticesUpdatePayload(pair.Value.Tank.GetOwnVertices()))));
        }

        public void UpdateObject(ObjectUpdate update)
        {
            _update.Obj.Add(update);
        }

        public void SetPlayer(PlayerUpdate playerUpdate)
        {
            _update.Player = playerUpdate;
        }

        public void OnUpdate(double dTime)
        {
            //Console.WriteLine("Time: " + dTime);

            foreach ((_, Player player) in Players)
            {
                UpdatePayload payload = player.Tank.Update(dTime);
                if (payload != null)
                {
                    _update.Obj.Add(new(player.Id, payload));
                }
            }
        }

        public Update GetUpdate()
        {
            return _update;
        }

        public void ClearUpdate()
        {
            _update.Obj.Clear();
        }


        public PlayerActions PlayerAction(string playerId)
        {
            return new PlayerActions(this, playerId);
        }

        public class PlayerActions
        {
            private Scene _scene;
            private string _playerId;

            public PlayerActions(Scene scene, string playerId)
            {
                _scene = scene;
                _playerId = playerId;
            }

            public void Shoot()
            {
                _scene.Players.TryGetValue(_playerId, out Player player);

                Vec2 turretDir = player.Tank.Turret.Dir;

                Bullet bullet = new DefaultBullet(_playerId);
                bullet.Transform(new Mat2x3(
                    turretDir,
                    new(-turretDir.Y, turretDir.X),
                    player.Tank.Pos + turretDir * 0.5 // For now a constant (offset from the center of the tank)
                ));

                _scene.Bullets.Add("b" + DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond, bullet);
            }
        }
    }
}