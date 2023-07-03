using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Stage main loop
/// </summary>
public class StageLoop : MonoBehaviour
{
	#region static 
	static public StageLoop Instance { get; private set; }
	#endregion

	//
	public TitleLoop m_title_loop;

	[Header("Layout")]
	public Transform m_stage_transform;
	public Text m_stage_score_text;
	public Text m_stage_time_text;
    public GameObject m_stage_dir_image;
	public Slider m_health_slider;
	public Slider m_special_slider;

    [Header("Prefab")]
	public Player m_prefab_player;
	public EnemySpawner m_prefab_enemy_spawner;

    public int m_spawn_interval = 2;
    public int timelimit;
	public int direction_interval;
	//
	int m_game_score = 0;
	float time=0;
	float temp_spawn=0;
	float temp_interval = 0;
	int player_health;
    Player player;
    List<EnemySpawner> spawners = new List<EnemySpawner>();
    //------------------------------------------------------------------------------

    #region loop
    public void StartStageLoop()
	{
		StartCoroutine(StageCoroutine());
	}

	/// <summary>
	/// stage loop
	/// </summary>
	private IEnumerator StageCoroutine()
	{
		Debug.Log("Start StageCoroutine");
        SetupStage();
		while (true)
		{
			time -= Time.deltaTime;
            temp_spawn -= Time.deltaTime;
			temp_interval -= Time.deltaTime;
            if (temp_spawn <= 0.0)
			{
				int spawner_choice = Random.Range(0, 4);
                //Debug.Log(spawner_choice);
                spawners[spawner_choice].GetComponent<EnemySpawner>().is_active = true;
				Debug.Log(spawners[spawner_choice].transform.position);
				temp_spawn = m_spawn_interval;

            }
			if(temp_interval <= 0.0)
			{
				player.GetComponent<Player>().player_dir = Random.Range(0, 4);
				temp_interval = direction_interval;

            }
			RefreshDirection();
            RefreshTime();
            player_health = player.GetComponent<Player>().max_hp;
            RefreshSlider();
            if (Input.GetKeyDown(KeyCode.Escape) || time <= 0.0|| player_health <= 0)
			{
				player.GetComponent<Player>().max_hp = 0;
				yield return new WaitForSeconds(2);
                //exit stage
                CleanupStage();
				m_title_loop.StartTitleLoop();
				yield break;
			}
			yield return null;
		}
	}
    #endregion

	void SetupStage()
	{
		Instance = this;

		m_game_score = 0;
        time = timelimit;
        temp_spawn = m_spawn_interval;
        temp_interval = direction_interval;
        spawners = new List<EnemySpawner>();
        RefreshScore();
		RefreshTime();

		//create player
		{
			player = Instantiate(m_prefab_player, m_stage_transform);
			if (player)
			{
				player.transform.position = new Vector3(0, 0, 0);
				player.StartRunning();
			}
		}
        m_health_slider.maxValue = player.max_hp;
        m_special_slider.maxValue = 100;
        //create enemy spawner
        {
			{
				EnemySpawner spawner = Instantiate(m_prefab_enemy_spawner, m_stage_transform);
				if (spawner)
				{
					spawner.transform.position = new Vector3(-4, 4, 0);
					spawner.StartRunning();
					spawners.Add(spawner);
					
				}
			}
			{
				EnemySpawner spawner = Instantiate(m_prefab_enemy_spawner, m_stage_transform);
				if (spawner)
				{
					spawner.transform.position = new Vector3(4, 4, 0);
					spawner.StartRunning();
                    spawners.Add(spawner);
                }
			}
            {
                EnemySpawner spawner = Instantiate(m_prefab_enemy_spawner, m_stage_transform);
                if (spawner)
                {
                    spawner.transform.position = new Vector3(-4, -4, 0);
                    spawner.StartRunning();
                    spawners.Add(spawner);
                }
            }
            {
                EnemySpawner spawner = Instantiate(m_prefab_enemy_spawner, m_stage_transform);
                if (spawner)
                {
                    spawner.transform.position = new Vector3(4, -4, 0);
                    spawner.StartRunning();
                    spawners.Add(spawner);
                }
            }
        }
	}

	void CleanupStage()
	{
		//delete all object in Stage
		{
			for (var n = 0; n < m_stage_transform.childCount; ++n)
			{
				Transform temp = m_stage_transform.GetChild(n);
				GameObject.Destroy(temp.gameObject);
			}
		}

		Instance = null;
	}

	//------------------------------------------------------------------------------

	public void AddScore(int a_value)
	{
		m_game_score += a_value;
		RefreshScore();
	}

	void RefreshScore()
	{
		if (m_stage_score_text)
		{
			m_stage_score_text.text = $"Score {m_game_score:00000}";
		}
	}
	void RefreshTime()
	{
        if (m_stage_time_text)
		{
            m_stage_time_text.text = $"Time Left {(int)time}";

        }
    }
	void RefreshSlider()
	{
		if (m_health_slider)
		{
			m_health_slider.value = player_health;
		}
		if (m_special_slider)
		{
			m_special_slider.value = player.recharge;
        }
	}
    void RefreshDirection()
    {
        if (m_stage_dir_image)
        {
			float angle = 0;
            switch (player.GetComponent<Player>().player_dir){
				case 0: 
					angle = 90;
				break;
				case 1:
					angle = 270;
				break;
				case 2: 
					angle = 0;
				break;
				case 3:
					angle = 180;
				break;
			}
            m_stage_dir_image.transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, angle);

        }

    }

}
