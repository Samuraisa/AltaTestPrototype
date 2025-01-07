using Alta.Gameplay;
using Alta.Utils;
using UnityEngine;

namespace Gameplay
{
    public class Shot : MonoBehaviour
    {
        [SerializeField] private GameplaySettings settings;
        [SerializeField] private Sphere sphere;
        [SerializeField] private MaterialColorSetter colorSetter;
        [SerializeField] private GameplayData gameplayData;

        public float Radius
        {
            get => sphere.Radius;
            set => SetRadius(value);
        }
        
        public float Power => Radius - settings.ShotMinRadius;
        public float PowerProgress => Mathf.InverseLerp(settings.ShotMinRadius, settings.ShotMaxRadius, Radius);


        private void SetRadius(float radius)
        {
            sphere.Radius = radius;

            var radiusProgress = Mathf.InverseLerp(settings.ShotMinRadius, settings.ShotMaxRadius, radius);
            var powerModifier = settings.ExplosionPowerCurve.Evaluate(radiusProgress) - radiusProgress;
            var powerModifierProgress = Mathf.InverseLerp(gameplayData.MinShotPowerModifier,
                gameplayData.MaxShotPowerModifier, powerModifier);
            var color = Color.Lerp(settings.NegativeColor, settings.PositiveColor, powerModifierProgress);
            colorSetter.SetColor(color);
        }
    }
}