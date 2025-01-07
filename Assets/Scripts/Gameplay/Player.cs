using Extensions;
using Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private GameFlow gameFlow;
        [SerializeField] private GameplaySettings settings;
        [SerializeField] private Sphere sphere;
        [SerializeField] private MaterialColorSetter colorSetter;
        [SerializeField] private Image healthBar;

        public float Radius
        {
            get => sphere.Radius;
            set => SetRadius(value);
        }
        
        public float ConsumableRadius => Radius - settings.PlayerMinRadius;


        public void Initialize()
        {
            SetRadius(settings.PlayerInitialRadius);
            transform.position = Vector3.zero.WithY(Radius);
        }

        private void SetRadius(float newRadius)
        {
            sphere.Radius = newRadius;
            transform.position = transform.position.WithY(newRadius);
            var radiusProgress = Mathf.InverseLerp(settings.PlayerMinRadius, settings.PlayerInitialRadius, Radius);

            var color = Color.Lerp(settings.NegativeColor, settings.PositiveColor, radiusProgress);
            colorSetter.SetColor(color);
            
            healthBar.fillAmount = radiusProgress;
            healthBar.color = color;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(Consts.FinishTag))
                gameFlow.SetWinState();
        }
    }
}