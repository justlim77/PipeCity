using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UpgradeDesalinationButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {

	public GameObject priceTag;
	public Text priceText;

    public AudioClip buttonHoverSound;
    public AudioClip buttonClickSound;

	void Awake ()
	{
		priceText = priceTag.transform.GetChild (0).GetComponent<Text>();
		priceTag.SetActive (false);
	}


	#region IPointerEnterHandler implementation

	public void OnPointerEnter (PointerEventData eventData)
	{
		if (priceText.text != "")
		{
			priceTag.SetActive (true);

			if (FundManager.totalFund >= UpgradeManager.desalinationUpgrade1 && UpgradeManager.desalinationUpgradeCount == 1)
				AudioManager.Instance.PlaySFX(buttonHoverSound);
			if (FundManager.totalFund >= UpgradeManager.desalinationUpgrade2 && UpgradeManager.desalinationUpgradeCount == 2)
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
		if (FundManager.totalFund >= UpgradeManager.desalinationUpgrade1 && UpgradeManager.desalinationUpgradeCount == 1)
            AudioManager.Instance.PlaySFX(buttonClickSound);
		if (FundManager.totalFund >= UpgradeManager.desalinationUpgrade2 && UpgradeManager.desalinationUpgradeCount == 2)
            AudioManager.Instance.PlaySFX(buttonClickSound);
	}
}
