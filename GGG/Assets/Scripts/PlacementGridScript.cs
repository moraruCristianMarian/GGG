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
    private bool[,] _gridDFS;

    // Start is called before the first frame update
    void Start()
    {
        _gridObjects = new GameObject[HCells, VCells];
        _gridDFS = new bool[HCells, VCells];


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
        if (CanStartLevel())
        {
            TogglePhysics(true);
            Debug.Log("LETSGOOOO");

            GameObject centerPiece = null;
            FramePieceScript[] framePieces = FindObjectsOfType<FramePieceScript>();
            //  Find center piece
            foreach (FramePieceScript framePiece in framePieces)
            {
                if (framePiece.gameObject.HasCustomTag("CenterPiece"))
                {
                    centerPiece = framePiece.gameObject;
                    break;
                }
            }

            if (centerPiece)
                //  Join all pieces to center piece 
                foreach (FramePieceScript framePiece in framePieces)
                    framePiece.JoinToCenterPiece(centerPiece);
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
        //  Down = pressed (start dragging piece), up = released (place down piece to move/swap)
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
        {
            //  Get mouse position in world
            Vector2 mousePos = Input.mousePosition;
            var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.x = Mathf.Round(mouseWorldPos.x);
            mouseWorldPos.y = Mathf.Round(mouseWorldPos.y);
            mouseWorldPos.z = 0;

            //  Get indices for the grid positions
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
                        if ((clickedObject) && (clickedObject.HasCustomTag("CanDragInPlacement")))
                        {
                            _prevHeldX = gridX;
                            _prevHeldY = gridY;

                            _heldObject = clickedObject;
                            _heldObjectOffset = mouseWorldPos - _heldObject.transform.position;
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
                            previousObject.transform.position = _bottomLeftPos + new Vector2(_prevHeldX, _prevHeldY);
                            _gridObjects[_prevHeldX, _prevHeldY] = previousObject;

                            _heldObject.transform.position = _bottomLeftPos + new Vector2(gridX, gridY);
                            _gridObjects[gridX, gridY] = _heldObject;
                        }
                        else
                        {
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


    private bool CanStartLevel()
    {        
        //  Initialize visited matrix for DFS
        for (int i = 0; i < HCells; i++)
            for (int j = 0; j < VCells; j++)
                _gridDFS[i,j] = (_gridObjects[i,j] == null);

        //  Do the DFS once
        bool onlyOneDFS = false;
        for (int i = 0; i < HCells; i++)
        {
            for (int j = 0; j < VCells; j++)
            {
                if (_gridObjects[i,j] != null)
                {
                    // Debug.Log(string.Format("DFS start on: {0}", _gridObjects[i,j].name));
                    PiecesConnectedDFS(i, j);

                    onlyOneDFS = true;
                    break;
                }
            }
            if (onlyOneDFS)
                break;
        }

        //  Check if any pieces weren't visited by DFS
        for (int i = 0; i < HCells; i++)
            for (int j = 0; j < VCells; j++)
                if (!_gridDFS[i,j])
                {
                    // Debug.Log(string.Format("Fail {0},{1}", i, j));
                    return false;
                }

        return true;
    }
    private void PiecesConnectedDFS(int i, int j)
    {
        // Debug.Log(string.Format("   DFSing: {0},{1}", i, j));
        
        if ((i < 0) || (i >= HCells) || (j < 0) || (j >= VCells))
            return;
        if (_gridDFS[i,j])
            return;

        _gridDFS[i,j] = true;

        PiecesConnectedDFS(i + 1, j);
        PiecesConnectedDFS(i - 1, j);
        PiecesConnectedDFS(i, j + 1);
        PiecesConnectedDFS(i, j - 1);
    }
}
