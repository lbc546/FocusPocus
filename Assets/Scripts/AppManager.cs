using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : MonoBehaviour {

    #region Singleton
    public static AppManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
#endregion

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddToCurrency(int value)
    {
        int newCurrencyAmount = Mathf.Clamp(PlayerPrefs.GetInt("Currency", 0) + value, 0, 999);
        PlayerPrefs.SetInt("Currency", newCurrencyAmount);
    }

    public int GetCurrency()
    {
        return PlayerPrefs.GetInt("Currency", 0);
    }

    public void BackToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }
}
