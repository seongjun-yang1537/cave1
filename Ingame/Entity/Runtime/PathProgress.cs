using System;
using System.Collections.Generic;
using Corelib.Utils;
using PathX;
using UnityEngine;
using UnityEngine.Events;

namespace Ingame
{
    [Serializable]
    public class PathProgress
    {
        public UnityEvent<Vector3> onTargetPosition = new();

        [SerializeField, Range(0f, 1f)]
        private float turnStartRatio = 0.5f;

        [SerializeField]
        public Vector3 targetPosition { get; private set; }
        public NavSurfacePoint nextPathStep => GetNextPathStep();
        [SerializeField]
        public List<NavSurfacePoint> plannedPath { get; private set; }
        public float pathProgressRatio;

        private List<float> segmentLengths = new();
        private List<float> accumulatedLengths = new();
        private float totalLength;

        public NavSurfacePoint Advance(float speed)
        {
            float newRatio = GetAdvancedRatio(this.pathProgressRatio, speed);
            this.pathProgressRatio = newRatio;

            NavSurfacePoint ret = GetInterpolatedNormalPointByRatio(this.pathProgressRatio);
            if (Mathf.Approximately(this.pathProgressRatio, 1f))
            {
                Clear();
            }
            return ret;
        }

        private NavSurfacePoint GetNextPathStep()
        {
            if (plannedPath == null || plannedPath.Count < 2) return null;

            float targetDistance = totalLength * Mathf.Clamp01(pathProgressRatio);
            int currentIndex = FindSegmentIndex(targetDistance);

            if (currentIndex + 1 < plannedPath.Count)
            {
                return plannedPath[currentIndex + 1];
            }
            return plannedPath[^1];
        }

        public void SetTargetPosition(Vector3 newTargetPosition)
        {
            if (targetPosition == newTargetPosition) return;

            this.targetPosition = newTargetPosition;
            onTargetPosition.Invoke(newTargetPosition);
        }

        public void SetCurrentPath(List<NavSurfacePoint> newPath)
        {
            plannedPath = newPath;
            pathProgressRatio = 0f;
            PrecomputePathDistances();

            if (newPath == null || newPath.Count == 0)
                return;

            SetTargetPosition(newPath.Back().point);
        }

        public void Clear()
        {
            plannedPath?.Clear();
            segmentLengths.Clear();
            accumulatedLengths.Clear();
            totalLength = 0f;
            pathProgressRatio = 0f;
            SetTargetPosition(Vector3.zero);
        }

        private void PrecomputePathDistances()
        {
            segmentLengths.Clear();
            accumulatedLengths.Clear();
            totalLength = 0f;

            if (plannedPath == null || plannedPath.Count < 2)
                return;

            for (int i = 0; i < plannedPath.Count - 1; i++)
            {
                float len = Vector3.Distance(plannedPath[i].point, plannedPath[i + 1].point);
                segmentLengths.Add(len);
                accumulatedLengths.Add(totalLength);
                totalLength += len;
            }
        }

        private int FindSegmentIndex(float distance)
        {
            if (plannedPath == null || plannedPath.Count < 2) return -1;

            int low = 0;
            int high = accumulatedLengths.Count - 1;
            int bestIndex = -1;

            while (low <= high)
            {
                int mid = low + (high - low) / 2;
                if (accumulatedLengths[mid] <= distance)
                {
                    bestIndex = mid;
                    low = mid + 1;
                }
                else
                {
                    high = mid - 1;
                }
            }
            return bestIndex == -1 ? 0 : bestIndex;
        }

        public NavSurfacePoint GetInterpolatedNormalPointByRatio(float ratio)
        {
            if (plannedPath == null || plannedPath.Count == 0) return null;
            if (plannedPath.Count == 1) return plannedPath[0];
            if (totalLength <= 0f) return plannedPath[0];

            float targetDistance = totalLength * Mathf.Clamp01(ratio);
            int index = FindSegmentIndex(targetDistance);

            if (index >= segmentLengths.Count || index + 1 >= plannedPath.Count)
            {
                var last = plannedPath[^1];
                return new NavSurfacePoint(last.point, last.normal);
            }

            float segStart = accumulatedLengths[index];
            float segLen = segmentLengths[index];
            float t = (segLen > 0) ? Mathf.Clamp01((targetDistance - segStart) / segLen) : 0;

            Vector3 point = Vector3.Lerp(plannedPath[index].point, plannedPath[index + 1].point, t);

            Vector3 normal;
            if (t < turnStartRatio)
            {
                normal = plannedPath[index].normal;
            }
            else
            {
                float remappedT = (1.0f - turnStartRatio <= 0f) ? 1.0f : (t - turnStartRatio) / (1.0f - turnStartRatio);
                normal = Vector3.Slerp(plannedPath[index].normal, plannedPath[index + 1].normal, remappedT).normalized;
            }

            return new NavSurfacePoint(point, normal);
        }

        public float GetAdvancedRatio(float currentRatio, float speed)
        {
            if (plannedPath == null || plannedPath.Count < 2 || speed <= 0f || totalLength <= 0f)
                return currentRatio;

            float currentDistance = totalLength * Mathf.Clamp01(currentRatio);
            float targetDistance = currentDistance + speed;

            if (targetDistance >= totalLength)
                return 1f;

            return targetDistance / totalLength;
        }
    }
}