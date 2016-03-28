using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class QuitToTitleScreen : MonoBehaviour, IPointerDownHandler {

    public Fader fader;

	IEnumerator ChangeLevel()
	{
		PlayButton.reduceVolume = true;
		float fadeTime = fader.BeginFade(1);
		yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(0);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if(eventData.button == PointerEventData.InputButton.Left)
			StartCoroutine(ChangeLevel());
	}
}
