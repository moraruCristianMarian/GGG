using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementGridScript : MonoBehaviour
{
    public GameObject FramePrefab;

    public int HCells = 3;
    public int VCells = 3;

    private Vector2 _bottomLeftPos;
    private Vector2 _topRightPos;

    private GameObject _heldObject = null;
    private Vector2 _heldObjectOffset;
    private bool _boughtObject = false;

    private int _prevHeldX = -1;
    private int _prevHeldY = -1;

    private GameObject[,] _gridObjects;
    private bool[,] _gridSearchVisited;

    private GameObject _centerPiece = null;

    public void SetHeldObject(GameObject heldObject, bool boughtObject)
    {
        _heldObject = heldObject;
        _boughtObject = boughtObject;

        StartCoroutine(SimulatePhysics(0.0f));
    }
    
    void Start()
    {
        _gridObjects = new GameObject[HCells, VCells];
        _gridSearchVisited = new bool[HCells, VCells];


        SpriteRenderer sprRender = gameObject.GetComponent<SpriteRenderer>();
        sprRender.size = new Vector2(HCells, VCells);

        _bottomLeftPos = new Vector2(transform.position.x - HCells/2, transform.position.y - VCells/2);
        _topRightPos   = new Vector2(transform.position.x + HCells/2, transform.position.y + VCells/2);

        TogglePhysics(false);
        StartCoroutine(SimulatePhysics(0.1f));
    }

    private void TogglePhysics(bool physicsOn)
    {
        if (physicsOn)
            Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
        else
            Physics2D.simulationMode = SimulationMode2D.Script;
    }
    //  Physics need to be simulated briefly at the start
    //  so that colliders and raycasts work properly.
    private IEnumerator SimulatePhysics(float delay = 0.0f)
    {
        yield return new WaitForSeconds(delay);

        Rigidbody2D[] rigidbodies = FindObjectsOfType<Rigidbody2D>();
        foreach (Rigidbody2D rb in rigidbodies)
            rb.gravityScale = 0f;

        Physics2D.Simulate(0.02f);

        foreach (Rigidbody2D rb in rigidbodies)
            rb.gravityScale = 1f;
    }

    public void StartLevel()
    {
        if (CanStartLevel())
        {
            TogglePhysics(true);
            Debug.Log("LETSGOOOO");

            JoinPieces();
        }
    }
    //  BFS to join all pieces to each of their direct neighbours
    private void JoinPieces()
    {
        Dictionary<GameObject, bool> visitedPieces = new Dictionary<GameObject, bool>();

        Queue<GameObject> piecesQueue = new Queue<GameObject>();
        piecesQueue.Enqueue(_centerPiece);

        while (piecesQueue.Count > 0)
        {
            FramePieceScript framePieceScript = piecesQueue.Dequeue().GetComponent<FramePieceScript>();

            Debug.Log(string.Format("BFS at {0}", framePieceScript.gameObject.name));

            List<GameObject> myNeighbours = framePieceScript.GetNeighbours();
            foreach (GameObject neighbour in myNeighbours)
            {
                Debug.Log(string.Format("BFS neighbour {0}", neighbour.name));

                // framePieceScript.JoinToPiece(neighbour);

                bool visited;
                if (!(visitedPieces.TryGetValue(neighbour, out visited) && visited))
                {
                    visitedPieces[neighbour] = true;
                    piecesQueue.Enqueue(neighbour);

                    framePieceScript.JoinToPiece(neighbour);
                }
            }
        }
    }

    private bool InVectorRange(Vector3 mouseWorldPos, Vector2 minBound, Vector2 maxBound)
    {
        return ((mouseWorldPos.x >= minBound.x && mouseWorldPos.x <= maxBound.x) &&
                (mouseWorldPos.y >= minBound.y && mouseWorldPos.y <= maxBound.y));
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SimulatePhysics();
        }

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
                    RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(Camera.main.ScreenPointToRay(Input.mousePosition));

                    foreach (RaycastHit2D hit in hits)
                        if (hit.collider.gameObject.HasCustomTag("CanDragInPlacement"))
                        {
                            clickedObject = hit.collider.gameObject;
                            break;
                        }

                    if (clickedObject)
                    {
                        if (!_heldObject)
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
            {
                // MouseButtonUp
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
                else
                {
                    if (_heldObject)
                    {
                        if (!_boughtObject)
                        {
                            _heldObject.transform.position = _bottomLeftPos + new Vector2(_prevHeldX, _prevHeldY);
                            _gridObjects[_prevHeldX, _prevHeldY] = _heldObject;
                        }
                        else
                        {
                            Destroy(_heldObject);
                            _boughtObject = false;
                        }
                    }
                }
                _heldObject = null;
                _prevHeldX = -1;
                _prevHeldY = -1;
            }
        }

        if (_heldObject)
            DragHeldObject();
    }
    private void DragHeldObject()
    {
        Vector2 mousePos = Input.mousePosition;
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
        
        _heldObject.transform.position = (Vector2)mouseWorldPos + _heldObjectOffset;
    }


    private bool CanStartLevel()
    {   
        int centerPieceCount = 0;

        //  Find if there is any center piece
        _centerPiece = null;
        FramePieceScript[] framePieces = FindObjectsOfType<FramePieceScript>();

        foreach (FramePieceScript framePiece in framePieces)
        {
            if (framePiece.gameObject.HasCustomTag("CenterPiece"))
            {
                _centerPiece = framePiece.gameObject;
                centerPieceCount += 1;
            }
        }

        if (centerPieceCount != 1)
            return false;



        //  Initialize visited matrix for DFS
        for (int i = 0; i < HCells; i++)
            for (int j = 0; j < VCells; j++)
                _gridSearchVisited[i,j] = (_gridObjects[i,j] == null);

        //  Do the DFS once
        bool onlyOneDFS = false;
        for (int i = 0; i < HCells; i++)
        {
            for (int j = 0; j < VCells; j++)
            {
                if (_gridObjects[i,j] != null)
                {
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
                if (!_gridSearchVisited[i,j])
                    return false;

        return true;
    }
    private void PiecesConnectedDFS(int i, int j)
    {
        if ((i < 0) || (i >= HCells) || (j < 0) || (j >= VCells))
            return;
        if (_gridSearchVisited[i,j])
            return;

        _gridSearchVisited[i,j] = true;

        PiecesConnectedDFS(i + 1, j);
        PiecesConnectedDFS(i - 1, j);
        PiecesConnectedDFS(i, j + 1);
        PiecesConnectedDFS(i, j - 1);
    }
}
