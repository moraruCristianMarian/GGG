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
    private Vector2 _heldObjectOffset;

    private int _prevHeldX = -1;
    private int _prevHeldY = -1;

    private GameObject[,] _gridObjects;

    // Start is called before the first frame update
    void Start()
    {
        _gridObjects = new GameObject[HCells, VCells];


        SpriteRenderer sprRender = gameObject.GetComponent<SpriteRenderer>();
        sprRender.size = new Vector2(HCells, VCells);

        _bottomLeftPos = new Vector2(transform.position.x - HCells/2, transform.position.y - VCells/2);
        _topRightPos   = new Vector2(transform.position.x + HCells/2, transform.position.y + VCells/2);

        Debug.Log(Physics2D.simulationMode);
        TogglePhysics(false);
    }

    private void TogglePhysics(bool physicsOn)
    {
        if (physicsOn)
            Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
        else
            Physics2D.simulationMode = SimulationMode2D.Script;
    }

    public void StartLevel()
    {
        if (true)
        {
            TogglePhysics(true);
            Debug.Log("LETSGOOOO");
        }
    }

    private bool InVectorRange(Vector3 mouseWorldPos, Vector2 minBound, Vector2 maxBound)
    {
        return ((mouseWorldPos.x >= minBound.x && mouseWorldPos.x <= maxBound.x) &&
                (mouseWorldPos.y >= minBound.y && mouseWorldPos.y <= maxBound.y));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
        {
            Vector2 mousePos = Input.mousePosition;
            var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.x = Mathf.Round(mouseWorldPos.x);
            mouseWorldPos.y = Mathf.Round(mouseWorldPos.y);
            mouseWorldPos.z = 0;

            int gridX = (int)Mathf.Round(mouseWorldPos.x - transform.position.x + HCells/2);
            int gridY = (int)Mathf.Round(mouseWorldPos.y - transform.position.y + VCells/2);

            if (Input.GetMouseButtonDown(0))
            {
                if (InVectorRange(mouseWorldPos, _bottomLeftPos, _topRightPos))
                {
                    GameObject clickedObject = null;
                    RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
                    if (hit.collider != null)
                        clickedObject = hit.collider.gameObject;

                    if (!_heldObject)
                    {
                        if (clickedObject)
                        {
                            _prevHeldX = gridX;
                            _prevHeldY = gridY;

                            _heldObject = clickedObject;
                            _heldObjectOffset = mouseWorldPos - _heldObject.transform.position;
                            Debug.Log("Target: " + _heldObject.name);
                        }
                    }
                }
            }
            else
            // MouseButtonUp
            {
                if (InVectorRange(mouseWorldPos, _bottomLeftPos, _topRightPos))
                {
                    GameObject previousObject = _gridObjects[gridX, gridY];

                    if (_heldObject)
                    {
                        if (previousObject)
                        {
                            Debug.Log("Swap!!!");
                            previousObject.transform.position = _bottomLeftPos + new Vector2(_prevHeldX, _prevHeldY);
                            _gridObjects[_prevHeldX, _prevHeldY] = previousObject;

                            _heldObject.transform.position = _bottomLeftPos + new Vector2(gridX, gridY);
                            _gridObjects[gridX, gridY] = _heldObject;
                        }
                        else
                        {
                            Debug.Log("Place (Y)");
                            _gridObjects[gridX, gridY] = _heldObject;
                            _heldObject.transform.position = _bottomLeftPos + new Vector2(gridX, gridY);

                            if ((_prevHeldX != -1) && (_prevHeldY != -1))
                                _gridObjects[_prevHeldX, _prevHeldY] = null;
                        }
                    }
                }
                _heldObject = null;
                _prevHeldX = -1;
                _prevHeldY = -1;
            }
        }
    }
}
