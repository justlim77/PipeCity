using UnityEngine;
using System.Collections;

[System.Serializable]
public class Pipe {

	public string pipeName;
	public int pipeID;
	public Sprite pipeIcon;
	public int pipePrice;
	public int pipeAmount;
	public PipeType pipeType;
	public enum PipeType
	{
		Default,
		Bronze,
		Silver
	}
	public bool pipeHasTop;
	public bool pipeHasRight;
	public bool pipeHasBottom;
	public bool pipeHasLeft;
	public int outletCount;
	public int connectedCount;
	public bool isConnected;
	public bool hasWater;
	public bool isLeaking;
	public float pipeHealth;

	public Pipe (string name, int id, int price, int amount,
	             PipeType type, bool hasTop, bool hasRight, bool hasBottom, bool hasLeft,
	             int OutletCount, int ConnectedCount, bool _isConnected, bool _hasWater, float health)
	{
		pipeName = name;
		pipeID = id;
		pipePrice = price;
		pipeAmount = amount;
		pipeType = type;
		pipeHasTop = hasTop;
		pipeHasRight = hasRight;
		pipeHasBottom = hasBottom;
		pipeHasLeft = hasLeft;
		outletCount = OutletCount;
		connectedCount = ConnectedCount;
		isConnected = _isConnected;
		hasWater = _hasWater;
		pipeHealth = health;
	
		pipeIcon = Resources.Load<Sprite> ("sprites/ui/pipes/" + name);
	}

	public Pipe ()
	{

	}
}
