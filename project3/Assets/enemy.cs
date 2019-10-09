using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    [SerializeField] GameObject player;
    private playerMovement playerMove;
    public Vector2 enemyPosition;
    // Start is called before the first frame update
    void Start()
    {
        playerMove = player.GetComponent<playerMovement>();
        enemyPosition = transform.position / 0.25f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void moveEnemy() {
        Vector3 change = Vector3.zero;
        change.x = Random.Range(-1, 2);
        change.y = Random.Range(-1,2);
        if (change.x != 0)
        {
            change.y = 0;
            change.x = change.x / Mathf.Abs(change.x);//to move one unit in x
        }
        else if (change.y != 0)
        {
            change.y = change.y / Mathf.Abs(change.y);//to move one unit in y
        }
        if (!playerMove.tileMapHasObstacle(transform.position.x + change.x * 0.5f, transform.position.y + change.y * 0.5f))
        {
            transform.position += (change * 0.5f);
            enemyPosition += (Vector2)change;
        }
        if (change.x != 0)
        {
            transform.localScale = new Vector3(-change.x, transform.localScale.y, transform.localScale.z);//flips sprite if player moved on x axis
        }
    }
}
