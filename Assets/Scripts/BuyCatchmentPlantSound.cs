﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class BuyCatchmentPlantSound : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler {

	public AudioSource audioSource;
	public AudioClip buttonHoverSound;
	public AudioClip buttonClickSound;

	public void OnPointerEnter (PointerEventData eventData)
	{
		if(FundManager.totalFund >= UpgradeManager.catchmentPrice)
		{
			if(this.GetComponent<Button>().interactable == true)
			{
				audioSource.PlayOneShot(buttonHoverSound);
			}
		}
	}
	
	public void OnPointerDown(PointerEventData eventData)
	{
		if(FundManager.totalFund >= UpgradeManager.catchmentPrice)
		{
			if(this.GetComponent<Button>().interactable == true)
			{
				audioSource.PlayOneShot(buttonClickSound);
			}
		}
	}
}
