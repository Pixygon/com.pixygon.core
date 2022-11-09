using UnityEngine;

namespace Pixygon.Core {
    public abstract class SubsystemModule : MonoBehaviour {
        [SerializeField] protected bool _useDebug;
        protected virtual void Initialize() {

        }

        public virtual void SceneChange() {

        }

        public virtual void LeaveScene() {

        }

        public virtual void UpdateSettings() {

        }

        public virtual void RestartApp() {
            
        }
    }
}