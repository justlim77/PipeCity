using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Tank : MonoBehaviour, IPointerDownHandler, IPointerExitHandler {

	public Reservoir reservoir;
	public bool isAutoRefill;

	Image modeSwitch;

	AudioSource audioSource;

	public static bool isRefilling;

	void Awake ()
	{
	}
	
	void Start () {
		reservoir = this.GetComponentInParent<Reservoir> ();
		modeSwitch = transform.GetChild (0).GetChild (0).GetComponent<Image> ();
		modeSwitch.color = Color.gray;
		audioSource = gameObject.AddComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {

		isRefilling = false;

		if (isAutoRefill) {
			reservoir.AutoRefill ();
		}
	}

	#region IPointerDownHandler implementation

	public void OnPointerDown (PointerEventData eventData)
	{
		InputManager2.CancelTooltip ();

		if (eventData.button == PointerEventData.InputButton.Left) {
			reservoir.Refill ();
		}

		if(isRefilling) {
			audioSource.PlayOneShot (InventorySound.refillTankSound);
		}

		if (eventData.button == PointerEventData.InputButton.Middle) {
			isAutoRefill = !isAutoRefill;
			CheckColor (isAutoRefill);
		}
	}

	#endregion

	#region IPointerExitHandler implementation

	public void OnPointerExit (PointerEventData eventData)
	{
		
	}

	#endregion

	void CheckColor (bool value)
	{
		if (value)
			modeSwitch.color = Color.green;
		else {
			modeSwitch.color = Color.gray;
		}
	}
}
