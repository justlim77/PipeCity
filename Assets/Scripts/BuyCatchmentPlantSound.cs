using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class BuyCatchmentPlantSound : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler {

	public AudioClip buttonHoverSound;
	public AudioClip buttonClickSound;

    Button _button;

    void Start()
    {
        _button = GetComponent<Button>();
    }

	public void OnPointerEnter (PointerEventData eventData)
	{
		if(FundManager.totalFund >= UpgradeManager.catchmentPrice)
			if(_button.interactable == true) 
				AudioManager.Instance.PlaySFX(buttonHoverSound);
	}
	
	public void OnPointerDown(PointerEventData eventData)
	{
		if(FundManager.totalFund >= UpgradeManager.catchmentPrice)
			if(_button.interactable)
                AudioManager.Instance.PlaySFX(buttonClickSound);
	}
}
