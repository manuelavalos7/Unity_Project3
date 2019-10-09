using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    private GameObject player;
    private playerMovement playerMove;
    private Vector2 enemyPosition;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMove = player.GetComponent<playerMovement>();
        enemyPosition = transform.position / 0.25f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void moveEnemy() {
        Vector3 change = Vector3.zero;
        int rand = Random.Range(-1, 0);
        if (rand < 0)
        {
            change = bestChange();
        }
        else
        {
            change.x = Random.Range(-1, 1);
            change.y = Random.Range(-1, 2);
            if (change.x != 0)
            {
                change.y = 0;
                change.x = change.x / Mathf.Abs(change.x);//to move one unit in x
            }
            else if (change.y != 0)
            {
                change.y = change.y / Mathf.Abs(change.y);//to move one unit in y
            }
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

    private Vector3 bestChange() {
        
        Vector3 best = Vector3.zero;
        Debug.Log(enemyPosition);
        Debug.Log("PLAYER: " + playerMove.playerPosition);
        Vector3 currentPlayerPos = playerMove.playerPosition;
        for (int i = -1; i < 2; i++) {
            Vector3 tester = new Vector3(i, 0, 0);
            Vector3 test = tester + (Vector3)enemyPosition;
            float newDist = Vector3.Distance(test, currentPlayerPos);
            if (newDist < Vector3.Distance(best+(Vector3)enemyPosition,currentPlayerPos)) {
                best = tester;
            }
            tester = new Vector3(0, i, 0);
            test = (tester + (Vector3)enemyPosition);
            newDist = Vector3.Distance(test,currentPlayerPos);

            if (newDist < Vector3.Distance(best + (Vector3)enemyPosition, currentPlayerPos))
            {
                best = tester;
            }

        }
        
        return best;
    }
}
