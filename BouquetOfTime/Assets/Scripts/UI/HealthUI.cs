using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bouquet
{
    public class HealthUI : MonoBehaviour
    {
        public HealthComponent healthComponent;

        [SerializeField] Image image;

        private void OnEnable()
        {
            healthComponent.OnHealthChange.AddListener(UpdateVisuals);
        }

        public void UpdateVisuals(float value)
        {
            image.fillAmount = value / healthComponent.MaxHealth;
        }

        private void OnDisable()
        {
            healthComponent.OnHealthChange.RemoveListener(UpdateVisuals);
        }

    }
}
