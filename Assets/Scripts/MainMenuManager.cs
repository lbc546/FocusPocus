using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    public Slider loadingBar;

    // Load a scene using the laoding bar because it looks super spazzy.
    public void LoadScene(int index)
    {
        StartCoroutine(LoadLevel(SceneManager.LoadSceneAsync(index)));
    }

    // Override method to use scene names instead of indices
    public void LoadScene(string name)
    {
        StartCoroutine(LoadLevel(SceneManager.LoadSceneAsync(name)));
    }

    // Load scenes without a loading bar in case you're too lazy to set up
    // a loading bar like I am.
    public void LoadSceneNoLoadingBar(int index)
    {
        SceneManager.LoadScene(index);
    }

    // Override to use scene names instead of numbers
    public void LoadSceneNoLoadingBar(string name)
    {
        SceneManager.LoadScene(name);
    }

    // Allows for a dynamic loading bar, which may not see any use
    // due to how small our scenes are.
    IEnumerator LoadLevel(AsyncOperation loading)
    {
        loadingBar.gameObject.SetActive(true);
        while (!loading.isDone)
        {
            loadingBar.value = loading.progress;
            yield return null;
        }
        Debug.Log("Done Loading");
    }
}
