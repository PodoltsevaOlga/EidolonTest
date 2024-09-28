using System;
using Newtonsoft.Json;

namespace EventsSystem
{
    [Serializable]
    public class EventsJson
    {
        [JsonProperty("events")] 
        public Event[] Events;
    }
}