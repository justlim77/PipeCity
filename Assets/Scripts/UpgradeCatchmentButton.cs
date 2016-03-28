using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UpgradeCatchmentButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {

	public GameObject priceTag;
	public Text priceText;
	public AudioClip buttonHoverSound;
	public AudioClip buttonClickSound;

	void Awake  ()
	{
		priceTag.SetActive (false);
		priceText = priceTag.transform.GetChild (0).GetComponent <Text> ();
	}

	#region IPointerEnterHandler implementation

	public void OnPointerEnter (PointerEventData eventData)
	{
		if (priceText.text != "")
		{
			priceTag.SetActive (true);
			if (FundManager.totalFund >= UpgradeManager.catchmentUpgrade1 && UpgradeManager.catchmentUpgradeCount == 1)
	    		AudioManager.Instance.PlaySFX(buttonHoverSound);
			if (FundManager.totalFund >= UpgradeManager.catchmentUpgrade2 && UpgradeManager.catchmentUpgradeCount == 2)
                AudioManager.Instance.PlaySFX(buttonHoverSound);
		}
	}

	#endregion

	#region IPointerExitHandler implementation

	public void OnPointerExit (PointerEventData eventData)
	{
		priceTag.SetActive (false);
	}

	#endregion

	public void OnPointerDown(PointerEventData eventData)
	{
		if (FundManager.totalFund >= UpgradeManager.catchmentUpgrade1 && UpgradeManager.catchmentUpgradeCount == 1)
            AudioManager.Instance.PlaySFX(buttonClickSound);
		if (FundManager.totalFund >= UpgradeManager.catchmentUpgrade2 && UpgradeManager.catchmentUpgradeCount == 2)
            AudioManager.Instance.PlaySFX(buttonClickSound);
	}
}
