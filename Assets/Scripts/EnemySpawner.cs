using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Enemy SpawnPoint
/// </summary>
public class EnemySpawner : MonoBehaviour
{
	[Header("Prefab")]
	public Enemy m_prefab_enemy;

	[Header("Parameter")]
	public float m_spawn_interval = 2;
	public bool is_active = false;

	//------------------------------------------------------------------------------

	public void StartRunning()
	{
		StartCoroutine(MainCoroutine());
	}

	private IEnumerator MainCoroutine()
	{
		while (true)
		{
			//spawn enemy
			if (m_prefab_enemy && is_active == true)
			{
				Enemy enemy = Instantiate(m_prefab_enemy, transform.parent);
				enemy.transform.position = transform.position;
				is_active = false;
			}

            //yield return new WaitForSeconds(m_spawn_interval);
            yield return null;
        }
	}

	//------------------------------------------------------------------------------

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, 1.0f);
	}
}
