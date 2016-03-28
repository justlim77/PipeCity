using UnityEngine;
using System.Collections;

public class EndGameManager : MonoBehaviour {

	public void BackToMain (bool isClicked)
	{
		if (isClicked)
			Application.LoadLevel (0);
	}
}
