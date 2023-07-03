using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player Character
/// </summary>
public class Player : MonoBehaviour
{
	[Header("Prefab")]
	public PlayerBullet m_prefab_player_bullet;
	public PlayerBullet m_prefab_player_special;

    [Header("Parameter")]
	public float m_move_speed = 1;
	public int max_hp;
	public AudioSource audio;
	public AudioClip shoot_effect;
    public AudioClip hit_effect;
    public GameObject death_particles;
    //
    public float recharge = 0;
	public int player_dir = 0;
    List<Vector3> directions = new List<Vector3>() 
	{
    new Vector3(-1, 0, 0),
    new Vector3(1, 0, 0),
    new Vector3(0, 1, 0),
    new Vector3(0, -1, 0)
    };

    //------------------------------------------------------------------------------

    public void StartRunning()
	{
        StartCoroutine(MainCoroutine());
	}

    //
    private IEnumerator MainCoroutine()
	{
		while (true)
		{
			if (recharge < 100)
			{
				recharge += 10 * Time.deltaTime;
			}
            if (max_hp == 0)
            {
                GameObject particles = Instantiate(death_particles, transform.position, Quaternion.identity);
				this.gameObject.SetActive(false);
            }
			ScreenWrapping();
            //moving

            {
				if (Input.GetKey(KeyCode.LeftArrow))
				{
					//transform.position += new Vector3(-1, 0, 0) * m_move_speed * Time.deltaTime;
                    GetComponent<Rigidbody>().AddForce(new Vector3(-1, 0, 0)/10 * m_move_speed, ForceMode.Impulse);
                }
				if (Input.GetKey(KeyCode.RightArrow))
				{
					//transform.position += new Vector3(1, 0, 0) * m_move_speed * Time.deltaTime;
                    GetComponent<Rigidbody>().AddForce(new Vector3(1, 0, 0)/10 * m_move_speed, ForceMode.Impulse);
                }
				if (Input.GetKey(KeyCode.UpArrow))
				{
					//transform.position += new Vector3(0, 1, 0) * m_move_speed * Time.deltaTime;
                    GetComponent<Rigidbody>().AddForce(new Vector3(0, 1, 0)/10 * m_move_speed, ForceMode.Impulse);
                }
				if (Input.GetKey(KeyCode.DownArrow))
				{
					//transform.position += new Vector3(0, -1, 0) * m_move_speed * Time.deltaTime;
                    GetComponent<Rigidbody>().AddForce(new Vector3(0, -1, 0)/10 * m_move_speed, ForceMode.Impulse);
                }
			}

			//shoot
			{
				if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X))
				{

					if (Input.GetKeyDown(KeyCode.Z))
					{
                        Shoot(m_prefab_player_bullet);
					}
					if (Input.GetKeyDown(KeyCode.X) && recharge >= 100)
					{
						recharge = 0;
                        Shoot(m_prefab_player_special);
                    }
				}
            }

            yield return null;
		}
	}
    private void DeleteObject()
    {
        GameObject.Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Enemy")
		{
            audio.PlayOneShot(hit_effect, 0.2f);
            max_hp -= 10;
		}
    }
	private void Shoot(PlayerBullet b)
	{
        audio.PlayOneShot(shoot_effect, 0.2f);
        PlayerBullet bullet = Instantiate(b, transform.parent);
        bullet.transform.position = transform.position;
        Camera cam = Camera.main;
        //Vector3 d = (cam.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        Vector3 d = directions[player_dir];
        //d.z = 0;
        d.Normalize();
        bullet.SetDirection(d);
    }
	private void ScreenWrapping()
	{
		Camera cam = Camera.main;
		Vector3 newPosition = cam.WorldToViewportPoint(transform.position);
		Vector3 temp = transform.position;

        if (newPosition.x < -0.1f || newPosition.x > 1.1f)
        {
			temp.x = -temp.x;
        }
        if (newPosition.y < -0.1f || newPosition.y > 1.1f)
        {
            temp.y = -temp.y;
        }
        transform.position = temp;
    }
}
