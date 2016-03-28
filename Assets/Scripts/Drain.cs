using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Drain : MonoBehaviour {

	public List<GameObject> connectedBuildings = new List<GameObject> ();

	public float drainOutputRate;

	public void ComputeDrainOutput ()
	{
        WaterManager.TempWasteWaterOutput += WaterManager.PermWasteWaterOutput * connectedBuildings.Count;
        WaterManager.ComputeWasteOutput (0, false);
	}

	public void ComputeDrainOutput2 ()
	{
        WaterManager.TempWasteWaterOutput += WaterManager.PermWasteWaterOutput * connectedBuildings.Count;
        WaterManager.ComputeWasteOutput (0, false);
	}
}
