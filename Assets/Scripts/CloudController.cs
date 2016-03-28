using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloudController : MonoBehaviour {

	public Cloud cloud;
	public Sprite cloudSprite;

	public Vector2 v2_startPoint = new Vector2 (-7, 3);
	public float fl_startPoint;
	public float fl_endPoint;

	public float fl_randXposMin = -7.0f;
	public float fl_randXposMax = 4.0f;
	public float fl_randXpos;

	public float fl_randYposMin = 2.4f;
	public float fl_randYposMax = 3.5f;
	public float fl_randYpos;
	
	public float fl_scaleMin = 0.2f;
	public float fl_scaleMax = 0.3f;
	public float fl_scale;

	public float fl_speedMin = 0.05f;
	public float fl_speedMax = 0.1f;
	public float fl_speed;

	public float fl_alphaMin = 0.5f;
	public float fl_alphaMax = 0.8f;
	public float fl_alpha;

	float fl_minX;
	float fl_maxX;
	//float fl_minY;
	//float fl_maxY;

    SpriteRenderer _cloudRenderer;
	
    void Awake ()
	{
		_cloudRenderer = GetComponent<SpriteRenderer>();
	}

	void Start ()
	{
		CalculateExtents ();
		RollStats ();

		fl_randXpos = Randomize ("horizontal");
		transform.position = new Vector2 (fl_randXpos, fl_randYpos);

		fl_alpha = Randomize ("alpha");
		_cloudRenderer.color = new Color (1f, 1f, 1f, fl_alpha);
	}

	void Update ()
	{
		Move ();
	}

	void CalculateExtents ()
	{
		float fl_vertExtent = Camera.main.GetComponent<Camera>().orthographicSize;
		float fl_horzExtent = fl_vertExtent * Screen.width / Screen.height;

		fl_minX = 0 - fl_horzExtent;
		fl_maxX = fl_horzExtent;
		//fl_minY = 0 - fl_vertExtent;
		//fl_maxY = fl_vertExtent;
	}

	void Move ()
	{
		transform.Translate (Vector2.right * fl_speed * Time.deltaTime);

		if (IsEndpoint (transform.localPosition.x))
		{
			RollStats ();
		}
	}

	public bool IsEndpoint (float x)
	{
		return (x >= fl_endPoint);
	}

	float Randomize (string stat)
	{
		switch (stat)
		{
		case "speed":
			fl_speed = Random.Range (fl_speedMin, fl_speedMax);
			return fl_speed;
		case "scale":
			fl_scale = Random.Range (fl_scaleMin, fl_scaleMax);
			return fl_scale;
		case "alpha":
			fl_alpha = Random.Range (fl_alphaMin, fl_alphaMax);
			return fl_alpha;
		case "vertical":
			fl_randYpos = Random.Range (fl_randYposMin, fl_randYposMax);
			return fl_randYpos;
		case "horizontal":
			fl_randXpos = Random.Range (fl_randXposMin, fl_randXposMax);
			return fl_randXpos;
		default:
			Debug.Log ("Invalid attribute!");
			return 0;
		}
	}

	void RollStats ()
	{
		cloud = CloudDatabase.Clouds [Random.Range (0, CloudDatabase.Clouds.Count)];		    // change cloud to random cloud from cloud database
		cloudSprite = cloud.cloudIcon;														// update sprite
		_cloudRenderer.sprite = cloudSprite;													// set new sprite

		fl_speed = Randomize ("speed");														// update new random speed
		fl_scale = Randomize ("scale");														// update new random scale
		fl_randYpos = Randomize ("vertical");												// update new random vertical position

		float spriteSize = (cloudSprite.bounds.extents.x) * fl_scale;
		fl_startPoint = fl_minX - spriteSize;
		fl_endPoint = fl_maxX + spriteSize;

		transform.localScale = new Vector3 (fl_scale, fl_scale, transform.localScale.z);	// new scale
		transform.position = new Vector2 (fl_startPoint, fl_randYpos);						// restart position
	}
}