using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class BuyReclamationPlantSound : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler {

	public AudioClip buttonHoverSound;
	public AudioClip buttonClickSound;

    Button _button;

    void Start()
    {
        _button = GetComponent<Button>();
    }

	public void OnPointerEnter (PointerEventData eventData)
	{
		if(FundManager.totalFund >= UpgradeManager.reclamationPrice)
			if(_button.interactable)
                AudioManager.Instance.PlaySFX(buttonHoverSound);
	}
	
	public void OnPointerDown(PointerEventData eventData)
	{
		if(FundManager.totalFund >= UpgradeManager.reclamationPrice)
			if(_button.interactable)
                AudioManager.Instance.PlaySFX(buttonClickSound);
	}
}
