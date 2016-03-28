using UnityEngine;
using System.Collections;

public class AudioSound3 : MonoBehaviour {

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
		if(Application.loadedLevel >= 3)
		{
			audioSource.volume += 0.02f;
		}

	}
}
