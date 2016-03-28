using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeManager : MonoBehaviour {

	public GameObject year;

	public int initialYear;
	public float counter;
	public int interval = 12;

	public GameObject buywaterButton;

	static int CurrentYear;
	static int SustenanceYear;
    Text _yearText;

	void Awake ()
	{
        _yearText = year.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
	}

	void Start ()
	{
		counter = 1;
		initialYear = 2000;
		SustenanceYear = 2060;

		CurrentYear = initialYear;
	}

	void Update ()
	{
		if (!InputManager2._isPaused)
			RunClock(interval);
	}

	void RunClock (int secondstoYear)
	{
		counter += 1 * Time.deltaTime;

		if (counter >= 12) {
			CurrentYear ++;
			counter = 1;
		}

		// Disable buy water button when 2061 is reached
		if (EndYear) {
			buywaterButton.GetComponent<Button>().interactable = false;
			buywaterButton.GetComponent<Image>().color = new Color (0.5F, 0.5F, 0.5F, 1F);
		}

		_yearText.text = "" + counter.ToString ("F0") + "    " + CurrentYear.ToString ();
	}

    public static bool EndYear
    {
        get { return CurrentYear >= SustenanceYear; }
    }
}
