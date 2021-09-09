using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnProjectile : MonoBehaviour
{
    public GameObject[] Projectiles;
    public Vector2 SpawnDirection = Vector2.up;
    public float AngleRange = 30f;
    public float SpawnForce = 10f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnSingleProjectile() {
        GameObject go = Projectiles[Random.Range(0, Projectiles.Length - 1)];
        float randomAngle = Random.Range(-AngleRange, AngleRange);
        Vector2 projectileDirection = Quaternion.Euler(0, 0, randomAngle) * SpawnDirection;
        GameObject projectileInstance = Instantiate(go, transform.position, Quaternion.identity);
        projectileInstance.name = "ProjectileInstance";
        projectileInstance.GetComponent<Rigidbody2D>().AddForce(projectileDirection * SpawnForce, ForceMode2D.Impulse);
    }

    public void SpawnProjectiles() {
		List<GameObject> projectilesList = new List<GameObject>(Projectiles);
		projectilesList.OrderBy(x => Random.value).ToList();

		float multiplier = -1f;
		foreach (GameObject go in projectilesList) {
			Vector2 projectileDirection = Quaternion.Euler(0, 0, (AngleRange * multiplier)) * SpawnDirection;
            GameObject projectileInstance = Instantiate(go, transform.position, Quaternion.identity);
            projectileInstance.name = "ProjectileInstance";
            projectileInstance.GetComponent<Rigidbody2D>().AddForce(projectileDirection * SpawnForce, ForceMode2D.Impulse);
			multiplier++;
		}
	}
}
