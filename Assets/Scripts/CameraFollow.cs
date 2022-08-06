using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Boss boss;

	void LateUpdate ()
    {
        Follow();
	}

    void Follow()
    {
        transform.position = new Vector3(boss.transform.position.x, transform.position.y, boss.transform.position.z);
    }
}