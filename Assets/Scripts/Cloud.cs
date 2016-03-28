using UnityEngine;
using System.Collections;

[System.Serializable]
public class Cloud {

	public string cloudName;
	public int cloudID;
	public float cloudSpeed;
	public float cloudScale;
	public Sprite cloudIcon;

	public Cloud (string name, int ID, float speed, float scale, Sprite sprite)
	{
		cloudName = name;
		cloudID = ID;
		cloudSpeed = speed;
		cloudScale = scale;
        cloudIcon = sprite;
	}
}
