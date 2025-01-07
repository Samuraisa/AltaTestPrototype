using UnityEngine;

namespace Alta.Gameplay
{
    [CreateAssetMenu(fileName = "GameplayData", menuName = "Gameplay/GameplayData", order = 0)]
    public class GameplayData : ScriptableObject
    {
        public float MaxExplosionRadius;
        public Vector3 ShotDirectionNormalized;
        public Vector3 ShotFinalPosition;

        private void OnDestroy()
        {
            Reset();
        }

        public void Reset()
        {
            MaxExplosionRadius = 0f;
            ShotDirectionNormalized = Vector3.zero;
            ShotFinalPosition = Vector3.zero;
        }
    }
}