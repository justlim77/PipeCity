using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class HelpXButton : MonoBehaviour, IPointerUpHandler, IPointerEnterHandler {

	public AudioSource audioSource;
	public AudioClip buttonHoverSound;
	public AudioClip buttonClickSound;

	public void OnPointerUp (PointerEventData eventData)
	{
		if(eventData.button == PointerEventData.InputButton.Left)
		{
			audioSource.PlayOneShot(buttonClickSound);
			NextButton.helpPage = 1;
		}
	}

	public void OnPointerEnter (PointerEventData eventData)
	{
		if(eventData.button == PointerEventData.InputButton.Left)
		{
			audioSource.PlayOneShot(buttonHoverSound);
		}
	}
	
}
