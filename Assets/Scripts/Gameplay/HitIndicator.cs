using Gameplay;
using UnityEngine;

namespace Alta.Gameplay
{
    public class HitIndicator : MonoBehaviour
    {
        [SerializeField] private Shot shot;
        [SerializeField] private GameplaySettings settings;
        [SerializeField] private GameplayData gameplayData;

        private void LateUpdate()
        {
            GizmosDrawer.DrawSphere(gameplayData.ShotFinalPosition, 2, Color.blue);
            transform.position = gameplayData.ShotFinalPosition;
            var progress = Mathf.InverseLerp(settings.ShotMinRadius, settings.ShotMaxRadius, shot.Radius);
            var scale = Mathf.Lerp(settings.ShotMinRadius, gameplayData.MaxExplosionRadius, progress) * 2f;;
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}