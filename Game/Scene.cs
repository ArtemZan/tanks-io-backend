using System;
using System.Collections.Generic;
using System.Linq;

using Box2DX.Dynamics;
using Box2DX.Collision;

namespace TanksIO.Game
{
    using Box2DX.Common;
    using Objects.Bullets;
    using Objects.Tanks;
    
    using ObjectsContainer = Dictionary<string, Objects.Object>;

    class Scene
    {
        public ObjectsContainer GameObjects = new();
        public Dictionary<string, Player> Players = new();
        public Dictionary<string, Bullet> Bullets = new();

        public World _world;

        private Update _update = new();

        public Scene(Vec2 size)
        {
            AABB aabb = new();

            aabb.LowerBound = new((float)-size.X / 2, (float)-size.Y / 2);
            aabb.UpperBound = new((float)size.X / 2, (float)size.Y / 2);
            _world = new World(aabb, new(), true);
        }

        public void UpdateAll()
        {
            _update.Obj = GameObjects.Select(pair => new ObjectUpdate(pair.Key, new VerticesUpdatePayload(pair.Value.Mesh))).ToList();
            _update.Obj.AddRange(Players.Select(pair => new ObjectUpdate(pair.Key, new VerticesUpdatePayload(pair.Value.Tank.Mesh))));
        }

        public void UpdateObject(ObjectUpdate update)
        {
            _update.Obj.Add(update);
        }

        public void UpdatePlayer(PlayerUpdate playerUpdate)
        {
            _update.Player = playerUpdate;
        }

        public Player CreatePlayer(string id)
        {
            Player player = new(id);

            //For now always default
            player.Tank = new DefaultTank(_world);
            player.Tank.Body.SetPosition(new(0, Players.Count * 7 - 7));

            Players.Add(id, player);

            return player;
        }

        public void OnUpdate(double dTime)
        {
            _world.Step((float)dTime, 1, 1);

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
                double turretRot = _player.Tank.Turret.Rot;

                Bullet bullet = new DefaultBullet(_scene._world, _player.Id);
                bullet.Body.SetAngle((float)turretRot);
                bullet.Body.SetPosition(_player.Tank.Pos + new Vec2((float)System.Math.Cos(turretRot), (float)System.Math.Sin(turretRot)) * 0.5f);

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

                    _scene.UpdateObject(new(id, new VerticesUpdatePayload(bullet.Mesh)));
                    _scene.Bullets.Add(id, bullet);

                    //Console.WriteLine("Shoot: unlocked");
                }
            }
        }
    }
}