using UnityEngine;
using System;

namespace Pixygon.Core {
    public class PauseManager : MonoBehaviour {
        public static Action OnPause;
        public static Action OnUnpause;
        public static bool Pause { get; protected set; }
        
        public static void SetPause(bool pause) {
            Pause = pause;
            if(Pause) OnPause?.Invoke();
            else OnUnpause?.Invoke();
        }

        public static void ResetPause() {
            Pause = false;
            OnPause = null;
            OnUnpause = null;
        }
    }
}
