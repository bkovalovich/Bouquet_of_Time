using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Bouquet
{
    public class PlayerObject : MonoBehaviour
    {
        public int playerNumber = -1;
        [SerializeField] PlayerInfoSO _playerinfo;
        [SerializeField] InputSO _input;
        [SerializeField] PlayerCameraSetup playerCam;

        [SerializeField] LayerMask cameraStartLayer;

        public UnityEvent<PlayerInfoSO, InputSO> playerInstantiatedEvent;

        private void Awake()
        {
            if(playerNumber == -1)
            {
                playerNumber = 1;
            }
            _playerinfo = ScriptableObject.CreateInstance<PlayerInfoSO>();
            _playerinfo.playerNumber = playerNumber;
            _playerinfo.camera = playerCam._camera;
            _input = ScriptableObject.CreateInstance<InputSO>();
            Debug.Log($"NewLayer {(int)Mathf.Log(cameraStartLayer, 2) + (playerNumber - 1)}");
            playerCam.gameObject.layer = (int)Mathf.Log(cameraStartLayer, 2) + (playerNumber - 1);
            playerCam.playerInfo = _playerinfo;
        }

        private void Start()
        {
            GameObject character = transform.GetChild(0).gameObject;
            character.SetActive(true);
            playerInstantiatedEvent?.Invoke(_playerinfo, _input);
            character.SetActive(false);
            character.SetActive(true);
        }

        public void SetPlayerNumber(int value)
        {
            playerNumber = value;
        }
    }
}
