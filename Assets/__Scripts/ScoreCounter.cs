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
        //�������� ������ �� ������� ������ ScoreCounter
        GameObject scoreGO = GameObject.Find("ScoreCounter");
        //�������� ��������� Text ����� �������� �������
        scoreGT = scoreGO.GetComponent<Text>();
        // ���������� ��������� ����� ����� ������ 
        scoreGT.text = "0";
    }

    public static void AddScore(int score)
    {
        //������������� ����� � ScoreGT  � ����� �����
        int scoreCounter = int.Parse(scoreGT.text);
        //�������� ���� �� ������������� ����������
        scoreCounter += score;
        //������������� ����� ����� ������� � ������ � ������� �� �����
        scoreGT.text = scoreCounter.ToString();
    }

}
