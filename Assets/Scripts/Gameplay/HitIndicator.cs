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
            var progress = settings.ExplosionPowerCurve.Evaluate(shot.PowerProgress);
            var scale = Mathf.Lerp(settings.ExplosionMinRadius, settings.ExplosionMaxRadius, progress) * 2f;;
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}