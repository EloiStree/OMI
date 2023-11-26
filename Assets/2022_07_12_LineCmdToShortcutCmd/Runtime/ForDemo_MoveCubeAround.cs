using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForDemo_MoveCubeAround : MonoBehaviour
{
    public Transform m_startPoint;
    public Transform m_cubeToMove;
    public float m_distanceToMoveMultiplicator = 1;
    public Renderer m_cubeRenderer;


    [ContextMenu("Move Left")]
    public void MoveLeft() => MoveCube(-1, 0);
    [ContextMenu("Move Right")]
    public void MoveRight() => MoveCube(1, 0);
    [ContextMenu("Move Front")]
    public void MoveFront() => MoveCube(0,1);
    [ContextMenu("Move Back")]
    public void MoveBack() => MoveCube(0,-1);

    public void MoveCube(float leftToRight, float backToFront) {
        m_cubeToMove.Translate(new Vector3(leftToRight, 0, backToFront) * m_distanceToMoveMultiplicator);
    }

    [ContextMenu("Reset Position")]
    public void ResetPosition()
    {
        m_cubeToMove.position = m_startPoint.position;
    }

    [ContextMenu("SetRed")]
    public void SetRed() => ResetChangeColor(255, 0, 0);

    [ContextMenu("SetGreen")]
    public void SetGreen() => ResetChangeColor(0, 255, 0);

    [ContextMenu("SetBlue")] 
    public void SetBlue() => ResetChangeColor(0, 0, 255);

    public void ResetChangeColor(int r255, int g255 ,int b255)
    {
        m_cubeRenderer.material.color = new Color(r255 / 255.0f, g255 / 255.0f, b255/255.0f);
    }
}
