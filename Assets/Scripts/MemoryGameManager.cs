using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryGameManager : MonoBehaviour {

    public GameObject tilePrefab;
    public Transform tileParent;
    public GameObject completeText;
    [Range(2, 12)]
    public int numberOfTiles;
    private int[] numbers;
    private GameObject[] tiles;
    private int flippedTilesNumber = 0;
    private int numberOfMatches;
    private GameObject flippedTile1;
    private GameObject flippedTile2;
    private bool gameComplete = false;
	// Use this for initialization
	void Start () {
        if (numberOfTiles % 2 != 0)
            numberOfTiles++;
        numbers = new int[numberOfTiles];
        tiles = new GameObject[numberOfTiles];
		for(int i = 0; i < numbers.Length / 2; i++)
        {
            numbers[i] = Random.Range(1, 100);
            numbers[i + numbers.Length / 2] = numbers[i];
        }
        completeText.SetActive(false);
        for (int i = 0; i < numberOfTiles; i++)
        {
            tiles[i] = Instantiate(tilePrefab);
            tiles[i].transform.SetParent(tileParent);
            tiles[i].GetComponent<RectTransform>().anchoredPosition = FigureOutPosition(i);
            //tiles[i].transform.localScale = Vector3.one * 2;
            tiles[i].transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = numbers[i].ToString();
            tiles[i].name = numbers[i].ToString();
            int count = i;
            tiles[i].transform.GetChild(0).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(
                delegate { FlipTile(tiles[count].transform.GetChild(0).gameObject); });
        }
	}

    private Vector3 FigureOutPosition(int index)
    {
        float x = -350f + (index % 3) * 350f;
        float y = 400f - (index / 3) * 400f;
        return new Vector3(x, y, 0f);
    }
	
	// Update is called once per frame
	void Update () {
		if (flippedTilesNumber == 2)
        {
            CheckMatch();
        }
        if (numberOfMatches == numberOfTiles / 2 && !gameComplete)
        {
            AppManager.instance.AddToCurrency(5);
            completeText.SetActive(true);
            gameComplete = true;
        }
	}

    void CheckMatch()
    {
        int first = int.Parse(flippedTile1.transform.parent.name);
        int second = int.Parse(flippedTile2.transform.parent.name);
        if (first == second)
        {
            Debug.Log("match");
            flippedTile1.GetComponent<UnityEngine.UI.Button>().interactable = false;
            flippedTile2.GetComponent<UnityEngine.UI.Button>().interactable = false;
            flippedTile1 = flippedTile2 = null;
            flippedTilesNumber = 0;
            numberOfMatches++;
        }
        else
        {
            StartCoroutine(WaitForABitToReset(1f));
        }
    }

    void FlipTile(GameObject tile)
    {
        if (flippedTilesNumber > 1)
            return;
        if (flippedTile1 != null && flippedTile1.GetInstanceID() == tile.GetInstanceID())
        {
            StartCoroutine(ResetTile(flippedTile1));
            flippedTilesNumber = 0;
            return;
        }
        tile.transform.parent.GetChild(1).gameObject.SetActive(true);
        //tile.transform.eulerAngles = new Vector3(0, 180, 0);
        StartCoroutine(RotateTile(Vector3.up * 180, tile));
        if (flippedTilesNumber == 1)
            flippedTile2 = tile;
        else
            flippedTile1 = tile;
        flippedTilesNumber++;
    }

    void ResetTiles()
    {
        StartCoroutine(ResetTile(flippedTile1));
        StartCoroutine(ResetTile(flippedTile2));
        flippedTilesNumber = 0;
    }

    IEnumerator ResetTile(GameObject tile)
    {
        tile.transform.parent.GetChild(1).gameObject.SetActive(false);
        yield return StartCoroutine(RotateTile(Vector3.zero, tile));
        tile = null;
    }

    private IEnumerator RotateTile( Vector3 end, GameObject tile)
    {
        while (Mathf.Abs(tile.transform.eulerAngles.y - end.y) > 0.1f)
        {
            tile.transform.eulerAngles = Vector3.Lerp(tile.transform.eulerAngles, end, .3f);
            yield return null;
        }
        tile.transform.eulerAngles = end;
    }
    
    private IEnumerator WaitForABitToReset(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ResetTiles();
    }

    public void BackToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }
}
