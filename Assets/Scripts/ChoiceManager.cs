using UnityEngine;
using System.IO;
using System;

public class ChoiceManager : MonoBehaviour {

    // An event to shoutout when the user clicks Yes/No
    public event Action ChoiceMade;

    // Yes = 0; No = 1
    public void Choice(int answer)
    {
        string response = (answer == 0) ? "Yes" : "No";
        SaveDataContainer container = SaveDataContainer.Load(Path.Combine(Application.persistentDataPath, "responses.xml"));
        container.Responses.Add(new ResponseData(response, DateTime.Now));
        container.Save(Path.Combine(Application.persistentDataPath, "responses.xml"));
        // Call the event if there are methods subscribed to it.
        if (ChoiceMade != null)
            ChoiceMade();
    }

}

// A simple struct to hold save data that can be added on to later.
public struct ResponseData
{
    // Yes/No
    public string response;
    // Keep track of Date and Time for Calendar purposes.
    public DateTime timeOfResponse;

    // Constructor. 
    public ResponseData(string response, DateTime timeOfResponse)
    {
        this.response = response;
        this.timeOfResponse = timeOfResponse;
    }
}
