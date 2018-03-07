using System.Collections;
using System;
using UnityEngine;
using UnityEngine.AI;

public class ChickenController : MonoBehaviour {

    public GameObject food;
    public TMPro.TextMeshPro lastFed;

    private NavMeshAgent nav;
    private Animator anim;

    private bool feeding;

	// Use this for initialization
	void Start () {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        nav.updateRotation = false;
        lastFed.text = "Last Fed: " + PlayerPrefs.GetString("LastFedTime", "12/12/2000");
	}
	
	// Update is called once per frame
	void Update () {
		if (!feeding)
        {
            if (Input.touchCount > 0)
            {
                Vector3 targetPos = Input.GetTouch(0).position;
                Ray ray = Camera.main.ScreenPointToRay(targetPos);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100f))
                {
                    SetDestinationAndRotation(hit.point);
                }
            }
        }
	}

    private void SetDestinationAndRotation(Vector3 target)
    {
        target = new Vector3(Mathf.Clamp(target.x, -1f, 1f), target.y, Mathf.Clamp(target.z, -1f, 1f));
        nav.SetDestination(target);
        Vector3 rot = Quaternion.LookRotation(target).eulerAngles;
        rot = new Vector3(0, rot.y, 0);
        transform.rotation = Quaternion.RotateTowards(transform.rotation,
            Quaternion.Euler(rot), 360f);
    }

    public void StartFeeding()
    {
        if (AppManager.instance.GetCurrency() >= 5)
        {
            feeding = true;
            AppManager.instance.AddToCurrency(-5);
            Feed();
            PlayerPrefs.SetString("LastFedTime", DateTime.Now.ToShortDateString());
            lastFed.text = "Last Fed: " + PlayerPrefs.GetString("LastFedTime", "error that should never be seen");
        }
    }

    

    private void Feed()
    {
        GameObject[] foods = new GameObject[5];
        for (int i = 0; i < 5; i++)
        {
            float x = UnityEngine.Random.Range(-1f, 1f);
            float y = -3.8f;
            float z = UnityEngine.Random.Range(-1f, 1f);
            foods[i] = Instantiate(food, new Vector3(x, y, z), Quaternion.identity);
        }

        StartCoroutine(FinishFood(foods));
    }

    IEnumerator FinishFood(GameObject[] foods)
    {
        for (int i = 0; i < foods.Length; i++)
        {
            yield return StartCoroutine(ChaseAndEat(foods[i]));
            Debug.Log("Finished food");
            yield return null;
            Destroy(foods[i]);
        }
        feeding = false;
        StopAllCoroutines();
    }

    IEnumerator ChaseAndEat(GameObject food)
    {
        SetDestinationAndRotation(food.transform.position);
        yield return null;
        anim.SetBool("Feeding", true);
        yield return new WaitForSeconds(2.917f);
        Debug.Log("fed");
        anim.SetBool("Feeding", false);
        yield return null;
    }
}
