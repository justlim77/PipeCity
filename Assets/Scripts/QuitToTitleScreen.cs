using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class QuitToTitleScreen : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler {

	public GameObject audioManagerPrefab;
    public Fader fader;

	public AudioSource audioSource;
	public AudioClip buttonHoverSound;
	public AudioClip buttonClickSound;

	void Start()
	{
		audioManagerPrefab = GameObject.Find ("AudioManager");
	}

	IEnumerator ChangeLevel()
	{
		PlayButton.reduceVolume = true;
		float fadeTime = fader.BeginFade(1);
		yield return new WaitForSeconds(fadeTime);
		Destroy (audioManagerPrefab);
		Application.LoadLevel(0);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if(eventData.button == PointerEventData.InputButton.Left)
		{
			audioSource.PlayOneShot(buttonClickSound, 0.5f);
			StartCoroutine(ChangeLevel());
		}
	}

	public void OnPointerEnter (PointerEventData eventData)
	{
		audioSource.PlayOneShot(buttonHoverSound, 0.5f);
	}
}
