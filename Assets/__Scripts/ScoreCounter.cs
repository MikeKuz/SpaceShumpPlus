using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    [Header("Set Dynamically")]
    public static Text scoreGT;

    void Start()
    {
        //Получить ссылку на игровой объект ScoreCounter
        GameObject scoreGO = GameObject.Find("ScoreCounter");
        //Получить компонент Text этого игрового объекта
        scoreGT = scoreGO.GetComponent<Text>();
        // Установить начальное число очков равным 
        scoreGT.text = "0";
    }

    public static void AddScore(int score)
    {
        //Преобразовать текст в ScoreGT  в целое число
        int scoreCounter = int.Parse(scoreGT.text);
        //Добавить очки за уничтоженного противника
        scoreCounter += score;
        //Преобразовать число очков обратно в строку и вывести на экран
        scoreGT.text = scoreCounter.ToString();
    }

}
