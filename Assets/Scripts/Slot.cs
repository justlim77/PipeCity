using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Slot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler {

	public static Pipe clickedPipe;

    public Pipe selectedPipe;
	public static bool _isDragging;
	public GameObject priceImage;
	public Text priceText;
	public int slotNumber;
	public GameObject inputManager;
	public InputManager2 inputScript;

	Image _pipeImage; 
	Inventory _inventory;

	void Start ()
	{
		inputManager = GameObject.Find ("InputManager");
		inputScript = (InputManager2) inputManager.GetComponent(typeof(InputManager2));

		priceImage = transform.GetChild (1).gameObject;
		priceText = priceImage.transform.GetChild (0).GetComponent<Text> ();
		priceImage.SetActive (false);

		_inventory = GameObject.FindGameObjectWithTag ("Inventory").GetComponent<Inventory> ();
		_pipeImage = gameObject.transform.GetChild (0).GetComponent<Image> ();

		selectedPipe = _inventory.Pipes [slotNumber + 1];	// +1 to compensate for [0] default empty pipe
		_pipeImage.sprite = selectedPipe.pipeIcon;
	}

	#region IPointerClickHandler implementation

	public void OnPointerDown (PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			InputManager2.CancelTooltip ();													// Reset to default state

			inputScript.ResetCursor ();

			clickedPipe = selectedPipe;

			inputScript.ResetPipeConnectorFlags ();

			InputManager2.img_tooltip.transform.position = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
			InputManager2.img_tooltip.sprite = _pipeImage.sprite;								// Set tooltip sprite to selected icon sprite
			InputManager2.img_tooltip.type = Image.Type.Simple;								// Set image type to "Simple"
			InputManager2.tooltipCanvas.alpha = 1;											// Set image to visible
			_isDragging = true;																// Set _isDragging to true for InputManager script
		}
	}

	#endregion

	#region IPointerEnterHandler implementation

	public void OnPointerEnter (PointerEventData eventData)
	{
		TogglePrice(true);
	}

	#endregion
	
	#region IPointerExitHandler implementation
	
	public void OnPointerExit (PointerEventData eventData)
	{
		TogglePrice(false);
	}
	
	#endregion

	void TogglePrice(bool value)
	{
        if (value)
        {
		    priceText.text = "$" + selectedPipe.pipePrice.ToString ();
		    priceImage.SetActive (true);
        }
        else
            priceImage.SetActive(false);
    }
}
