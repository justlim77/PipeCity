using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloudDatabase : MonoBehaviour {

    [SerializeField] string path = "Sprites/Clouds";
    [SerializeField] Sprite[] cloudSprites;

    static List<Cloud> _Clouds = new List<Cloud>();
    public static List<Cloud> Clouds { get { return _Clouds; } }

    void Awake() {
        for(int i = 0; i < cloudSprites.Length; i++) {
            Cloud newCloud = new Cloud(cloudSprites[i].name, i, 1, 1, cloudSprites[i]);
            _Clouds.Add(newCloud);
        }
    }
}
