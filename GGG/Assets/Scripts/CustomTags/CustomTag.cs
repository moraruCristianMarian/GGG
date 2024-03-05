using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Custom Tag", menuName = "Custom Tags/New Custom Tag")]
public class CustomTag : ScriptableObject
{
    public string Name => name;
}