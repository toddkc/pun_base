namespace NetworkTutorial.GameEvents
{
    using UnityEngine;
    using System.Collections.Generic;
    using NaughtyAttributes;

    [CreateAssetMenu(fileName = "GameEvent", menuName = "GameEvent")]
    public class GameEvent : ScriptableObject
    {
        private List<GameEventListener> listeners = new List<GameEventListener>();

        public void Raise()
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised();
            }
        }

        public void AddListener(GameEventListener listener)
        {
            listeners.Add(listener);
        }

        public void RemoveListener(GameEventListener listener)
        {
            listeners.Remove(listener);
        }


        [Button]
        private void ListAllListeners()
        {
            foreach(var listener in listeners)
            {
                Debug.Log($"{name}: {listener.name}", listener);
            }
        }
    }
}