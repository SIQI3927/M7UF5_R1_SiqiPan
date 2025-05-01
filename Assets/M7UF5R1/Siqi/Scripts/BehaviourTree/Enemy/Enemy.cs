using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private GameObject tutorial;
    [SerializeField]
    private float enemyLife;
    [SerializeField]
    private LayerMask playerLayerMask;
    [SerializeField]
    private float detectionRadius = 15f;
    [SerializeField]
    private float patrolSpeed;
    [SerializeField]
    private List<Transform> patrolPoints = new List<Transform>();
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private BehaviourTree tree;

    [SerializeField]
    public bool playerDetected = false;
    [SerializeField]
    private bool lookingForPlayer = false;

    public GameObject target = null;
    private void Awake()
    {
        tree = new BehaviourTree("Enemy");

        Selector rootSelector = new Selector("Root Selector");

        Leaf playerDetection = new Leaf("Player Detection", new Condition(() => playerDetected));

        Leaf chasePlayer = new Leaf("Chase player", new ChaseStrategy(transform, agent, () => target, new Condition(() => enemyLife >= 50f)));

        Sequence chaseSequence = new Sequence("Chase Sequence", 1);
        chaseSequence.AddChild(playerDetection);
        chaseSequence.AddChild(chasePlayer);

        Leaf patrolSystem = new Leaf(
            "Patrol", 
            new PatrolStrategy(
                transform, 
                agent, 
                patrolPoints, 
                patrolSpeed,  
                new Condition(() => playerDetected)),
            0);

        rootSelector.AddChild(chaseSequence);
        rootSelector.AddChild(patrolSystem);

        tree.AddChild(rootSelector);

        DetectPlayerStrategy detector = gameObject.AddComponent<DetectPlayerStrategy>();
        detector.Configure(
            () => target,
            (newTarget) => target = newTarget,
            (detected) => playerDetected = detected,
            detectionRadius, playerLayerMask,
            tutorial
        );
    }
    // Start is called before the first frame update
    void Start()
    {
        SphereCollider sphereDetection = GetComponent<SphereCollider>();
        sphereDetection.radius = detectionRadius;
        sphereDetection.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (tree.Process() == Node.NodeState.SUCCESS)
            tree.Reset();
    }
}
