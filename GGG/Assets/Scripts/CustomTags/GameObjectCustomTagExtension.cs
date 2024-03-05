using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectCustomTagExtension
{
    public static bool HasCustomTag(this GameObject gameObject, CustomTag ct)
    {
        if (gameObject.TryGetComponent<MyCustomTags>(out var customTags))
            return customTags.HasCustomTag(ct);

        return false;
    }
    public static bool HasCustomTag(this GameObject gameObject, string ctName)
    {
        if (gameObject.TryGetComponent<MyCustomTags>(out var customTags))
            return customTags.HasCustomTag(ctName);

        return false;
    }
}