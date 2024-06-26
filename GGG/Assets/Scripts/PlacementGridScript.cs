using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlacementGridScript : MonoBehaviour
{
    public UnityEvent OnLevelStartEvent;
    public GameObject AbilitiesBar;
    public GameObject StartButton;
    public GameObject Shop;
    public ShopMenuScript TheShopMenuScript;

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

        
        //  Adjust position if grid has an even number of cellson an axis, to align with the tilemap
        if (HCells % 2 == 0)
        {
            transform.position -= new Vector3(0.5f, 0f, 0f); 
            _topRightPos -= new Vector2(1.0f, 0f);
        }
        if (VCells % 2 == 0)
        {
            transform.position -= new Vector3(0f, 0.5f, 0f);
            _topRightPos -= new Vector2(0f, 1.0f);
        }
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
        {
            PopupTextSingletonScript.Get().Show("King Goblin must be placed.");
            return false;
        }



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
                {
                    PopupTextSingletonScript.Get().Show("All goblins must be connected.");
                    return false;
                }

        return true;
    }

    public void StartLevel()
    {
        if (CanStartLevel())
        {
            EnablePhysics();
            ParentPieces(_centerPiece);
            JoinPieces();

            ChangeUIToLevelStart();
            CreateAbilityIcons();

            DoPlacementPhaseAbilities();

            StartCameraFollow();

            InitOffenseMode();

            AllowGameOver();

            StartTimer();

            DoLevelStartEvents();

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
    
    static void DeparentPieces()
    {
        FramePieceScript[] framePieceScripts = GameObject.FindObjectsOfType<FramePieceScript>();
        foreach (FramePieceScript fps in framePieceScripts)
            fps.transform.SetParent(null);
    }
    //  BFS to link pieces in a parent hierarchy starting from the center piece
    static public void ParentPieces(GameObject centerPiece)
    {
        DeparentPieces();
        Dictionary<GameObject, bool> visitedPieces = new Dictionary<GameObject, bool>();

        Queue<GameObject> piecesQueue = new Queue<GameObject>();
        piecesQueue.Enqueue(centerPiece);
        visitedPieces[centerPiece] = true;

        while (piecesQueue.Count > 0)
        {
            FramePieceScript framePieceScript = piecesQueue.Dequeue().GetComponent<FramePieceScript>();

            // Debug.Log(string.Format("BFS at {0}", framePieceScript.gameObject.name));
            visitedPieces[framePieceScript.gameObject] = true;

            List<GameObject> myNeighbours = framePieceScript.GetNeighbours();
            foreach (GameObject neighbour in myNeighbours)
            {
                // Debug.Log(string.Format("{0} parented {1}", framePieceScript.gameObject.name, neighbour.name));

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
    
    //  - Spawn all wheels as indicated by wheel highlights of the goblin mechanic frame pieces
    //  - Set the Goblin Dashers' dash direction
    private void DoPlacementPhaseAbilities()
    {
        WheelHighlightScript[] wheelHighlightScripts = GameObject.FindObjectsOfType<WheelHighlightScript>();
        foreach (WheelHighlightScript whs in wheelHighlightScripts)
            whs.AttachWheelsToFramePiece();

        DasherHighlightScript[] dasherHighlightScripts = GameObject.FindObjectsOfType<DasherHighlightScript>();
        foreach (DasherHighlightScript dhs in dasherHighlightScripts)
            dhs.SetRocketRotation();
    }

    //  The camera will follow the king goblin during gameplay
    private void StartCameraFollow()
    {
        CameraFollowScript cfs = FindObjectOfType<CameraFollowScript>();
        if (cfs)
            cfs.KingGoblin = FramePieceScript.FindKingGoblin();
    }

    //  Lancers with the specific settings should march forward
    private void InitOffenseMode()
    {
        LancerScript[] lancerScripts = GameObject.FindObjectsOfType<LancerScript>();
        foreach (LancerScript ls in lancerScripts)
            if (ls.MarchOnStart)
                ls.Marching = true;
    }

    //  From this point, game over is possible
    private void AllowGameOver()
    {
        GameOverScript gos = FindObjectOfType<GameOverScript>();
        if (gos)
            gos.CanGameOverNow = true;
    }

    //  Start the timer 
    private void StartTimer()
    {
        TimerSingletonScript.Get().TimerActive = true;
    }

    //  Level-specific events that should happen as soon as the level successfully starts
    private void DoLevelStartEvents()
    {
        if (OnLevelStartEvent != null)
            OnLevelStartEvent.Invoke();
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
            // int gridX = (int)Mathf.Round(mouseWorldPos.x - transform.position.x + HCells/2);
            // int gridY = (int)Mathf.Round(mouseWorldPos.y - transform.position.y + VCells/2);
            int gridX = (int)Mathf.Round(mouseWorldPos.x - _topRightPos.x + HCells - 1);
            int gridY = (int)Mathf.Round(mouseWorldPos.y - _topRightPos.y + VCells - 1);

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
                TheShopMenuScript.CurrentlyHoldingBoughtPiece = false;

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
                                FramePieceScript heldFps = _heldObject.GetComponent<FramePieceScript>();
                                Shop.GetComponentInChildren<ShopMenuScript>().SellPiece(heldFps.ShopIndex);

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

                    _boughtObject = false;
                }
                else
                {
                    //  Mouse released outside the placement grid
                    if (_heldObject)
                    {
                        if ((_prevHeldX != -1) && (_prevHeldY != -1))
                            _gridObjects[_prevHeldX, _prevHeldY] = null;

                        FramePieceScript heldFps = _heldObject.GetComponent<FramePieceScript>();
                        Shop.GetComponentInChildren<ShopMenuScript>().SellPiece(heldFps.ShopIndex);
                            
                        Destroy(_heldObject);
                        _boughtObject = false;
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
