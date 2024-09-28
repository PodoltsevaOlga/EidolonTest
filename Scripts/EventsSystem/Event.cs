using System;
using Newtonsoft.Json;
using SaveLoadSystem;

namespace EventsSystem
{
    [Serializable]
    public class Event : ISavableData, ILoadableData
    {
        [JsonProperty("type")]
        public string Type { get; private set; }
        [JsonProperty("data")]
        public string Data { get; private set; }

        public Event(string _type, string _data)
        {
            Type = _type;
            Data = _data;
        }

        public Event()
        {
        }

        string ISavableData.ConvertToString()
        {
            return $"{Type};{Data}";
        }

        void ILoadableData.CreateFromString(string _event)
        {
            var entry = _event.Split(';');
            if (entry == null || entry.Length < 2)
            {
                throw new ArgumentException($"Can't create event from string {_event}");
            }

            Type = entry[0];
            Data = entry[1];
        }
    }
}
