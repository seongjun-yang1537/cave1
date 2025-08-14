using UnityEngine;
using UnityEngine.U2D;
using VContainer;
using VContainer.Unity;

namespace Ingame
{
    public class GrapplingController : MonoBehaviour
    {
        public PawnController pawnController;

        private Transform pivotTransform;
        public Vector3 grapplePoint;
        private CharacterController characterController;
        private PlayerPhysics playerPhysics;
        private Vector3 currentVelocity;
        private Vector3 horizontalVelocity;
        private bool isGrappling;

        public float overshootYAxis;

        public float maxGrappingDistance = 25f;
        public float forwardThrustForce = 1.0f;

        // CharacterController based movement no longer requires SpringJoint

        public void UseSecondaryDown(PawnController pawnController)
        {
            this.pawnController = pawnController;
            pivotTransform = pawnController.transform;
            characterController = pawnController.GetComponent<CharacterController>();
            var scope = pawnController.GetComponent<PlayerScope>();
            if (scope != null)
                playerPhysics = scope.Container.Resolve<PlayerPhysics>();

            StartGrappling();
        }

        public void UseSecondaryUp(PawnController pawnController)
        {
            StopGrappling();
        }

        private void ExecuteGrapple()
        {
            Vector3 lowestPoint = new Vector3(pivotTransform.position.x, pivotTransform.position.y - 1f, pivotTransform.position.z);

            float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
            float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

            if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;

            JumpToPosition(grapplePoint, highestPointOnArc);
        }

        private Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
        {
            float gravity = Physics.gravity.y;
            float displacementY = endPoint.y - startPoint.y;
            Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

            Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
            Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity)
                + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

            return velocityXZ + velocityY;
        }

        Vector3 velocityToSet;
        public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
        {
            velocityToSet = CalculateJumpVelocity(pivotTransform.position, targetPosition, trajectoryHeight);
            currentVelocity = velocityToSet;
            horizontalVelocity = new Vector3(currentVelocity.x, 0f, currentVelocity.z);
            playerPhysics?.SetVelocity(currentVelocity.y);
            isGrappling = true;
        }

        private void StartGrappling()
        {
            Vector3 grappleDirection = Camera.main.transform.forward;
            Vector3 pivot = transform.position;

            RaycastHit hit;
            Ray ray = new(Camera.main.transform.position, grappleDirection);

            Vector3 targetPosition = pivot + grappleDirection * maxGrappingDistance;
            if (Physics.Raycast(ray,
                out hit,
                maxGrappingDistance,
                1 << LayerMask.NameToLayer("Landscape")))
            {
                targetPosition = hit.point;
            }

            grapplePoint = targetPosition;

            Invoke(nameof(ExecuteGrapple), 0.1f);
        }

        private void Update()
        {
            if (!isGrappling)
                return;

            characterController.Move(horizontalVelocity * Time.deltaTime);

            if (playerPhysics != null && playerPhysics.IsGrounded &&
                Vector3.Distance(pivotTransform.position, grapplePoint) < 1f)
            {
                StopGrappling();
            }
        }

        private void StopGrappling()
        {
            isGrappling = false;
            currentVelocity = Vector3.zero;
            horizontalVelocity = Vector3.zero;
            playerPhysics?.ResetVelocity();
        }

        public bool IsGrappling()
        {
            return isGrappling;
        }

        public Vector3 GetGrapplePoint()
        {
            return grapplePoint;
        }
    }
}
