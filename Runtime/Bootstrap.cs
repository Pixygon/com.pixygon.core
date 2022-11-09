using System.Threading.Tasks;
using Pixygon.Addressable;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Pixygon.Core {
    public class Bootstrap : MonoBehaviour {
        public static Bootstrap Instance;

        public static bool IsNewVersion;

        [SerializeField] private GameObject _actorSubsystemPrefab;
        [SerializeField] private GameObject _onlineSubsystemPrefab;
        [SerializeField] private GameObject _settingsSubsystemPrefab;
        [SerializeField] private GameObject _gatheringSubsystemPrefab;
        [SerializeField] private GameObject _adSubsystemPrefab;
        [SerializeField] private GameObject _storeSubsystemPrefab;
        [SerializeField] private ExperienceSettings _experienceSettings;
        [SerializeField] private AssetReference _triplanarShader;
        public Shader TriplanarShader { get; private set; }
        public SubsystemModule AreaSubsystem { get; set; }
        public SubsystemModule ActorSubsystem { get; private set; }
        public SubsystemModule OnlineSubsystem { get; private set; }
        public SubsystemModule SettingsSubsystem { get; private set; }
        public SubsystemModule GatheringSubsystem { get; private set; }
        public SubsystemModule AdSubsystem { get; private set; }
        public SubsystemModule StoreSubsystem { get; private set; }
        public ExperienceSettings ExperienceSettings => _experienceSettings;
        public bool ReconnectOnline { get; set; }
        public bool IsVR { get; set; }
        public bool CombatEnabled { get; set; }
        public bool IsLoading { get; private set; }
        public string NextArea { get; set; }
        public string PreviousAddress { get; set; }
        public SceneData SceneData { get; set; }

        private void Awake() {
            if(Instance == null) {
                Instance = this;
                if(!GameLoader.Instance._activated)
                    GameLoader.Instance.ActivateGameLoader();
            } else
                Destroy(this);
        }

        private async void Start() {
            while(GameLoader.Instance == null)
                await Task.Yield();
            GameLoader.Instance.ActivateLoadScene();
            //FindObjectOfType<AudioListener>().enabled = false;
            TriplanarShader = await AddressableLoader.LoadAsset<Shader>(_triplanarShader);
            await Task.Yield();
            CombatEnabled = PlayerPrefs.GetInt("CombatEnabled") == 1;
            ActorSubsystem = Instantiate(_actorSubsystemPrefab, transform).GetComponent<SubsystemModule>();
            OnlineSubsystem = Instantiate(_onlineSubsystemPrefab, transform).GetComponent<SubsystemModule>();
            SettingsSubsystem = Instantiate(_settingsSubsystemPrefab, transform).GetComponent<SubsystemModule>();
            GatheringSubsystem = Instantiate(_gatheringSubsystemPrefab, transform).GetComponent<SubsystemModule>();
            AdSubsystem = Instantiate(_adSubsystemPrefab, transform).GetComponent<SubsystemModule>();
            StoreSubsystem = Instantiate(_storeSubsystemPrefab, transform).GetComponent<SubsystemModule>();
            IsLoading = true;
            if(!GameLoader.DebugMode)
                GameLoader.Instance.LoadSceneAsync(ExperienceSettings._mainScene);
            else
                GameLoader.Instance.LoadSceneAsync("DebugRoom");
        }

        public void LoadedExperience() {
            if(ActorSubsystem != null)
                ActorSubsystem.SceneChange();
            ResetAll();
        }

        public void OnNewSceneLoaded(Scene scene, LoadSceneMode loadMode) {
            LoadedExperience();
        }

        public void ResetAll() {
        }

        public async void LoadScene(string scene, string address = "") {
            GameLoader.Instance.ActivateLoadScene();
            await Task.Yield();
            ReconnectOnline = true;
            OnlineSubsystem.LeaveScene();
            ActorSubsystem.LeaveScene();
            GameLoader.Instance.LoadSceneAsync(scene, address);
        }

        public void RestartApp() {
            ActorSubsystem.RestartApp();
            GameLoader.Instance.ResetGameAsync();
        }
    }
}