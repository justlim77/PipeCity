using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	void Awake ()
	{
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
