using UnityEngine;
using UnityEngine.Tilemaps;

public class MapScroller : MonoBehaviour
{
    public Tilemap tileMap;
    private BoxCollider2D col;
    private Rigidbody2D rb;
    private MapGenerator dt;

    private float height;
    private float scrollSpeed = -4f;
    private Vector2 cameraPosition;
    private Vector2 initialPosition;

    private void Awake()
    {
        col = tileMap.GetComponent<BoxCollider2D>();
        rb = tileMap.GetComponent<Rigidbody2D>();
        dt = GetComponent<MapGenerator>();
        initialPosition = transform.position;
    }

    void Start()
    {

        height = dt.mapHeight;
        col.enabled = false;

        rb.velocity = new Vector2(0, scrollSpeed);

        cameraPosition = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }

    private void OnEnable()
    {
        transform.position = initialPosition;
    }

    void Update()
    {
        if (transform.position.y < -height - (cameraPosition.y - 1))
        {
            rb.velocity = new Vector2(0, 0);
        }
        else
        {
            rb.velocity = new Vector2(0, scrollSpeed);
        }
    }
}
