using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, camera, player;
    private float maxDistance = 100f;
    private SpringJoint joint;

    void Awake()
    {
        lr = GetComponent<LineRenderer>(); 
    }

    void Update()
    {


        if (Input.GetMouseButtonDown(0)) {
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(0)) {
            StopGrapple();
        }
    }  
    void LateUpdate() 
        {
            DrawRope();
        } 
       
        // call when we want to start grapple 
        void StartGrapple()
        {
            RaycastHit hit;
            if (Physics.Raycast(origin: camera.position, direction: camera.forward, out hit, maxDistance, whatIsGrappleable))
            {
                grapplePoint = hit.point;
                joint = player.gameObject.AddComponent<SpringJoint>();
                joint.autoConfigureConnectedAnchor = false;
                joint.connectedAnchor = grapplePoint;

                float distanceFromPoint = Vector3.Distance(a: player.position, b: grapplePoint);

                // the distance grapple will try to keep from point (can be changed)
                joint.maxDistance = distanceFromPoint * 1.2f;
                joint.minDistance = distanceFromPoint * 0.25f;

                // these can also be changed 
                joint.spring = 4.5f;
                joint.damper = 7.5f;
                joint.massScale = 4.5f;

                lr.positionCount = 2; 
            }
        }

        // call when we want to stop grapple 
        void StopGrapple()
        {
            lr.positionCount = 0;
            Destroy(joint);
        }

        void DrawRope()
        {
            // if we are not grappling, don't draw a rope
            if (!joint) return;
            lr.SetPosition(index: 0, gunTip.position);
            lr.SetPosition(index: 1, grapplePoint);
        }


        public bool IsGrappling()
        {
            return joint != null;
        }

        public Vector3 GetGrapplePoint()
        {
            return grapplePoint; 
        }


    


}