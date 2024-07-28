using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bouquet
{
    public class PlayerCameraSetup : MonoBehaviour
    {
        [SerializeField] Camera _camera;
        public CinemachineVirtualCameraBase[] VirtualCameras;

        public PlayerInfoSO playerInfo;

        private void OnEnable()
        {
            if(_camera == null) { _camera = GetComponent<Camera>(); }
            _camera.cullingMask |= (1 << gameObject.layer);
            if(VirtualCameras == null) { gameObject.SetActive(false); return; }
            foreach(var cam in VirtualCameras)
            {
                if (cam == null) { continue; }

                cam.gameObject.layer = gameObject.layer;
                cam.GetComponent<CinemachineInputProvider>().PlayerIndex = playerInfo.playerNumber - 1;
            }
        }
    }
}
