using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramePieceScript : MonoBehaviour
{
    public GameObject MyGoblin;
    public List<Vector3> NeighbourDirections;
    public List<GameObject> Neighbours;
    public int ShopIndex = -1;

    public List<GameObject> GetNeighbours()
    {
        Neighbours = new List<GameObject>();

        foreach (Vector3 dir in NeighbourDirections)
        {
            Collider2D[] hitColliders = Physics2D.OverlapPointAll(transform.position + transform.TransformVector(dir));
            if (hitColliders.Length > 0)
            {
                foreach (Collider2D hitCollider in hitColliders)
                    if (hitCollider.gameObject.HasCustomTag("FramePiece"))
                    {
                        // Debug.Log(string.Format("   {0} found a neighbour {1} at {2}", gameObject.name, hitCollider.gameObject.name, dir));  
                        Neighbours.Add(hitCollider.gameObject);
                        break;
                    }
            }
        }
        // Debug.Log(string.Format("  {0} neighbours", Neighbours.Count));
        return Neighbours;
    }
    public void JoinToPiece(GameObject otherPiece)
    {
        Rigidbody2D otherRigidbody = otherPiece.GetComponent<Rigidbody2D>();
        Rigidbody2D myRigidbody = gameObject.GetComponent<Rigidbody2D>();
        if (myRigidbody)
        {
            FixedJoint2D joint = otherPiece.AddComponent<FixedJoint2D>();
            joint.connectedBody = myRigidbody;
        }
    }
    public void ParentPiece(GameObject otherPiece)
    {
        otherPiece.transform.SetParent(transform);
    }



    public static GameObject FindKingGoblin()
    {
        GameObject[] goblins = GameObject.FindGameObjectsWithTag("Goblin");
        foreach (GameObject goblin in goblins)
            if (goblin.HasCustomTag("KingGoblin"))
                return goblin;
        return null;
    }
    public static GameObject FindKingGoblinsPiece()
    {
        GameObject kingGoblin = FindKingGoblin();
        if (kingGoblin)
            return kingGoblin.transform.parent.gameObject;
        return null;
    }
    public bool FindWayToKingsPieceOrElse(GameObject destroyedExcluded, GameObject kingGoblinsPiece)
    {
        //  If the piece that king goblin inhabits wasn't found in previous calls, find it now
        if (!kingGoblinsPiece)
            kingGoblinsPiece = FindKingGoblinsPiece();
        //  If it couldn't be found now, then it doesn't exist => autofail
        if (!kingGoblinsPiece)
            return false;

        if (gameObject == kingGoblinsPiece)
            return true;

        Dictionary<GameObject, bool> visitedPieces = new Dictionary<GameObject, bool>();

        Queue<GameObject> piecesQueue = new Queue<GameObject>();
        piecesQueue.Enqueue(gameObject);
        visitedPieces[gameObject] = true;

        while (piecesQueue.Count > 0)
        {
            FramePieceScript framePieceScript = piecesQueue.Dequeue().GetComponent<FramePieceScript>();

            if (framePieceScript.gameObject == kingGoblinsPiece)
                return true;

            List<GameObject> myNeighbours = framePieceScript.GetNeighbours();
            myNeighbours.Remove(destroyedExcluded);
            foreach (GameObject neighbour in myNeighbours)
            {
                bool visited;
                if (!(visitedPieces.TryGetValue(neighbour, out visited) && visited))
                {
                    visitedPieces[neighbour] = true;
                    piecesQueue.Enqueue(neighbour);
                    // Debug.Log(string.Format("{0} visited {1}", framePieceScript.gameObject.name, neighbour.name));
                }
            }
        }

        return false;
    }
    public void DestroyPiece(bool reparentChildren = true)
    {
        if (reparentChildren)
        {
            GetNeighbours();
            foreach (GameObject neighbour in Neighbours)
                if (!neighbour.GetComponent<FramePieceScript>().FindWayToKingsPieceOrElse(gameObject, null))
                {
                    // Debug.Log("Ded: " + neighbour.name);
                    Destroy(neighbour);
                }
                else
                {
                    // Debug.Log("Saved: " + neighbour.name);
                    if (neighbour.transform.parent == gameObject.transform)
                    {
                        // Debug.Log("TO-DO: Reparent " + neighbour.name);
                        neighbour.transform.SetParent(gameObject.transform.parent);
                    }
                }
        }

        Destroy(gameObject);
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space") && gameObject.HasCustomTag("CenterPiece"))
        {
            FrameCommandScript sendCommandScript = gameObject.GetComponent<FrameCommandScript>();
            if (sendCommandScript)
                sendCommandScript.SendCommand();
        }
    }

    void OnDrawGizmos()
    {
        foreach (Vector3 dir in NeighbourDirections)
            Gizmos.DrawLine(transform.position, transform.position+dir);
    }
}
