using UnityEngine;
using UnityEngine.Events;

namespace Pixygon.Core {
    public class PauseToggler : MonoBehaviour {
        [SerializeField] private UnityEvent _pauseEvent;
        [SerializeField] private UnityEvent _unpauseEvent;

        private void OnEnable() {
            PauseManager.OnPause += OnPause;
            PauseManager.OnUnpause += OnUnpause;
        }

        private void OnDisable() {
            PauseManager.OnPause -= OnPause;
            PauseManager.OnUnpause -= OnUnpause;
        }

        protected virtual void OnPause() {
            _pauseEvent?.Invoke();
        }

        protected virtual void OnUnpause() {
            _unpauseEvent?.Invoke();
        }
    }
}