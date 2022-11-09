using UnityEngine;

namespace Pixygon.Core {
    [CreateAssetMenu(fileName = "New Experience", menuName = "Pixygon/Create New Experience")]
    public class ExperienceData : ScriptableObject {
        [SerializeField] public int Major;
        [SerializeField] public int Minor;
        [SerializeField] public int Patch;
        [SerializeField] public int Build;
        public string MainScene;
        public TabletAppData[] TabletAppDatas;
        public SceneSettings[] LinkedScenes;
        public string Version { get { return Major + "." + Minor + "." + Patch + "." + Build; } }
    }
}