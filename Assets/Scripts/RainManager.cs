using UnityEngine;
using System.Collections;

public class RainManager : MonoBehaviour {

	public ParticleSystem rain;
	public bool isRaining;

	public int rainInterval = 5;
	public float rainCounter = 0;
	public float rainDuration = 0;

	public AudioSource audioSource;
	
	void Start ()
	{

	}

	void Update ()
	{
		RandomRain ();
	}

	void RandomRain ()
	{
		if (rain.isPlaying == false) {
			isRaining = false;
			rainCounter += 1 * Time.deltaTime;
		}

		if (rainCounter > rainInterval) {
			rainDuration += 1 * Time.deltaTime;
			rain.Play ();
			rain.loop = false;
			isRaining = true;
			if(!audioSource.isPlaying)
				PlayRainSound();
		}

		if (rainDuration > rainInterval) {
			rain.Stop ();
			rainCounter = 0;
			rainDuration = 0;
			StopPlayRainSound();
		}

        audioSource.mute = InputManager2._isPaused;
    }

	void PlayRainSound()
	{
		audioSource.Play ();
	}

	void StopPlayRainSound()
	{
		audioSource.Stop();
	}
}
