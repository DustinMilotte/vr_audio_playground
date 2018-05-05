using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ControllerActions : MonoBehaviour {
    public XRNode NodeType;
    public string grabInputName;
    private Vector3 lastFramePosition;
    private bool isGrabbing;
    private bool isResizing;
    public ControllerActions OtherHandReference;
    public Transform currentGrabbedObject;

    void Start () {
        lastFramePosition = transform.position;
        XRDevice.SetTrackingSpaceType(TrackingSpaceType.RoomScale);

    }

    void Update () {
        //update hand position and rotation
        transform.localPosition = InputTracking.GetLocalPosition(NodeType);
        transform.localRotation = InputTracking.GetLocalRotation(NodeType);

        if(isResizing) {
            ResizeGrabbedObject(currentGrabbedObject);
            if(Input.GetAxis(grabInputName) < .01f || Input.GetAxis(OtherHandReference.grabInputName) < .01f) {
                isResizing = false;
            }
        }
    }

    private void OnTriggerStay(Collider col) {
        //if it's grabbable, and you squeeze the controller
        if(col.gameObject.tag == "Grabbable" && Input.GetAxis(grabInputName) == 1 ){
            // if other hand isn't grabbing it, move it
            if(OtherHandReference.isGrabbing == false) {
                MoveGrabbedObject(col);
            }
            // if other hand is grabbing it, resize it
            if(OtherHandReference.isGrabbing == true) {
                col.transform.SetParent(null);
                currentGrabbedObject = col.transform;
                isResizing = true;
            }
        }
        if(isGrabbing && Input.GetAxis(grabInputName) < .01f) {
            ReleaseGrabbedObject(col);
        }
    }

    void MoveGrabbedObject(Collider col) {
        isGrabbing = true;
        col.transform.SetParent(transform);
    }

    void ReleaseGrabbedObject(Collider col) {
        col.transform.SetParent(null);
        isGrabbing = false;
        isResizing = false;
    }

    void ResizeGrabbedObject(Transform trans) {
        float distBetweenHands = Vector3.Distance(gameObject.transform.position, OtherHandReference.transform.position);
        print("distBetweenHands " + distBetweenHands);
        trans.localScale = new Vector3(distBetweenHands, distBetweenHands, distBetweenHands);
    }
}
