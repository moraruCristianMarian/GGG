using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramePieceScript : MonoBehaviour
{
    public List<Vector3> NeighbourDirections;
    public List<GameObject> Neighbours;

    public List<GameObject> GetNeighbours()
    {
        Neighbours = new List<GameObject>();

        foreach (Vector3 dir in NeighbourDirections)
        {
            Collider2D hitCollider = Physics2D.OverlapPoint(transform.position + transform.TransformVector(dir));
            if (hitCollider != null)
            {
                if (hitCollider.gameObject.HasCustomTag("FramePiece"))
                {
                    // Debug.Log(string.Format("   {0} found a neighbour {1} at {2}", gameObject.name, hitCollider.gameObject.name, dir));  
                    Neighbours.Add(hitCollider.gameObject);
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
            otherRigidbody.transform.SetParent(transform);
        
            FixedJoint2D joint = otherPiece.AddComponent<FixedJoint2D>();
            joint.connectedBody = myRigidbody;
        }
    }


    public bool FindWayToCenterPieceOrElse(GameObject destroyedExcluded)
    {
        if (gameObject.tag == "CenterPiece")
            return true;

        Dictionary<GameObject, bool> visitedPieces = new Dictionary<GameObject, bool>();

        Queue<GameObject> piecesQueue = new Queue<GameObject>();
        piecesQueue.Enqueue(gameObject);
        visitedPieces[gameObject] = true;

        while (piecesQueue.Count > 0)
        {
            FramePieceScript framePieceScript = piecesQueue.Dequeue().GetComponent<FramePieceScript>();

            if (framePieceScript.gameObject.tag == "CenterPiece")
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
                    Debug.Log(string.Format("{0} visited {1}", framePieceScript.gameObject.name, neighbour.name));
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
                if (!neighbour.GetComponent<FramePieceScript>().FindWayToCenterPieceOrElse(gameObject))
                    Destroy(neighbour);
                else
                {
                    // if (neighbour.transform.parent == gameObject.transform)
                    //     neighbour.transform.SetParent(gameObject.transform.parent);
                    Debug.Log("TO-DO: Reparent " + neighbour.name);
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
