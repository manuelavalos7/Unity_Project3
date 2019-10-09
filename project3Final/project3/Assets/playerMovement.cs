using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.LWRP;

public class playerMovement : MonoBehaviour
{
    private float speed= 10f;
    [SerializeField] TileBase selectedTile;
    [SerializeField] Tilemap tileObstacles;
    [SerializeField] Tilemap tileBackground;
    [SerializeField] Tilemap tileForeground;
    [SerializeField] Tilemap tileWalls;
    [SerializeField] GameObject[] enemies;
    [SerializeField] Sprite openChest;
    private GameManager gameManager;
    
    private GameObject lastSavePoint;
    private Vector2 lastSavePosition; //integer representation on grid
    private Vector2[] startPoint = {new Vector2(0.25f,0.3f), new Vector2(0.25f, 0.3f), new Vector2(0.25f, 0.3f) };

    private Vector3Int max;
    private Vector3Int min;
    public Vector2 playerPosition = Vector2.zero;
    private float moveCooldown = 0;
    private const float coolDown = 1f;
    // Start is called before the first frame update
    void Start()
    {
        min = new Vector3Int(-17, -9, 0);
        max = new Vector3Int(16, 8, 0);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerPosition = transform.position / 0.5f;
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
        if (Input.GetKeyDown(KeyCode.Space) && gameManager.supers>0) {
            gameManager.usedSuper();
            StartCoroutine(FlashScreen());

        }

    }
    private void movePlayer() {
        Vector3 playerChange = Vector3.zero;
        playerChange.x = Input.GetAxisRaw("Horizontal");
        playerChange.y = Input.GetAxisRaw("Vertical");
        if (playerChange.x != 0 || playerChange.y != 0)
        {
            if (playerChange.x != 0)
            {
                playerChange.y = 0;
                playerChange.x = playerChange.x / Mathf.Abs(playerChange.x);//to move one unit in x
            }
            else if (playerChange.y != 0)
            {
                playerChange.y = playerChange.y / Mathf.Abs(playerChange.y);//to move one unit in y
            }
            if (!tileMapHasObstacle(transform.position.x + playerChange.x * 0.5f, transform.position.y + playerChange.y * 0.5f))
            {
                transform.position += (playerChange * 0.5f);
                playerPosition += (Vector2)playerChange;
            }
            if (playerChange.x != 0)
            {
                transform.localScale = new Vector3(playerChange.x, transform.localScale.y, transform.localScale.z);//flips sprite if player moved on x axis
            }
            enemies[0].GetComponent<enemy>().moveEnemy();
           // enemies = GameObject.FindGameObjectsWithTag("Enemy");
            //foreach (GameObject e in enemies)
            //{
            //    e.GetComponent<enemy>().moveEnemy();
            //}
        }
    }

    public bool tileMapHasObstacle(float x, float y) {
        //checks the tilemap at x,y to see if there is an obstacle
  
        TileBase obstacle = tileObstacles.GetTile(tileObstacles.WorldToCell(new Vector3(x,y,0)));
        TileBase wall = tileWalls.GetTile(tileObstacles.WorldToCell(new Vector3(x, y, 0)));
        return (wall != null || obstacle != null);//returns true if there is an obstacle (obstacle is not null)
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        if (collision.tag == "Collect")
        {
            Destroy(collision.gameObject);
            gameManager.addSkull();
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

    IEnumerator FlashScreen() {
        float elapsed = 0f;

        float original = GetComponentInChildren<Light2D>().pointLightOuterRadius;
        while(elapsed < 1f)
        {
            GetComponentInChildren<Light2D>().pointLightOuterRadius += 25*Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }
        elapsed = 0;
        while (elapsed < 1f)
        {
            GetComponentInChildren<Light2D>().pointLightOuterRadius -= 25 * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }
        GetComponentInChildren<Light2D>().pointLightOuterRadius =original;
    }

    private void WinLevel(int index, GameObject chest)
    {
        chest.GetComponent<SpriteRenderer>().sprite = openChest;
    }

}


