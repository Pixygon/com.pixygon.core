using Pixygon.PagedContent;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Pixygon.Core {
    [CreateAssetMenu(fileName = "New App", menuName = "Pixygon/New Tablet App")]
    public class TabletAppData : PagedContentDataObject {
        public AssetReference _iconRef;
        public AssetReference _prefabRef;
        public AppCategory _category;
        public string _version;
        public bool _beta;
        public bool _requiredNft;
        public int _template;
        public int _dropID;
        public bool _neftyDrop;

        public enum AppCategory {
            Core = 0,
            Tools = 1,
            Utility = 2,
            Collections = 3,
            Games = 4
        }
    }
}