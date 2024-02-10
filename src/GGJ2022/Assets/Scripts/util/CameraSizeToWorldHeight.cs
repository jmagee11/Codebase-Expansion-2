using UnityEngine;

namespace util
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class CameraSizeToWorldHeight : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D worldBounds;

        private void Update()
        {
            var cam = GetComponent<Camera>();
            if (null != worldBounds && null != cam)
            {
                cam.orthographicSize = worldBounds.size.y / 2f;

                //var asp = Screen.width / (float)Screen.height;
                //cam.rect = new Rect((1 - asp)/2,0,asp,1);
            }
        }
    }
}