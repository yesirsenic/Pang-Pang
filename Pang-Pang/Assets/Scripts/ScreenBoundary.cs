using UnityEngine;

public class ScreenBoundary : MonoBehaviour
{
    public float margin = 2f; // 월드 단위 여유

    void Start()
    {
        Camera cam = Camera.main;

        Vector3 bottomLeft = cam.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = cam.ScreenToWorldPoint(
            new Vector3(Screen.width, Screen.height, 0));

        Vector2 size = topRight - bottomLeft;

        BoxCollider2D col = gameObject.AddComponent<BoxCollider2D>();
        col.isTrigger = true;

        // 상하좌우로 margin 만큼 확장
        col.size = size + Vector2.one * margin * 2f;

        // 중심은 그대로
        col.offset = (topRight + bottomLeft) * 0.5f;
    }
}
