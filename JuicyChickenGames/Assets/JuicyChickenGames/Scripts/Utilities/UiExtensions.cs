using UnityEngine;
using UnityEngine.EventSystems;

namespace JuicyChickenGames
{
    public static class UiExtensions
    {
        public static bool IsPointerOverGameObject()
        {
            //check mouse
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return true;
            }

            //check touch
            if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                {
                    return true;
                }
            }

            return false;
        }

        public static void ScrollToPosition(this UnityEngine.UI.ScrollRect instance, Vector3 childPosition)
        {
            instance.content.ForceUpdateRectTransforms();
            instance.viewport.ForceUpdateRectTransforms();

            // now takes scaling into account
            Vector2 viewportLocalPosition = instance.viewport.localPosition;
            Vector2 childLocalPosition = instance.content.InverseTransformPoint(childPosition);
            Vector2 newContentPosition = new Vector2(
                0 - ((viewportLocalPosition.x * instance.viewport.localScale.x) + (childLocalPosition.x * instance.content.localScale.x)),
                0 - ((viewportLocalPosition.y * instance.viewport.localScale.y) + (childLocalPosition.y * instance.content.localScale.y))
            );

            // clamp positions
            instance.content.localPosition = newContentPosition;
            Rect contentRectInViewport = TransformRectFromTo(instance.content.transform, instance.viewport);
            float deltaXMin = contentRectInViewport.xMin - instance.viewport.rect.xMin;
            if (deltaXMin > 0) // clamp to <= 0
            {
                newContentPosition.x -= deltaXMin;
            }
            float deltaXMax = contentRectInViewport.xMax - instance.viewport.rect.xMax;
            if (deltaXMax < 0) // clamp to >= 0
            {
                newContentPosition.x -= deltaXMax;
            }
            float deltaYMin = contentRectInViewport.yMin - instance.viewport.rect.yMin;
            if (deltaYMin > 0) // clamp to <= 0
            {
                newContentPosition.y -= deltaYMin;
            }
            float deltaYMax = contentRectInViewport.yMax - instance.viewport.rect.yMax;
            if (deltaYMax < 0) // clamp to >= 0
            {
                newContentPosition.y -= deltaYMax;
            }

            // apply final position
            instance.content.localPosition = newContentPosition;
            instance.content.ForceUpdateRectTransforms();
        }

        /// <summary>
        /// Converts a Rect from one RectTransfrom to another RectTransfrom.
        /// </summary>
        public static Rect TransformRectFromTo(Transform from, Transform to)
        {
            RectTransform fromRectTrans = from.GetComponent<RectTransform>();
            RectTransform toRectTrans = to.GetComponent<RectTransform>();

            if (fromRectTrans != null && toRectTrans != null)
            {
                Vector3[] fromWorldCorners = new Vector3[4];
                Vector3[] toLocalCorners = new Vector3[4];
                Matrix4x4 toLocal = to.worldToLocalMatrix;
                fromRectTrans.GetWorldCorners(fromWorldCorners);
                for (int i = 0; i < 4; i++)
                {
                    toLocalCorners[i] = toLocal.MultiplyPoint3x4(fromWorldCorners[i]);
                }

                return new Rect(toLocalCorners[0].x, toLocalCorners[0].y, toLocalCorners[2].x - toLocalCorners[1].x, toLocalCorners[1].y - toLocalCorners[0].y);
            }

            return default(Rect);
        }
    }
}
