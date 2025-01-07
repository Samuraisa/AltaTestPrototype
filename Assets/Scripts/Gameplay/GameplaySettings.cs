using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(fileName = "GameplaySettings", menuName = "Gameplay/Gameplay Settings")]
    public class GameplaySettings : ScriptableObject
    {
        public float ShotMinRadius => PlayerMinRadius;
        public float ShotMaxRadius => PlayerInitialRadius;
        
        [Tooltip("The radius of the Player Sphere below which we're losing. Shots also will start growing from this radius.")]
        public float PlayerMinRadius = 0.3f;
        public float PlayerInitialRadius = 2f;
        public Color PositiveColor = Color.green;
        public Color NegativeColor = Color.red;

        public AnimationCurve ExplosionPowerCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        [Range(1f, 2f)]
        public float TotalExplosionPowerMultiplier = 1.2f;
        
        public float ShotChargeRate = 0.05f;
        public float ShotSpeed = 10f;

        public LayerMask ObstacleLayerMask;
    }
}