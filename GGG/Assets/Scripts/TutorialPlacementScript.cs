using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlacementScript : MonoBehaviour
{
    public GameObject FramePieceDemo;
    public GameObject GridDemo;
    public GameObject ConnectedConfig;
    public GameObject UnconnectedConfig;
    public GameObject ControlPointsBlackboard;
    public GameObject ControlPoints;
    public GameObject TimerObject;
    public GameObject ShopObject;
    private int _controlPointsCaptured = 0;

    public void WhatIsAFramePiece()
    {
        FramePieceDemo.SetActive(true);
    }
    public void HowToGrid()
    {
        FramePieceDemo.SetActive(false);
        GridDemo.SetActive(true);
    }
    public void Connected()
    {
        GridDemo.SetActive(false);
        ConnectedConfig.SetActive(true);
    }

    public void Unconnected()
    {
        ConnectedConfig.SetActive(false);
        UnconnectedConfig.SetActive(true);
    }

    public void TestTime()
    {
        TimerObject.SetActive(true);
        ShopObject.SetActive(true);
    }

    public void ShowControlPoints()
    {
        UnconnectedConfig.SetActive(false);
        ControlPointsBlackboard.SetActive(true);
        ControlPoints.SetActive(true);

        SetControlPointsEnabled(false);
    }

    public void SetControlPointsEnabled(bool enableStatus)
    {
        ControlPointScript[] controlPointScripts = ControlPoints.GetComponentsInChildren<ControlPointScript>();
        foreach (ControlPointScript controlPointScript in controlPointScripts)
        {
            controlPointScript.enabled = enableStatus;
            controlPointScript.gameObject.GetComponent<BoxCollider2D>().enabled = enableStatus;
        }
    }

    public void CaptureControlPoint()
    {
        _controlPointsCaptured += 1;

        if (_controlPointsCaptured == 3)        
            FindObjectOfType<WinConScript>().YouWin();
    }
}
