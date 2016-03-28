using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Slot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler {

	public Pipe selectedPipe;
	public static bool _isDragging;
	public GameObject priceImage;
	public Text priceText;

	Image img_pipe; 
	Inventory inventory;
	public int slotNumber;

	public static Pipe clickedPipe;

	public GameObject inputManager;
	public InputManager2 inputScript;

	public AudioSource audioSource;
	//public AudioClip buttonHoverSound;
	//public AudioClip buttonClickSound;

	void Start ()
	{
		audioSource = gameObject.AddComponent<AudioSource>();
		//buttonHoverSound = Resources.Load ("Audio/Button_MikeKoenig") as AudioClip;
		//buttonHoverSound = InventorySound.buttonHoverSound;
		//buttonClickSound = InventorySound.buttonClickSound;

		inputManager = GameObject.Find ("InputManager");
		inputScript = (InputManager2) inputManager.GetComponent(typeof(InputManager2));

		priceImage = transform.GetChild (1).gameObject;
		priceText = priceImage.transform.GetChild (0).GetComponent<Text> ();
		priceImage.SetActive (false);

		inventory = GameObject.FindGameObjectWithTag ("Inventory").GetComponent<Inventory> ();
		img_pipe = gameObject.transform.GetChild (0).GetComponent<Image> ();

		selectedPipe = inventory.Pipes [slotNumber + 1];	// +1 to compensate for [0] default empty pipe
		img_pipe.sprite = selectedPipe.pipeIcon;
	}

	#region IPointerClickHandler implementation

	public void OnPointerDown (PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			audioSource.PlayOneShot(AudioDatabase.buttonClickSound, 0.25f);
			InputManager2.CancelTooltip ();													// Reset to default state

			inputScript.ResetCursor ();

			clickedPipe = selectedPipe;

			inputScript.ResetPipeConnectorFlags ();

			InputManager2.img_tooltip.transform.position = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
			InputManager2.img_tooltip.sprite = img_pipe.sprite;								// Set tooltip sprite to selected icon sprite
			InputManager2.img_tooltip.type = Image.Type.Simple;								// Set image type to "Simple"
			InputManager2.tooltipCanvas.alpha = 1;											// Set image to visible
			_isDragging = true;																// Set _isDragging to true for InputManager script
		}
	}

	#endregion

	#region IPointerEnterHandler implementation

	public void OnPointerEnter (PointerEventData eventData)
	{
		audioSource.PlayOneShot(AudioDatabase.buttonHoverSound, 0.25f);
		ShowPrice ();
	}

	#endregion
	
	#region IPointerExitHandler implementation
	
	public void OnPointerExit (PointerEventData eventData)
	{
		HidePrice ();
	}
	
	#endregion

	void ShowPrice ()
	{
		priceText.text = "$" + selectedPipe.pipePrice.ToString ();
		priceImage.SetActive (true);
	}

	void HidePrice ()
	{
		priceImage.SetActive (false);
	}
}
