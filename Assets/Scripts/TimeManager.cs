using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeManager : MonoBehaviour {

	public GameObject year;
	public Text yearText;

	public int initialYear;
	public int currentYear;
	public int sustenanceYear;
	public float counter;
	public int interval = 12;

	public GameObject buywaterButton;

	void Awake ()
	{
		year = GameObject.Find ("Year");
		yearText = year.transform.GetChild (0).transform.GetChild (0).GetComponent<Text> ();
		buywaterButton = GameObject.Find ("BuyWaterButton");
	}

	void Start ()
	{
		counter = 1;
		initialYear = 2000;
		sustenanceYear = 2060;

		currentYear = initialYear;
	}

	void Update ()
	{
		if (!InputManager2._isPaused) {
			RunClock (interval);
		}
	}

	void RunClock (int secondstoYear)
	{
		counter += 1 * Time.deltaTime;

		if (counter >= 12) {
			currentYear ++;
			counter = 1;
		}

		// Disable buy water button when 2061 is reached
		if (currentYear > sustenanceYear) {
			buywaterButton.GetComponent<Button> ().interactable = false;
			buywaterButton.GetComponent<Image> ().color = new Color (0.5F, 0.5F, 0.5F, 1F);
		}

		yearText.text = "" + counter.ToString ("F0") + "    " + currentYear.ToString ();
	}
}
