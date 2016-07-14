using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GridLayout bronzeGrid;
    public GridLayout silverGrid;

    public Inventory inventory;

	void Awake ()
	{
        if (Instance == null)
            Instance = this;
	}

    void OnDestroy()
    {
        Instance = null;
    }
	
	void Update()
	{
		if (TimeManager.EndYear)
			CheckGameWin ();
	}

	void CheckGameWin()
	{
		if (UpgradeManager.AllUpgradesPurchased()) {
			StartCoroutine (FadeMain.ChangeLevel (1));
		} else
			StartCoroutine (FadeMain.ChangeLevel (2));
	}
}
