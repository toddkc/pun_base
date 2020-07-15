namespace NetworkTutorial.GameEvents
{
    using UnityEngine;
    using UnityEngine.Events;

    public class GameEventListener : MonoBehaviour
    {
        [SerializeField] private GameEvent gameEvent;
        [SerializeField] private UnityEvent responseEvent;

        private void OnEnable()
        {
            gameEvent.AddListener(this);
        }

        private void OnDisable()
        {
            gameEvent.RemoveListener(this);
        }

        public void OnEventRaised()
        {
            responseEvent.Invoke();
        }
    }
}