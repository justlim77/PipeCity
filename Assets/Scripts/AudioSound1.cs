using UnityEngine;
using UnityEngine.SceneManagement;
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
		if(SceneManager.GetActiveScene().buildIndex == 0)
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

		if(SceneManager.GetActiveScene().buildIndex == 2)
		{
			Destroy(this.gameObject);
		}
	}
}
