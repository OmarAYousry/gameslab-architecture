﻿using System.Collections.Generic;
using UnityEngine;

public class TransformControl : MonoBehaviour
{
    private Vector3 offset;

    MovementPlane movementPlane;
    bool firstClick = false;
    bool originalPosSet = false;
    int objLayer = 0;
    int planeLayer = 0;

    public bool useLimiter = true;
    public float moveDistanceLimit = 3;

    Vector3 originalPosition;
    List<LineRenderer> boundLines;

    void OnEnable()
    {
        objLayer = LayerMask.NameToLayer("MovementObject");
        planeLayer = LayerMask.NameToLayer("MovementPlane");
        transform.gameObject.layer = objLayer;
        movementPlane = Global.MovementPlane;
        originalPosition = transform.position;
        if (movementPlane == null)
        {
            Debug.LogWarning("[MovementControl] There is no MovementPlane in the scene.");
            enabled = false;
        }
        //if (useLimiter)
        //{
        //    if (boundLines == null)
        //    {
        //        boundLines = new List<LineRenderer>();
                
        //        for (int i = 0; i < 4; i++)
        //        {
        //            GameObject newGO = Instantiate(new GameObject());
        //            LineRenderer lineRenderer = newGO.AddComponent<LineRenderer>();
        //            boundLines.Add(lineRenderer);
        //        }
        //    }
        //    RenderBounds();
        //}
    }

    void RenderBounds()
    {
        float widthOffset = GetComponent<MeshRenderer>().bounds.size.x / 2;
        float depthOffset = GetComponent<MeshRenderer>().bounds.size.z / 2;
        float width = 0.025f;

        //Vector3[] vecs1 = new Vector3[2];
        //vecs1[0] = new Vector3(originalPosition.x + (moveDistanceLimit + widthOffset), transform.position.y, originalPosition.z + (moveDistanceLimit + depthOffset));
        //vecs1[1] = new Vector3(originalPosition.x - (moveDistanceLimit + widthOffset), transform.position.y, originalPosition.z + (moveDistanceLimit + depthOffset));
        //boundLines[0].SetPositions(vecs1);

        //Vector3[] vecs2 = new Vector3[2];
        //vecs2[0] = new Vector3(originalPosition.x + (moveDistanceLimit + widthOffset), transform.position.y, originalPosition.z - (moveDistanceLimit + depthOffset));
        //vecs2[1] = new Vector3(originalPosition.x - (moveDistanceLimit + widthOffset), transform.position.y, originalPosition.z - (moveDistanceLimit + depthOffset));
        //boundLines[1].SetPositions(vecs2);

        //Vector3[] vecs3 = new Vector3[2];
        //vecs3[0] = new Vector3(originalPosition.x + (moveDistanceLimit + widthOffset), transform.position.y, originalPosition.z + (moveDistanceLimit + depthOffset));
        //vecs3[1] = new Vector3(originalPosition.x + (moveDistanceLimit + widthOffset), transform.position.y, originalPosition.z - (moveDistanceLimit + depthOffset));
        //boundLines[2].SetPositions(vecs3);

        //Vector3[] vecs4 = new Vector3[2];
        //vecs4[0] = new Vector3(originalPosition.x - (moveDistanceLimit + widthOffset), transform.position.y, originalPosition.z + (moveDistanceLimit + depthOffset));
        //vecs4[1] = new Vector3(originalPosition.x - (moveDistanceLimit + widthOffset), transform.position.y, originalPosition.z - (moveDistanceLimit + depthOffset));
        //boundLines[3].SetPositions(vecs4);

        Vector3[] vecs1 = new Vector3[2];
        vecs1[0] = originalPosition + transform.right * (moveDistanceLimit + widthOffset) + transform.forward * (moveDistanceLimit + depthOffset);
        vecs1[1] = originalPosition - transform.right * (moveDistanceLimit + widthOffset) + transform.forward * (moveDistanceLimit + depthOffset);
        boundLines[0].SetPositions(vecs1);

        Vector3[] vecs2 = new Vector3[2];
        vecs2[0] = originalPosition + transform.right * (moveDistanceLimit + widthOffset) - transform.forward * (moveDistanceLimit + depthOffset);
        vecs2[1] = originalPosition - transform.right * (moveDistanceLimit + widthOffset) - transform.forward * (moveDistanceLimit + depthOffset);
        boundLines[1].SetPositions(vecs2);

        Vector3[] vecs3 = new Vector3[2];
        vecs3[0] = originalPosition + transform.right * (moveDistanceLimit + widthOffset) + transform.forward * (moveDistanceLimit + depthOffset);
        vecs3[1] = originalPosition + transform.right * (moveDistanceLimit + widthOffset) - transform.forward * (moveDistanceLimit + depthOffset);
        boundLines[2].SetPositions(vecs3);

        Vector3[] vecs4 = new Vector3[2];
        vecs4[0] = originalPosition - transform.right * (moveDistanceLimit + widthOffset) + transform.forward * (moveDistanceLimit + depthOffset);
        vecs4[1] = originalPosition - transform.right * (moveDistanceLimit + widthOffset) - transform.forward * (moveDistanceLimit + depthOffset);
        boundLines[3].SetPositions(vecs4);

        foreach (LineRenderer bound in boundLines)
        {
            bound.startWidth = width;
            bound.endWidth = width;
            bound.gameObject.SetActive(true);
            Color color = bound.material.color;
            color.a = 0.25f;
            bound.material.color = color;
        }
    }

    void OnDisable()
    {
        transform.gameObject.layer = 0;

        foreach (LineRenderer bound in boundLines)
            Destroy(bound.gameObject);

        boundLines = null;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (!firstClick)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << objLayer))
                {
                    offset = gameObject.transform.position - hit.point;
                    movementPlane.transform.position = hit.point - Vector3.up * 0.5f;
                    movementPlane.gameObject.SetActive(true);
                    firstClick = true;
                }
                return;
            }
            Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit2;
            if (Physics.Raycast(ray2, out hit2, Mathf.Infinity, 1 << planeLayer, QueryTriggerInteraction.Collide))
            {
                Vector3 curPosition = hit2.point + offset;
                curPosition = new Vector3(curPosition.x, transform.position.y, curPosition.z);

                //if (useLimiter)
                //    curPosition = ClampVector(curPosition, moveDistanceLimit);

                //if(IsWithinBounds(curPosition, moveDistanceLimit))
                //    transform.position = curPosition;

                if (useLimiter)
                    transform.position = ClampByDistance(curPosition, moveDistanceLimit);
            }
        }
        else
        {
            firstClick = false;
            movementPlane.gameObject.SetActive(false);
        }
    }

    public bool IsWithinBounds(Vector3 position, float distance)
    {
        return (Vector3.Distance(originalPosition, position) < distance);
    }

    public Vector3 ClampByDistance(Vector3 position, float maxDistance)
    {
        float distance = Vector3.Distance(originalPosition, position);
        if (distance > maxDistance)
        {
            Vector3 direction = position - originalPosition;
            return originalPosition + direction.normalized * maxDistance;
        } else
        {
            return position;
        }
    }

    public Vector3 ClampVector(Vector3 input, float distance)
    {
        Vector3 forward = originalPosition + transform.forward * distance;
        Vector3 backward = originalPosition - transform.forward * distance;
        Vector3 right = originalPosition + transform.right * distance;
        Vector3 left = originalPosition - transform.right * distance;

        float x = Mathf.Clamp(input.x, left.x, right.x);
        float z = Mathf.Clamp(input.z, backward.z, forward.z);
        float y = Mathf.Clamp(input.y, originalPosition.y - distance, originalPosition.y + distance);

        Vector3 output = new Vector3(x, transform.position.y, z);
        return output;
    }

    public Vector3 GetOriginalPosition()
    {
        return originalPosition;
    }
}
