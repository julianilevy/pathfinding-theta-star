using UnityEngine;

public class Boss : MonoBehaviour
{
    public float speed = 5f;

    private float _shoulderMultiplier = 0.5f;
    private float _maxAvoidObstacleTime = 0.25f;
    private float _avoidObstacleTimer;
    private bool _obstacleDetected;

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
        if (Physics.Raycast(transform.position - transform.right * _shoulderMultiplier, transform.forward, out hit, 2))
        {
            if (hit.collider.gameObject.layer == K.LAYER_OBSTACLE)
            {
                _avoidObstacleTimer = 0f;
                _obstacleDetected = true;
            }
        }
        if (Physics.Raycast(transform.position + transform.right * _shoulderMultiplier, transform.forward, out hit, 2))
        {
            if (hit.collider.gameObject.layer == K.LAYER_OBSTACLE)
            {
                _avoidObstacleTimer = 0f;
                _obstacleDetected = true;
            }
        }

        Debug.DrawRay(transform.position + transform.right * _shoulderMultiplier, transform.forward * 2, Color.red);
        Debug.DrawRay(transform.position - transform.right * _shoulderMultiplier, transform.forward * 2, Color.red);
    }

    public void Move(Grid grid)
    {
        Node farthestNode = null;

        if (grid.path.Count > 0)
        {
            for (int i = grid.path.Count - 1; i > 0; i--)
            {
                RaycastHit hit;
                if (Physics.Raycast(grid.path[i].worldPosition, transform.position - grid.path[i].worldPosition, out hit, Mathf.Infinity))
                {
                    if (hit.collider.gameObject.layer == K.LAYER_BOSS)
                    {
                        farthestNode = grid.path[i];
                        break;
                    }
                }
            }
        }

        if (farthestNode != null)
        {
            if (!_obstacleDetected)
                transform.LookAt(farthestNode.worldPosition);
            else
                transform.LookAt(grid.path[0].worldPosition);
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        else
        {
            if (grid.path.Count > 0)
            {
                transform.LookAt(grid.path[0].worldPosition);
                transform.position += transform.forward * speed * Time.deltaTime;
            }
        }

        /*if (farthestNode != null)
            transform.position = Vector3.MoveTowards(transform.position, farthestNode.worldPosition, _speed * Time.deltaTime);
        else
        {
            if (grid.path.Count > 0)
                transform.position = Vector3.MoveTowards(transform.position, grid.path[0].worldPosition, _speed * Time.deltaTime);
        }*/
    }
}