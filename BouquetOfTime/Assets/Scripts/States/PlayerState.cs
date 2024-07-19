using Bouquet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Bouquet
{
    public abstract class PlayerState : State
    {
        [SerializeField] protected InputSO input;

        public PlayerInfoSO playerInfo;

        protected Rigidbody rb => playerInfo.rb;
    }
}
