using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class PlayButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler {

	public static bool reduceVolume = false;
	public AudioSource audioSource;
	public AudioClip buttonHoverSound;
	public AudioClip buttonClickSound;

    [SerializeField] Fader fader;

	public void OnPointerEnter (PointerEventData eventData)
	{
		audioSource.PlayOneShot(buttonHoverSound, 0.5f);
	}

	IEnumerator ChangeLevel()
	{
		float fadeTime = fader.BeginFade(1);
		reduceVolume = true;
		yield return new WaitForSeconds(fadeTime);
		Application.LoadLevel(Application.loadedLevel + 1);
	}

	#region IPointerDownHandler implementation
	public void OnPointerDown (PointerEventData eventData)
	{
		if(eventData.button == PointerEventData.InputButton.Left)
		{
			audioSource.PlayOneShot(buttonClickSound);
			StartCoroutine(ChangeLevel());
		}
	}
	#endregion
}