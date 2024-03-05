using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCustomTags : MonoBehaviour
{
    [SerializeField]
    private List<CustomTag> _customTags;
    public List<CustomTag> AllCustomTags => _customTags;

    public bool HasCustomTag(CustomTag customTag)
    {
        return _customTags.Contains(customTag);
    }
    public bool HasCustomTag(string customTagName)
    {
        return _customTags.Exists(ct => ct.Name == customTagName);
    }
}