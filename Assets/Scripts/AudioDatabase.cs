using UnityEngine;
using System.Collections;

public class AudioDatabase : MonoBehaviour {

	public AudioSource efxSource;

	public static AudioDatabase instance = null;

	public AudioClip buttonHoverSoundAttach;
	public AudioClip buttonClickSoundAttach;
	public AudioClip fundSound1Attach;
	public AudioClip fundSound2Attach;
	public AudioClip rotatePipeSoundAttach;
	public AudioClip refillTankSoundAttach;
	public AudioClip toggleWaterSoundAttach;
	public AudioClip splashSoundAttach;

	public static AudioClip buttonHoverSound;
	public static AudioClip buttonClickSound;
	public static AudioClip fundsound1;
	public static AudioClip fundsound2;
	public static AudioClip rotatePipeSound;
	public static AudioClip refillTankSound;
	public static AudioClip toggleWaterSound;
	public static AudioClip splashSound;

	void Awake () 
	{
		if(instance == null)
		{
			instance = this;
		}
		else if(instance != this)
		{
			Destroy (gameObject);
		}
	}
	
	void Start () 
	{
		buttonHoverSound = buttonHoverSoundAttach;
		buttonClickSound = buttonClickSoundAttach;
		fundsound1 = fundSound1Attach;
		fundsound2 = fundSound2Attach;
		rotatePipeSound = rotatePipeSoundAttach;
		refillTankSound = refillTankSoundAttach;
		toggleWaterSound = toggleWaterSoundAttach;
		splashSound = splashSoundAttach;
	}

	public void RandomizeSfx(params AudioClip [] clips)
	{
		int randomIndex = Random.Range (0, clips.Length);

		efxSource.clip = clips [randomIndex];
		efxSource.Play ();
	}
}
