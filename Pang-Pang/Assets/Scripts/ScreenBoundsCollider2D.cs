using UnityEngine;

public class ScreenBoundsCollider2D : MonoBehaviour
{
    public float WallThickness = 1f;

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
        CreateWalls();
        
    }

    void CreateWalls()
    {
        Vector2 bottomLeft = cam.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 topRight = cam.ViewportToWorldPoint(new Vector2(1, 1));

        float width = topRight.x - bottomLeft.x;
        float height = topRight.y - bottomLeft.y;

        //Left
        CreateWall(
            "LeftWall",
            new Vector2(bottomLeft.x - WallThickness / 2f, 0),
            new Vector2(WallThickness, height * 2)
            );

        //Right
        CreateWall(
            "RightWall",
            new Vector2(topRight.x + WallThickness / 2f, 0),
            new Vector2(WallThickness, height * 2)
            );

        //Bottom
        CreateWall(
            "BottomWall",
            new Vector2(0, bottomLeft.y - WallThickness / 2f),
            new Vector2(width * 2, WallThickness)
            );

        //Top
        CreateWall(
            "TopWall",
            new Vector2(0, topRight.y + WallThickness / 2f),
            new Vector2(width * 2, WallThickness)
            );

        
    }

    void CreateWall(string name, Vector2 position, Vector2 size)
    {
        GameObject wall = new GameObject(name);
        wall.transform.parent = transform;
        wall.transform.position = position;
        wall.tag = "Wall";

        BoxCollider2D col = wall.AddComponent<BoxCollider2D>();
        col.size = size;
    }
}
