using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyPlanManager : MonoBehaviour {

    public Transform listHolder;
    public GameObject completionText;
    private List<GameObject> plans = new List<GameObject>();
    private List<bool> planObjs = new List<bool>();
    private bool toggled;
    private int index = 0;

    private Coroutine currentCoroutine;

	// Use this for initialization
	void Start () {
        completionText.SetActive(false);
        LoadPlan();
    }
	

    public void ToggleObjective(System.Boolean toggled, GameObject obj)
    {
        this.toggled = toggled;
        if (toggled)
        {
            planObjs.Add(true);
            currentCoroutine = StartCoroutine(DeletePlan(obj));
        }
        else
        {
            StopCoroutine(currentCoroutine);
            planObjs.RemoveAt(0);
        }
        CheckAllComplete();
    }

    public void SetImageBackground(Image background)
    {
        if (!toggled)
        {
            background.color = new Color(126f / 255f, 173f / 255f, 157f / 255f, 1f);
        }
        else
        {
            background.color = new Color(103f / 255f, 223f / 255f, 129f / 255f, 1f);
        }
    }

    public void SpawnNewListItem()
    {
        if (index > 7)
        {
            return;
        }
        GameObject planObj = Instantiate(Resources.Load("DailyPlanObject", typeof(GameObject)) as GameObject,
            FigureOutPosition(index), Quaternion.identity, listHolder);
        plans.Add(planObj);
        Toggle toggle = planObj.GetComponentInChildren<Toggle>();
        toggle.onValueChanged.AddListener(delegate { ToggleObjective(toggle.isOn, planObj); });
        toggle.onValueChanged.AddListener(delegate {
            SetImageBackground(planObj.transform.GetChild(0).GetComponent<Image>()); });
        index++;
    }

    private Vector3 FigureOutPosition(int i)
    {
        return new Vector3(listHolder.position.x, i * -150 + listHolder.position.y, listHolder.position.z);
    }

    public void BackToMain()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }

    private bool CheckAllComplete()
    {
        bool complete = index == planObjs.Count;
        if (complete)
        {
            completionText.SetActive(true);
            AppManager.instance.AddToCurrency(5);
            index = 0;
        }
        else
            completionText.SetActive(false);
        return complete;
    }

    private IEnumerator DeletePlan(GameObject objToRemove)
    {
        yield return new WaitForSeconds(2);
        plans.Remove(objToRemove);
        Destroy(objToRemove);
        index = listHolder.childCount;
        for (int i = 0; i < listHolder.childCount; i++)
        {
            listHolder.GetChild(i).position = FigureOutPosition(i);
        }
    }

    public void OpenKeyboard(System.String str)
    {
        TouchScreenKeyboard.Open(str, TouchScreenKeyboardType.Default);
    }

    public void SavePlan()
    {
        PlayerPrefs.SetInt("PlanCount", listHolder.childCount);
        for (int i = 0; i < listHolder.childCount; i++)
        {
            PlayerPrefs.SetString(i + "text",
                listHolder.GetChild(i).GetComponentInChildren<TMPro.TMP_InputField>().text);
        }
    }

    public void LoadPlan()
    {
        int numberOfPlans = PlayerPrefs.GetInt("PlanCount", 0);
        if (numberOfPlans == 0)
        {
            SpawnNewListItem();
            return;
        }
        for (int i = 0; i < numberOfPlans; i++)
        {
            SpawnNewListItem();
            listHolder.GetChild(i).GetComponentInChildren<TMPro.TMP_InputField>().text =
                PlayerPrefs.GetString(i + "text", "We messed up saving this sorry");
        }
    }
}
