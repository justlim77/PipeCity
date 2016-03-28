using UnityEngine;
using UnityEngine.EventSystems;

public class PlaySFX : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler {

    public AudioClip[] clickAudio;
    public AudioClip[] hoverAudio;

    void Start() { }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickAudio.Length == 0)
            return;
        float pitch = UnityEngine.Random.Range(0.95f, 1.05f);
        foreach (var clip in clickAudio)
            AudioManager.Instance.PlaySFX(clip, pitch, 0.5f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverAudio.Length == 0)
            return;
        float pitch = UnityEngine.Random.Range(0.95f, 1.05f);
        foreach (var clip in hoverAudio)
            AudioManager.Instance.PlaySFX(clip, pitch, 0.5f);
    }
}
