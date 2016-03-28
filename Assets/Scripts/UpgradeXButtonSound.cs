using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class UpgradeXButtonSound : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler {

	public AudioSource audioSource;
	public AudioClip buttonHoverSound;
	public AudioClip buttonClickSound;

	public void OnPointerEnter (PointerEventData eventData)
	{
		audioSource.PlayOneShot(buttonHoverSound);
	}
	
	public void OnPointerDown(PointerEventData eventData)
	{
		audioSource.PlayOneShot(buttonClickSound);
	}
}
