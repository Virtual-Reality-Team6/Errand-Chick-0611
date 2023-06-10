using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    //게임 매니저가 필요한 변수 세팅
    public GameObject startCam;
    public GameObject gameCam;
    public Player player;

    public Button startButton;
    public Button ContinueButton;
    public bool hasPlaylog;
    
    public string[] errandLists;
    public GameObject[] ErrandListObjects;
    public Text[] errandTexts;
    public Image checkFrontAImg;
    public Image checkFrontBImg;
    public Image checkFrontCImg;
    public Image checkFrontDImg;

    public GameObject stampA;
    public GameObject stampB;
    public GameObject stampC;
    public GameObject stampD;
    public GameObject stampE;
    
    public float playTime;
    private float playTimeLimit = 10 * 60;
    public RectTransform timeBar;

    public GameObject startPanel;
    public GameObject gamePanel;
    public Text playTimeText;
    public Text playerCoinText;

    public Dictionary<ItemType, int> itemPool = new Dictionary<ItemType, int>();

    public int itemCount;

    public bool endShopping = false;

    public int selectionCount = 4;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this);

        checkPlayLog();
        makeErrandList();
    }

    void checkPlayLog()
    {
        if(hasPlaylog){
            Text startButtonText = startButton.GetComponentInChildren<Text>();
            startButtonText.text = "새 게임 시작";
            ContinueButton.gameObject.SetActive(true);
        }
        
    }

    int[] generateRandomNumbers(){
        int[] selectedIndices = {-1, -1, -1, -1};
        bool[] boolMap = new bool[18];

        // 배열을 false로 초기화
        for (int i = 0; i < boolMap.Length; i++)
        {
            boolMap[i] = false;
        }
        
        for(int i=0; i<selectedIndices.Length; i++){
            int randomNum = Random.Range(0, errandLists.Length);
            if(!boolMap[randomNum]){
                selectedIndices[i] = randomNum;
                boolMap[randomNum] = true;
            }
            else i--;
        }
        return selectedIndices;
    }

    void makeErrandList()
    {
        itemCount = 0;
        int[] selectedIndices = generateRandomNumbers();

        for(int i=0; i<ErrandListObjects.Length; i++){
            GameObject AErrandList = ErrandListObjects[i];
            Image[] childImages = AErrandList.GetComponentsInChildren<Image>();

            int randomIndex = selectedIndices[i];
            Debug.Log(randomIndex);
            for (int j = 0; j < childImages.Length; j++){
                childImages[j].gameObject.SetActive(false);
            }
            childImages[randomIndex].gameObject.SetActive(true);

            int buyQuantity = Random.Range(1, 4);
            if(randomIndex >= 14) buyQuantity = 1;

            errandTexts[i].text = errandLists[randomIndex] + " " + buyQuantity.ToString() + " 개";

            itemPool.Add((ItemType)selectedIndices[i], buyQuantity);
            itemCount += buyQuantity;
            Debug.Log((ItemType)selectedIndices[i] + "," + buyQuantity);
        }
    }
    
    public void GameStart()
    {
        var camHandler = FindObjectOfType<CameraHandler>();

        player.isMove = true;

        startCam = camHandler.startCam;
        gameCam = camHandler.gameCam;
        startCam.SetActive(false);
        gameCam.SetActive(true);

        startPanel.SetActive(false);
        gamePanel.SetActive(true);
    }

    void Update()
    {
        
        playTime += Time.deltaTime;
    }

    void LateUpdate()
    {
        playerCoinText.text = string.Format("{0:n0}", player.coin);

        int min = (int)(playTime / 60);
        int second = (int)(playTime % 60);
        playTimeText.text = string.Format("{0:00}", min) + ":" + string.Format("{0:00}", second);

        checkFrontAImg.color = new Color(1,1,1, player.iscompletedErrand[0] ? 1:0);
        checkFrontBImg.color = new Color(1,1,1, player.iscompletedErrand[1] ? 1:0); 
        checkFrontCImg.color = new Color(1,1,1, player.iscompletedErrand[2] ? 1:0); 
        checkFrontDImg.color = new Color(1,1,1, player.iscompletedErrand[3] ? 1:0);   

        timeBar.localScale = new Vector3((playTimeLimit-playTime)/playTimeLimit, 1, 1);

        stampA.SetActive(player.iscompletedStamp[0]);
        stampB.SetActive(player.iscompletedStamp[1]);
        stampC.SetActive(player.iscompletedStamp[2]);
        stampD.SetActive(player.iscompletedStamp[3]);
        stampE.SetActive(player.iscompletedStamp[4]);
    }
}
