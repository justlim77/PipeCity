using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Drain : MonoBehaviour {

	public List<GameObject> connectedBuildings = new List<GameObject> ();

	public float drainOutputRate;

	WaterManager waterManager;
	UpgradeManager upgradeManager;

	void Awake ()
	{
		waterManager = GameObject.Find ("WaterManager").GetComponent<WaterManager> ();
		upgradeManager = GameObject.Find ("UpgradeManager").GetComponent<UpgradeManager> ();
	}

	public void ComputeDrainOutput ()
	{
		waterManager.tempwasteWaterOutput +=  waterManager.permwasteWaterOutput * connectedBuildings.Count;
		waterManager.ComputeWasteOutput (0, false);
	}

	public void ComputeDrainOutput2 ()
	{
		waterManager.tempwasteWaterOutput +=  waterManager.permwasteWaterOutput * connectedBuildings.Count;
		waterManager.ComputeWasteOutput (0, false);
	}
}
