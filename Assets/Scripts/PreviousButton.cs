using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PreviousButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler {

	public AudioSource audioSource;
	public AudioClip buttonHoverSound;
	public AudioClip buttonClickSound;

	// Update is called once per frame
	void Update () 
	{
		OnButtonPress ();

		if(NextButton.helpPage < 2)
		{
			this.GetComponent<Button>().interactable = false;
		}
		else
		{
			this.GetComponent<Button>().interactable = true;
		}
	}

	public void OnPointerDown (PointerEventData eventData)
	{
		if(eventData.button == PointerEventData.InputButton.Left)
		{
			if(this.GetComponent<Button>().interactable == true)
			{
                AudioManager.Instance.PlaySFX(buttonClickSound);
			}
			NextButton.helpPage -= 1;
			if(NextButton.helpPage < 2)
			{
				NextButton.helpPage = 1;
			}
		}
	}

	public void OnPointerEnter (PointerEventData eventData)
	{
		if(this.GetComponent<Button>().interactable == true)
		{
            AudioManager.Instance.PlaySFX(buttonHoverSound);
		}
	}

	public void OnButtonPress ()
	{
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			
			NextButton.helpPage -= 1;

			if(NextButton.helpPage < 2) {
				NextButton.helpPage = 1;
			}
		}
	}
}
