using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WaterManager : MonoBehaviour {

	public static bool hasReclamation;

	public float minWater = 0;
	public float maxWater = 100F;
	public Text waterText;
	public Image visualWaterBar;
	
	public float cooldown;
	public bool onCD;
	public float delayTime;
	
	public WaterMode waterMode;
	
	public int lerpSpeed;
	private float currentValue;
	
	private float initialWater = 0;
	public float currentWater;
	
	private int initLevel = 1;
	private int currentLevel;
	
	public float totalWaterOutput;
	public float rainWaterOutput;
	public float permwasteWaterOutput;
	public float tempwasteWaterOutput;
	public float wasteWaterOutput;
	public float seaWaterOutput;
	
	RainManager rainManager;
	
	public enum WaterMode
	{
		Static,
		Gain,
		Lose
	}
	
	private int CurrentLevel
	{
		get { return currentLevel; }
		set {
			currentLevel = value;
		}
	}
	
	void Awake  ()
	{
		rainManager = GameObject.Find ("RainManager").GetComponent<RainManager> ();
	}
	
	void Start ()
	{
		currentLevel = initLevel;
		currentWater = initialWater;
		onCD = false;
		waterText = GameObject.Find ("WaterText").GetComponent<Text> ();
	}
	
	void Update ()
	{
		if (!InputManager2._isPaused)
		{
			SimulateWater ();
			HandleWaterBar ();
		}
		
		MaxCapacityClamp ();
	}
	
	IEnumerator Delay (float _delayTime)
	{
		yield return new WaitForSeconds (_delayTime);
	}
	
	public IEnumerator Cooldown ()
	{
		onCD = true;
		yield return new WaitForSeconds (cooldown);
		onCD = false;
	}
	
	void HandleWaterBar ()
	{
		//expText.text = "WATER RESERVES  " + currentLevel;
		
		currentValue = MapValues (currentWater, 0, maxWater, 0, 1);
		
		visualWaterBar.fillAmount = Mathf.Lerp (visualWaterBar.fillAmount, currentValue, Time.deltaTime * lerpSpeed);
	}
	
	float MapValues (float x, float inMin, float inMax, float outMin, float outMax)
	{
		return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
	}
	
	void SimulateWater ()
	{
		// Debug Lose mode
		if(!onCD && currentWater > 0 && waterMode == WaterMode.Lose)
		{
			StartCoroutine(Cooldown ());
			currentWater -= 1;
			if (currentWater <= 0) { waterMode = WaterMode.Gain; }
		}
		
		// Debug Gain mode
		if(!onCD && currentWater <= maxWater && waterMode == WaterMode.Gain)
		{
			StartCoroutine(Cooldown ());
			currentWater += 1;
			if (currentWater > maxWater)
			{
				StartCoroutine(Delay (delayTime));
				waterMode = WaterMode.Lose;
			}
		}
		
		// Game mode
		if(waterMode == WaterMode.Static)
		{
			if (!onCD && currentWater < maxWater)
			{
				StartCoroutine (Cooldown ());
				ConditionalOutput ();
				waterText.text = "" + ComputeOutputRate (rainWaterOutput, wasteWaterOutput, seaWaterOutput).ToString ("F1") 
					+ " units generated per day";
			}
		}
	}
	
	public void AddImportedWater (int amount)
	{
		if (currentWater < maxWater) {
			currentWater += amount;
			//currentWater = Mathf.Clamp (currentWater, minWater, maxWater);
		}
	}
	
	public float ComputeOutputRate (float rain, float waste, float sea)
	{
		if (!rainManager.isRaining)
			rain = 0;
		if (!hasReclamation)
			waste = 0;
		
		float outputRate = rain + waste + sea;
		return outputRate;
	}
	
	public void ComputeTotalOutput (float rain, float waste, float sea)
	{
		totalWaterOutput += rain + waste + sea;
	}

	public void ComputeRainOutput (float rainOutput)
	{
		rainWaterOutput += rainOutput;
	}

	public void ComputeWasteOutput (float wasteOutput, bool isPermanent)
	{
		//wasteWaterOutput = 0;	// Reset temp waste water output

		if (isPermanent)
			permwasteWaterOutput += wasteOutput;
		else {
			wasteWaterOutput = tempwasteWaterOutput;
		}
	}

	public void ComputeSeaOutput (float seaOutput)
	{
		seaWaterOutput += seaOutput;
	}

	void ConditionalOutput ()
	{
		if (rainManager.isRaining) {
			//ComputeTotalOutput (rainWaterOutput, 0, seaWaterOutput);
			currentWater += rainWaterOutput + wasteWaterOutput + seaWaterOutput;
		} else {
			currentWater += 0 + wasteWaterOutput + seaWaterOutput;
		}
	}

	public void ResetDrainOutput ()
	{
		tempwasteWaterOutput = 0;	// Reset temp water output to 0
		wasteWaterOutput = 0;
	}

	public void ComputeBuildingOutput ()
	{
		tempwasteWaterOutput += permwasteWaterOutput;
	}

	void MaxCapacityClamp ()
	{
		currentWater = Mathf.Clamp (currentWater, minWater, maxWater);
	}
}
