using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayBGM : MonoBehaviour {

    public AudioClip bgm;

	void Start()
	{
        AudioManager.Instance.PlayBGM(bgm);
	}

	void Update()
	{
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            AudioManager.Instance.BGMVolume += 0.02f;
            if (AudioManager.Instance.BGMVolume > 0.8f)
                AudioManager.Instance.BGMVolume = 0.8f;
        }

        if (FadeMain.reduceVolume == true)
            AudioManager.Instance.BGMVolume -= 0.05f;
	}
}
