using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameHelper : MonoBehaviour
{
    const float startTime = 10.0f, frostAmount = 0.5f;

    public GameObject LVLPanel;
    public Camera UICamera;
    public Button StartBtn;
    public List<string> Figyres = new List<string>();

    Text textTime, textScore;
    Image helpImage;
    float currentTime = 10.0f;
    float currentScore = 0.0f;
    bool startGame = false;
    string currentFigyre = "";

    public void StartGame()
    {
        Debug.Log("StartGame");

        startGame = true;
        LVLPanel.gameObject.SetActive(true);
        InvokeRepeating("GameTimer", 0.0f, 0.1f);

        currentScore = 0.0f;
        textScore.text = "0.0";
        currentTime = startTime;
        GenerationFigyre();
    }

    public void EndGame()
    {
        Debug.Log("EndGame");
        CancelInvoke("GameTimer");
        startGame = false;
        StartBtn.gameObject.SetActive(true);
        LVLPanel.gameObject.SetActive(false);
        UICamera.GetComponent<FrostEffect>().FrostAmount = 0.0f;
        gameObject.GetComponent<DrawPointHelper>().ClearPoints();
        gameObject.GetComponent<DrawPointHelper>().enabled = false;
    }

    void GenerationFigyre()
    {
        currentFigyre = Figyres[Random.Range(0, Figyres.Count - 1)];
        Debug.Log("currentFigyre = " + currentFigyre);

        helpImage.sprite = Resources.Load(currentFigyre, typeof(Sprite)) as Sprite;
    }

    public void ChengeFigyre(Result rs)
    {
        if (rs.Name != currentFigyre)
            return;

        currentTime += rs.Score / 10.0f;
        GenerationFigyre();
        gameObject.GetComponent<DrawPointHelper>().ClearPoints();
        currentScore++;
        textScore.text = currentScore.ToString("F1");
    }

    void GameTimer()
    {
        if (currentTime <= 0.01)
        {
            EndGame();
            return;
        }
        currentTime -= 0.1f;
        textTime.text = currentTime.ToString("F2");
    }

    void Start()
    {
        helpImage = LVLPanel.transform.FindChild("HelpImage").gameObject.GetComponent<Image>();
        textTime = LVLPanel.transform.FindChild("NTime").gameObject.GetComponent<Text>();
        textScore = LVLPanel.transform.FindChild("NScore").gameObject.GetComponent<Text>();
    }

    void Update()
    {
        if (!startGame)
            return;

        UICamera.GetComponent<FrostEffect>().FrostAmount = frostAmount - currentTime / (startTime / frostAmount);
    }
}
