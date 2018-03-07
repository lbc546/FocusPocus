using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateCurrencyText : MonoBehaviour {

    TMPro.TextMeshProUGUI text;

	// Use this for initialization
	void Start () {
        text = GetComponent<TMPro.TextMeshProUGUI>();
	}
	
	// Update is called once per frame
	void Update () {
        text.text = AppManager.instance.GetCurrency().ToString();
	}
}
