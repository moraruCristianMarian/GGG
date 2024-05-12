using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementGridScript : MonoBehaviour
{
    public GameObject AbilitiesBar;
    public GameObject StartButton;
    public GameObject Shop;

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
    }
    
    void Start()
    {
        _gridObjects = new GameObject[HCells, VCells];
        _gridSearchVisited = new bool[HCells, VCells];


        SpriteRenderer sprRender = gameObject.GetComponent<SpriteRenderer>();
        sprRender.size = new Vector2(HCells, VCells);

        _bottomLeftPos = new Vector2(transform.position.x - HCells/2, transform.position.y - VCells/2);
        _topRightPos   = new Vector2(transform.position.x + HCells/2, transform.position.y + VCells/2);
    }

    //  Conditions to start level:
    //  - there is (only) 1 Center/King piece placed
    //  - every piece placed is connected to every other piece
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

    public void StartLevel()
    {
        if (CanStartLevel())
        {
            EnablePhysics();
            ParentPieces();
            JoinPieces();

            ChangeUIToLevelStart();
            CreateAbilityIcons();

            SpawnMechanicWheels();

            InitOffenseMode();

            //  Destroy the placement grid
            Destroy(gameObject);
        }
    }
    
    //  When the level starts, the placed pieces' rigidbodies must go from static to dynamic.
    private void EnablePhysics()
    {
        FramePieceScript[] framePieceScripts = FindObjectsOfType<FramePieceScript>();
        foreach (FramePieceScript fps in framePieceScripts)
            fps.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }
    
    //  Hide shop and start button, display the ability bar
    private void ChangeUIToLevelStart()
    {
        AbilitiesBar.SetActive(true);
        StartButton.SetActive(false);
        Shop.SetActive(false);
    } 
    
    //  BFS to link pieces in a parent hierarchy starting from the center piece
    private void ParentPieces()
    {
        Dictionary<GameObject, bool> visitedPieces = new Dictionary<GameObject, bool>();

        Queue<GameObject> piecesQueue = new Queue<GameObject>();
        piecesQueue.Enqueue(_centerPiece);
        visitedPieces[_centerPiece] = true;

        while (piecesQueue.Count > 0)
        {
            FramePieceScript framePieceScript = piecesQueue.Dequeue().GetComponent<FramePieceScript>();

            // Debug.Log(string.Format("BFS at {0}", framePieceScript.gameObject.name));
            visitedPieces[framePieceScript.gameObject] = true;

            List<GameObject> myNeighbours = framePieceScript.GetNeighbours();
            foreach (GameObject neighbour in myNeighbours)
            {
                // Debug.Log(string.Format("BFS neighbour {0}", neighbour.name));

                framePieceScript.ParentPiece(neighbour);
                
                bool visited;
                if (!(visitedPieces.TryGetValue(neighbour, out visited) && visited))
                {
                    piecesQueue.Enqueue(neighbour);
                }
            }
        }
    }
    //  Every piece creates a joint to every other piece (even non-neighbouring ones)
    //  Reason: it makes the frame structure as a whole entirely rigid, instead of wobbling around like jelly
    private void JoinPieces()
    {
        FramePieceScript[] framePieceScripts = GameObject.FindObjectsOfType<FramePieceScript>();
        for (int i = 0; i < framePieceScripts.Length; i++)
            for (int j = i+1; j < framePieceScripts.Length; j++)
                framePieceScripts[i].JoinToPiece(framePieceScripts[j].gameObject);
    }

    //  Create all ability icons of placed pieces
    private void CreateAbilityIcons()
    {
        CreateAbilityButtonScript[] createAbilityButtons = GameObject.FindObjectsOfType<CreateAbilityButtonScript>();
        foreach (CreateAbilityButtonScript cab in createAbilityButtons)
            cab.CreateMyAbilityButton();
    }
    
    //  Spawn all wheels as indicated by wheel highlights of the goblin mechanic frame pieces
    private void SpawnMechanicWheels()
    {
        WheelHighlightScript[] wheelHighlightScripts = GameObject.FindObjectsOfType<WheelHighlightScript>();
        foreach (WheelHighlightScript whs in wheelHighlightScripts)
            whs.AttachWheelsToFramePiece();
    }

    //  Lancers with the specific settings should march forward
    private void InitOffenseMode()
    {
        LancerScript[] lancerScripts = GameObject.FindObjectsOfType<LancerScript>();
        foreach (LancerScript ls in lancerScripts)
            if (ls.MarchOnStart)
                ls.Marching = true;
    }

    private bool InVectorRange(Vector3 mouseWorldPos, Vector2 minBound, Vector2 maxBound)
    {
        return ((mouseWorldPos.x >= minBound.x && mouseWorldPos.x <= maxBound.x) &&
                (mouseWorldPos.y >= minBound.y && mouseWorldPos.y <= maxBound.y));
    }

    
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
                // MouseButtonUp (release the mouse) over the placement grid
                if (InVectorRange(mouseWorldPos, _bottomLeftPos, _topRightPos))
                {
                    GameObject previousObject = _gridObjects[gridX, gridY];

                    //  Release the mouse and a piece is held
                    if (_heldObject)
                    {
                        //  Release the mouse, a piece is held,
                        //  ... and there is another piece previously placed where the mouse is
                        if (previousObject)
                        {
                            //  ... and the piece held waas just dragged from the shop
                            if (_boughtObject)
                            {
                                _boughtObject = false;
                                Destroy(_heldObject);
                                return;
                            }

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

                    //  Testing
                    _boughtObject = false;
                }
                else
                {
                    //  Mouse released outside the placement grid
                    if (_heldObject)
                    {
                        // //  Pieces which were previously placed on the grid return to their original position
                        // if (!_boughtObject)
                        // {
                        //     _heldObject.transform.position = _bottomLeftPos + new Vector2(_prevHeldX, _prevHeldY);
                        //     _gridObjects[_prevHeldX, _prevHeldY] = _heldObject;
                        // }
                        // //  Pieces dragged out of the shop are instead deleted
                        // else
                        // {
                            if ((_prevHeldX != -1) && (_prevHeldY != -1))
                                _gridObjects[_prevHeldX, _prevHeldY] = null;
                                
                            Destroy(_heldObject);
                            _boughtObject = false;
                        // }
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
