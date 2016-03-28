using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GridLayout : MonoBehaviour {
	
	public List<GameObject> GridSlots = new List<GameObject> ();	// List of grids
	public List<GameObject> WaterSources = new List<GameObject> ();	// List of designated water sources
	public List<GameObject> BuildingWaterSources = new List<GameObject> ();	// List of building water sources
	public List<GameObject> DrainSources = new List<GameObject> ();	// List of designated drainage sources

	public Sprite rock;

	// Create new queue to process pipes
	Queue pipesWithWaterToProcess = new Queue ();
	Queue bronzepipesWithWaterToProcess = new Queue ();

	public int columns;
	public int rows;
	public float fl_slotSize;
	public GameObject slotPrefab;
	public GameObject tankPrefab;
	public int GridNumber;

	private RectTransform gridRect;
	private float gridWidth, gridHeight;

	private float fl_targetAspect = 4.0f / 3.0f;
	
	public static bool isEvaluating;

	[SerializeField] PipeDatabase pipeDatabase;
	[SerializeField] BuildingDatabase buildingDatabase;
	[SerializeField] FundManager fundManager;
	[SerializeField] WaterManager waterManager;

	public GridLayout silverLayout;
	public GridLayout bronzeLayout;

	void Start ()
	{
		WaterSources = new List<GameObject> ();

		rock = Resources.Load<Sprite>("Sprites/Obstacles/rock");

		pipeDatabase = GameObject.Find ("PipeDatabase").GetComponent<PipeDatabase> ();
		buildingDatabase = GameObject.Find ("BuildingDatabase").GetComponent<BuildingDatabase> ();
		fundManager = GameObject.Find ("FundManager").GetComponent<FundManager> ();
		waterManager = GameObject.Find ("WaterManager").GetComponent<WaterManager> ();

		silverLayout = GameObject.FindWithTag ("Silver").GetComponent<GridLayout> ();
		bronzeLayout = GameObject.FindWithTag ("Bronze").GetComponent<GridLayout> ();

		//CalculateScale ();
		DrawGrid ();
	}

	private void DrawGrid ()
	{
		gridWidth = columns * fl_slotSize;
		gridHeight = rows * fl_slotSize;

		gridRect = GetComponent<RectTransform> ();
		gridRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, gridWidth);
		gridRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, gridHeight);

		for (int y = 0; y < rows; y++)
		{
			for (int x = 0; x < columns; x++)
			{
				GameObject gridSlot = (GameObject) Instantiate (slotPrefab);
				RectTransform slotRect = gridSlot.GetComponent<RectTransform> ();

				gridSlot.name = "Grid " + (y+1) + "." + (x+1);
				gridSlot.GetComponent<Grid> ().gridNumber = GridNumber;
				GridNumber++;
				gridSlot.transform.SetParent (this.transform);
				GridSlots.Add (gridSlot);

				slotRect.localPosition = new Vector2 ((-gridWidth/2) + (fl_slotSize * x), -(fl_slotSize * y));
				slotRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, fl_slotSize);
				slotRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, fl_slotSize);
			}
		}

		SetBuildings ();

		if (transform.tag == "Silver") {
			SetSupplySources ();
		}
		if (transform.tag == "Bronze") {
			SetDrainageSources ();
		}

		SetObstacles ();
	}

	Building BuildingOrder (GameObject grid, int order)	// Manually assign spawning order of buildings
	{
		switch (order) {
		case 1:
			return grid.GetComponent<Grid> ().building = BuildingDatabase.Buildings[1];
		case 2:
            return grid.GetComponent<Grid>().building = BuildingDatabase.Buildings[3];
		case 3:
            return grid.GetComponent<Grid>().building = BuildingDatabase.Buildings[2];
		case 4:
			return grid.GetComponent<Grid> ().building = BuildingDatabase.Buildings[7];
		case 5:
            return grid.GetComponent<Grid>().building = BuildingDatabase.Buildings[5];
		case 6:
			return grid.GetComponent<Grid> ().building = BuildingDatabase.Buildings[4];
		case 7:
			return grid.GetComponent<Grid> ().building = BuildingDatabase.Buildings[4];
		case 8:
			return grid.GetComponent<Grid> ().building = BuildingDatabase.Buildings[5];
		case 9:
			return grid.GetComponent<Grid> ().building = BuildingDatabase.Buildings[7];
		case 10:
			return grid.GetComponent<Grid> ().building = BuildingDatabase.Buildings[2];
		case 11:
			return grid.GetComponent<Grid> ().building = BuildingDatabase.Buildings[3];
		case 12:
			return grid.GetComponent<Grid> ().building = BuildingDatabase.Buildings[1];
		default:
			return grid.GetComponent<Grid> ().building = BuildingDatabase.Buildings[0];
		}
	}

	void SetBuildings ()
	{
		int gridOrder = 1;

		// Set every other grid slot to a building
		for (int b = 1; b < columns - 1; b += 2) {
			GridSlots[b].GetComponent<Button> ().interactable = false;
			GridSlots[b].GetComponent<Grid> ().isBuilding = true;
			GridSlots[b].GetComponent<Grid> ().pipe.pipeHasBottom = true;
			GridSlots[b].GetComponent<Grid> ().pipe.outletCount = 1;
			BuildingOrder (GridSlots[b], gridOrder); gridOrder++;
			GridSlots[b].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Sprites/Buildings/" + GridSlots[b].GetComponent<Grid> ().building.buildingName);
			//GridSlots[a].GetComponent<Image> ().color = Color.gray;
			GridSlots[b].transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, 96);
			GridSlots[b].transform.GetComponent<RectTransform>().anchoredPosition = new Vector2 (fl_slotSize * b, 64);
			if (transform.tag == "Bronze") {
				BuildingWaterSources.Add (GridSlots[b]);
				GridSlots[b].GetComponent<Grid> ().isSource = true;
				GridSlots[b].GetComponent<Grid> ().waterOrigin = GridSlots[b];
			}
		}

		// Close every other grid slot
		for (int c = 0; c < columns; c += 2) {
			GridSlots[c].GetComponent<Button> ().interactable = false;
		}
	}

	void SetSupplySources ()
	{
		// Populate water sources list with manual positioning
		int spacing = Equidistance (columns);

		WaterSources.Add (GridSlots[columns * (rows) - columns + spacing - 2]);		// Bottom 1st tap
		WaterSources.Add (GridSlots[columns * (rows) - columns  + spacing * 2 - 1]);	// Bottom 2nd tap
		WaterSources.Add (GridSlots[columns * (rows) - spacing * 2]);	// Bottom 3rd tap
		WaterSources.Add (GridSlots[columns * (rows) - spacing + 1]);	// Bottom 4th tap

		// For each water source in group, turn hasWater to true and set as isSource, also add Reservoir script
		foreach (GameObject waterSource in WaterSources) {
			Grid gridComponent = waterSource.GetComponent<Grid> ();
			RectTransform imageRect = waterSource.transform.GetChild(0).GetComponent<RectTransform> ();
			gridComponent.isSource = true;
			//waterSource.GetComponent<Button>().interactable = false;
			waterSource.AddComponent <Reservoir> ();

			gridComponent.waterOrigin = waterSource;

			// Disable "transparent pixel" image
			waterSource.GetComponent<Image>().enabled = false;

			// Assign a new I pipe with top/bottom orientation
			gridComponent.pipe = new Pipe ("silver_I_pipe", 1, 20, 1,
			                                        Pipe.PipeType.Silver,
			                                        true, false, true, false, 2, 1, false, false, 100F);

			// Assign tank prefab
			GameObject waterTank = Instantiate (tankPrefab);
			waterTank.transform.SetParent (waterSource.GetComponent<RectTransform> ());
			waterTank.transform.SetAsFirstSibling ();
			//waterTank.GetComponent<RectTransform>().SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, 30);
			waterTank.GetComponent<RectTransform>().anchoredPosition = new Vector2 (0, -fl_slotSize);

			// Activate child image and assign corresponding pipe icon
			gridComponent.pipeImage.SetActive (true);
			gridComponent.img_pipe.sprite = gridComponent.pipe.pipeIcon;

			// Flip source pipe 90degrees facing up to match top/bottom "I" orientation
			imageRect.rotation = Quaternion.Euler (0, 0, 90);

			gridComponent.EvaluatePipeState ();
		}
	}

	void SetDrainageSources ()
	{		
		DrainSources.Add (GridSlots[75]);	// Bottom 1st tap
		DrainSources.Add (GridSlots[175]);	// Bottom 2nd tap
		DrainSources.Add (GridSlots[99]);	// Bottom 3rd tap
		DrainSources.Add (GridSlots[199]);	// Bottom 4th tap
		
		// For each water source in group, turn hasWater to true and set as isSource, also add Reservoir script
		foreach (GameObject drainSource in DrainSources) {
			Grid gridComponent = drainSource.GetComponent<Grid> ();
			RectTransform imageRect = drainSource.transform.GetChild(0).GetComponent<RectTransform> ();
			
			// Disable "transparent pixel" image & disable interactivity
			drainSource.GetComponent<Image> ().enabled = false;
			drainSource.GetComponent<Button> ().interactable = false;
			drainSource.AddComponent <Drain> ();
			
			// Assign a new I pipe with top/bottom orientation
			gridComponent.pipe = new Pipe ("bronze_I_pipe", 1, 20, 1,
			                               Pipe.PipeType.Bronze,
			                               false, true, false, true, 1, 0, false, false, 100F);
			
			// Activate child image and assign corresponding pipe icon
			gridComponent.pipeImage.SetActive (true);
			gridComponent.img_pipe.sprite = gridComponent.pipe.pipeIcon;
			gridComponent.isDrain = true;
			
			gridComponent.EvaluatePipeState ();
		}
	}

	void SetObstacles ()
	{
		foreach (GameObject grid in GridSlots) {
			Grid gridComponent = grid.GetComponent<Grid> ();

			if (grid == GridSlots[27] || grid == GridSlots[35] || grid == GridSlots[47] ||
			    grid == GridSlots[68] || grid == GridSlots[115] || grid == GridSlots[134] ||
			    grid == GridSlots[170] || grid == GridSlots[188] || grid == GridSlots[206]) {
				gridComponent.pipeImage.SetActive (true);
				gridComponent.GetComponent<Button> ().interactable = false;
				gridComponent.isObstacle = true;
				gridComponent.pipeImage.GetComponent<Image> ().sprite = rock;
			}
		}
	}

	public void EvaluateGrid ()
	{
		if (transform.tag == "Silver") {
			ResetGrid ();				// Remove water from all pipes
			CheckSourceNeighbors ();	// Check adjacent source neighbors for connectivity
			CheckTileNeighbors ();		// Check adjacent tile neighbors for connectivity
			CheckBuildingSource ();		// Check for building water source
			CheckPipeLeak ();			// Check for any leaks

			bronzeLayout.EvaluateGrid ();
		}

		if (transform.tag == "Bronze") {
			ResetBronzeGrid ();
			CheckBronzeSourceNeighbors ();
			CheckBronzeTileNeighbors ();
			CheckBronzeBuildingSource ();
		}
	}

	void ResetGrid ()
	{	
		fundManager.ResetIncreaseRate ();

		// Loop through all grid slots and set hasWater to false
		foreach (GameObject gridSlot in GridSlots)
		{
			if(gridSlot.GetComponent<Grid>().isSource) {
				gridSlot.GetComponent<Grid> ().pipe.connectedCount = 1;
			}

			if(!gridSlot.GetComponent<Grid> ().isSource) {
				gridSlot.transform.GetChild (1).transform.rotation = Quaternion.Euler (0, 0, 0);
				gridSlot.transform.GetChild (1).gameObject.SetActive (false);
				gridSlot.GetComponent<Grid> ().pipe.hasWater = false;
				gridSlot.GetComponent<Grid> ().waterOrigin = null;
				gridSlot.GetComponent<Grid> ().pipe.connectedCount = 0;

			}
			
			if(gridSlot.GetComponent<Grid> ().isBuilding) {
				if (gridSlot.GetComponent<Grid> ().waterOrigin != null) {
					gridSlot.GetComponent<Grid> ().waterOrigin.GetComponent<Reservoir> ().connectedBuildings.Clear ();
				}
			}
		}

		// Loop through all designated water sources and set hasWater to true
		foreach (GameObject waterSource in WaterSources)
		{
			waterSource.GetComponent<Grid> ().waterOrigin = waterSource;
			waterSource.GetComponent<Reservoir> ().connectedBuildings.Clear ();
			waterSource.GetComponent<Reservoir> ().ResetDrainRate ();
		}
	}

	void ResetBronzeGrid ()
	{		
		waterManager.ResetDrainOutput ();

		// Loop through all grid slots and set hasWater to false
		foreach (GameObject bronzegridSlot in GridSlots)
		{
			if(bronzegridSlot.GetComponent<Grid>().isSource) {
				bronzegridSlot.GetComponent<Grid> ().pipe.hasWater = 
					silverLayout.GridSlots [bronzegridSlot.GetComponent<Grid> ().gridNumber].GetComponent <Grid> ().pipe.hasWater;
				bronzegridSlot.GetComponent<Grid> ().pipe.connectedCount = 0;
				bronzegridSlot.GetComponent<Grid> ().waterOrigin = bronzegridSlot;
			}
			
			if(!bronzegridSlot.GetComponent<Grid> ().isSource) {
				bronzegridSlot.transform.GetChild (1).transform.rotation = Quaternion.Euler (0, 0, 0);	// Splash
				bronzegridSlot.transform.GetChild (1).gameObject.SetActive (false);
				bronzegridSlot.GetComponent<Grid> ().pipe.hasWater = false;
				bronzegridSlot.GetComponent<Grid> ().waterOrigin = null;
				bronzegridSlot.GetComponent<Grid> ().pipe.connectedCount = 0;
				bronzegridSlot.GetComponent<Grid> ().sewageBuildingSources.Clear ();
			}
		}
		
		// Loop through all designated water sources and set hasWater to true
		foreach (GameObject drainSource in DrainSources)
		{
			drainSource.GetComponent<Grid> ().waterOrigin = null;
			drainSource.GetComponent<Grid> ().pipe.hasWater = false;
			drainSource.GetComponent<Drain> ().connectedBuildings.Clear ();
			//drainSource.GetComponent<Drain> ().ComputeDrainOutput ();
			if (!drainSource.GetComponent<Grid> ().pipe.hasWater) {
				waterManager.ResetDrainOutput ();
			}

			drainSource.GetComponent<Drain> ().ComputeDrainOutput ();
		}
	}

	#region Limit Calculators
	
	// Takes in column count and divides into 4 equidistant grid spaces
	int Equidistance (int e)
	{
		int equidistance = (int)(columns / 5);

		return equidistance;
	}
	// Takes in a grid index and returns the corresponding row left limit
	int RightLimit (int rNumber)
	{
		int negativeNumber = rNumber;

		do
		{
			negativeNumber = negativeNumber - columns;
		} while (negativeNumber > -1);

		int rightLimit = (Mathf.Abs (negativeNumber)) + rNumber - 1;

		//Debug.Log ("Right limit : " + rightLimit);
		return rightLimit;
	}

	// Takes in a grid index and returns the corresponding row right limit
	int LeftLimit (int lNumber)
	{
		int newColumn = columns;

		while (newColumn < lNumber) {
			newColumn = newColumn + columns;
		}

		int leftLimit;

		if (newColumn == lNumber) {
			leftLimit = newColumn;
		} 
		else {
			leftLimit = newColumn - columns;
		}

		//Debug.Log ("Lef limit : " + leftLimit);
		return leftLimit;
	}

	private void CalculateScale ()
	{
		Camera cam = Camera.main;
		float fl_camHeight = 2.0f * cam.orthographicSize;
		float fl_camWidth = fl_camHeight * cam.aspect;
		float fl_scale = cam.aspect / fl_targetAspect;
		Debug.Log (fl_scale);
		
		fl_slotSize = fl_slotSize * fl_scale;
	}
	#endregion

	void CheckSourceNeighbors ()	// Silver [OK]
	{
		foreach (GameObject waterSource in WaterSources) {
			CheckAdjacent (waterSource);
		}
	}
	
	void CheckBronzeSourceNeighbors ()		// Bronze
	{
		foreach (GameObject buildingSource in BuildingWaterSources) {
			CheckBronzeAdjacent (buildingSource);
		}
	}
	
	void CheckTileNeighbors ()	// Silver [OK]
	{
		while (pipesWithWaterToProcess.Count > 0) {
			GameObject currentPipe = (GameObject) pipesWithWaterToProcess.Dequeue ();
			CheckAdjacent (currentPipe);
		}
	}

	void CheckBronzeTileNeighbors ()	// Bronze
	{
		while (bronzepipesWithWaterToProcess.Count > 0) {
			GameObject currentPipe = (GameObject) bronzepipesWithWaterToProcess.Dequeue ();
			CheckBronzeAdjacent (currentPipe);
		}
	}
	
	// Takes in gameobject currentPipe and returns adjacent connected tiles
	void CheckAdjacent (GameObject tile)
	{
		Grid _grid = tile.GetComponent<Grid>();
		
		//Debug.Log ((LeftLimit (_grid.gridNumber)));
		//Debug.Log ((RightLimit (_grid.gridNumber)));
		
		// Check right [OK]
		if (_grid.gridNumber > LeftLimit (_grid.gridNumber) - 1 && _grid.gridNumber < RightLimit (_grid.gridNumber)) {
			//If this pipe has right outlet and next pipe has left outlet
			if (GridSlots[_grid.gridNumber + 1].GetComponent<Grid> ().pipe.pipeHasLeft &&
			    GridSlots[_grid.gridNumber].GetComponent<Grid> ().pipe.pipeHasRight) {
				// Increase the connected count [OK]
				if(GridSlots[_grid.gridNumber].GetComponent<Grid> ().pipe.connectedCount <
				   GridSlots[_grid.gridNumber].GetComponent<Grid> ().pipe.outletCount)
						GridSlots[_grid.gridNumber].GetComponent<Grid> ().pipe.connectedCount++;
				// Check if next pipe does not have water [OK]
				if (GridSlots[_grid.gridNumber + 1].GetComponent<Grid> ().pipe.hasWater == false &&
				    GridSlots[_grid.gridNumber].GetComponent<Grid> ().pipe.hasWater) {
					pipesWithWaterToProcess.Enqueue (GridSlots[_grid.gridNumber + 1]);
					// Check if next pipe is not water source, then set origin
					if (GridSlots[_grid.gridNumber + 1].GetComponent<Grid> ().isSource == false) {
					GridSlots[_grid.gridNumber + 1].GetComponent<Grid> ().pipe.hasWater = true;
					GridSlots[_grid.gridNumber + 1].GetComponent<Grid> ().waterOrigin = 
						GridSlots[_grid.gridNumber].GetComponent<Grid> ().waterOrigin;
					}
				}
				// Check if current and next pipe both have water [OK]
				if (GridSlots[_grid.gridNumber + 1].GetComponent<Grid> ().pipe.hasWater &&
				    GridSlots[_grid.gridNumber].GetComponent<Grid> ().pipe.hasWater) {
					// If true, explode both pipes [OK]
					if (GridSlots[_grid.gridNumber + 1].GetComponent<Grid> ().waterOrigin != 
					    GridSlots[_grid.gridNumber].GetComponent<Grid> ().waterOrigin) {
						GridSlots[_grid.gridNumber + 1].GetComponent<Grid> ().ClearGrid ();
						GridSlots[_grid.gridNumber].GetComponent<Grid> ().ClearGrid ();
					}
				}
			}

			// Check for right outlet leak
			else if (!GridSlots[_grid.gridNumber + 1].GetComponent<Grid> ().pipe.pipeHasLeft &&
			         GridSlots[_grid.gridNumber].GetComponent<Grid> ().pipe.pipeHasRight &&
			         GridSlots[_grid.gridNumber].GetComponent<Grid> ().pipe.hasWater) {
				GridSlots[_grid.gridNumber + 1].transform.GetChild (1).gameObject.SetActive (true);
				GridSlots[_grid.gridNumber + 1].transform.GetChild (1).rotation = Quaternion.Euler (0, 0, 0);
			}
		}
		
		// Check left [OK]
		if (_grid.gridNumber > LeftLimit (_grid.gridNumber) && _grid.gridNumber < RightLimit (_grid.gridNumber) + 1) {
			//If this pipe has left outlet and next pipe has right outlet
			if (GridSlots[_grid.gridNumber - 1].GetComponent<Grid> ().pipe.pipeHasRight &&
			    GridSlots[_grid.gridNumber].GetComponent<Grid> ().pipe.pipeHasLeft) {
				// Increase the connected count [OK]
				if(GridSlots[_grid.gridNumber].GetComponent<Grid> ().pipe.connectedCount <
				   GridSlots[_grid.gridNumber].GetComponent<Grid> ().pipe.outletCount)
						GridSlots[_grid.gridNumber].GetComponent<Grid> ().pipe.connectedCount++;
				// Check if next pipe does not have water [OK]
				if (GridSlots[_grid.gridNumber - 1].GetComponent<Grid> ().pipe.hasWater == false &&
				    GridSlots[_grid.gridNumber].GetComponent<Grid> ().pipe.hasWater) {
					pipesWithWaterToProcess.Enqueue (GridSlots[_grid.gridNumber - 1]);
					// Check if next pipe is not water source, then set origin
					if (GridSlots[_grid.gridNumber - 1].GetComponent<Grid> ().isSource == false) {
					GridSlots[_grid.gridNumber - 1].GetComponent<Grid> ().pipe.hasWater = true;
					GridSlots[_grid.gridNumber - 1].GetComponent<Grid> ().waterOrigin = 
						GridSlots[_grid.gridNumber].GetComponent<Grid> ().waterOrigin;
					}
				}
				// Check if current and next pipe both have water [OK]
				if (GridSlots[_grid.gridNumber - 1].GetComponent<Grid> ().pipe.hasWater &&
				    GridSlots[_grid.gridNumber].GetComponent<Grid> ().pipe.hasWater) {
					// If true, explode both pipes [OK]
					if (GridSlots[_grid.gridNumber - 1].GetComponent<Grid> ().waterOrigin != 
					    GridSlots[_grid.gridNumber].GetComponent<Grid> ().waterOrigin) {
						GridSlots[_grid.gridNumber - 1].GetComponent<Grid> ().ClearGrid ();
						GridSlots[_grid.gridNumber].GetComponent<Grid> ().ClearGrid ();
					}
				}
			}

			// Check for left outlet leak [OK]
			else if (!GridSlots[_grid.gridNumber - 1].GetComponent<Grid> ().pipe.pipeHasRight &&
			         GridSlots[_grid.gridNumber].GetComponent<Grid> ().pipe.pipeHasLeft &&
			         GridSlots[_grid.gridNumber].GetComponent<Grid> ().pipe.hasWater) {
				GridSlots[_grid.gridNumber - 1].transform.GetChild (1).gameObject.SetActive (true);
				GridSlots[_grid.gridNumber - 1].transform.GetChild (1).rotation = Quaternion.Euler (0, 0, 180);
			}
		}
		
		// Check top [OK]
		if (_grid.gridNumber > columns) {
			//If this pipe has top outlet and next pipe has bottom outlet
			if (GridSlots[_grid.gridNumber - columns].GetComponent<Grid> ().pipe.pipeHasBottom &&
			    GridSlots[_grid.gridNumber].GetComponent<Grid> ().pipe.pipeHasTop) {
				// Increase the connected count [OK]
				if (GridSlots[_grid.gridNumber].GetComponent<Grid> ().pipe.connectedCount <
				   GridSlots[_grid.gridNumber].GetComponent<Grid> ().pipe.outletCount)
					GridSlots[_grid.gridNumber].GetComponent<Grid> ().pipe.connectedCount++;
				// Check if next pipe does not have water [OK]
				if (GridSlots[_grid.gridNumber - columns].GetComponent<Grid> ().pipe.hasWater == false &&
				    GridSlots[_grid.gridNumber].GetComponent<Grid> ().pipe.hasWater) {
					pipesWithWaterToProcess.Enqueue (GridSlots[_grid.gridNumber - columns]);
					// Check if next pipe is not water source, then set origin
					if (GridSlots[_grid.gridNumber - columns].GetComponent<Grid> ().isSource == false) {
					GridSlots[_grid.gridNumber - columns].GetComponent<Grid> ().pipe.hasWater = true;
					GridSlots[_grid.gridNumber - columns].GetComponent<Grid> ().waterOrigin = 
						GridSlots[_grid.gridNumber].GetComponent<Grid> ().waterOrigin;
					}
				}
				// Check if current and next pipe both have water [OK]
				if (GridSlots[_grid.gridNumber - columns].GetComponent<Grid> ().pipe.hasWater &&
				    GridSlots[_grid.gridNumber].GetComponent<Grid> ().pipe.hasWater) {
					// If true, explode both pipes [OK]
					if (GridSlots[_grid.gridNumber - columns].GetComponent<Grid> ().waterOrigin != 
					    GridSlots[_grid.gridNumber].GetComponent<Grid> ().waterOrigin) {
						GridSlots[_grid.gridNumber - columns].GetComponent<Grid> ().ClearGrid ();
						GridSlots[_grid.gridNumber].GetComponent<Grid> ().ClearGrid ();
					}
				}
			}

			// Check for top outlet leak [OK]
			else if (!GridSlots[_grid.gridNumber - columns].GetComponent<Grid> ().pipe.pipeHasBottom &&
			         GridSlots[_grid.gridNumber].GetComponent<Grid> ().pipe.pipeHasTop &&
			         GridSlots[_grid.gridNumber].GetComponent<Grid> ().pipe.hasWater) {
				GridSlots[_grid.gridNumber - columns].transform.GetChild (1).gameObject.SetActive (true);
				GridSlots[_grid.gridNumber - columns].transform.GetChild (1).rotation = Quaternion.Euler (0, 0, 90);
			}
		}
		
		// Check bottom [OK]
		if (_grid.gridNumber < (columns * rows) - columns) {
			//If this pipe has bottom outlet and next pipe has top outlet
			if (GridSlots [_grid.gridNumber + columns].GetComponent<Grid> ().pipe.pipeHasTop &&
			    GridSlots [_grid.gridNumber].GetComponent<Grid> ().pipe.pipeHasBottom) {
				// Increase the connected count [OK]
				if(GridSlots[_grid.gridNumber].GetComponent<Grid> ().pipe.connectedCount <
				   GridSlots[_grid.gridNumber].GetComponent<Grid> ().pipe.outletCount)
					GridSlots[_grid.gridNumber].GetComponent<Grid> ().pipe.connectedCount++;
				// Check if next pipe does not have water [OK]
				if (GridSlots [_grid.gridNumber + columns].GetComponent<Grid> ().pipe.hasWater == false &&
				    GridSlots[_grid.gridNumber].GetComponent<Grid> ().pipe.hasWater) {
					pipesWithWaterToProcess.Enqueue (GridSlots [_grid.gridNumber + columns]);
					// Check if next pipe is not water source, then set origin
					if (GridSlots[_grid.gridNumber + columns].GetComponent<Grid> ().isSource == false) {
					GridSlots [_grid.gridNumber + columns].GetComponent<Grid> ().pipe.hasWater = true;
					GridSlots[_grid.gridNumber + columns].GetComponent<Grid> ().waterOrigin = 
						GridSlots[_grid.gridNumber].GetComponent<Grid> ().waterOrigin;
					}
				}
				// Check if current and next pipe both have water [OK]
				if (GridSlots[_grid.gridNumber + columns].GetComponent<Grid> ().pipe.hasWater &&
				    GridSlots[_grid.gridNumber].GetComponent<Grid> ().pipe.hasWater) {
					// If true, explode both pipes [OK]
					if (GridSlots[_grid.gridNumber  + columns].GetComponent<Grid> ().waterOrigin != 
					    GridSlots[_grid.gridNumber].GetComponent<Grid> ().waterOrigin) {
						GridSlots[_grid.gridNumber + columns].GetComponent<Grid> ().ClearGrid ();
						GridSlots[_grid.gridNumber].GetComponent<Grid> ().ClearGrid ();
					}
				}
			}

			// Check for bottom outlet leak [OK]
			else if (!GridSlots[_grid.gridNumber + columns].GetComponent<Grid> ().pipe.pipeHasTop &&
			         GridSlots[_grid.gridNumber].GetComponent<Grid> ().pipe.pipeHasBottom &&
			         GridSlots[_grid.gridNumber].GetComponent<Grid> ().pipe.hasWater) {
				GridSlots[_grid.gridNumber + columns].transform.GetChild (1).gameObject.SetActive (true);
				GridSlots[_grid.gridNumber + columns].transform.GetChild (1).rotation = Quaternion.Euler (0, 0, 270);
			}
		}

		_grid.EvaluatePipeState ();
	}

	// BRONZE VERSION
	void CheckBronzeAdjacent (GameObject tile)
	{
		Grid _Bgrid = tile.GetComponent<Grid>();
		
		//Debug.Log ((LeftLimit (_grid.gridNumber)));
		//Debug.Log ((RightLimit (_grid.gridNumber)));
		
		// Check right [OK]
		if (_Bgrid.gridNumber > LeftLimit (_Bgrid.gridNumber) - 1 && _Bgrid.gridNumber < RightLimit (_Bgrid.gridNumber)) {
			//If this pipe has right outlet and next pipe has left outlet
			if (GridSlots[_Bgrid.gridNumber + 1].GetComponent<Grid> ().pipe.pipeHasLeft &&
			    GridSlots[_Bgrid.gridNumber].GetComponent<Grid> ().pipe.pipeHasRight) {
				// Increase the connected count [OK]
				if(GridSlots[_Bgrid.gridNumber].GetComponent<Grid> ().pipe.connectedCount <
				   GridSlots[_Bgrid.gridNumber].GetComponent<Grid> ().pipe.outletCount)
					GridSlots[_Bgrid.gridNumber].GetComponent<Grid> ().pipe.connectedCount++;
				// Check if next pipe does not have water [OK]
				if (GridSlots[_Bgrid.gridNumber + 1].GetComponent<Grid> ().pipe.hasWater == false &&
				    GridSlots[_Bgrid.gridNumber].GetComponent<Grid> ().pipe.hasWater) {
					bronzepipesWithWaterToProcess.Enqueue (GridSlots[_Bgrid.gridNumber + 1]);
					// Check if next pipe is not water source, then set origin
					if (GridSlots[_Bgrid.gridNumber + 1].GetComponent<Grid> ().isSource == false) {
						GridSlots[_Bgrid.gridNumber + 1].GetComponent<Grid> ().pipe.hasWater = true;
						GridSlots[_Bgrid.gridNumber + 1].GetComponent<Grid> ().waterOrigin = 
							GridSlots[_Bgrid.gridNumber].GetComponent<Grid> ().waterOrigin;
						GridSlots[_Bgrid.gridNumber + 1].GetComponent<Grid> ().sewageBuildingSources.Add ( 
							GridSlots[_Bgrid.gridNumber + 1].GetComponent<Grid> ().waterOrigin);
					}
				}
			}
			
			// Check for right outlet leak
			if (!GridSlots[_Bgrid.gridNumber + 1].GetComponent<Grid> ().pipe.pipeHasLeft &&
			         GridSlots[_Bgrid.gridNumber].GetComponent<Grid> ().pipe.pipeHasRight &&
			         GridSlots[_Bgrid.gridNumber].GetComponent<Grid> ().pipe.hasWater) {
				GridSlots[_Bgrid.gridNumber + 1].transform.GetChild (1).gameObject.SetActive (true);
				GridSlots[_Bgrid.gridNumber + 1].transform.GetChild (1).rotation = Quaternion.Euler (0, 0, 0);
			}
		}
		
		// Check left [OK]
		if (_Bgrid.gridNumber > LeftLimit (_Bgrid.gridNumber) && _Bgrid.gridNumber < RightLimit (_Bgrid.gridNumber) + 1) {
			//If this pipe has left outlet and next pipe has right outlet
			if (GridSlots[_Bgrid.gridNumber - 1].GetComponent<Grid> ().pipe.pipeHasRight &&
			    GridSlots[_Bgrid.gridNumber].GetComponent<Grid> ().pipe.pipeHasLeft) {
				// Increase the connected count [OK]
				if(GridSlots[_Bgrid.gridNumber].GetComponent<Grid> ().pipe.connectedCount <
				   GridSlots[_Bgrid.gridNumber].GetComponent<Grid> ().pipe.outletCount)
					GridSlots[_Bgrid.gridNumber].GetComponent<Grid> ().pipe.connectedCount++;
				// Check if next pipe does not have water [OK]
				if (GridSlots[_Bgrid.gridNumber - 1].GetComponent<Grid> ().pipe.hasWater == false &&
				    GridSlots[_Bgrid.gridNumber].GetComponent<Grid> ().pipe.hasWater) {
					bronzepipesWithWaterToProcess.Enqueue (GridSlots[_Bgrid.gridNumber - 1]);
					// Check if next pipe is not water source, then set origin
					if (GridSlots[_Bgrid.gridNumber - 1].GetComponent<Grid> ().isSource == false) {
						GridSlots[_Bgrid.gridNumber - 1].GetComponent<Grid> ().pipe.hasWater = true;
						GridSlots[_Bgrid.gridNumber - 1].GetComponent<Grid> ().waterOrigin = 
							GridSlots[_Bgrid.gridNumber].GetComponent<Grid> ().waterOrigin;
						GridSlots[_Bgrid.gridNumber - 1].GetComponent<Grid> ().sewageBuildingSources.Add ( 
							GridSlots[_Bgrid.gridNumber - 1].GetComponent<Grid> ().waterOrigin);
					}
				}
			}
			
			// Check for left outlet leak [OK]
			if (!GridSlots[_Bgrid.gridNumber - 1].GetComponent<Grid> ().pipe.pipeHasRight &&
			         GridSlots[_Bgrid.gridNumber].GetComponent<Grid> ().pipe.pipeHasLeft &&
			         GridSlots[_Bgrid.gridNumber].GetComponent<Grid> ().pipe.hasWater) {
				GridSlots[_Bgrid.gridNumber - 1].transform.GetChild (1).gameObject.SetActive (true);
				GridSlots[_Bgrid.gridNumber - 1].transform.GetChild (1).rotation = Quaternion.Euler (0, 0, 180);
			}
		}
		
		// Check top [OK]
		if (_Bgrid.gridNumber > columns) {
			//If this pipe has top outlet and next pipe has bottom outlet
			if (GridSlots[_Bgrid.gridNumber - columns].GetComponent<Grid> ().pipe.pipeHasBottom &&
			    GridSlots[_Bgrid.gridNumber].GetComponent<Grid> ().pipe.pipeHasTop) {
				// Increase the connected count [OK]
				if (GridSlots[_Bgrid.gridNumber].GetComponent<Grid> ().pipe.connectedCount <
				    GridSlots[_Bgrid.gridNumber].GetComponent<Grid> ().pipe.outletCount)
					GridSlots[_Bgrid.gridNumber].GetComponent<Grid> ().pipe.connectedCount++;
				// Check if next pipe does not have water [OK]
				if (GridSlots[_Bgrid.gridNumber - columns].GetComponent<Grid> ().pipe.hasWater == false &&
				    GridSlots[_Bgrid.gridNumber].GetComponent<Grid> ().pipe.hasWater) {
					bronzepipesWithWaterToProcess.Enqueue (GridSlots[_Bgrid.gridNumber - columns]);
					// Check if next pipe is not water source, then set origin
					if (GridSlots[_Bgrid.gridNumber - columns].GetComponent<Grid> ().isSource == false) {
						GridSlots[_Bgrid.gridNumber - columns].GetComponent<Grid> ().pipe.hasWater = true;
						GridSlots[_Bgrid.gridNumber - columns].GetComponent<Grid> ().waterOrigin = 
							GridSlots[_Bgrid.gridNumber].GetComponent<Grid> ().waterOrigin;
						GridSlots[_Bgrid.gridNumber - columns].GetComponent<Grid> ().sewageBuildingSources.Add ( 
						  GridSlots[_Bgrid.gridNumber - columns].GetComponent<Grid> ().waterOrigin);
					}
				}
			}
			
			// Check for top outlet leak [OK]
			if (!GridSlots[_Bgrid.gridNumber - columns].GetComponent<Grid> ().pipe.pipeHasBottom &&
			         GridSlots[_Bgrid.gridNumber].GetComponent<Grid> ().pipe.pipeHasTop &&
			         GridSlots[_Bgrid.gridNumber].GetComponent<Grid> ().pipe.hasWater) {
				GridSlots[_Bgrid.gridNumber - columns].transform.GetChild (1).gameObject.SetActive (true);
				GridSlots[_Bgrid.gridNumber - columns].transform.GetChild (1).rotation = Quaternion.Euler (0, 0, 90);
			}
		}
		
		// Check bottom [OK]
		if (_Bgrid.gridNumber < (columns * rows) - columns) {
			//If this pipe has bottom outlet and next pipe has top outlet
			if (GridSlots [_Bgrid.gridNumber + columns].GetComponent<Grid> ().pipe.pipeHasTop &&
			    GridSlots [_Bgrid.gridNumber].GetComponent<Grid> ().pipe.pipeHasBottom) {
				// Increase the connected count [OK]
				if(GridSlots[_Bgrid.gridNumber].GetComponent<Grid> ().pipe.connectedCount <
				   GridSlots[_Bgrid.gridNumber].GetComponent<Grid> ().pipe.outletCount)
					GridSlots[_Bgrid.gridNumber].GetComponent<Grid> ().pipe.connectedCount++;
				// Check if next pipe does not have water [OK]
				if (GridSlots [_Bgrid.gridNumber + columns].GetComponent<Grid> ().pipe.hasWater == false &&
				    GridSlots[_Bgrid.gridNumber].GetComponent<Grid> ().pipe.hasWater) {
					bronzepipesWithWaterToProcess.Enqueue (GridSlots [_Bgrid.gridNumber + columns]);
					// Check if next pipe is not water source, then set origin
					if (GridSlots[_Bgrid.gridNumber + columns].GetComponent<Grid> ().isSource == false) {
						GridSlots [_Bgrid.gridNumber + columns].GetComponent<Grid> ().pipe.hasWater = true;
						GridSlots[_Bgrid.gridNumber + columns].GetComponent<Grid> ().waterOrigin = 
							GridSlots[_Bgrid.gridNumber].GetComponent<Grid> ().waterOrigin;
						GridSlots[_Bgrid.gridNumber + columns].GetComponent<Grid> ().sewageBuildingSources.Add ( 
						 GridSlots[_Bgrid.gridNumber + columns].GetComponent<Grid> ().waterOrigin	);
					}
				}
			}
			
			// Check for bottom outlet leak [OK]
			if (!GridSlots[_Bgrid.gridNumber + columns].GetComponent<Grid> ().pipe.pipeHasTop &&
			         GridSlots[_Bgrid.gridNumber].GetComponent<Grid> ().pipe.pipeHasBottom &&
			         GridSlots[_Bgrid.gridNumber].GetComponent<Grid> ().pipe.hasWater) {
				GridSlots[_Bgrid.gridNumber + columns].transform.GetChild (1).gameObject.SetActive (true);
				GridSlots[_Bgrid.gridNumber + columns].transform.GetChild (1).rotation = Quaternion.Euler (0, 0, 270);
			}
		}
		
		_Bgrid.EvaluatePipeState ();
	}

	void CheckBuildingSource ()	// Silver [OK]
	{
		foreach (GameObject grid in GridSlots) {
			Grid gridComponent = grid.GetComponent<Grid> ();

			if(gridComponent.isBuilding) {
				if (gridComponent.pipe.hasWater) {
					gridComponent.waterOrigin.GetComponent<Reservoir> ().connectedBuildings.Add (grid);
					gridComponent.waterOrigin.GetComponent<Reservoir> ().CalculateDrainRate (gridComponent.building.drainRate);
					fundManager.CalculateCloudFund (gridComponent.building.payOut);
				}
			}
		}
	}

	void CheckBronzeBuildingSource ()	// Bronze
	{
		/*
		foreach (GameObject grid in GridSlots) {
			Grid gridComponent = grid.GetComponent<Grid> ();
			
			if (gridComponent.isDrain) {
				if (gridComponent.pipe.hasWater) {
					foreach (GameObject building in BuildingWaterSources) {
						if (building.GetComponent<Grid> ().pipe.hasWater
							&& !building.GetComponent<Grid> ().pipe.isLeaking) {
							gridComponent.GetComponent<Drain> ().connectedBuildings.Add (building);
							gridComponent.GetComponent<Drain> ().ComputeDrainOutput ();
						}
					}
				//	gridComponent.GetComponent<Drain> ().ComputeDrainOutput (); // <<<<<----- CHECK
				}
			}							
		}
		*/

		foreach (GameObject building in BuildingWaterSources) {
			Grid gridComponent = building.GetComponent<Grid> ();
			if (building.GetComponent<Grid> ().pipe.hasWater
			    && !building.GetComponent<Grid> ().pipe.isLeaking) {
				//gridComponent.GetComponent<Drain> ().connectedBuildings.Add (building);
				//gridComponent.GetComponent<Drain> ().ComputeDrainOutput ();
				waterManager.ComputeBuildingOutput ();
				waterManager.ComputeWasteOutput (0, false);
			}
		}
	}

	void CheckPipeLeak ()
	{
		foreach (GameObject grid in GridSlots) {
			Grid gridComponent = grid.GetComponent<Grid> ();
			
			if(gridComponent.waterOrigin != null && gridComponent.pipe.isLeaking) {
				gridComponent.waterOrigin.GetComponent<Reservoir> ().CalculateDrainRate (-0.5f *
				                                                                         (gridComponent.pipe.outletCount - 
				                                                                         gridComponent.pipe.connectedCount));	
			}
		}
	}
}
