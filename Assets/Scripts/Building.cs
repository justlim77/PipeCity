using UnityEngine;
using System.Collections;

[System.Serializable]
public class Building {

	public string buildingName;
	public int buildingID;
	public Sprite buildingIcon;
	public BuildingType buildingType;
	public enum BuildingType
	{
		Default,
		House,
		City,
	}
	public float drainRate;
	public int payOut;

	public Building (string name, int id, BuildingType type, float rate, int pay, Sprite sprite)
	{
		buildingName = name;
		buildingID = id;
		buildingType = type;
		drainRate = rate;
		payOut = pay;
        buildingIcon = sprite;
	}

    public Building()
    { 
    
    }
}
