using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player Bullet
/// </summary>
public class PlayerBullet : MonoBehaviour
{
	[Header("Parameter")]
	public float m_move_speed = 5;
	public float m_life_time = 2;
	public Vector3 dir;

	//
	public virtual void Update()
	{
		transform.position += dir * m_move_speed * Time.deltaTime;

		m_life_time -= Time.deltaTime;
		if (m_life_time <= 0)
		{
			DeleteObject();
		}
	}

	public void DeleteObject()
	{
		GameObject.Destroy(gameObject);
	}

	public void SetDirection(Vector3 d)
	{
		dir = d;
	}
}
