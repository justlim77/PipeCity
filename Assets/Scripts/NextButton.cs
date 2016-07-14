using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class NextButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler {

	public static int helpPage;

	public GameObject help1;
	public GameObject help2;
	public GameObject help3;
	public GameObject help4;
	public GameObject help5;
	public GameObject help6;
	public GameObject help7;
	public GameObject help8;
	public GameObject help9;
	public GameObject help10;
	public GameObject help11;
	public GameObject help12;
	public GameObject help13;
	public GameObject help14;

	public GameObject page1;
	public GameObject page2;
	public GameObject page3;
	public GameObject page4;
	public GameObject page5;
	public GameObject page6;
	public GameObject page7;
	public GameObject page8;
	public GameObject page9;
	public GameObject page10;
	public GameObject page11;
	public GameObject page12;
	public GameObject page13;
	public GameObject page14;

	public AudioSource audioSource;
	public AudioClip buttonHoverSound;
	public AudioClip buttonClickSound;
	public AudioClip buttonDisabledSound;

	// Use this for initialization
	void Start () 
	{
		helpPage = 1;
	}
	
	// Update is called once per frame
	void Update () 
	{
		OnButtonPress ();

		if(helpPage == 1)
		{
			help1.SetActive(true);
			help2.SetActive(false);
			help3.SetActive(false);
			help4.SetActive(false);
			help5.SetActive(false);
			help6.SetActive(false);
			help7.SetActive(false);
			help8.SetActive(false);
			help9.SetActive(false);
			help10.SetActive(false);
			help11.SetActive(false);
			help12.SetActive(false);
			help13.SetActive(false);
			help14.SetActive(false);

			page1.SetActive(true);
			page2.SetActive(false);
			page3.SetActive(false);
			page4.SetActive(false);
			page5.SetActive(false);
			page6.SetActive(false);
			page7.SetActive(false);
			page8.SetActive(false);
			page9.SetActive(false);
			page10.SetActive(false);
			page11.SetActive(false);
			page12.SetActive(false);
			page13.SetActive(false);
			page14.SetActive(false);
		}
		if(helpPage == 2)
		{
			help1.SetActive(false);
			help2.SetActive(true);
			help3.SetActive(false);

			page1.SetActive(false);
			page2.SetActive(true);
			page3.SetActive(false);
		}
		if(helpPage == 3)
		{
			help2.SetActive(false);
			help3.SetActive(true);
			help4.SetActive(false);

			page2.SetActive(false);
			page3.SetActive(true);
			page4.SetActive(false);
		}
		if(helpPage == 4)
		{
			help3.SetActive(false);
			help4.SetActive(true);
			help5.SetActive(false);

			page3.SetActive(false);
			page4.SetActive(true);
			page5.SetActive(false);
		}
		if(helpPage == 5)
		{
			help4.SetActive(false);
			help5.SetActive(true);
			help6.SetActive(false);

			page4.SetActive(false);
			page5.SetActive(true);
			page6.SetActive(false);
		}
		if(helpPage == 6)
		{
			help5.SetActive(false);
			help6.SetActive(true);
			help7.SetActive(false);

			page5.SetActive(false);
			page6.SetActive(true);
			page7.SetActive(false);
		}
		if(helpPage == 7)
		{
			help6.SetActive(false);
			help7.SetActive(true);
			help8.SetActive(false);

			page6.SetActive(false);
			page7.SetActive(true);
			page8.SetActive(false);
		}
		if(helpPage == 8)
		{
			help7.SetActive(false);
			help8.SetActive(true);
			help9.SetActive(false);

			page7.SetActive(false);
			page8.SetActive(true);
			page9.SetActive(false);
		}
		if(helpPage == 9)
		{
			help8.SetActive(false);
			help9.SetActive(true);
			help10.SetActive(false);

			page8.SetActive(false);
			page9.SetActive(true);
			page10.SetActive(false);
		}
		if(helpPage == 10)
		{
			help9.SetActive(false);
			help10.SetActive(true);
			help11.SetActive(false);

			page9.SetActive(false);
			page10.SetActive(true);
			page11.SetActive(false);
		}
		if(helpPage == 11)
		{
			help10.SetActive(false);
			help11.SetActive(true);
			help12.SetActive(false);

			page10.SetActive(false);
			page11.SetActive(true);
			page12.SetActive(false);
		}
		if(helpPage == 12)
		{
			help11.SetActive(false);
			help12.SetActive(true);
			help13.SetActive(false);

			page11.SetActive(false);
			page12.SetActive(true);
			page13.SetActive(false);
		}
		if(helpPage == 13)
		{
			help12.SetActive(false);
			help13.SetActive(true);
			help14.SetActive(false);

			page12.SetActive(false);
			page13.SetActive(true);
			page14.SetActive(false);
		}
		if(helpPage == 14)
		{
			help13.SetActive(false);
			help14.SetActive(true);

			page13.SetActive(false);
			page14.SetActive(true);
		}

		if(helpPage > 13)
		{
			this.GetComponent<Button>().interactable = false;
		}
		if(helpPage < 14)
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
			helpPage += 1;
			if(helpPage > 13)
			{
				helpPage = 14;
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
		if (Input.GetKeyDown (KeyCode.RightArrow)) {

			helpPage += 1;

			if (helpPage > 13) {
				helpPage = 14;
			}
		}
	}

}
