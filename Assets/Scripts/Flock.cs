using UnityEngine;

public class Flock : MonoBehaviour
{
    public float minSpeed = 3f;
    public float maxSpeed = 4.5f;
    public float rotationSpeed = 4f;
    public float neighbourDistance = 3f;

    private Vector3 _averageHeading;
    private Vector3 _averagePosition;
    private float _speed;
    private float _shoulderMultiplier = 0.25f;
    private float _maxAvoidObstacleTime = 0.25f;
    private float _avoidObstacleTimer;
    private bool _turning;
    private bool _obstacleDetected;

    void Start ()
    {
        _speed = Random.Range(minSpeed, maxSpeed);
	}
	
	void Update ()
    {
        Move();
	}

    void FixedUpdate()
    {
        DetectObstacle();
        AvoidObstacle();
    }

    void AvoidObstacle()
    {
        if (_obstacleDetected)
        {
            _avoidObstacleTimer += Time.deltaTime;

            if (_avoidObstacleTimer >= _maxAvoidObstacleTime)
            {
                _obstacleDetected = false;
                _avoidObstacleTimer = 0f;
            }
        }
    }

    void DetectObstacle()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position - transform.right * _shoulderMultiplier, transform.forward, out hit, 1))
        {
            if (hit.collider.gameObject.layer == K.LAYER_OBSTACLE)
            {
                _avoidObstacleTimer = 0f;
                _obstacleDetected = true;

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(hit.normal), rotationSpeed * Time.deltaTime);
            }
        }
        if (Physics.Raycast(transform.position + transform.right * _shoulderMultiplier, transform.forward, out hit, 1))
        {
            if (hit.collider.gameObject.layer == K.LAYER_OBSTACLE)
            {
                _avoidObstacleTimer = 0f;
                _obstacleDetected = true;

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(hit.normal), rotationSpeed * Time.deltaTime);
            }
        }
    }

    void Move()
    {
        if (!_obstacleDetected)
        {
            if (Vector3.Distance(transform.position, Vector3.zero) >= GlobalFlock.gridSize)
                _turning = true;
            else
                _turning = false;

            if (_turning)
            {
                var direction = Vector3.zero - transform.position;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);

                _speed = Random.Range(minSpeed, maxSpeed);
            }
            else
            {
                if (Random.Range(0, 1.5f) < 1)
                    Flocking();
            }
        }

        if (_speed >= 7)
            _speed = Random.Range(6, 7);

        transform.Translate(0, 0, _speed * Time.deltaTime);
    }

    void Flocking()
    {
        var vectorCenter = Vector3.zero;
        var vectorAvoid = Vector3.zero;
        var groupSpeed = 0.1f;
        var distance = 0f;
        var groupSize = 0;

        foreach (var boid in GlobalFlock.allBoids)
        {
            if (boid != gameObject)
            {
                distance = Vector3.Distance(boid.transform.position, transform.position);

                if (distance <= neighbourDistance)
                {
                    vectorCenter += boid.transform.position;
                    groupSize++;

                    if (distance < 1)
                        vectorAvoid += transform.position - boid.transform.position;

                    var anotherFlock = boid.GetComponent<Flock>();
                    groupSpeed += anotherFlock._speed;
                }
            }
        }

        if (groupSize > 0)
        {
            vectorCenter = vectorCenter / groupSize + (GlobalFlock.bossPosition - transform.position);
            _speed = groupSpeed / groupSize;

            var direction = (vectorCenter + vectorAvoid) - transform.position;

            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
        }
    }
}