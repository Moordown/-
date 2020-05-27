using UnityEngine;
using UnityEngine.AI;

public class MoveToPosition : MonoBehaviour
{
    public float knockbackTime = 1;
    public float kick = 1.8f;
    
    private Transform goal;
    private NavMeshAgent agent;
    private bool hit;
    private ContactPoint contact;
    private float timer;
    private Rigidbody _rigidbody;
    private NavMeshAgent _navMeshAgent;

    void Start()
    {
        _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        
        goal = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        //Set timer to the same a knockback in first instance.
        timer = knockbackTime;
    }

    void Update()
    {
        if (hit)
        {
            //Allow physics to be applied.
            _rigidbody.isKinematic = false;
            //Stop our AI navigation.
            _navMeshAgent.isStopped = true;
            //Push back our enemy with an impulse force set via the kick value.
            _rigidbody.AddForceAtPosition(Camera.main.transform.forward * kick, contact.point, ForceMode.Impulse);
            hit = false;
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
            //After being knocked back, restart movement after X seconds.
            if (knockbackTime < timer)
            {
                _rigidbody.isKinematic = true;
                _navMeshAgent.isStopped = false;
                agent.SetDestination(goal.position);
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        //We compare the tag in the other object to the tag name we set earlier.
        if (other.transform.CompareTag("bullet"))
        {
            contact = other.contacts[0];
            hit = true;
        }
    }
}