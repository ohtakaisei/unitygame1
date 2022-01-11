using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//Textを使えるように追加
using UnityEngine.SceneManagement;//SceneManagerを使えるように追加

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    BoardManager boardManager;

    public bool playerTurn = true;
    public bool enemyTurn = false;

    public int Level = 1;

    private bool doingSetup;
    public Text levelText;
    public GameObject levelImage;


    public int foodPoint = 100;//初期food

    private List<Enemy> enemies;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        enemies = new List<Enemy>();

        boardManager = GetComponent<BoardManager>();
        //Map生成
        InitGame();

    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]//ゲームが開始されたら一度だけ必ず呼ばれる
    static public void Call()//ロード時に呼ばれる関数を登録する
    {
        SceneManager.sceneLoaded += OnSceneLoded;//登録
    }


    static private void OnSceneLoded(Scene next,LoadSceneMode a)//ロード時に毎回呼ばれる関数
    {
        instance.Level++;//レベルの数値を上げる
        instance.InitGame();//Mapの生成関数を呼ぶ
    }

    public void InitGame()
    {
        doingSetup = true;//ロード中の判定をtrueに

        levelImage = GameObject.Find("LevelImage");//LevelImageをヒエラルキーから取得し、変数に格納
        levelText = GameObject.Find("LevelText").GetComponent<Text>();//LevelTextをヒエラルキーから探し、Textコンポーネントを取得し、変数に格納
        levelText.text = "Day:" + Level;//ロード時に表示されるTextの数値をLevelにする

        levelImage.SetActive(true);//ロード時の黒い画面を表示する

        Invoke("HideLevelImage", 2f);//2秒後に関数を呼ぶ

        enemies.Clear();

        boardManager.SetupSecene(Level);
    }

    public void HideLevelImage()//黒い画面を非表示にする関数
    {
        levelImage.SetActive(false);//levelImageを非表示に

        doingSetup = false;//ロード中の判定をfalseに
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerTurn || enemyTurn || doingSetup)//条件を追加
        {
            return;
        }

        //エネミー関数
        StartCoroutine(MoveEnemies());
    }

    public void AddEnemy(Enemy script)
    {
        enemies.Add(script);
    }
    public void DestroyEnemyToList(Enemy script)
    {
        enemies.Remove(script);
    }

    IEnumerator MoveEnemies()
    {
        enemyTurn = true;

        yield return new WaitForSeconds(0.1f);

        if(enemies.Count == 0)
        {
            yield return new WaitForSeconds(0.1f);
        }

        for(int i = 0; i < enemies.Count; i++)
        {
            enemies[i].MoveEnemy();

            yield return new WaitForSeconds(0.1f);
        }
        
        enemyTurn = false;
        playerTurn = true;
    }


    public void GameOver()
    {
        levelText.text = "GAME OVER!";
        levelImage.SetActive(true);

        enabled = false;
    }
}