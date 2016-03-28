using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndGameManager : MonoBehaviour {

	public void BackToMain (bool isClicked)
	{
		if (isClicked)
			SceneManager.LoadScene(0);
	}
}
