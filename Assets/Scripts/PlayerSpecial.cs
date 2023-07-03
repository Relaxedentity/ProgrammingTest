using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpecial : PlayerBullet
{
    public float m_rotation_speed = 200;

    public override void Update()
    {
        transform.position += dir * m_move_speed * Time.deltaTime;
        transform.rotation *= Quaternion.AngleAxis(m_rotation_speed * Time.deltaTime, new Vector3(1, 1, 0));

        m_life_time -= Time.deltaTime;
        if (m_life_time <= 0)
        {
            DeleteObject();
        }
    }
}
