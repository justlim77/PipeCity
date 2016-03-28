using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayButton : MonoBehaviour, IPointerDownHandler {

	public static bool reduceVolume = false;

    [SerializeField] Fader fader;

	IEnumerator ChangeLevel()
	{
		float fadeTime = fader.BeginFade(1);
		reduceVolume = true;
		yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	#region IPointerDownHandler implementation
	public void OnPointerDown (PointerEventData eventData)
	{
		if(eventData.button == PointerEventData.InputButton.Left)
			StartCoroutine(ChangeLevel());
	}
	#endregion
}