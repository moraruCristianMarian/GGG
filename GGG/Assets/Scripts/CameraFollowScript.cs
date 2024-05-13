using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    public GameObject KingGoblin;
    public float FollowSpeed = 5.0f;

    void Update()
    {
        if (KingGoblin)
        {
            Vector2 kingPos = KingGoblin.transform.position;
            Vector2 newPos = Vector2.Lerp(transform.position, kingPos, FollowSpeed * Time.deltaTime);
            transform.position = new Vector3(newPos.x, newPos.y, -10);
        }
    }
}
