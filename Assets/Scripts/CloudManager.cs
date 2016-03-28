using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloudManager : MonoBehaviour {

	public GameObject cloudPrefab;
	public List<GameObject> cloudGroup = new List<GameObject> ();
	public int cloudAmount;

	[SerializeField] private CloudDatabase cloudDatabase;

	private Vector2 startPos = new Vector2 (0, 0);

	void Start ()
	{
		SpawnClouds ();
	}

	void SpawnClouds ()
	{
		cloudGroup.Clear();

		for (int i = 0; i < cloudAmount; i++)
		{
			GameObject cloud = (GameObject) Instantiate (cloudPrefab);
			cloud.name = "Cloud " + i;
			cloud.transform.SetParent (this.transform);
			cloud.GetComponent<Renderer>().sortingLayerName = "Background";
			cloud.transform.localPosition = startPos;
			cloudGroup.Add (cloud);										// Add new instantiated gameobject to list of gameobjects
		}
	}	
}
