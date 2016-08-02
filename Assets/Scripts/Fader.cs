using UnityEngine;
using System.Collections;

#if UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif

public class Fader : MonoBehaviour
{
	public Texture2D fadeOutTexture;
	public float fadeSpeed = 0.8f;

	private int drawDepth = -1000;
	private float alpha = 1.0f;
	private int fadeDir = -1;

	void OnGUI()
	{
		alpha += fadeDir * fadeSpeed * Time.deltaTime;

		alpha = Mathf.Clamp01(alpha);

		GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = drawDepth;
		GUI.DrawTexture (new Rect(0,0,Screen.width, Screen.height), fadeOutTexture);
	}

	public float BeginFade(int direction)
	{
		fadeDir = direction;
		return(fadeSpeed);
	}

    void Start()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        //alpha = 1;
        BeginFade(-1);
    }

#if !UNITY_5_3_OR_NEWER
    void OnLevelWasLoaded()
	{
		//alpha = 1;
		BeginFade(-1);
	}
#endif
}
