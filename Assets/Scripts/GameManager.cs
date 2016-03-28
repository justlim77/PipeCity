using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	FadeMain fadeMain;

	UpgradeManager upgradeManager;
	TimeManager timeManager;

	void Awake ()
	{
		fadeMain = GameObject.Find ("Managers").GetComponent<FadeMain> ();
		upgradeManager = GameObject.Find ("UpgradeManager").GetComponent<UpgradeManager> ();
		timeManager = GameObject.Find ("TimeManager").GetComponent<TimeManager> ();
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Debug
		//CheckGameWin ();

		if (timeManager.currentYear >= timeManager.sustenanceYear) {
			CheckGameWin ();
		}

		
	}

	void CheckGameWin ()
	{
		if (UpgradeManager.catchmentUpgradeCount >= 3 && UpgradeManager.reclamationUpgradeCount >= 3 && 
			UpgradeManager.desalinationUpgradeCount >= 3) {
			StartCoroutine (fadeMain.ChangeLevel (1));
		} else {
			StartCoroutine (fadeMain.ChangeLevel (2));
		}
	}

}
