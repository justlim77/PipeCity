using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class QuitButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler {

	public AudioSource audioSource;
	public AudioClip buttonHoverSound;
	public AudioClip buttonClickSound;
	
	public void OnPointerEnter (PointerEventData eventData)
	{
		audioSource.PlayOneShot(buttonHoverSound, 0.5f);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if(eventData.button == PointerEventData.InputButton.Left)
		{
			audioSource.PlayOneShot(buttonClickSound, 0.5f);
			Application.Quit();
		}
	}
}
