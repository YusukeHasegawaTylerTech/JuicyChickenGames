using System;
using UnityEngine;

namespace JuicyChickenGames
{
    public class SwipeDetector : MonoBehaviour
    {
        private Vector2 fingerDownPosition;
        private Vector2 fingerUpPosition;

        [SerializeField]
        private bool detectSwipeOnlyAfterRelease = false;

        [SerializeField]
        private float minDistanceForSwipe = 20f;

        // Static so any listener can subscribe without a reference to a specific instance.
        // This means only one swipe zone's worth of gestures should be active at a time -
        // if a project needs several independent swipe zones, switch this to an instance event.
        public static event Action<SwipeData> OnSwipe = delegate { };

        public Camera Camera;

        private void Awake()
        {
            if (Camera == null)
            {
                Camera = UnityEngine.Camera.main;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                fingerDownPosition = Input.mousePosition;
                fingerUpPosition = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                fingerUpPosition = Input.mousePosition;
            }

            if ((detectSwipeOnlyAfterRelease && Input.GetMouseButtonUp(0)) ||
                SwipeMinDistanceReached())
            {
                fingerUpPosition = Input.mousePosition;
                DetectSwipe();
            }
        }

        internal void ResetSwipe()
        {
            fingerDownPosition = Input.mousePosition;
        }

        private void DetectSwipe()
        {
            if (SwipeMinDistanceReached())
            {
                Vector2 swipeDirection = fingerDownPosition - fingerUpPosition;
                float swipeVelocity = swipeDirection.magnitude / Time.deltaTime;

                float swipeAngle = Mathf.Atan2(fingerUpPosition.y - fingerDownPosition.y, fingerUpPosition.x - fingerDownPosition.x) * 180 / Mathf.PI;
                if (swipeAngle > -45 && swipeAngle <= 45)
                {
                    // Right swipe
                    OnSwipe?.Invoke(new SwipeData()
                    {
                        Direction = SwipeDirection.Right,
                        Velocity = swipeVelocity
                    });
                }
                else if (swipeAngle > 45 && swipeAngle <= 135)
                {
                    // Up swipe
                    OnSwipe?.Invoke(new SwipeData()
                    {
                        Direction = SwipeDirection.Up,
                        Velocity = swipeVelocity
                    });
                }
                else if (swipeAngle > 135 || swipeAngle <= -135)
                {
                    // Left swipe
                    OnSwipe?.Invoke(new SwipeData()
                    {
                        Direction = SwipeDirection.Left,
                        Velocity = swipeVelocity
                    });
                }
                else if (swipeAngle < -45 && swipeAngle >= -135)
                {
                    // Down swipe
                    OnSwipe?.Invoke(new SwipeData()
                    {
                        Direction = SwipeDirection.Down,
                        Velocity = swipeVelocity
                    });
                }

                // Reset so each gesture only fires OnSwipe once, instead of every
                // frame the finger stays past the distance threshold.
                ResetSwipe();
            }
        }

        private bool SwipeMinDistanceReached()
        {
            var worldFingerDown = GetWorldPosition(fingerDownPosition);
            var worldFingerUp = GetWorldPosition(fingerUpPosition);

            return Vector2.Distance(worldFingerDown, worldFingerUp) > minDistanceForSwipe;
        }

        private Vector3 GetWorldPosition(Vector3 position)
        {
            Vector3 worldPosition = Camera.ScreenToWorldPoint(position);
            worldPosition.z = 0;
            return worldPosition;
        }
    }

    public struct SwipeData
    {
        public SwipeDirection Direction;
        public float Velocity;
    }

    public enum SwipeDirection
    {
        Up,
        Down,
        Left,
        Right
    }
}
