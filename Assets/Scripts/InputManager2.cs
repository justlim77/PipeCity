using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class InputManager2 : MonoBehaviour {

	Fader fader;
	public static bool reduceVolume = false;

	// Canvas controls
	public GameObject upgradeMenu;
	public CanvasGroup upgradeMenuCanvas;

	// Pause controls
	public GameObject pauseMenu;
	public CanvasGroup pauseMenuCanvas;
	public CanvasGroup interactivePanelCanvas;
	public static bool _isPaused;

	// Help controls
	public GameObject helpMenu;
	public CanvasGroup helpMenuCanvas;

	// Fund controls
	public int fundIncrease = 100;

	// Time controls
	private float fl_lastTime;
	private float fl_currentTime = 1.0f;
	private float fl_timeIncrease = 2.0f;

	// Tooltip controls
	private Image img_pipe;
	private Inventory inventory;
	public static Image img_tooltip;
	public static int tooltipNumber;
	public static CanvasGroup tooltipCanvas;
	public float fl_offset = 0;
	public static int currentRotation = 0;
	public static int rotateLPipe = 0;
	public static int rotateTPipe = 0;

	// Tab controls
	public GameObject bronzeGrid;
	public GameObject silverGrid;
	public List<GameObject> bronzeList = new List<GameObject> ();
	public List<GameObject> silverList = new List<GameObject> ();
	public static Color transparent = new Color (1, 1, 1, 0.5f);
	public static Color opaque =  new Color (1, 1, 1, 1.0f);

	// Hierarchy Sorting Order
	GameObject go_year;
	GameObject go_fund;
	GameObject go_timer;
	GameObject go_water;
	GameObject go_inventory;
	GameObject go_hotbar;
	int inventoryOrder;
	int hotbarOrder;
	int _bronzeOrder;
	int _silverOrder;
	int tooltipOrder;
	int pausepanelOrder;
	int yearOrder;
	int fundOrder;
	int timerOrder;
	int waterOrder;

	// Cancel controls
	public static GameObject mousedOverGrid;		// Returns gameobject from "Grid" script pointerEnter
	public static bool isRecycle;
	public Texture2D recycleCursor;
	public static bool isFix;
	public Texture2D fixCursor;
	public CursorMode cursorMode = CursorMode.Auto;
	public Vector2 hotSpot = Vector2.zero;

	#region Assign Keyboard Shortcut Values

	public KeyCode pauseKey = KeyCode.Escape;
	public KeyCode upgradeKey = KeyCode.U;
	public KeyCode tabKey = KeyCode.Tab;
	public KeyCode screenshotKey = KeyCode.F12;

	public KeyCode speedtimeupKey = KeyCode.PageUp;
	public KeyCode slowtimedownKey = KeyCode.PageDown;
	public KeyCode normalizetimeKey = KeyCode.Home;

	public KeyCode upFund = KeyCode.LeftBracket;
	public KeyCode downFund = KeyCode.RightBracket;

	public KeyCode recycleKey = KeyCode.R;
	public KeyCode fixKey = KeyCode.F;
	public int cleartooltipKey = 1;
	public int rotatepipeKey = 2;
	//public KeyCode rotatepipeKey = KeyCode.Space;
	public KeyCode deletepipeKey = KeyCode.Z;

	#endregion

	void Awake ()
	{
		fader = GameObject.Find ("Managers").GetComponent<Fader>();

		upgradeMenu = GameObject.Find ("UpgradeMenu");
		upgradeMenuCanvas = upgradeMenu.GetComponent<CanvasGroup>();
		upgradeMenuCanvas.alpha = 0;

		helpMenu = GameObject.Find ("HelpCanvas");
		helpMenuCanvas = helpMenu.GetComponent<CanvasGroup>();
		helpMenuCanvas.alpha = 0;

		go_year = GameObject.Find ("Year");
		go_fund = GameObject.Find ("Fund");
		go_timer = GameObject.Find ("Timer");
		go_water = GameObject.Find ("Water");
		go_inventory = GameObject.FindGameObjectWithTag("Inventory");
		go_hotbar = GameObject.Find ("Hotbar");

		pauseMenu = GameObject.Find ("PauseMenu");															// Return gameobject PausePanel by name
		pauseMenuCanvas = pauseMenu.GetComponent<CanvasGroup>();											// Return canvas group component
		interactivePanelCanvas = pauseMenu.transform.GetChild (0).GetComponentInChildren<CanvasGroup>();

		inventory = GameObject.FindGameObjectWithTag ("Inventory").GetComponent<Inventory>();
		img_tooltip = GameObject.Find ("Tooltip").GetComponent<Image>();									// Return Tooltip game object's Image component
		tooltipCanvas = img_tooltip.GetComponent<CanvasGroup>();											// Return Tooltip's CanvasGroup component
		//fl_offset = tooltip.rectTransform.sizeDelta.x / 2;												// Offset for rotation pivot

		bronzeGrid = GameObject.Find ("BronzeGrid");
		silverGrid = GameObject.Find ("SilverGrid");

		bronzeList = bronzeGrid.GetComponent<GridLayout>().GridSlots;
		silverList = silverGrid.GetComponent<GridLayout>().GridSlots;

		mousedOverGrid = null;
		isRecycle = false;
		isFix = false;

		recycleCursor = Resources.Load<Texture2D>("sprites/ui/cursors/recycle");
		fixCursor = Resources.Load<Texture2D>("sprites/ui/cursors/wrench");
	}

	void Start ()
	{
		InitializeOrder ();
	}

	void Update ()
	{
		PauseControl ();
		TimeControl ();
		FundControl ();
		SwitchTooltip ();
		TooltipControl ();
		TabControl ();
		CursorControl ();
	}

	void LateUpdate ()
	{
		ScreenshotControl ();			
	}

	// Set up hierarchy sorting of 2D GUI elements
	void InitializeOrder ()
	{
		/*
		go_inventory.transform.SetAsFirstSibling ();
		inventoryOrder = go_inventory.transform.GetSiblingIndex ();
		*/
		go_year.transform.SetAsLastSibling ();
		yearOrder = go_year.transform.GetSiblingIndex ();

		go_fund.transform.SetSiblingIndex (yearOrder - 1);
		fundOrder = go_fund.transform.GetSiblingIndex ();

		go_timer.transform.SetSiblingIndex (fundOrder - 1);
		timerOrder = go_timer.transform.GetSiblingIndex ();

		go_water.transform.SetSiblingIndex (timerOrder - 1);
		waterOrder = go_water.transform.GetSiblingIndex ();

		pauseMenu.transform.SetSiblingIndex (waterOrder - 1);
		pausepanelOrder = pauseMenu.transform.GetSiblingIndex ();

		go_inventory.transform.SetSiblingIndex (pausepanelOrder - 1);
		inventoryOrder = go_inventory.transform.GetSiblingIndex ();
		
		go_hotbar.transform.SetSiblingIndex (inventoryOrder - 1);
		hotbarOrder = go_hotbar.transform.GetSiblingIndex ();

		img_tooltip.rectTransform.SetSiblingIndex (hotbarOrder - 1);
		tooltipOrder = img_tooltip.rectTransform.GetSiblingIndex ();

		silverGrid.transform.SetSiblingIndex (tooltipOrder - 1);
		_silverOrder = silverGrid.transform.GetSiblingIndex ();

		bronzeGrid.transform.SetSiblingIndex (_silverOrder - 1);
		_bronzeOrder = bronzeGrid.transform.GetSiblingIndex ();

		_isPaused = false;
	}

	void PauseControl ()
	{
		if(Input.GetKeyUp (pauseKey))
			_isPaused = !_isPaused;								// Switch between paused and not paused

		if(_isPaused)
		{
			CancelTooltip ();									// Remove tooltip
			pauseMenu.SetActive (true);							// Set pausePanel game object to active
			pauseMenuCanvas.alpha = 1;							// Set to fully visible
			pauseMenuCanvas.blocksRaycasts = true;				// Enable raycast blocker
			interactivePanelCanvas.ignoreParentGroups = true; 	// Ignore parent setting
			Time.timeScale = 0;									// Time.timeScale 0 to pause game
		}
		else
		{
			pauseMenu.SetActive (false);						// Disable game object
			pauseMenuCanvas.alpha = 0;							// Set to fully invisible
			pauseMenuCanvas.blocksRaycasts = false;				// Disable raycast blocker
			interactivePanelCanvas.ignoreParentGroups = false; 	// Ignore parent setting
			Time.timeScale = fl_currentTime;					// Set timeScale to timeScale before pausing
		}
	}

	public void PauseToggle (bool isPause)
	{
		_isPaused = isPause;
	}

	public void ReloadLevelToggle (bool isReload)
	{
		if(isReload)
		{
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			PauseToggle (_isPaused);
		}
	}

	public void QuitToggle (bool isQuit)
	{
		if(isQuit)
		{
			//PauseToggle (_isPaused);
			//Application.LoadLevel(0);
			StartCoroutine(ChangeLevel());
		}
	}

	IEnumerator ChangeLevel()
	{
		PauseToggle (false);
		float fadeTime = fader.BeginFade(1);
		reduceVolume = true;
		yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(0);
	}

	void TimeControl ()
	{
		if(Input.GetKeyUp(speedtimeupKey) && !_isPaused)
			fl_currentTime += fl_timeIncrease;

		if(Input.GetKeyUp(slowtimedownKey) && !_isPaused)
			if(fl_currentTime > 0)
				fl_currentTime -= fl_timeIncrease;

		if(Input.GetKeyUp(normalizetimeKey) && !_isPaused)
			fl_currentTime = 1;
	}

	void FundControl ()
	{
		if(Input.GetKeyUp(upFund) && !_isPaused)
			FundManager.totalFund += fundIncrease;

		if(Input.GetKeyUp (downFund) && !_isPaused)
			FundManager.totalFund -= fundIncrease;
	}

	#region Tooltip Control Methods [Moving, Rotating, Switching, Assigning, Cancelling]

	void TooltipControl ()
	{
		if(Slot._isDragging && !_isPaused)
		{
			// Movement
			img_tooltip.transform.position = new Vector2 (Input.mousePosition.x + fl_offset, Input.mousePosition.y - fl_offset);

			// Rotation
			if(Input.GetMouseButtonDown (rotatepipeKey) && (Slot.clickedPipe.pipeID == 1 || Slot.clickedPipe.pipeID == 5))
			{
				RotationControl ();

				Slot.clickedPipe.pipeHasTop = !Slot.clickedPipe.pipeHasTop;
				Slot.clickedPipe.pipeHasRight = !Slot.clickedPipe.pipeHasRight;
				Slot.clickedPipe.pipeHasBottom = !Slot.clickedPipe.pipeHasBottom;
				Slot.clickedPipe.pipeHasLeft = !Slot.clickedPipe.pipeHasLeft;
			}

			if(Input.GetMouseButtonDown (rotatepipeKey) && (Slot.clickedPipe.pipeID == 2 || Slot.clickedPipe.pipeID == 6))
			{
				RotationControl ();

				if(rotateLPipe == 0)
				{
					rotateLPipe = 1;
					Slot.clickedPipe.pipeHasTop = !Slot.clickedPipe.pipeHasTop;
					Slot.clickedPipe.pipeHasBottom = !Slot.clickedPipe.pipeHasBottom; 
				}

				else if(rotateLPipe == 1)
				{
					rotateLPipe = 2;
					Slot.clickedPipe.pipeHasRight = !Slot.clickedPipe.pipeHasRight;
					Slot.clickedPipe.pipeHasLeft = !Slot.clickedPipe.pipeHasLeft; 
				}
				
				else if(rotateLPipe == 2)
				{
					rotateLPipe = 3;
					Slot.clickedPipe.pipeHasTop = !Slot.clickedPipe.pipeHasTop;
					Slot.clickedPipe.pipeHasBottom = !Slot.clickedPipe.pipeHasBottom;
				}

				else if(rotateLPipe == 3)
				{
					rotateLPipe = 1;
					Slot.clickedPipe.pipeHasRight = !Slot.clickedPipe.pipeHasRight;
					Slot.clickedPipe.pipeHasLeft = !Slot.clickedPipe.pipeHasLeft; 
				}
			}

			if(Input.GetMouseButtonDown (rotatepipeKey) && (Slot.clickedPipe.pipeID == 3 || Slot.clickedPipe.pipeID == 7))
			{
				RotationControl ();

				if(rotateTPipe == 0)
				{
					rotateTPipe = 1;
					Slot.clickedPipe.pipeHasTop = !Slot.clickedPipe.pipeHasTop;
					Slot.clickedPipe.pipeHasRight = !Slot.clickedPipe.pipeHasRight;
				}

				else if(rotateTPipe == 1)
				{
					rotateTPipe = 2;
					Slot.clickedPipe.pipeHasRight = !Slot.clickedPipe.pipeHasRight;
					Slot.clickedPipe.pipeHasBottom = !Slot.clickedPipe.pipeHasBottom;
				}

				else if(rotateTPipe == 2)
				{
					rotateTPipe = 3;
					Slot.clickedPipe.pipeHasBottom = !Slot.clickedPipe.pipeHasBottom;
					Slot.clickedPipe.pipeHasLeft = !Slot.clickedPipe.pipeHasLeft;
				}

				else if(rotateTPipe == 3)
				{
					rotateTPipe = 1;
					Slot.clickedPipe.pipeHasTop = !Slot.clickedPipe.pipeHasTop;
					Slot.clickedPipe.pipeHasLeft = !Slot.clickedPipe.pipeHasLeft;
				}
			}
		}

		if(Input.GetMouseButtonDown (cleartooltipKey) && !_isPaused)
			CancelTooltip ();
	}

	void RotationControl ()
	{
		AudioManager.Instance.PlaySFX(AudioDatabase.rotatePipeSound);
		currentRotation -= 90;
		img_tooltip.rectTransform.rotation = Quaternion.Euler (0, 0, currentRotation);
	}

	public void ResetPipeConnectorFlags ()
	{
		if(Slot.clickedPipe.pipeID == 1 || Slot.clickedPipe.pipeID == 5)	// Reset flags for I pipes			
		{
			Slot.clickedPipe.pipeHasTop = false;
			Slot.clickedPipe.pipeHasRight = true;
			Slot.clickedPipe.pipeHasBottom = false;
			Slot.clickedPipe.pipeHasLeft = true;
		}
		
		else if(Slot.clickedPipe.pipeID == 2 || Slot.clickedPipe.pipeID == 6)	// Reset flags for L pipes		
		{
			Slot.clickedPipe.pipeHasTop = true;
			Slot.clickedPipe.pipeHasRight = true;
			Slot.clickedPipe.pipeHasBottom = false;
			Slot.clickedPipe.pipeHasLeft = false;
		}
		
		else if(Slot.clickedPipe.pipeID == 3 || Slot.clickedPipe.pipeID == 7)	// Reset flags for T pipes			
		{
			Slot.clickedPipe.pipeHasTop = false;
			Slot.clickedPipe.pipeHasRight = true;
			Slot.clickedPipe.pipeHasBottom = true;
			Slot.clickedPipe.pipeHasLeft = true;
		}
		
		// Why would you even rotate an X pipe?
	}

	void SwitchTooltip ()
	{
		if(Input.GetKeyDown (KeyCode.Alpha1)) { tooltipNumber = 1; AssignTooltip(tooltipNumber); ResetCursor (); }
		if(Input.GetKeyDown (KeyCode.Alpha2)) { tooltipNumber = 2; AssignTooltip(tooltipNumber); ResetCursor ();}
		if(Input.GetKeyDown (KeyCode.Alpha3)) { tooltipNumber = 3; AssignTooltip(tooltipNumber); ResetCursor ();}
		if(Input.GetKeyDown (KeyCode.Alpha4)) { tooltipNumber = 4; AssignTooltip(tooltipNumber); ResetCursor ();}
		if(Input.GetKeyDown (KeyCode.Alpha5)) { tooltipNumber = 5; AssignTooltip(tooltipNumber); ResetCursor ();}
		if(Input.GetKeyDown (KeyCode.Alpha6)) { tooltipNumber = 6; AssignTooltip(tooltipNumber); ResetCursor ();}
		if(Input.GetKeyDown (KeyCode.Alpha7)) { tooltipNumber = 7; AssignTooltip(tooltipNumber); ResetCursor ();}
		if(Input.GetKeyDown (KeyCode.Alpha8)) { tooltipNumber = 8; AssignTooltip(tooltipNumber); ResetCursor ();}
	}

	void AssignTooltip (int tooltipNumber)
	{
		CancelTooltip();										// Reset to default state

		Slot.clickedPipe = inventory.Pipes [tooltipNumber];

		ResetPipeConnectorFlags ();

		img_tooltip.sprite = Slot.clickedPipe.pipeIcon;			// Set tooltip sprite to selected icon sprite
		img_tooltip.type = Image.Type.Simple;					// Set image type to "Simple"
		tooltipCanvas.alpha = 1;								// Set image to visible
		Slot._isDragging = true;								// Set _isDragging to true for InputManager script
	}

	public static void CancelTooltip ()
	{
		tooltipCanvas.alpha = 0;
		Slot._isDragging = false;
		Slot.clickedPipe = null;
		img_tooltip.sprite = null;
		tooltipNumber = 0;
		rotateLPipe = 0;
		rotateTPipe = 0;
		currentRotation = 0;
		img_tooltip.rectTransform.rotation = Quaternion.Euler (0, 0, currentRotation);
		img_tooltip.rectTransform.anchoredPosition = Vector2.zero;
	}
	
	#endregion

	void TabControl ()
	{
		_bronzeOrder = bronzeGrid.transform.GetSiblingIndex ();
		_silverOrder = silverGrid.transform.GetSiblingIndex ();

		if (Input.GetKeyDown (tabKey) && !_isPaused)
		{
			if(_bronzeOrder < _silverOrder)
			{
				bronzeGrid.transform.SetSiblingIndex (tooltipOrder - 1);

				foreach (GameObject grid in bronzeList)
				{
					Image img_pipe = grid.transform.GetChild (0).GetComponent<Image>();
					img_pipe.color = new Color (img_pipe.color.r, img_pipe.color.g, img_pipe.color.b, opaque.a);
				}
				foreach (GameObject grid in silverList)
				{
					Image img_pipe = grid.transform.GetChild (0).GetComponent<Image>();
					img_pipe.color = new Color (img_pipe.color.r, img_pipe.color.g, img_pipe.color.b, transparent.a);
				}
			}
			else if (_silverOrder < _bronzeOrder)
			{
				silverGrid.transform.SetSiblingIndex (tooltipOrder - 1);

				foreach (GameObject grid in bronzeList)
				{
					Image img_pipe = grid.transform.GetChild (0).GetComponent<Image>();
					img_pipe.color = new Color (img_pipe.color.r, img_pipe.color.g, img_pipe.color.b, transparent.a);
				}
				foreach (GameObject grid in silverList)
				{
					Image img_pipe = grid.transform.GetChild (0).GetComponent<Image>();
					img_pipe.color = new Color (img_pipe.color.r, img_pipe.color.g, img_pipe.color.b, opaque.a);
				}
			}
		}
	}

	public void CursorControl ()
	{
		if (Slot._isDragging)
			Cursor.visible = false;
        else
			Cursor.visible = true;

		if (Input.GetKeyDown (recycleKey) && !Slot._isDragging) {
			isRecycle = true;
			isFix = false;
		}

		if (isRecycle)
			Cursor.SetCursor (recycleCursor, hotSpot, cursorMode);

		if (Input.GetKeyDown (fixKey) && !Slot._isDragging) {
			isFix = true;
			isRecycle = false;
		}

		if (isFix)
			Cursor.SetCursor (fixCursor, hotSpot, cursorMode);

		if (Input.GetMouseButtonDown (cleartooltipKey)) {
			isRecycle = false;
			isFix = false;
			Cursor.SetCursor (null, Vector2.zero, cursorMode);
		}

		/*
		if (isRecycle == false) {
			Cursor.SetCursor (null, Vector2.zero, cursorMode);
		}
		*/
	}

	public void RecycleClick ()
	{
		isRecycle = true;
	}

	public void FixClick ()
	{
		isFix = true;
	}


	public void ResetCursor ()
	{
		isRecycle = false;
		isFix = false;
		Cursor.SetCursor (null, Vector2.zero, cursorMode);
	}

	void ScreenshotControl ()
	{
		if (Input.GetKeyDown (screenshotKey))
		{
			string currentTime = string.Format("{0:yyyy-MM-dd_HH-mm-SS}", System.DateTime.Now);
			Application.CaptureScreenshot ("Screenshots/" + currentTime + ".png");
			Debug.Log ("Screenshot saved.");
		}
	}

	public void ToggleUpgrade (bool isToggle)
	{
		if (isToggle) {
			upgradeMenuCanvas.alpha = 1;
			upgradeMenuCanvas.interactable = isToggle;
			upgradeMenuCanvas.blocksRaycasts = isToggle;
			upgradeMenu.transform.SetSiblingIndex (waterOrder - 1);
		} else {
			upgradeMenuCanvas.alpha = 0;
			upgradeMenuCanvas.interactable = isToggle;
			upgradeMenuCanvas.blocksRaycasts = isToggle;
			upgradeMenu.transform.SetAsFirstSibling ();
		}
	}

	public void ToggleHelp (bool isToggle)
	{
		if (isToggle) {
			helpMenuCanvas.alpha = 1;
			helpMenuCanvas.interactable = isToggle;
			helpMenuCanvas.blocksRaycasts = isToggle;
			helpMenu.transform.SetAsLastSibling ();
		} else {
			helpMenuCanvas.alpha = 0;
			helpMenuCanvas.interactable = isToggle;
			helpMenuCanvas.blocksRaycasts = isToggle;
			helpMenu.transform.SetAsFirstSibling ();
		}
	}
}