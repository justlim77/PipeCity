using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class HelpButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler {

	public AudioSource audioSource;
	public AudioClip buttonHoverSound;
	public AudioClip buttonClickSound;

	public void OnPointerEnter (PointerEventData eventData)
	{
		audioSource.PlayOneShot(buttonHoverSound, 0.5f);
	}

	public void OnPointerDown (PointerEventData eventData)
	{
		audioSource.PlayOneShot(buttonClickSound, 0.5f);
	}
}
