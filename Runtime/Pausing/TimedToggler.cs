using UnityEngine;
using UnityEngine.Events;

namespace Pixygon.Core {
    public class TimedToggler : MonoBehaviour {
        [SerializeField] private float _timerStart;
        [SerializeField] private float _timerSet;
        [SerializeField] private UnityEvent _hideEvent;
        [SerializeField] private UnityEvent _respawnEvent;

        private float _timer;
        private bool _isSpawned;

        private void Start() {
            _timer = _timerStart;
        }

        private void Update() {
            if (PauseManager.Pause) return;
            if (_isSpawned) return;
            if (_timer >= 0f)
                _timer -= Time.deltaTime;
            else {
                Respawn();
            }
        }

        public void Hide() {
            _isSpawned = false;
            _timer = _timerSet;
            _hideEvent.Invoke();
        }

        public void Respawn() {
            _isSpawned = true;
            _respawnEvent.Invoke();
        }
    }
}