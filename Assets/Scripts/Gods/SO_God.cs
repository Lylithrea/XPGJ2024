using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gods/God")]
public class SO_God : ScriptableObject
{
    public string godName;
    public Gods god;
    public Color godColor;
    public string godDescription;
    public int godLevel;
    public Sprite godSprite;

    public List<int> godLevelStats = new List<int>();
    public int maxFavour = 100;
    public int currentFavour = 50;
}
