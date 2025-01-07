using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils
{
    public class Restarter : MonoBehaviour
    {
        [UsedImplicitly]
        public void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
