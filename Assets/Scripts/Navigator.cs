using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public enum enemyStates
{
    PATROL, 
    INVESTIGATE,
    PURSUE
}

public class Navigator : MonoBehaviour
{
    //variables 
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform[] patrolPoints;
    [SerializeField] Transform target;

    [SerializeField] HealthSO health;
    [SerializeField] UnityEvent GameOver;

    PlayerMovement player;

    Transform currentPatrolPoint;
    enemyStates states;
    int gotPosition;
     private bool playerIsSeen;
    Vector3 getPlayerPostion;
    int PatrolPointIndex = 0;
    float visionRadius = 0.8f;
    float timeElapsed;

    Sensor hearing;
    SightSensor sight;

    [SerializeField] LayerMask environmentLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hearing = FindAnyObjectByType<Sensor>();  
        player = FindAnyObjectByType<PlayerMovement>();    
        currentPatrolPoint = patrolPoints[0];
        agent.SetDestination(currentPatrolPoint.position);  
    }

    // Update is called once per frame
    void Update()
    {

        //Patrol();


        switch (states)
        {
            case enemyStates.PATROL:
                Patrol();
                break;
            case enemyStates.INVESTIGATE:
                Investigate();
                break;
            case enemyStates.PURSUE:
                Pursue();
                break;
        }
    }

    public void HeardSomething(bool playerIsMakingSound, bool isMoving, bool onSensor)
    {
        if (playerIsMakingSound && isMoving && onSensor)
        {
            Debug.Log("Actively litsening now");
            states = enemyStates.INVESTIGATE;
        }
        else
            Debug.Log("Player is QUIET or not in HEARING Sensor");

    }

    public void SawSomething(bool onSensor)
    {
        Vector3 guardForward = transform.forward;
        guardForward.y = 0;
        guardForward.Normalize();
        Vector3 lineToTarget = (target.position - transform.position).normalized;
        float dot = Vector3.Dot(guardForward, lineToTarget);
        if (onSensor)
        {
            Debug.Log("Player in SIGHT sensor");

            if (dot > visionRadius)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, lineToTarget, out hit, 100, environmentLayer))
                {
                    Debug.Log("Seeing a wall");
                }
                else
                {
                    Debug.Log("Saw the player");
                    playerIsSeen = true;
                    states = enemyStates.PURSUE;
                    GetPlayerPosition();
                }

            }
            else
            {
                Debug.Log("Player is not seen");
                playerIsSeen = false;
                gotPosition = 0;
            }
        }
        else
        {
            Debug.Log("Player not in SIGHT sensor");
            playerIsSeen = false;
            gotPosition = 0;
        }
    }

    void GetPlayerPosition()
    {
      if(gotPosition == 0)
      {
        getPlayerPostion = target.position;
            gotPosition = 1;
      }
    }

    void Patrol()
    {
        Debug.Log("PATROL STATE");
        float distance = Vector3.Distance(transform.position, currentPatrolPoint.position);
      
        if (distance < 1)
        {
            PatrolPointIndex++;

            if(PatrolPointIndex >= patrolPoints.Length)
            {
                PatrolPointIndex = 0;
            }

            currentPatrolPoint = patrolPoints[PatrolPointIndex];
            agent.SetDestination(currentPatrolPoint.position);


        }
    }

    void Investigate()
    {
        Debug.Log("INVESTIGATE STATE");

        if (hearing.playerInSensor && player.isMakingSound)
        {
            transform.forward = (target.position - transform.position).normalized;

            Debug.Log("Rotating...");
        }
        else
        {
            timeElapsed += Time.deltaTime;

            if (timeElapsed > 5)
            {
                Debug.Log("going to patrol state");
                states = enemyStates.PATROL;
                timeElapsed = 0;
            }
        }

    }

    void Pursue()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        Debug.Log("PURSUE STATE");
        if (playerIsSeen)
        {
            agent.SetDestination(target.position);
            Debug.Log("PURSUING THE PLAYER");

        }
        else
        {
            Debug.Log("LOST PLAYER GOING BACK");
            float distance2 = Vector3.Distance(transform.position, getPlayerPostion);
            agent.SetDestination(getPlayerPostion);

            if (distance2 < 0.1f)
            {
                states = enemyStates.INVESTIGATE;
            }
        }


        if (distance < 1)
        {
            Debug.Log("ELIMINATED THE PLAYER");
            health.currentHealth = 0;
            GameOver.Invoke();
        }
    }
}
