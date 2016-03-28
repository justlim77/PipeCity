using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonSound : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler {

	public AudioSource audioSource;
	public AudioClip buttonHoverSound;
	public AudioClip buttonClickSound;
	
	public void OnPointerEnter (PointerEventData eventData)
	{
		audioSource.PlayOneShot(buttonHoverSound);
	}
	
	public void OnPointerDown (PointerEventData eventData)
	{
		audioSource.PlayOneShot(buttonClickSound);
	}
}
