using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KSPInputLock {

    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class KSPInputLock : MonoBehaviour {

        private IButton lockButton;
        private bool lockState = false;

        private void toggleState() {
            if (lockState) {
                lockState = false;
                if (InputLockManager.GetControlLock("KSPInputLock") == ControlTypes.All)
                    InputLockManager.RemoveControlLock("KSPInputLock");
                lockButton.ToolTip = "Lock ship controls";
                lockButton.TexturePath = "KSPInputLock/Icons/unlocked";
            } else {
                lockState = true;
                if (InputLockManager.GetControlLock("KSPInputLock") != ControlTypes.All)
                    InputLockManager.SetControlLock(ControlTypes.All, "KSPInputLock");
                lockButton.ToolTip = "Unlock ship controls";
                lockButton.TexturePath = "KSPInputLock/Icons/locked";
            }
        }

        public void Start() {
            if (!ToolbarManager.ToolbarAvailable) return;
            lockButton = ToolbarManager.Instance.add("KSPInputLock", "lockbutton");
            lockButton.Text = "Input Lock";
            lockButton.ToolTip = "Lock ship controls";
            lockButton.TexturePath = "KSPInputLock/Icons/unlocked";
            lockButton.Visibility = new GameScenesVisibility(GameScenes.FLIGHT);
            lockButton.OnClick += e => toggleState();
        }

        internal void OnDestroy() {
            if (lockButton != null) lockButton.Destroy();
        }
    }
}
