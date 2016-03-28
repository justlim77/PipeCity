using UnityEngine;
using System.Collections;

public class AudioSound2 : MonoBehaviour {

	AudioSource audioSource;

	void Awake()
	{
		audioSource = GetComponent<AudioSource>();
	}

	void Start()
	{
		audioSource.Play();
	}

	void Update()
	{
		if(Application.loadedLevel == 2)
		{
			audioSource.volume += 0.02f;
			if(audioSource.volume > 0.2f)
			{
				audioSource.volume = 0.2f;
			}
		}
		if(FadeMain.reduceVolume == true)
		{
			audioSource.volume -= 0.05f;
		}
	}
}
