using JetBrains.Annotations;
using UnityEngine;

namespace Utils
{
    public class QuitApp : MonoBehaviour
    {
        [UsedImplicitly]
        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }
}
