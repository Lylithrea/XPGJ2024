using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Card")]
public class Cards : ScriptableObject
{
    public string followerName;
    public Gods followingGod;

    public int damage = 0;
    public int healing = 0;
    public int shield = 0;
    public int draw = 0;

    public Sprite followerSprite;
    public string description;


}

public enum Gods
{
    Offensive,
    Defensive,
    Fertility,
    Socializing
}
