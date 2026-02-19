using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMove : MonoBehaviour
{
    public float fixedY = -4f;

    private Camera cam;
    private float minX;
    private float maxX;
    private Collider2D col;

    private float targetX;
    private bool hasInput;

    private Rigidbody2D rb;

    private void Start()
    {
        cam = Camera.main;
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();

        Vector2 left = cam.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 right = cam.ViewportToWorldPoint(new Vector2(1, 0));

        float halfWidth = col.bounds.extents.x;
        minX = left.x + halfWidth;
        maxX = right.x - halfWidth;


    }

    private void Update()
    {
        if (GameManager.Instance.IsPaused)
            return;

        float? screenX = null;

        if (Touchscreen.current != null &&
            Touchscreen.current.primaryTouch.press.isPressed)
        {
            screenX = Touchscreen.current.primaryTouch.position.ReadValue().x;
        }

#if UNITY_EDITOR
        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            screenX = Mouse.current.position.ReadValue().x;
        }
#endif

#if UNITY_WEBGL
        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            screenX = Mouse.current.position.ReadValue().x;
        }
#endif

        if (screenX.HasValue)
        {
            float depth = cam.WorldToScreenPoint(transform.position).z;
            Vector3 world = cam.ScreenToWorldPoint(new Vector3(screenX.Value, 0, depth));
            targetX = Mathf.Clamp(world.x, minX, maxX);
            hasInput = true;
        }
    }

    private void FixedUpdate()
    {
        if (!hasInput) return;

        if(PlayerPrefs.GetInt("TutorialShown") == 0)
        {
            GameManager.Instance.TutorialOffAndPlay();
        }

        rb.MovePosition(new Vector2(targetX, fixedY));
    }


}
