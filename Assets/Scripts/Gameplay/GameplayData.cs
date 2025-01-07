using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(fileName = "GameplayData", menuName = "Gameplay/GameplayData", order = 0)]
    public class GameplayData : ScriptableObject
    {
        public float MinShotPowerModifier;
        public float MaxShotPowerModifier;
        public Vector3 ShotDirectionNormalized;
        public Vector3 ShotFinalPosition;
        public bool ObstacleWasHit;

        private void OnDestroy()
        {
            Reset();
        }

        public void Reset()
        {
            MinShotPowerModifier = 0f;
            MaxShotPowerModifier = 0f;
            ShotDirectionNormalized = Vector3.zero;
            ShotFinalPosition = Vector3.zero;
            ObstacleWasHit = false;
        }
    }
}