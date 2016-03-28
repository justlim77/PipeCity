using UnityEngine;
using System.Collections;

public class AudioSound1 : MonoBehaviour {

	public AudioSource audioSource;

	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}

	void Start()
	{
		PlayButton.reduceVolume = false;
		audioSource.Play();
	}

	void Update()
	{
		if(Application.loadedLevel == 0)
		{
			audioSource.volume += 0.02f;
			if(audioSource.volume > 0.2f)
			{
				audioSource.volume = 0.2f;
			}
		}

		if(PlayButton.reduceVolume == true)
		{
			audioSource.volume -= 0.02f;
		}

		if(Application.loadedLevel == 2)
		{
			Destroy(this.gameObject);
		}
	}
}
