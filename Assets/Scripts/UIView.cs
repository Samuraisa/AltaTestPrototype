using UnityEngine;

public class UIView : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private GameObject gameplayUI;
    
    public void ShowMainMenu()
    {
        Show(mainMenu);
    }

    public void ShowWinScreen()
    {
        Show(winScreen);
    }
    
    public void ShowLoseScreen()
    {
        Show(loseScreen);
    }

    public void ShowGameplayUI()
    {
        Show(gameplayUI);
    }

    private void Show(GameObject activeScreen)
    {
        mainMenu.SetActive(mainMenu == activeScreen);
        winScreen.SetActive(winScreen == activeScreen);
        loseScreen.SetActive(loseScreen == activeScreen);
        gameplayUI.SetActive(gameplayUI == activeScreen);
    }
}
