using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    //calling upon the Player Controller Script
    
    private GameManager theGameManager;
    public float cameraDistance;
    private Vector3 lastPlayerPosition = new Vector3(0, 0, 0);
    private float distanceToMove;
    [SerializeField] Camera myCamera;

    // Start is called before the first frame update//

    void Start()
    {
        theGameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
        CameraScaler();
    }

    private void MoveCamera()
    {
        if(theGameManager.warbandMembers.Count != 0)
        {
            Vector3 cameraAnchor = theGameManager.warbandMembers[0].transform.GetChild(1).transform.position;

            distanceToMove = cameraAnchor.x - lastPlayerPosition.x;
            //the transform of the current object that this script is attatched to
            transform.position = new Vector3(transform.position.x + distanceToMove, transform.position.y, transform.position.z);

            lastPlayerPosition = cameraAnchor;

        }


    }

    private void CameraScaler()
    {
        if (theGameManager.warbandMembers.Count <= 5)
        {
            myCamera.orthographicSize = 6;
        }
        else if (theGameManager.warbandMembers.Count > myCamera.orthographicSize)
        {
            myCamera.orthographicSize += 1;
        }
        else if (theGameManager.warbandMembers.Count < myCamera.orthographicSize && theGameManager.warbandMembers.Count > 5)
        {
            myCamera.orthographicSize -= 1;
        }
    }
}
