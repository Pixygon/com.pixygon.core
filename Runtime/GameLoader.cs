using Pixygon.Addressable;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Pixygon.Core {
    public class GameLoader : MonoBehaviour {
        public static GameLoader Instance;
        public static bool DebugMode;
        
        [FormerlySerializedAs("loadObject")] [SerializeField] private GameObject _loadObject;
        [FormerlySerializedAs("loadMask")] [SerializeField] private LayerMask _loadMask;
        [FormerlySerializedAs("progressBar")] [SerializeField] private Slider _progressBar;

        private const string MenuScene = "Menu";
        private const string Bootstrapper = "Bootstrap";

        private AsyncOperationHandle _sceneLoading;
        private OnNewSceneLoaded _sceneLoad;
        private LayerMask _currentMask;
        private float _totalSceneProgress;
        private static string _currentScene;
        private static AsyncOperationHandle<SceneInstance> _currentSceneInstance;
        private static AsyncOperationHandle<SceneInstance> _appSession;
        private static AsyncOperationHandle<SceneInstance> _environment;

        [FormerlySerializedAs("activated")] public bool _activated;
        public delegate void OnNewSceneLoaded(Scene scene, LoadSceneMode sceneMode);

        private void Awake() {
            Instance = this;
            DoSceneLoad(MenuScene);
        }
        private void DoSceneLoad(string scene) {
            _loadObject.SetActive(true);
            _currentScene = scene;
            _currentSceneInstance = AddressableSceneLoader.LoadSceneOperation(scene, true);
            _sceneLoading = _currentSceneInstance;
            GetSceneLoadProgressAsync();
        }
        private async void GetSceneLoadProgressAsync() {
            while(_sceneLoading.Status != AsyncOperationStatus.Succeeded) {
                _totalSceneProgress = _sceneLoading.PercentComplete;
                _progressBar.value = _totalSceneProgress;
                await Task.Yield();
            }
            while(!SceneManager.GetSceneByName(_currentScene).isLoaded)
                await Task.Yield();
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(_currentScene));
            var timer = 2f;
            while(timer >= 0f) {
                timer -= Time.deltaTime;
                await Task.Yield();
            }
            if(_loadObject != null)
                _loadObject.SetActive(false);
            else {
                Debug.Log("Load object is null!?");
            }
            if(Camera.main != null)
                Camera.main.cullingMask = _currentMask;
            _sceneLoad?.Invoke(SceneManager.GetSceneByName(_currentScene), LoadSceneMode.Additive);
        }
        private async Task DoUnload(AsyncOperationHandle<SceneInstance> scene) {
            var unload = AddressableSceneLoader.UnloadSceneOperation(scene);
            while(!unload.IsDone) await Task.Yield();
        }
        public void ActivateGameLoader() {
            _activated = true;
            _currentScene = MenuScene;
            _loadObject.SetActive(false);
            _sceneLoad += Bootstrap.Instance.OnNewSceneLoaded;
        }
        public void ActivateLoadScene() {
            _loadObject.SetActive(true);
        }
        public async void LoadSceneAsync(string scene, string address = "") {
            Bootstrap.Instance.NextArea = _currentScene;
            Bootstrap.Instance.PreviousAddress = address;
            _currentMask = Camera.main.cullingMask;
            Camera.main.cullingMask = _loadMask;
            await DoUnload(_currentSceneInstance);
            DoSceneLoad(scene);
        }
        public async void ResetGameAsync() {
            _sceneLoad -= Bootstrap.Instance.OnNewSceneLoaded;
            Destroy(Bootstrap.Instance.gameObject);
            Bootstrap.Instance = null;
            await DoUnload(_appSession);
            _loadObject.SetActive(true);
            await DoUnload(_currentSceneInstance);
            _currentSceneInstance = AddressableSceneLoader.LoadSceneOperation(MenuScene, true);
            _loadObject.SetActive(false);
            _activated = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public static async void LoadAppSession() {
            DebugMode = false;
            _appSession = AddressableSceneLoader.LoadSceneOperation(Bootstrapper, true);
            while(!_appSession.IsDone) await Task.Yield();
        }

        public static async void LoadDebugSession() {
            Debug.Log("Loading Debug-session");
            DebugMode = true;
            _appSession = AddressableSceneLoader.LoadSceneOperation(Bootstrapper, true);
            while(!_appSession.IsDone) await Task.Yield();
        }
    }
}