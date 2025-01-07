using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(fileName = "GameplaySettings", menuName = "Gameplay/Gameplay Settings")]
    public class GameplaySettings : ScriptableObject
    {
        public float ShotMinRadius => PlayerMinRadius;
        public float ShotMaxRadius => PlayerInitialRadius;
        public float ExplosionMinRadius => PlayerMinRadius;
        
        [Tooltip("The radius of the Player Sphere below which we're losing. Shots and Explosions also will start growing from this radius.")]
        public float PlayerMinRadius = 0.3f;
        public float PlayerInitialRadius = 2f;
        public Color PositiveColor = Color.green;
        public Color NegativeColor = Color.red;

        public float ExplosionMaxRadius = 5f;
        public AnimationCurve ExplosionPowerCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        
        public float ShotChargeRate = 0.05f;
        public float ShotSpeed = 10f;
        
        [Tooltip("The distance to obstacles in addition to the 3x player's radius (2 for possible max shot size + 1 for the player itself).")]
        public float PlayerExtraSafeDistance = 1f;
        public float PlayerMovementSpeed = 5f;

        public LayerMask ObstacleLayerMask;
    }
}