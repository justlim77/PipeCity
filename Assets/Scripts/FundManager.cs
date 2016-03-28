using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FundManager : MonoBehaviour {

	public Text txt_fund;					// Text component to display fund update
	public Text txt_timer;					// Text component to display payout timer update
	private int startingFund = 500;			// Starting fund
	public static int totalFund;			// Current player fund
	public float cloudFund;					// Funds being earned
	public int maxFund = 10000;				// Max amount of fund
	public int minFund = 0;					// Fund can never go negative
	public float increase = 0;				// After increase reaches interval, increase amount is added to fund
	public int multiplier = 1;				// Increase rate at which fund generates
	public int interval = 1;				// Increase fund by (increase*multiplier) per second (interval)
	public float countdown;
	public float increaseRate;
	public int importedwaterCost = 50;

	WaterManager waterManager;

	
	void Awake ()
	{
		txt_fund = GameObject.Find ("Fund").GetComponentInChildren<Text> ();
		txt_timer = GameObject.Find ("Timer").GetComponentInChildren<Text> ();

		waterManager = GameObject.Find ("WaterManager").GetComponent<WaterManager> ();
	}

	void Start ()
	{
		totalFund = startingFund;
		countdown = interval;
	}

	void Update ()
	{
		//FundControl ();
		HandleFund ();
	}

	void FundControl ()
	{
		totalFund = Mathf.Clamp (totalFund, minFund, maxFund);

		increase += Time.deltaTime * multiplier;

		totalFund += (int)increase;

		/*
		if(increase >= interval && totalFund != maxFund)
		{
			totalFund += (int)increase;
			increase -= (int)increase;
		}
		*/

		txt_fund.text = "$" + totalFund.ToString ();
	}

	public float CalculateCloudFund (int payment)
	{
		increaseRate += payment;
		return increaseRate;
	}

	public void ResetIncreaseRate ()
	{
		increaseRate = 0;
	}
	
	void HandleFund ()
	{
		totalFund = Mathf.Clamp (totalFund, minFund, maxFund);	// Clamp total funds

		cloudFund += increaseRate * Time.deltaTime;
		//totalFund += (int) cloudFund * Time.deltaTime;	

		countdown -= Time.deltaTime;						// Countdown timer

		if (countdown <= 0) {
			totalFund += (int) cloudFund;	// Increase total fund by amount of cloud funds earned during the interval
			cloudFund = 0;					// Reset cloud fund
			countdown = interval;			// Reset countdown
		}

		txt_fund.text = "$" + totalFund.ToString ();

		//txt_fund.text = "$" + cloudFund.ToString ("F0") + " | $" + totalFund.ToString ();	// Showing cloud+total
		//txt_timer.text = "Payout in " + countdown.ToString ("F0");
	}

	public void BuyWater ()
	{
		if (totalFund >= importedwaterCost && waterManager.currentWater < waterManager.maxWater) {
			totalFund -= importedwaterCost;
			waterManager.AddImportedWater (importedwaterCost);

		} else {
			Debug.Log ("Insufficient Funds");
		}
	}
}
