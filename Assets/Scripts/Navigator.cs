using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
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

    [SerializeField] HealthSO heatlh;
    [SerializeField] UnityEvent GameOver;

    Transform currentPatrolPoint;
    enemyStates states;
    int gotPosition; 
    Vector3 getPlayerPostion;
    int PatrolPointIndex = 0;
    float visionRadius;
    float timeElapsed;

    Sensor hearing;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hearing = GetComponent<Sensor>();  
        currentPatrolPoint = patrolPoints[0];
        agent.SetDestination(currentPatrolPoint.position);  
    }

    // Update is called once per frame
    void Update()
    {

        Patrol();


        /*switch (states)
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
        }*/
    }

    public void HeardSomething(bool playerIsMakingSound)
    {
        if (playerIsMakingSound)
        {
            Debug.Log("I heard something...");
            states = enemyStates.INVESTIGATE;
        }
        else
            Debug.Log("Not hearing anything");

    }

    public void SawSomething()
    {
        Vector3 guardForward = transform.forward;
        guardForward.y = 0;
        guardForward.Normalize();
        Vector3 lineToTarget = (target.position - transform.position).normalized;
        float dot = Vector3.Dot(guardForward, lineToTarget);

        if(dot > visionRadius)
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, lineToTarget, out hit))
            {
                //if(hit.)
            }
            else
            {
                Debug.Log("Saw the player");
                states = enemyStates.PURSUE;
                GetPlayerPosition();
            }

        }
        else
        {
            Debug.Log("Did not see the player");
            agent.SetDestination(getPlayerPostion); 
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
        if (hearing.playerInSensor)
        {
            agent.isStopped = true;
            Vector3 direction = (target.position - transform.position).normalized;
            direction.y = 0;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1 * Time.deltaTime);
        }
        else
        {
            timeElapsed = Time.deltaTime;

            if (timeElapsed > 5)
            {
                states = enemyStates.PATROL;
            }
        }

    }

    void Pursue()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        agent.SetDestination(target.position);

        if (distance > 0.1f)
        {
            GameOver.Invoke();
        }
    }

}
