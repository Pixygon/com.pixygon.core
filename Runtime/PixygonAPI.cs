using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Pixygon.Core {
    public class PixygonAPI : MonoBehaviour {
        private const string AuthEndpoint = "users/authWithWallet";
        private const string ExpEndpoint = "users/experience";
        private const string SkillEndpoint = "users/skill";
        private const string StatsEndpoint = "users/account";
        private const string MintEndpoint = "nft/mint";
        private const string PropertyEndpoint = "v2/nft/properties?propertyId=";
        private const string NodeEndpoint = "game-settings/nodepoint";
        private const string BaseEndpoint = "https://api.pixygon.io/";

        private const string Keyword = "boob";
        private const string WaxWallet = "WaxWallet";

        private static async Task<string> GetTokenAsync() {
            var form = new WWWForm();
            form.AddField("walletAddress", PlayerPrefs.GetString(WaxWallet));
            var www = UnityWebRequest.Post(BaseEndpoint + AuthEndpoint, form);
            www.SendWebRequest();
            while(!www.isDone)
                await Task.Yield();
            if(www.result != UnityWebRequest.Result.Success)
                Debug.Log($"GetTokenAsync: {www.error}\n{www.downloadHandler.text}");
            else {
                //Debug.Log(www.downloadHandler.text);
                var s = JsonUtility.FromJson<UserData>(www.downloadHandler.text).token;
                www.Dispose();
                return s;
            }
            www.Dispose();
            return string.Empty;
        }

        private static async Task<UnityWebRequest> SendRequestAsync(string data, string endpoint) {
            var form = new WWWForm();
            form.AddField("data", data);
            var www = UnityWebRequest.Post(BaseEndpoint + endpoint, form);
            www.SetRequestHeader("Authorization", "Bearer " + await GetTokenAsync());
            www.SendWebRequest();
            while(!www.isDone)
                await Task.Yield();
            if (www.error == null) return www;
            Debug.Log("ERROR! " + www.error + www.downloadHandler.text);
            www.Dispose();
            return null;
        }
        public static async Task<accountData> GetStatsAsync() {
            var www = UnityWebRequest.Get(BaseEndpoint + StatsEndpoint);
            www.SetRequestHeader("Authorization", "Bearer " + await GetTokenAsync());
            www.SendWebRequest();
            while(!www.isDone) await Task.Yield();
            if (www.result != UnityWebRequest.Result.Success) {
                Debug.Log("GetStatsAsync: " + www.error);
                return new accountData();
            }
            var ad = JsonUtility.FromJson<accountData>(www.downloadHandler.text);
            return ad;
        }
        public static async Task<bool> GetXpAsync(StatType type, int amount) {
            return false;
            var d = new xpdata
            {
                name = type.ToString(),
                experience = amount.ToString(),
                //d.hour = EnvironmentController.hour.ToString();
                //d.date = EnvironmentController.date;
                hash = HashGenerator.CreateHash(Keyword + PlayerPrefs.GetString(WaxWallet) + type + amount
                    //+ EnvironmentController.date
                    //+ EnvironmentController.hour.ToString()
                )
            };
            var www = await SendRequestAsync(HashGenerator.Base64Encode(JsonUtility.ToJson(d)), ExpEndpoint);
            var levelUp = JsonUtility.FromJson<levelResponse>(www.downloadHandler.text).levelUp;
            www.Dispose();
            return levelUp;
        }
        public static async Task<bool> GetSpAsync(SkillType type, int amount) {
            return false;
            var d = new spdata
            {
                name = type.ToString(),
                sp = amount.ToString(),
                //d.hour = EnvironmentController.hour.ToString();
                //d.date = EnvironmentController.date;
                hash = HashGenerator.CreateHash(Keyword + PlayerPrefs.GetString(WaxWallet) + type + amount
                    //+ EnvironmentController.date
                    //+ EnvironmentController.hour.ToString()
                )
            };
            var www = await SendRequestAsync(HashGenerator.Base64Encode(JsonUtility.ToJson(d)), SkillEndpoint);
            var levelUp = JsonUtility.FromJson<levelResponse>(www.downloadHandler.text).levelUp;
            www.Dispose();
            return levelUp;
        }
        public static async void MintAssetAsync(string collection, string schema, int template, Action onSuccess) {
            var d = new mintdata
            {
                collection = collection,
                schema = schema,
                template = template,
                //d.hour = EnvironmentController.hour.ToString();
                //d.date = EnvironmentController.date;
                hash = HashGenerator.CreateHash(Keyword + PlayerPrefs.GetString(WaxWallet)
                    //+ EnvironmentController.date
                    //+ EnvironmentController.hour.ToString()
                )
            };
            var www = await SendRequestAsync(HashGenerator.Base64Encode(JsonUtility.ToJson(d)), MintEndpoint);
            www.Dispose();
            onSuccess?.Invoke();
        }
        public static async Task<string> GetEndpointNodesAsync() {
            var www = UnityWebRequest.Get(BaseEndpoint + NodeEndpoint);
            www.SetRequestHeader("Authorization", "Bearer " + await GetTokenAsync());
            www.SendWebRequest();
            
            while(!www.isDone)
                await Task.Yield();
            if(www.result != UnityWebRequest.Result.Success)
                Debug.Log(www.error);
            else
                return www.downloadHandler.text;
            return null;
        }
        public static async Task<string> GetPropertyAsync(string id) {
            var url = BaseEndpoint + PropertyEndpoint + id;
            var www = UnityWebRequest.Get(url);
            www.SetRequestHeader("Authorization", "Bearer " + await GetTokenAsync());
            www.SendWebRequest();
            while(!www.isDone)
                await Task.Yield();
            if(www.result != UnityWebRequest.Result.Success)
                Debug.Log(url + " " + www.error + www.downloadHandler.text);
            else {
                return www.downloadHandler.text;
            }
            return null;
        }
        public static async void SetPropertyAsync(string json) {
            var www = new UnityWebRequest(BaseEndpoint + "v2/nft/properties", UnityWebRequest.kHttpVerbPOST);
            var b = new System.Text.UTF8Encoding().GetBytes(json);
            var uH = new UploadHandlerRaw(b);
            //www.method = "POST";
            www.uploadHandler = uH;
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Authorization", "Bearer " + await GetTokenAsync());
            www.SetRequestHeader("Content-Type", "application/json");

            www.SendWebRequest();
            while(!www.isDone)
                await Task.Yield();
            if(www.error != null)
                Debug.Log("ERROR! " + www.error + www.downloadHandler.text);
            www.Dispose();
            //Notifications.instance.SendNotification("Property", "Saved property!", null);
        }
    }
    [Serializable]
    public class levelResponse {
        public string message;
        public bool levelUp;
    }
    public enum StatType {
        Red,
        Orange,
        Yellow,
        Green,
        Blue,
        Purple,
        White
    }
    public enum SkillType {
        Woodcutting,
        Geology,
        Herbalism,
        Archeology,
        Fishing,
        Entomology,
        Shortsword,
        Axe,
        Spear,
        Bow,
        Fists,
        Katana,
        Greatsword,
        Dagger,
        Hammer,
        Wand,
        EleBlob
    }
    [Serializable]
    public class xpdata {
        public string name;
        public string experience;
        public string date;
        public string hour;
        public string hash;
    }
    [Serializable]
    public class spdata {
        public string name;
        public string sp;
        public string date;
        public string hour;
        public string hash;
    }
    [Serializable]
    public class mintdata {
        public string collection;
        public string schema;
        public int template;
        public string date;
        public string hour;
        public string hash;
    }

    [Serializable]
    public class accountData {
        public Experience[] experiences;
        public Skills[] skills;

        public accountData() {
            experiences = Array.Empty<Experience>();
            skills = Array.Empty<Skills>();
        }
    }
    [Serializable]
    public class Experience {
        public string name;
        public int level;
        public int xp;
        public int maxXP;
    }

    [Serializable]
    public class Skills {
        public string name;
        public int level;
        public int sp;
        public int maxSp;
        public Sprite icon;
    }
    public class UserData {
        public int id;
        public string username;
        public string wallet;
        public string token;
    }
}