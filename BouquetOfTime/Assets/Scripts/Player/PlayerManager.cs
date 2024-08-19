using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bouquet
{
    public class PlayerManager : MonoBehaviour
    {
        public List<GameObject> Players;

        private void Awake()
        {
            Players = new List<GameObject>();
        }

        public void PlayerJoined(PlayerInput player)
        {
            Players.Add(player.gameObject);
            PlayerObject playerObject = player.GetComponent<PlayerObject>();
            playerObject.SetPlayerNumber(Players.Count);
        }

    }
}
