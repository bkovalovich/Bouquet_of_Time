using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Bouquet
{
    public class HealthComponent : MonoBehaviour
    {
        public UnityEvent<float> OnHealthChange;

        public float CurrentHealth { get { return _currentHealth; } set { OnHealthChange?.Invoke(value);  _currentHealth = value; } }

        private float _currentHealth;

        public float MaxHealth;



        // Start is called before the first frame update
        void Start()
        {
            CurrentHealth = MaxHealth;
        }

        public void AddHealth(float value)
        {
            CurrentHealth += value;
        }

        public float GetHealth()
        {
            return CurrentHealth;
        }

        public void SetHealth(float value)
        {
            CurrentHealth = value;
        }
    }
}
