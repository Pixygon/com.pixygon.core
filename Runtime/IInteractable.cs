using UnityEngine;

namespace Pixygon.Core {
    public interface IInteractable {
        Tool RequiredTool { get; }
        int RequiredToolLevel { get; }
        bool NotInteractable { get; }
        string InteractAction { get; }
        Sprite InteractSprite { get; }

        void Interact(Tool tool, int toolLevel);
    }
}