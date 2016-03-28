using UnityEngine;
using System.Collections;
using System.Collections.Generic;		// Namespace that contains the collection type Lists

public class Inventory : MonoBehaviour {

	#region Variable declaration

	public List<GameObject> InventorySlots = new List<GameObject>();
	public List<Pipe> Pipes = new List<Pipe> ();

	public int columns;
	public int rows;
	public float slotPaddingLeft, slotPaddingTop;
	public float slotSize;
	public int slotNumber;
	public GameObject slotPrefab;
	
	private RectTransform inventoryRect;
	private float inventoryWidth, inventoryHeight;

	[SerializeField] PipeDatabase database;

	#endregion

	#region Methods

	void Start ()
	{
		CreateInventory ();
	}

	void CreateInventory ()
	{
		InventorySlots = new List<GameObject> ();

		inventoryWidth = columns * (slotSize + slotPaddingLeft) + slotPaddingLeft;
		inventoryHeight = rows * (slotSize + slotPaddingTop) + slotPaddingTop;

		inventoryRect = GetComponent<RectTransform> ();
		inventoryRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, inventoryWidth);
		inventoryRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, inventoryHeight);

		for (int i = 0; i < (columns * rows) + 1; i++)
		{
			Pipes.Add (new Pipe());
			Pipes[i] = database.pipes [i];	// [0] is empty pipe, hence [i+1]
		}
	
		for (int y = 0; y < rows; y++)
		{
			for (int x = 0; x < columns; x++)
			{
				GameObject slot = (GameObject) Instantiate (slotPrefab);			// Cast instantiated slotPrefab as a GameObject
				slot.GetComponent<Slot>().slotNumber = slotNumber;
				RectTransform slotRect = slot.GetComponent<RectTransform>();
				
				slot.name = "Inventory Slot " + (slotNumber + 1);
				slotNumber++;
				slot.transform.SetParent(this.transform);						// Set new slot's parent to Inventory

				//slotRect.localPosition = inventoryRect.localPosition - new Vector3 (slotPaddingLeft * (x + 1) + (slotSize * x), -slotPaddingTop * (-y - 1) - (slotSize * -y));
				//slotRect.localPosition = new Vector3 (-slotPaddingLeft * (x + 1) - (slotSize * x), slotPaddingTop * (y - 1) - (slotSize * -y));
				slotRect.localPosition = new Vector2 ((-inventoryWidth) + slotPaddingLeft * (x + 1) + (slotSize * x), 0 - slotPaddingTop * (y + 1) - (slotSize * y));
				slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
				slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);
				
				InventorySlots.Add (slot);
			}
		}
	}

	/*
	void CreateLayoutv1 ()
	{
		database = GameObject.FindGameObjectWithTag ("Item Database").GetComponent <ItemDatabase> ();
		
		for (int i = 0; i < (slotsX * slotsY); i++)
		{
			GameObject slot = (GameObject)Instantiate (slotPrefab);		// Instantiate a slot cast as gameObject
			AllSlots.Add (slot);
			Items.Add (new Item ());
			Items[i] = database.items [i];
			AddItem (i);
			slot.name = "Shop Slot " + i;
			slot.transform.SetParent(this.gameObject.transform);	// Created slot will be a child of Inventory gameObject
		}
	}
	*/

	void AddItem (int id)
	{
		for(int i = 0; i < database.pipes.Count; i++)
		{
			if(database.pipes[i].pipeID == id)
			{
				Pipe item = database.pipes[i];
				AddToEmptySlot (item);

				break;
			}
		}
	}

	void AddToEmptySlot (Pipe item)
	{
		for (int i = 0; i < Pipes.Count; i++)
		{
			if(Pipes[i].pipeName == null)
			{
				Pipes[i] = item;

				break;
			}
		}
	}

	#endregion
}
