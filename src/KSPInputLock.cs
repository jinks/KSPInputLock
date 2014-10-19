using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KSPInputLock {

    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class KSPInputLock : MonoBehaviour {

        private IButton lockButton;
        private CameraManager cameraManager;
        private bool lockState = false;

        private KeyBinding rememberThrottleCutoffKey;
        private KeyBinding rememberThrottleFullKey;

        private void Lock() {
            if (lockState) return;

            lockState = true;
            cameraManager = CameraManager.Instance;
            cameraManager.enabled = false;
            InputLockManager.SetControlLock("KSPInputLock");

            // Some keys don't get locked by the LockManager :(
            // Fix stolen from kOS, thanks to Steven Mading for finding this out.
            // See http://forum.kerbalspaceprogram.com/threads/95378-HELP%21-Why-won-t-InputLockManager-lock-out-the-X-key?p=1452787&viewfull=1#post1452787
            // and https://github.com/KSP-KOS/KOS/blob/master/src/Screen/TermWindow.cs
            rememberThrottleCutoffKey = GameSettings.THROTTLE_CUTOFF;
            GameSettings.THROTTLE_CUTOFF = new KeyBinding(KeyCode.None);
            rememberThrottleFullKey = GameSettings.THROTTLE_FULL;
            GameSettings.THROTTLE_FULL = new KeyBinding(KeyCode.None);
        }

        private void Unlock() {
            if (!lockState) return;

            lockState = false;
            cameraManager = CameraManager.Instance;
            cameraManager.enabled = true;
            InputLockManager.RemoveControlLock("KSPInputLock");

            if (rememberThrottleCutoffKey != null)
                GameSettings.THROTTLE_CUTOFF = rememberThrottleCutoffKey;
            if (rememberThrottleFullKey != null)
                GameSettings.THROTTLE_FULL = rememberThrottleFullKey;
        }

        public void Start() {
            if (!ToolbarManager.ToolbarAvailable) return;
            lockButton = ToolbarManager.Instance.add("KSPInputLock", "lockbutton");
            lockButton.Text = "Input Lock";
            lockButton.ToolTip = "Lock ship controls";
            lockButton.TexturePath = "KSPInputLock/Icons/unlocked";
            lockButton.Visibility = new GameScenesVisibility(GameScenes.FLIGHT);
            lockButton.OnClick += e => {
                if (lockState) {
                    Unlock();
                    lockButton.ToolTip = "Lock ship controls";
                    lockButton.TexturePath = "KSPInputLock/Icons/unlocked";
                } else {
                    Lock();
                    lockButton.ToolTip = "Unlock ship controls";
                    lockButton.TexturePath = "KSPInputLock/Icons/locked";
                }
            };
        }

        internal void OnDestroy() {
            if (lockButton != null) lockButton.Destroy();
        }
    }
}
