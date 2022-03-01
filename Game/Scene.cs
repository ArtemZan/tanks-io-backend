using System;
using System.Collections.Generic;
using System.Linq;

namespace TanksIO.Game
{
    using Objects.Bullets;
    using Math;
    using ObjectsContainer = Dictionary<string, Objects.Mesh>;
    using TanksIO.Utils;

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
            _update.Obj = GameObjects.Select(pair => new ObjectUpdate(pair.Key, new VerticesUpdatePayload(pair.Value.GetVertices()))).ToList();
            _update.Obj.AddRange(Players.Select(pair => new ObjectUpdate(pair.Key, new VerticesUpdatePayload(pair.Value.Tank.GetVertices()))));
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
            lock (this)
            {
                //Console.WriteLine("Lock");
                foreach ((_, Player player) in Players)
                {
                    UpdatePayload payload = player.Tank.Update(dTime);
                    if (payload != null)
                    {
                        _update.Obj.Add(new(player.Id, payload));
                    }
                }

                List<string> removedBullets = new();

                foreach ((string id, Bullet bullet) in Bullets)
                {
                    if (bullet.Pos.Length() > 1e3)
                    {
                        removedBullets.Add(id);
                        _update.Obj.Add(new(id, new VerticesUpdatePayload(null)));
                        //Console.WriteLine("Add empty bullet update: " + id);
                        continue;
                    }

                    //Console.WriteLine("Update bullet: " + id);
                    UpdatePayload payload = bullet.Update(dTime);
                    if (payload != null)
                    {
                        _update.Obj.Add(new(id, payload));
                    }
                }

                foreach (string id in removedBullets)
                {
                    Bullets.Remove(id);
                    //Console.WriteLine("Remove bullet: " + id);
                }

                //Console.WriteLine("Unlock");
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


        public PlayerActions PlayerAction(Player player)
        {
            return new PlayerActions(this, player);
        }

        public class PlayerActions
        {
            private Scene _scene;
            private Player _player;

            public PlayerActions(Scene scene, Player player)
            {
                _scene = scene;
                _player = player;
            }

            public void Shoot()
            {
                double turretRot = _player.Tank.Rot + _player.Tank.Turret.Rot;

                Bullet bullet = new DefaultBullet(_player.Id);
                bullet.Rotate(turretRot);
                bullet.Move(_player.Tank.Pos + new Vec2(System.Math.Cos(turretRot), System.Math.Sin(turretRot)) * 0.5);

                lock (_scene)
                {
                    //Console.WriteLine("Shoot: locked");

                    string id;
                    int tail = 0;
                    do
                    {
                        id = "b" + DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond + (tail == 0 ? "" : tail);
                        tail++;
                    }
                    while (_scene.Bullets.ContainsKey(id));

                    _scene.UpdateObject(new(id, new VerticesUpdatePayload(bullet.GetVertices())));
                    _scene.Bullets.Add(id, bullet);

                    //Console.WriteLine("Shoot: unlocked");
                }
            }
        }
    }
}