using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingDatabase : MonoBehaviour {

    [SerializeField] string path = "Sprites/Buildings/";
    [SerializeField] Building[] buildings;

    static List<Building> _Buildings = new List<Building>();
    public static List<Building> Buildings { get { return _Buildings; } }

    void Awake()
    {
        foreach (Building building in buildings)
            _Buildings.Add(building);
    }
}
