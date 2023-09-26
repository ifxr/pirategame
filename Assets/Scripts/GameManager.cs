using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public PlayerShip player;
    public PlayerHideout playerHideout;
    public MainUI ui;

    public List<Port> ports;

    public string playerName;
    public Color playerColor;
    public DifficultySetting selectedDifficultySetting;
    public List<DifficultySetting> possibleDifficulties = new List<DifficultySetting>();
    public bool isEndlessMode;

    [SerializeField]
    private float difficulty;
    [SerializeField]
    private int maxMerchants;
    private int maxEnemies;
    private int maxBigEnemies;

    [SerializeField]
    private List<MerchantShip> merchantList = new List<MerchantShip>();
    [SerializeField]
    private MerchantShip merchantPrefab;

    private List<EnemyShip> enemyList = new List<EnemyShip>();
    [SerializeField]
    private EnemyShip enemyPrefab;

    private List<EnemyShip> bigEnemyList = new List<EnemyShip>();
    [SerializeField]
    private EnemyShip bigEnemyPrefab;

    private void Awake()
    {
        //Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        InitializeDifficultySettings();
        playerName = "";
        playerColor = Color.white;
        selectedDifficultySetting = possibleDifficulties[1];

        SceneManager.sceneLoaded += OnSceneLoaded;

        
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Scene_Game")
        {
            player = FindObjectOfType<PlayerShip>();
            playerHideout = FindObjectOfType<PlayerHideout>();
            ui = FindObjectOfType<MainUI>();

            Port[] portArray = FindObjectsOfType<Port>();
            ports = new List<Port>();
            foreach (Port p in portArray)
            {
                ports.Add(p);
            }
            UpdateDifficulty();
            isEndlessMode = false;
        }
        else
        {
            player = null;
            playerHideout = null;
            ui = null;
            ports = null;
        }
    }

    private void Update()
    {
        if (ports != null)
        {
            UpdateDifficulty();
            if (merchantList.Count < maxMerchants)
            {
                MerchantShip newMerchant = SpawnMerchant();
                if (newMerchant != null)
                {
                    merchantList.Add(newMerchant);
                }
            }

            if (enemyList.Count < maxEnemies)
            {
                EnemyShip newEnemy = SpawnEnemy(enemyPrefab);
                if(newEnemy != null)
                {
                    enemyList.Add(newEnemy);
                }
            }

            if (bigEnemyList.Count < maxBigEnemies)
            {
                EnemyShip newEnemy = SpawnEnemy(bigEnemyPrefab);
                if(newEnemy != null)
                {
                    bigEnemyList.Add(newEnemy);
                }
            }
        }
    }
    private void InitializeDifficultySettings()
    {
        DifficultySetting easy = new DifficultySetting("easy", 10000, 1000);
        DifficultySetting medium = new DifficultySetting("medium", 15000, 750);
        DifficultySetting hard = new DifficultySetting("hard", 20000, 500);
        possibleDifficulties.Add(easy);
        possibleDifficulties.Add(medium);
        possibleDifficulties.Add(hard);
    }

    private void UpdateDifficulty()
    {
        difficulty = DifficultyFunction(playerHideout.totalGold + player.gold);
        maxMerchants = 5 + Mathf.RoundToInt(difficulty);
        maxEnemies = 6 + Mathf.RoundToInt(difficulty);
        maxBigEnemies = Mathf.FloorToInt(difficulty / 4);
    }

    private float DifficultyFunction(int gold)
    {
        return gold / selectedDifficultySetting.goldPerDifficulty;
    }

    private MerchantShip SpawnMerchant()
    {
        int startIndex = Random.Range(0, ports.Count);

        Port startPort = ports[startIndex];

        if (startPort.isClearToSpawn())
        {
            MerchantShip newMerchant = Instantiate(merchantPrefab, startPort.transform.position, Quaternion.LookRotation(Vector3.Cross(startPort.transform.position - startPort.transform.parent.position, Vector3.up), Vector3.up));

            return newMerchant;
        }
        else
        {
            return null;
        }
    }

    private EnemyShip SpawnEnemy(EnemyShip prefab)
    {
        Port startPort = ports[Random.Range(0, ports.Count)];
        if (startPort.isClearToSpawn())
        {
            EnemyShip newEnemy = Instantiate(prefab, startPort.transform.position, Quaternion.LookRotation(startPort.transform.position - startPort.transform.parent.position, Vector3.up));
            return newEnemy;
        }
        return null;
    }

    public void MerchantGone(MerchantShip merchant)
    {
        merchantList.Remove(merchant);
    }

    public void EnemyGone(EnemyShip enemy)
    {
        if (enemyList.Contains(enemy))
        {
            enemyList.Remove(enemy);
        }
        else if (bigEnemyList.Contains(enemy))
        {
            bigEnemyList.Remove(enemy);
        }
    }

    public void Win()
    {
        ui.Win();
        isEndlessMode = true;
    }

    public void Lose()
    {
        ui.Lose();
    }

    public class DifficultySetting
    {
        public string name;
        public int goldTarget;
        public int goldPerDifficulty;

        public DifficultySetting(string _name, int _goldTarget, int _goldPerDifficulty)
        {
            name = _name;
            goldTarget = _goldTarget;
            goldPerDifficulty = _goldPerDifficulty;
        }
    }
}
