using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateResponses : MonoBehaviour {

    public GameObject parent;
    public GameObject responsePrefab;

	// Use this for initialization
	void Start () {
        responsePrefab = Resources.Load("ResponseData", typeof(GameObject)) as GameObject;
        List<ResponseData> responses = SaveDataContainer.Load(
            Path.Combine(Application.persistentDataPath, "responses.xml")).Responses;
        foreach (ResponseData response in responses)
        {
            GameObject instance = Instantiate(responsePrefab,
                Vector3.zero, Quaternion.identity, parent.transform);
            instance.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = response.timeOfResponse.ToShortDateString();
            instance.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = response.response;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
