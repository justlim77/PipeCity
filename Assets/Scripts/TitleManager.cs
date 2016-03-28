using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleManager : MonoBehaviour {

	[SerializeField] Fader fader;

	void Update()
	{
		if(Input.anyKeyDown)
			StartCoroutine(ChangeLevel());
	}

	IEnumerator ChangeLevel()
	{
		float fadeTime = fader.BeginFade(1);
		yield return new WaitForSeconds(fadeTime);
		Application.LoadLevel(Application.loadedLevel + 1);
	}
	
}
