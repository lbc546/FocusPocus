using UnityEngine;

public class MenuManager : MonoBehaviour {

    // Get references to all the menus.
    public GameObject timeMenu;
    public GameObject choiceMenu;

	// Use this for initialization
	void Start () {
        ShowMenu(choiceMenu);
        // Subscribe our method to the Choice event to know when a choice is made.
        FindObjectOfType<ChoiceManager>().ChoiceMade += ChoiceFinished;
        ShowMenu(timeMenu);
        // Subscribe our method to the Timer event to know when the timer finishes.
        FindObjectOfType<Timer>().TimerFinished += TimerFinish;
	}

    public void BackToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }

    // Shows one menu and hides all others
    public void ShowMenu(GameObject menu)
    {
        HideMenu();
        menu.SetActive(true);
    }

    // Hides all Menus
    public void HideMenu()
    {
        timeMenu.SetActive(false);
        choiceMenu.SetActive(false);
    }

    // This method is called when the Timer finishes.
    private void TimerFinish()
    {
        ShowMenu(choiceMenu);
    }

    // This method is called when a choice is made. 
    private void ChoiceFinished()
    {
        ShowMenu(timeMenu);
    }
}
