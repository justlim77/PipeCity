using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeMain : MonoBehaviour {

	Fader fader;
	public static bool reduceVolume = false;

	void Awake ()
	{
		fader = GameObject.Find ("Managers").GetComponent<Fader> ();
	}

	public IEnumerator ChangeLevel(int level)
	{
		float fadeTime = fader.BeginFade(1);
		reduceVolume = true;
		yield return new WaitForSeconds(fadeTime);
		Application.LoadLevel(Application.loadedLevel + level);
	}
	
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.W))
		{
			StartCoroutine(ChangeLevel(1));
		}
	}
}
