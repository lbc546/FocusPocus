using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class Timer : MonoBehaviour {

    private int minutes = 0;
    private int seconds = 0;

    public Button button;
    public TMPro.TextMeshProUGUI timeText;

    // Create an event. These are useful for when multiple things
    // may need to know when something happens. In this case,
    // we broadcast that the timer has finished, and any subscribed
    // methods will be invoked at that time.
    public event System.Action TimerFinished;

    // Used by the Input fields in the scene to set minutes from a dynamic string.
    public void SetMinutes(System.String value)
    {
        minutes = int.Parse(value);
    }

    // Used by the Input fields in the scene to set seconds from a dynamic string.
    public void SetSeconds(System.String value)
    {
        seconds = int.Parse(value);
    }

    // Called when the button in the scene is pressed. Starts the timer
    // With the inputted number of minutes and seconds.
    public void StartTimer()
    {
        if (minutes == 0 && seconds == 0)
        {
            return;
        }
        timeText.gameObject.SetActive(true);
        StartCoroutine(StartTimer(minutes * 60 + seconds));
        button.interactable = false;
    }

    // Use a Coroutine because the logic is simpler than if done in update.
    // Coroutines run like update but on a separate thread so that the whole
    // script doesn't stop while you're in a while loop.
    // This coroutine simply waits until the amount of specified seconds have
    // elapsed before re-enabling the button and invoking the event that the
    // timer has finished. 
    IEnumerator StartTimer(float seconds)
    {
        while (seconds > 0)
        {
            seconds -= Time.deltaTime;
            timeText.text = "Time: " + Mathf.Round(seconds * 100) / 100f + "s";
            yield return null;
        }
        button.interactable = true;
        timeText.gameObject.SetActive(false);
        if (TimerFinished != null)
            TimerFinished();
    }
}
