using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CanvasManager : MonoBehaviour {

	CanvasScaler _scaler;

    void Awake()
    { 
		_scaler = GetComponent<CanvasScaler>();

        _scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
    }
}