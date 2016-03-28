using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler {

	public Pipe pipe;
	public Building building;

	public bool isSource;
	public bool isDrain;
	public bool isBuilding;
	public bool isObstacle;
	public bool isBronze;
	public bool isSilver;
	public GameObject waterOrigin;
	public List<GameObject> sewageBuildingSources = new List<GameObject> ();

	public int gridNumber;
	public static Color32 colorTransparent = new Color32 (0, 0, 0, 0);
	public static Color32 colorWater = new Color32 (0, 255, 255, 255);
	public int connectedOutlets;

	public GameObject pipeImage;
	public Image img_pipe;
	Image img_tooltip;
	CanvasGroup tooltipCanvasGroup;

	PipeDatabase pipeDatabase;
	BuildingDatabase buildingDatabase;
	WaterManager waterManager;
	InputManager2 inputManager2;

	public GridLayout gridLayout;
	public GridLayout bronzeLayout;

    public float sfxVolume = 0.25f;

    public float counter = 0;
	public int interval = 1;
	public float decay = 1F;

    Image _image;
    Reservoir _reservoir;

	void Awake ()
	{
		pipeImage.SetActive (false);
        _image = GetComponent<Image>();
        _reservoir = GetComponent<Reservoir>();
        img_pipe = pipeImage.GetComponent<Image> ();
		img_tooltip = GameObject.Find ("Tooltip").GetComponent<Image> ();
		tooltipCanvasGroup = img_tooltip.GetComponent<CanvasGroup> ();
		pipeDatabase = GameObject.Find ("PipeDatabase").GetComponent<PipeDatabase> ();
		buildingDatabase = GameObject.Find ("BuildingDatabase").GetComponent<BuildingDatabase> ();

		waterManager = GameObject.Find ("WaterManager").GetComponent <WaterManager> ();
		inputManager2 = GameObject.Find ("InputManager").GetComponent <InputManager2> ();
	}

	void Start ()
	{
		bronzeLayout = GameObject.FindGameObjectWithTag ("Bronze").GetComponent<GridLayout> ();
		gridLayout = this.GetComponentInParent<GridLayout> ();

		decay = 1F;

		if (transform.parent.tag == "Bronze")
			isBronze = true;
		if (transform.parent.tag == "Silver")
			isSilver = true;
	}

	void LateUpdate ()
	{
		EvaluatePipeState ();	// Implement another breadth-first search with queue?
	}

	#region IPointerDownHandler implementation

	public void OnPointerDown (PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			// For regular pipe grids
			if(!isSource && !isBuilding)
			{
				AddGrid ();
			}

			// For water source pipe grids
			if(isSource)
			{
				InputManager2.CancelTooltip ();
				inputManager2.ResetCursor ();
				ToggleWater ();
			}

			if(!Slot._isDragging && InputManager2.isRecycle && !isSource && !isObstacle && !isDrain && img_pipe.sprite != null) {
				Refund ();
				ClearGrid ();
				//InputManager2.isRecycle = false;
			}

			if(!Slot._isDragging && InputManager2.isFix && !isSource && !isObstacle && !isDrain && img_pipe.sprite != null) {
				FixPipes ();
				//ClearGrid ();
				//InputManager2.isRecycle = false;
			}

		}

		/*
		if (eventData.button == PointerEventData.InputButton.Right)
		{
			if(!Slot._isDragging) {
			ClearGrid ();
			}
		}
		*/
	}

	#endregion

	public void AddGrid ()
	{
		if (img_pipe.sprite == null && img_tooltip.sprite != null)
		{
			if (transform.parent.tag == "Bronze" && Slot.clickedPipe.pipeType == Pipe.PipeType.Bronze && FundManager.totalFund >= Slot.clickedPipe.pipePrice)
			{
				pipe = new Pipe (Slot.clickedPipe.pipeName, Slot.clickedPipe.pipeID, Slot.clickedPipe.pipePrice,
				                 Slot.clickedPipe.pipeAmount, Pipe.PipeType.Bronze,
				                 Slot.clickedPipe.pipeHasTop, Slot.clickedPipe.pipeHasRight,
				                 Slot.clickedPipe.pipeHasBottom, Slot.clickedPipe.pipeHasLeft,
				                 Slot.clickedPipe.outletCount, Slot.clickedPipe.connectedCount,
				                 Slot.clickedPipe.isConnected, Slot.clickedPipe.hasWater, Slot.clickedPipe.pipeHealth);

				pipeImage.SetActive(true);
				img_pipe.sprite = img_tooltip.sprite;
				FundManager.totalFund -= Slot.clickedPipe.pipePrice;
                _image.enabled = false;
				img_pipe.rectTransform.rotation = Quaternion.Euler (0, 0, InputManager2.currentRotation);

				InputManager2.CancelTooltip();

				gridLayout.EvaluateGrid();
				bronzeLayout.EvaluateGrid();

				AudioManager.Instance.RandomizeSFX(AudioDatabase.fundsound1, AudioDatabase.fundsound2);
			}

			if (transform.parent.tag == "Silver" && Slot.clickedPipe.pipeType == Pipe.PipeType.Silver && FundManager.totalFund >= Slot.clickedPipe.pipePrice)
			{
				pipe = new Pipe (Slot.clickedPipe.pipeName, Slot.clickedPipe.pipeID, Slot.clickedPipe.pipePrice,
				                 Slot.clickedPipe.pipeAmount, Pipe.PipeType.Silver,
				                 Slot.clickedPipe.pipeHasTop, Slot.clickedPipe.pipeHasRight,
				                 Slot.clickedPipe.pipeHasBottom, Slot.clickedPipe.pipeHasLeft,
				                 Slot.clickedPipe.outletCount, Slot.clickedPipe.connectedCount,
				                 Slot.clickedPipe.isConnected, Slot.clickedPipe.hasWater, Slot.clickedPipe.pipeHealth);

				pipeImage.SetActive (true);
				img_pipe.sprite = img_tooltip.sprite;
				FundManager.totalFund -= Slot.clickedPipe.pipePrice;
                _image.enabled = false;
				img_pipe.rectTransform.rotation = Quaternion.Euler (0, 0, InputManager2.currentRotation);

				InputManager2.CancelTooltip();

				gridLayout.EvaluateGrid();
				bronzeLayout.EvaluateGrid();

				AudioManager.Instance.RandomizeSFX(AudioDatabase.fundsound1, AudioDatabase.fundsound2);
			}
		}
	}

	public void ClearGrid()
	{
		if (img_pipe.sprite != null && !isSource)
		{
			pipe = pipeDatabase.pipes[0];
			//pipe.hasWater = false;
			img_pipe.sprite = null;						// Clear pipe sprite'
			pipeImage.SetActive(false);                // Turn off pipe image child
            _image.enabled = true;	// Turn on transparent pixel icon

			gridLayout.EvaluateGrid();
			bronzeLayout.EvaluateGrid();

			EvaluatePipeState();
		}
	}
	
	public void EvaluatePipeState()
	{
		if (!isBuilding && !isSource && isSilver) {
			if (pipe.hasWater) {
				DegradePipes();
				if (pipe.pipeHealth > 25F) {
					img_pipe.color = colorWater;
				} else if (pipe.pipeHealth < 25F) {
					img_pipe.color = new Color (1F, 0.5F, 0.5F, colorWater.a);
				} /*else {
					img_pipe.color = Color.white;*/
			} else {
				img_pipe.color = Color.white;
			}
		}

		if (isSource && isSilver) {
			if (pipe.hasWater)
				img_pipe.color = colorWater;
			else {
				img_pipe.color = Color.white;
			}

			if (pipe.hasWater && _reservoir.currentCapacity <= 0) {
				ToggleWater();
			}
		}

		if (isBuilding && isSilver) {
			if (pipe.hasWater) {
				_image.color = Color.white;
				bronzeLayout.GridSlots[this.gridNumber].GetComponent<Grid> ().pipe.hasWater = true;
				bronzeLayout.GridSlots[this.gridNumber].GetComponent<Image> ().color = Color.white;
				//bronzeLayout.EvaluateGrid (); // Updater fix

				//waterOrigin.GetComponent<Reservoir> ().CalculateDrainRate (building.drainRate);
			} else {
				_image.color = Color.gray;
				bronzeLayout.GridSlots[this.gridNumber].GetComponent<Grid> ().pipe.hasWater = false;
				bronzeLayout.GridSlots[this.gridNumber].GetComponent<Image> ().color = Color.gray;
				//bronzeLayout.EvaluateGrid ();
			}
		}

		if (isBuilding && isBronze && isSource) {
		}

		if (pipe.outletCount > pipe.connectedCount && pipe.hasWater)
		{
			pipe.isLeaking = true;
			if(!AudioManager.Instance.isContinuousPlaying)
				AudioManager.Instance.PlaySFX(AudioDatabase.splashSound);
		}
		else 
		{ 
			pipe.isLeaking = false;
            AudioManager.Instance.StopSFXContinuous();
		}

	}

	public void ToggleWater ()
	{
		pipe.hasWater = !pipe.hasWater;
		waterManager.ResetDrainOutput ();
		//bronzeLayout.transform.GetComponent<Grid> ().pipe.hasWater = !bronzeLayout.transform.GetComponent<Grid> ().pipe.hasWater; ;
		gridLayout.EvaluateGrid ();
		bronzeLayout.EvaluateGrid ();	// [OK]
		//EvaluatePipeState (); // <<< LateUpdate
		AudioManager.Instance.PlaySFX(AudioDatabase.toggleWaterSound);
	}
	
	#region IPointerEnterHandler implementation

	public void OnPointerEnter (PointerEventData eventData)
	{
		InputManager2.mousedOverGrid = this.transform.gameObject;
	}

	#endregion

	#region IPointerExitHandler implementation

	public void OnPointerExit (PointerEventData eventData)
	{
		InputManager2.mousedOverGrid = null;
	}

	#endregion

	void Refund ()
	{
		FundManager.totalFund += pipe.pipePrice / 2;
		AudioManager.Instance.RandomizeSFX(AudioDatabase.fundsound1, AudioDatabase.fundsound2);
	}

	void DegradePipes ()
	{
		counter += 1 * Time.deltaTime;
		if (counter >= interval) {
			int chance = Random.Range(1, 10);
			if (chance >= 5) {
				pipe.pipeHealth -= decay;
			}

			counter = 0;
		}

		if (pipe.pipeHealth <= 0) { ClearGrid (); }
	}

	void FixPipes ()
	{
		if (pipe.pipeHealth < 25F && FundManager.totalFund >= 5) {
			pipe.pipeHealth = 100F;
			FundManager.totalFund -= 5;
			AudioManager.Instance.RandomizeSFX(AudioDatabase.fundsound1, AudioDatabase.fundsound2);
		}
	}
}
