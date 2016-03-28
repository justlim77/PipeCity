using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class BuyWaterButtonSound : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler {

	public AudioSource audioSource;
	public AudioClip buttonHoverSound;
	public AudioClip fundClickSound;

	FundManager fundManager;
	WaterManager waterManager;

	void Awake ()
	{
		fundManager = GameObject.Find ("FundManager").GetComponent<FundManager> ();
		waterManager = GameObject.Find ("WaterManager").GetComponent<WaterManager> ();
	}

	public void OnPointerEnter (PointerEventData eventData)
	{
		if (transform.GetComponent<Button> ().interactable == true) {
			audioSource.PlayOneShot (buttonHoverSound);
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (FundManager.totalFund >= fundManager.importedwaterCost && waterManager.currentWater < waterManager.maxWater) {
			if (transform.GetComponent<Button> ().interactable == true) {
				audioSource.PlayOneShot(fundClickSound);
			}
		}
	}
}
