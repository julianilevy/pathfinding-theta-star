using UnityEngine;

public class GlobalFlock : MonoBehaviour
{
    public static GameObject[] allBoids;
    public static Vector3 bossPosition;
    public static int gridSize = 32;

    public GameObject boidPrefab;
    public Boss boss;
    public int boidsAmount = 25;

    void Start ()
    {
        allBoids = new GameObject[boidsAmount];

        for (int i = 0; i < boidsAmount; i++)
        {
            var spawnPosition = new Vector3(Random.Range(-gridSize, gridSize), 0f, Random.Range(-gridSize, gridSize));
            allBoids[i] = (GameObject)Instantiate(boidPrefab, spawnPosition, Quaternion.identity);
        }
	}
	
	void Update ()
    {
        bossPosition = boss.transform.position;
    }
}