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
            Collider2D hitCollider = Physics2D.OverlapPoint(transform.position + dir);
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
