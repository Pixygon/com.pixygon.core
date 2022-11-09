using System;
using Pixygon.ID;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pixygon.Core {
	[Serializable]
	[CreateAssetMenu(fileName = "Experience Settings", menuName = "Pixygon/Experience Settings")]
	public class ExperienceSettings : ScriptableObject {
		[FormerlySerializedAs("Major")] [SerializeField] public int _major;
		[FormerlySerializedAs("Minor")] [SerializeField] public int _minor;
		[FormerlySerializedAs("Patch")] [SerializeField] public int _patch;
		[FormerlySerializedAs("Build")] [SerializeField] public int _build;
		[FormerlySerializedAs("MainScene")] public string _mainScene;
		[FormerlySerializedAs("TabletAppDatas")] public IdGroup _tabletAppDatas;
		//public TabletAppData[] TabletAppDatas;
		[FormerlySerializedAs("LinkedScenes")] public SceneSettings[] _linkedScenes;

		public string Version => $"{_major}.{_minor}.{_patch}.{_build}";
	}
}