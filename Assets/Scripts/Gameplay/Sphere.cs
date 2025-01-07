using UnityEngine;

namespace Alta.Gameplay
{
    public class Sphere : MonoBehaviour
    {
        private float _radius = 0.5f;
        
        public float Radius
        {
            get => _radius;
            set => SetRadius(value);
        }
        
        public float Volume
        {
            get => GetVolume(_radius);
            set => SetRadius(GetRadius(value));
        }

        private void Awake()
        {
            _radius = transform.localScale.x * 0.5f;
        }
        
        private void SetRadius(float radius)
        {
            _radius = radius;
            var diameter = _radius * 2f;
            transform.localScale = new Vector3(diameter, diameter, diameter);
        }

        private static float GetVolume(float radius)
        {
            return 4f / 3f * Mathf.PI * radius * radius * radius;
        }
        
        private static float GetRadius(float volume)
        {
            return Mathf.Pow((3f * volume) / (4f * Mathf.PI), 1f / 3f);
        }
    }
}
