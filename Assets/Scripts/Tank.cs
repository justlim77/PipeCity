using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Tank : MonoBehaviour, IPointerDownHandler {

	public static bool isRefilling;

	public Reservoir reservoir;
	public bool isAutoRefill;

	Image _image;
	
	void Start ()
    {
		reservoir = GetComponentInParent<Reservoir> ();
		_image = transform.GetChild(0).GetChild(0).GetComponent<Image>();
		_image.color = Color.gray;
	}
	
	void Update ()
    {
		isRefilling = false;

		if (isAutoRefill)
			reservoir.AutoRefill ();
	}

	#region IPointerDownHandler implementation

	public void OnPointerDown(PointerEventData eventData)
	{
		InputManager2.CancelTooltip();

		if (eventData.button == PointerEventData.InputButton.Left)
			reservoir.Refill();

		if(isRefilling)
			AudioManager.Instance.PlaySFX(AudioDatabase.refillTankSound);

		if (eventData.button == PointerEventData.InputButton.Middle) {
			isAutoRefill = !isAutoRefill;
			CheckColor (isAutoRefill);
		}
	}

	#endregion

	void CheckColor(bool value)
	{
		if (value)
			_image.color = Color.green;
		else
			_image.color = Color.gray;
	}
}
