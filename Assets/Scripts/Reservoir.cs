using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Reservoir : MonoBehaviour {


	public float maxCapacity = 25F;
	public float minCapacity = 0F;
	public float currentCapacity;
	public float currentWaterLevel;
	public int lerpSpeed = 10;

	public GameObject waterTank;
	public Image waterbarVisual;

	public float totaldrainRate;
	public float refillRate = 0.1F;
	public float autoRefillRate = 10F;

	//bool onCD;

	public List<GameObject> connectedBuildings = new List<GameObject> ();

	WaterManager waterManager;

	void Awake ()
	{
		waterManager = GameObject.Find ("WaterManager").GetComponent<WaterManager> ();
	}

	void Start ()
	{
		lerpSpeed = 5;
		autoRefillRate = 10F;
		currentCapacity = minCapacity;
		waterTank = this.transform.GetChild (0).GetChild (0).gameObject;
		waterbarVisual = waterTank.GetComponent<Image> ();
	}

	void Update ()
	{
		HandleWaterLevel ();
		HandleTankWaterBar ();
	}

	public float CalculateDrainRate (float drainRate)
	{
		totaldrainRate += drainRate;

		return totaldrainRate;
	}

	public void ResetDrainRate ()
	{
		totaldrainRate = 0;
	}

	public void Refill ()
	{
		while (currentCapacity < maxCapacity && waterManager.currentWater > waterManager.minWater) {
			currentCapacity += refillRate * Time.deltaTime;
			waterManager.currentWater -= refillRate * Time.deltaTime;
			Tank.isRefilling = true;
		}
	}

	//public IEnumerator Cooldown ()
	//{
	//	onCD = true;
	//	yield return new WaitForSeconds (1);
	//	onCD = false;
	//}

	public void AutoRefill ()
	{
		if (currentCapacity < maxCapacity / 2 && waterManager.currentWater > waterManager.minWater) {
			currentCapacity += 25F;
			waterManager.currentWater -= 25F;
		}
	}

	void HandleWaterLevel ()
	{
		currentCapacity = Mathf.Clamp (currentCapacity, minCapacity, maxCapacity);
		currentCapacity += totaldrainRate * Time.deltaTime;
	}

	void HandleTankWaterBar ()
	{
		//expText.text = "WATER RESERVES  " + currentLevel;
		
		currentWaterLevel = MapValues (currentCapacity, 0, maxCapacity, 0, 1);
		
		waterbarVisual.fillAmount = Mathf.Lerp (waterbarVisual.fillAmount, currentWaterLevel, Time.deltaTime * lerpSpeed);
	}
	
	float MapValues (float x, float inMin, float inMax, float outMin, float outMax)
	{
		return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
	}
}
