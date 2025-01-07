using UnityEngine;

namespace Alta.Utils
{
    public class MaterialColorSetter : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private string _colorPropertyName = "_BaseColor";

        private int _colorPropertyId;
        private MaterialPropertyBlock _propertyBlock;

        private void Awake()
        {
            if (_meshRenderer == null)
                _meshRenderer = GetComponent<MeshRenderer>();

            _colorPropertyId = Shader.PropertyToID(_colorPropertyName);
            _propertyBlock = new MaterialPropertyBlock();
        }
        
        public void SetColor(Color color)
        {
            _propertyBlock.SetColor(_colorPropertyId, color);
            _meshRenderer.SetPropertyBlock(_propertyBlock);
        }
    }
}
