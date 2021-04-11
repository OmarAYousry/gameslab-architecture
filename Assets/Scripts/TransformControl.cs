using UnityEngine;

public class TransformControl : MonoBehaviour
{
    private Vector3 offset;

    MovementPlane movementPlane;
    bool firstClick = false;
    bool originalPosSet = false;
    int objLayer = 0;
    int planeLayer = 0;

    float moveDistanceLimit = 3;

    Vector3 originalPosition;

    void OnEnable()
    {
        objLayer = LayerMask.NameToLayer("MovementObject");
        planeLayer = LayerMask.NameToLayer("MovementPlane");
        transform.gameObject.layer = objLayer;
        movementPlane = Global.MovementPlane;
        if (!originalPosSet && originalPosition != null)
        {
            originalPosition = transform.localPosition;
            originalPosSet = true;
        }
        if (movementPlane == null)
        {
            Debug.LogWarning("[MovementControl] There is no MovementPlane in the scene.");
            enabled = false;
        }
    }

    void OnDisable()
    {
        transform.gameObject.layer = 0;
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
                    Debug.Log(hit.collider.gameObject.name);
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
                curPosition = ClampVector(curPosition, moveDistanceLimit);
                transform.position = curPosition;
            }
        } else
        {
            firstClick = false;
            movementPlane.gameObject.SetActive(false);
        }
    }

    public Vector3 ClampVector(Vector3 input, float distance)
    {
        float x = Mathf.Clamp(input.x, originalPosition.x - distance, originalPosition.x + distance);
        float z = Mathf.Clamp(input.z, originalPosition.z - distance, originalPosition.z + distance);
        float y = Mathf.Clamp(input.y, originalPosition.y - distance, originalPosition.y + distance);
        Vector3 output = new Vector3(x, y, z);
        Debug.Log(output);
        return output;
    }

    public Vector3 GetOriginalPosition()
    {
        return originalPosition;
    }
}
