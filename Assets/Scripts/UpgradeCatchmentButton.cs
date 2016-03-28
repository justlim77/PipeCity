using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UpgradeCatchmentButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {

	public GameObject priceTag;
	public Text priceText;

	public AudioSource audioSource;
	public AudioClip buttonHoverSound;
	public AudioClip buttonClickSound;

	void Awake  ()
	{
		priceTag.SetActive (false);
		priceText = priceTag.transform.GetChild (0).GetComponent <Text> ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	#region IPointerEnterHandler implementation

	public void OnPointerEnter (PointerEventData eventData)
	{
		if (priceText.text != "")
		{
			priceTag.SetActive (true);
			if (FundManager.totalFund >= UpgradeManager.catchmentUpgrade1 && UpgradeManager.catchmentUpgradeCount == 1)
			{
				audioSource.PlayOneShot(buttonHoverSound);
			}
			if (FundManager.totalFund >= UpgradeManager.catchmentUpgrade2 && UpgradeManager.catchmentUpgradeCount == 2)
			{
				audioSource.PlayOneShot(buttonHoverSound);
			}
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
		{
			audioSource.PlayOneShot(buttonClickSound);
		}
		if (FundManager.totalFund >= UpgradeManager.catchmentUpgrade2 && UpgradeManager.catchmentUpgradeCount == 2)
		{
			audioSource.PlayOneShot(buttonClickSound);
		}
	}
}
