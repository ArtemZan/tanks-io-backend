using System;

namespace TanksIO.Sockets
{
    enum GameEventType {
        GameStarted,
        GameEnded,
        Update,
        PlayerLeft,
        PlayerJoined,
        Join
    }

    abstract class GameEvent
    {
        public GameEventType Type;

        public string TypeToString()
        {
            switch(Type)
            {
                case GameEventType.Update: return nameof(GameEventType.Update);
                case GameEventType.PlayerJoined: return nameof(GameEventType.PlayerJoined);
                case GameEventType.PlayerLeft: return nameof(GameEventType.PlayerLeft);
                case GameEventType.Join: return nameof(GameEventType.Join);
                case GameEventType.GameStarted: return nameof(GameEventType.GameStarted);
                case GameEventType.GameEnded: return nameof(GameEventType.GameEnded);
            }

            throw new Exception();
        }

        public abstract object GetPayload(string playerId);

        public GameEvent(GameEventType type)
        {
            Type = type;
        }
    }

    class PayloadEvent : GameEvent
    {
        public object Payload;

        public PayloadEvent(GameEventType type, params object[] payload)
            :base(type)
        {
            if(payload.Length == 1)
            {
                Payload = payload[0];
                return;
            }

            Payload = payload;
        }

        public override object GetPayload(string playerId)
        {
            return Payload;
        }
    }

    class EventGenerator : GameEvent
    {
        public delegate object PayloadGenerator(string playerId);

        public PayloadGenerator Generate;

        public EventGenerator(GameEventType type, PayloadGenerator payloadGenerator)
            : base(type)
        {
            Generate = payloadGenerator;
        }

        public override object GetPayload(string playerId)
        {
            return Generate(playerId);
        }
    }
}
