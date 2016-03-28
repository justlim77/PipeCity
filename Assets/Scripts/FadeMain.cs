using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeMain : MonoBehaviour {

	public static bool reduceVolume = false;
	static Fader _fader;

	void Awake ()
	{
		_fader = GameObject.Find("Managers").GetComponent<Fader> ();
	}

	public static IEnumerator ChangeLevel(int level)
	{
		float fadeTime = _fader.BeginFade(1);
		reduceVolume = true;
		yield return new WaitForSeconds(fadeTime);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + level);
	}
	
	void Update()
	{
		if(Input.GetKeyUp(KeyCode.W))
			StartCoroutine(ChangeLevel(1));
	}
}
