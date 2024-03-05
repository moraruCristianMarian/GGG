using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementGridScript : MonoBehaviour
{
    public int HCells = 5;
    public int VCells = 3;

    private Vector2 _bottomLeftPos;
    private Vector2 _topRightPos;

    private GameObject _heldObject;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer sprRender = gameObject.GetComponent<SpriteRenderer>();
        sprRender.size = new Vector2(HCells, VCells);

        _bottomLeftPos = new Vector2(transform.position.x - HCells/2, transform.position.y - VCells/2);
        _topRightPos   = new Vector2(transform.position.x + HCells/2, transform.position.y + VCells/2);

        Debug.Log(Physics2D.simulationMode);
        ToggleGravity();
    }

    private void ToggleGravity()
    {
        if (Physics2D.simulationMode == SimulationMode2D.Script)
            Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
        else
            Physics2D.simulationMode = SimulationMode2D.Script;
    }

    private bool InVectorRange(Vector3 mouseWorldPos, Vector2 minBound, Vector2 maxBound)
    {
        return ((mouseWorldPos.x >= minBound.x && mouseWorldPos.x <= maxBound.x) &&
                (mouseWorldPos.y >= minBound.y && mouseWorldPos.y <= maxBound.y));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Input.mousePosition;
            var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.x = Mathf.Round(mouseWorldPos.x);
            mouseWorldPos.y = Mathf.Round(mouseWorldPos.y);
            mouseWorldPos.z = 0;

            if (InVectorRange(mouseWorldPos, _bottomLeftPos, _topRightPos))
            {
                Debug.Log(mouseWorldPos);

                RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
                if (hit.collider != null)
                {
                    _heldObject = hit.collider.gameObject;
                    Debug.Log("Target: " + _heldObject.name);
                }
            }
        }
    }
}
