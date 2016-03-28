using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class BuyWaterButtonSound : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler {

	public AudioClip buttonHoverSound;
	public AudioClip fundClickSound;

	FundManager fundManager;
	WaterManager waterManager;
    Button _button;

	void Awake ()
	{
		fundManager = GameObject.Find ("FundManager").GetComponent<FundManager> ();
		waterManager = GameObject.Find ("WaterManager").GetComponent<WaterManager> ();
        _button = GetComponent<Button>();
	}

	public void OnPointerEnter (PointerEventData eventData)
	{
		if (_button.interactable)
            AudioManager.Instance.PlaySFX(buttonHoverSound);
	}

    public void OnPointerDown(PointerEventData eventData)
    {
        if (FundManager.totalFund >= fundManager.importedwaterCost && waterManager.currentWater < waterManager.maxWater)
            if (_button.interactable)
                AudioManager.Instance.PlaySFX(fundClickSound);
    }
}
