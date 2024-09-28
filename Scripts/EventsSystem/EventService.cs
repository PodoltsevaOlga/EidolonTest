using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SaveLoadSystem;
using UnityEngine;
using UnityEngine.Networking;
using Event = EventsSystem.Event;

namespace EventsSystem
{
    public class EventService : MonoBehaviour
    {
        [SerializeField] 
        private string m_EventsSaveFileName = "events.data";

        [SerializeField] 
        private float m_CooldownBeforeSend = 2.0f;

        [SerializeField] 
        private string m_ServerUrl;

        private SaveDataManager<Event> m_SaveManager;
        private LoadDataManager<Event> m_LoadManager;

        private float m_Timer = 0.0f;

        private List<Event> m_Events = new();
        private Dictionary<string, EventsJson> m_WaitingResponseEvents = new();

        private void Awake()
        {
            m_SaveManager = new(m_EventsSaveFileName);
            m_LoadManager = new(m_EventsSaveFileName);
        }
    
        private void Start()
        {
            m_Timer = Time.realtimeSinceStartup;
            m_Events = m_LoadManager.LoadData();
        }

        private void FixedUpdate()
        {
            if (Time.realtimeSinceStartup - m_Timer >= m_CooldownBeforeSend)
            {
                SendEvents();
                m_Timer = Time.realtimeSinceStartup;
            }
        }

        private void SendEvents()
        {
            if (m_Events.Count == 0)
                return;
        
            var events = new EventsJson() {Events = m_Events.ToArray()};
            string requestBody = JsonConvert.SerializeObject(events);
        
            m_WaitingResponseEvents.Add(requestBody, events);
            m_Events.Clear();

            StartCoroutine(SendRequestCoroutine(requestBody));
        }
    
        IEnumerator SendRequestCoroutine(string requestBody)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Post(m_ServerUrl, requestBody))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError(webRequest.error);
                    if (m_WaitingResponseEvents.TryGetValue(requestBody, out var events))
                    {
                        m_Events = m_Events.Concat(events.Events).ToList();
                        m_WaitingResponseEvents.Remove(requestBody);
                    }
                }
                else
                {
                    Debug.Log("Events sent!");
                }
            }
        }

        private void OnDestroy()
        {
            m_SaveManager.SaveEvents(m_Events);
        }
        
        public void TrackEvent(string type, string data) {
            m_Events.Add(new Event(type, data));
        }
    }
}
