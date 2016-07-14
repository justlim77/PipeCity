using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpgradeManager : MonoBehaviour {
	
	public GameObject catchmentTooltip;
	public GameObject reclamationTooltip;
	public GameObject desalinationTooltip;

	public Color32 lightGreen = new Color32 (100, 255, 100, 255);

	public static int catchmentPrice = 150;
	public Text catchmentUpgradeText;
	public static int catchmentUpgrade1;
	public static int catchmentUpgrade2;
	public int catchmentUpgrade3;
	public GameObject catchmentLevel1;
	public GameObject catchmentLevel2;
	public GameObject catchmentLevel3;
	public static int catchmentUpgradeCount = 0;
	public float catchmentSupplyRate = 0.25F;

	public static int reclamationPrice = 200;
	public Text reclamationUpgradeText;
	public static int reclamationUpgrade1;
	public static int reclamationUpgrade2;
	public int reclamationUpgrade3;
	public GameObject reclamationLevel1;
	public GameObject reclamationLevel2;
	public GameObject reclamationLevel3;
	public static int reclamationUpgradeCount = 0;
	public float reclamationSupplyRate = 0.05F;

	public static int desalinationPrice = 300;
	public Text desalinationUpgradeText;
	public static int desalinationUpgrade1;
	public static int desalinationUpgrade2;
	public int desalinationUpgrade3;
	public GameObject desalinationLevel1;
	public GameObject desalinationLevel2;
	public GameObject desalinationLevel3;
	public static int desalinationUpgradeCount = 0;
	public float desalinationSupplyRate = 1F;

	GridLayout bronzeLayout;
	
	void Awake ()
	{
		catchmentTooltip = GameObject.Find ("CatchmentTooltipPanel").gameObject;
		catchmentTooltip.SetActive (false);

		reclamationTooltip = GameObject.Find ("ReclamationTooltipPanel").gameObject;
		reclamationTooltip.SetActive (false);

		desalinationTooltip = GameObject.Find ("DesalinationTooltipPanel").gameObject;
		desalinationTooltip.SetActive (false);

		//bronzeLayout = GameObject.FindGameObjectWithTag ("Bronze").GetComponent <GridLayout> ();
	}

	void Start ()
	{
        bronzeLayout = GameManager.Instance.bronzeGrid;
		catchmentUpgrade1 = catchmentPrice * 2;
		catchmentUpgrade2 = catchmentUpgrade1 + catchmentPrice;
		catchmentUpgrade3 = catchmentUpgrade2 + catchmentPrice;

		reclamationUpgrade1 = reclamationPrice * 2;
		reclamationUpgrade2 = reclamationUpgrade1 + reclamationPrice;
		reclamationUpgrade3 = reclamationUpgrade2 + reclamationPrice;

		desalinationUpgrade1 = desalinationPrice * 2;
		desalinationUpgrade2 = desalinationUpgrade1 + desalinationPrice;
		desalinationUpgrade3 = desalinationUpgrade2 + desalinationPrice;
	}

	#region Catchment Methods
	public void BuyCatchment (GameObject thisObject)
	{
		if (FundManager.totalFund >= catchmentPrice) {
			FundManager.totalFund -= catchmentPrice;
			catchmentUpgradeText.text = "$" + catchmentUpgrade1;
			catchmentLevel1.GetComponent<Image> ().color = lightGreen;
			catchmentUpgradeCount ++;

            WaterManager.ComputeRainOutput (catchmentSupplyRate);

			thisObject.GetComponent<Button> ().interactable = false;
			thisObject.GetComponent<Image> ().color = Color.gray;
		}
	}

	public void CatchmentUpgrade (GameObject buttonObj)
	{
		if (FundManager.totalFund >= catchmentUpgrade1 && catchmentUpgradeCount == 1) {
			FundManager.totalFund -= catchmentUpgrade1;
			catchmentUpgradeText.text = "$" + catchmentUpgrade2;
			catchmentLevel2.GetComponent<Image> ().color = lightGreen;
			catchmentUpgradeCount ++;

            WaterManager.ComputeRainOutput (catchmentSupplyRate);
		}

		else if (FundManager.totalFund >= catchmentUpgrade2 && catchmentUpgradeCount == 2) {
			FundManager.totalFund -= catchmentUpgrade2;
			catchmentUpgradeText.text = "MAX";
			catchmentLevel3.GetComponent<Image> ().color = lightGreen;
			catchmentUpgradeCount ++;

            WaterManager.ComputeRainOutput (catchmentSupplyRate);

			buttonObj.GetComponent<Button> ().interactable = false;
			buttonObj.GetComponent<Image> ().color = Color.gray;
		}
	}
	#endregion

	#region Reclamation Methods
	public void BuyReclamation(GameObject buttonObj)
	{
		if (FundManager.totalFund >= reclamationPrice) {
			FundManager.totalFund -= reclamationPrice;
			reclamationUpgradeText.text = "$" + reclamationUpgrade1;
			reclamationLevel1.GetComponent<Image> ().color = lightGreen;
			reclamationUpgradeCount ++;

			WaterManager.hasReclamation = true;
            WaterManager.ComputeWasteOutput (reclamationSupplyRate, true);
			bronzeLayout.EvaluateGrid ();

			buttonObj.GetComponent<Button> ().interactable = false;
			buttonObj.GetComponent<Image> ().color = Color.gray;
		}
	}

	public void ReclamationUpgrade (GameObject buttonObj)
	{
		if (FundManager.totalFund >= reclamationUpgrade1 && reclamationUpgradeCount == 1) {
			FundManager.totalFund -= reclamationUpgrade1;
			reclamationUpgradeText.text = "$" + reclamationUpgrade2;
			reclamationLevel2.GetComponent<Image> ().color = lightGreen;
			reclamationUpgradeCount ++;

            WaterManager.ComputeWasteOutput (reclamationSupplyRate, true);
			bronzeLayout.EvaluateGrid ();
		}
		
		else if (FundManager.totalFund >= reclamationUpgrade2 && reclamationUpgradeCount == 2) {
			FundManager.totalFund -= reclamationUpgrade2;
			reclamationUpgradeText.text = "MAX";
			reclamationLevel3.GetComponent<Image> ().color = lightGreen;
			reclamationUpgradeCount ++;

            WaterManager.ComputeWasteOutput (reclamationSupplyRate, true);
			bronzeLayout.EvaluateGrid ();

			buttonObj.GetComponent<Button> ().interactable = false;
			buttonObj.GetComponent<Image> ().color = Color.gray;
		}
	}
	#endregion

	#region Desalination Methods
	public void BuyDesalination (GameObject buttonObj)
	{
		if (FundManager.totalFund >= desalinationPrice) {
			FundManager.totalFund -= desalinationPrice;
			desalinationUpgradeText.text = "$" + desalinationUpgrade1;
			desalinationLevel1.GetComponent<Image> ().color = lightGreen;
			desalinationUpgradeCount ++;

            WaterManager.ComputeSeaOutput (desalinationSupplyRate);

			buttonObj.GetComponent<Button> ().interactable = false;
			buttonObj.GetComponent<Image> ().color = Color.gray;
		}
	}

	public void DesalinationUpgrade (GameObject buttonObj)
	{
		if (FundManager.totalFund >= desalinationUpgrade1 && desalinationUpgradeCount == 1) {
			FundManager.totalFund -= desalinationUpgrade1;
			desalinationUpgradeText.text = "$" + desalinationUpgrade2;
			desalinationLevel2.GetComponent<Image> ().color = lightGreen;
			desalinationUpgradeCount ++;

            WaterManager.ComputeSeaOutput (desalinationSupplyRate);
		}
		
		else if (FundManager.totalFund >= desalinationUpgrade2 && desalinationUpgradeCount == 2) {
			FundManager.totalFund -= desalinationUpgrade2;
			desalinationUpgradeText.text = "MAX";
			desalinationLevel3.GetComponent<Image> ().color = lightGreen;
			desalinationUpgradeCount ++;

            WaterManager.ComputeSeaOutput (desalinationSupplyRate);

			buttonObj.GetComponent<Button> ().interactable = false;
			buttonObj.GetComponent<Image> ().color = Color.gray;
		}
	}
    #endregion

    public static bool AllUpgradesPurchased()
    {
        return catchmentUpgradeCount >= 3 && reclamationUpgradeCount >= 3 && desalinationUpgradeCount >= 3;
    }
}
