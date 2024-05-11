using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackgroundScript : MonoBehaviour
{
    public GameObject CameraObject;
    public float ParaDepth;
    public float AutoScroll = 0.0f;
    private float _paraLength;
    private float _paraPos;

    // Start is called before the first frame update
    void Start()
    {
        _paraLength = GetComponent<SpriteRenderer>().bounds.size.x;
        _paraPos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        //  Some parallax layers move even if the camera is still (exp. clouds)
        _paraPos += AutoScroll * Time.deltaTime;

        //  Move parallaxes along with camera according to their "depth"
        float dist = CameraObject.transform.position.x * ParaDepth;
        transform.position = new Vector2(_paraPos + dist, transform.position.y);

        //  Update parallaxes position if the camera moves too far
        float wrap = CameraObject.transform.position.x * (1 - ParaDepth);
        if (wrap > _paraPos + _paraLength)
            _paraPos += _paraLength;
        else
        if (wrap < _paraPos - _paraLength)
            _paraPos -= _paraLength;
    }
}
