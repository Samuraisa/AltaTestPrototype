using Alta.Utils;
using Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace Alta.Gameplay
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private GameplaySettings settings;
        [SerializeField] private Sphere sphere;
        [SerializeField] private MaterialColorSetter colorSetter;
        [SerializeField] private Image healthBar;

        public float Radius
        {
            get => sphere.Radius;
            set => SetRadius(value);
        }

        private void SetRadius(float newRadius)
        {
            sphere.Radius = newRadius;
            transform.position = transform.position.WithY(newRadius);
            var radiusProgress = Radius / settings.PlayerInitialRadius;
            
            var color = Color.Lerp(settings.NegativeColor, settings.PositiveColor, radiusProgress);
            colorSetter.SetColor(color);
            
            healthBar.fillAmount = radiusProgress;
            healthBar.color = color;
        }
    }
}