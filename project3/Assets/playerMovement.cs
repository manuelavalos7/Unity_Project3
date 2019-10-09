using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playerMovement : MonoBehaviour
{
    private float speed= 10f;
    [SerializeField] TileBase selectedTile;
    [SerializeField] TileBase boxTile;
    [SerializeField] TileBase boxTileTop;
    [SerializeField] Tilemap tileObstacles;
    [SerializeField] Tilemap tileBackground;
    [SerializeField] Tilemap tileForeground;
    [SerializeField] Tilemap tileWalls;
    [SerializeField] GameObject[] enemies;
    [SerializeField] Sprite openChest;
    private GameObject lastSavePoint;
    private Vector2 lastSavePosition; //integer representation on grid
    private Vector2[] startPoint = {new Vector2(0.25f,0.3f), new Vector2(0.25f, 0.3f), new Vector2(0.25f, 0.3f) };

    private int totalskulls = 0;
    private int numskulls = 0;
    private Vector3Int max;
    private Vector3Int min;
    private Vector2 playerPosition = Vector2.zero;
    private float moveCooldown = 0;
    private const float coolDown = 1f;
    // Start is called before the first frame update
    void Start()
    {
        min = new Vector3Int(-17, -9, 0);
        max = new Vector3Int(16, 8, 0);
        GameObject.Find("boxLeftText").GetComponent<Text>().text = "skulls Left: " + numskulls;
       
    }

    // Update is called once per frame
    void Update()
    {
        
        if (moveCooldown <= 0)
        {
            movePlayer();
            moveCooldown = coolDown;
        }
        moveCooldown -= Time.deltaTime * speed;
        if (Input.GetMouseButton(0))
        {
            placeObstacle(Camera.main.ScreenToWorldPoint(Input.mousePosition), false);
        }
        if (Input.GetMouseButton(1))
        {
            placeObstacle(Camera.main.ScreenToWorldPoint(Input.mousePosition), true);
        }


    }
    private void movePlayer() {
        Vector3 change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        if (change.x != 0 || change.y != 0)
        {
            if (change.x != 0)
            {
                change.y = 0;
                change.x = change.x / Mathf.Abs(change.x);//to move one unit in x
            }
            else if (change.y != 0)
            {
                change.y = change.y / Mathf.Abs(change.y);//to move one unit in y
            }
            if (!tileMapHasObstacle(transform.position.x + change.x * 0.5f, transform.position.y + change.y * 0.5f))
            {
                transform.position += (change * 0.5f);
                playerPosition += (Vector2)change;
            }
            if (change.x != 0)
            {
                transform.localScale = new Vector3(change.x, transform.localScale.y, transform.localScale.z);//flips sprite if player moved on x axis
            }
            foreach (GameObject e in enemies)
            {
                e.GetComponent<enemy>().moveEnemy();
            }
        }
    }

    public bool tileMapHasObstacle(float x, float y) {
        //checks the tilemap at x,y to see if there is an obstacle
  
        TileBase obstacle = tileObstacles.GetTile(tileObstacles.WorldToCell(new Vector3(x,y,0)));
        TileBase wall = tileWalls.GetTile(tileObstacles.WorldToCell(new Vector3(x, y, 0)));
        return (wall != null || obstacle != null);//returns true if there is an obstacle (obstacle is not null)
    }

    private void highlightTile(Vector2 tile) {
        Debug.Log(tile);
        tile.x = (tile.x * 2);
        tile.y = (tile.y * 2);
        if (tile.x < 0) {
            tile.x -= 1;
        }
        if (tile.y < 0)
        {
            tile.y -= 1;
        }
        tileBackground.SetTile(new Vector3Int((int)tile.x,(int) tile.y, 0), selectedTile);
        TileBase selected = tileBackground.GetTile(tileBackground.WorldToCell(tile));
    }

    private void placeObstacle(Vector2 tile, bool remove)
    {

        tile.x = (tile.x * 2);
        tile.y = (tile.y * 2);
        if (tile.x < 0)
        {
            tile.x -= 1;
        }
        if (tile.y < 0)
        {
            tile.y -= 1;
        }
        Vector3Int position = new Vector3Int((int)tile.x, (int)tile.y, 0);
        if (!remove && numskulls > 0 && canPlaceObstacle(position)) {
            tileObstacles.SetTile(position, boxTile);
            tileForeground.SetTile(new Vector3Int(position.x, position.y + 1, 0), boxTileTop);
            numskulls--;
            GameObject.Find("boxLeftText").GetComponent<Text>().text = "skulls Left: "+numskulls;
        }
        else if (tileObstacles.GetTile(position) != null && remove)
        {
            tileObstacles.SetTile(position, null);
            tileForeground.SetTile(new Vector3Int(position.x, position.y + 1, 0), null);
        
            numskulls++;
            GameObject.Find("boxLeftText").GetComponent<Text>().text = "skulls Left: " + numskulls;
        }

    }

    private bool canPlaceObstacle(Vector3Int tile_position) {
        //based on grid position
        TileBase wall = tileWalls.GetTile(tile_position);
        TileBase obstacle = tileObstacles.GetTile(tile_position);
        Debug.Log(tile_position.x + "," + tile_position.y);
        if (playerPosition.x == tile_position.x && playerPosition.y == tile_position.y) {
            Debug.Log("on player");
            return false;
        }
        if (wall!=null || obstacle!=null)
        {
            Debug.Log("wall/obstacle");
            return false;
        }
        if (tile_position.x < min.x || tile_position.x > max.x || tile_position.y < min.y || tile_position.y > max.y) {
            Debug.Log("Out of map");
            return false;
        }

        return true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        if (collision.tag == "boxCollect")
        {
            totalskulls++;
            numskulls++;
            Destroy(collision.gameObject);
            GameObject.Find("boxLeftText").GetComponent<Text>().text = "skulls Left: " + numskulls;
        }
        else if (collision.tag == "SavePoint")
        {
            lastSavePoint = collision.gameObject;
            lastSavePosition = playerPosition;
            collision.gameObject.GetComponent<torchScript>().toggleLight();
        }
        else if (collision.tag == "Win")
        {
            WinLevel(SceneManager.GetActiveScene().buildIndex, collision.gameObject);
        }
        else if (collision.tag == "Enemy") {
            if (lastSavePoint != null)
            {
                playerPosition = lastSavePosition;
                transform.position = lastSavePoint.transform.position;
            }
            else
            {
                playerPosition = new Vector2(0, 0);
                Debug.Log(startPoint[SceneManager.GetActiveScene().buildIndex]);
                transform.position = startPoint[SceneManager.GetActiveScene().buildIndex];
            }
        }
    }

    private void WinLevel(int index, GameObject chest)
    {
        chest.GetComponent<SpriteRenderer>().sprite = openChest;
    }

}


