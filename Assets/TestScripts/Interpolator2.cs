using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interpolator2 : MonoBehaviour
{
    [Header("Set in Inspector")]
    public Transform    c0;
    public Transform    c1;
    public float        uMin = 0;
    public float        uMax = 1;
    public float        timeDuration = 10;
    //Установите флажок checkToStart чтобы запустить перемещение
    public bool         checkToStart = false;
    public bool         loopMove = true;
    public Easing.Type  easingType = Easing.Type.linear;
    public float        easingMod = 2;

    [Header("Set Dynamically")]
    public Vector3      p01;
    public Color        c01;
    public Quaternion   r01;
    public Vector3      s01;
    public bool         moving = false;
    public float        timeStart;

    private Material mat, matC0, matC1;

    private void Awake()
    {
        mat = GetComponent<Renderer>().material;
        matC1 = c1.GetComponent<Renderer>().material;
        matC0 = c0.GetComponent<Renderer>().material;
    }


    void Update()
    {
        if(checkToStart)
        {
            checkToStart = false;

            moving = true;
            timeStart = Time.time;
        }

        if(moving)
        {
            float u=(Time.time-timeStart)/timeDuration;
            if(u>=1)
            {
                u = 1;
                if(loopMove)
                {
                    timeStart = Time.time;
                }
                else
                {
                    moving = false;                    
                }
            }
            //Скорректировать значение u, чтобы уместить его в диапазо от uMin до uMax
            u = (1 - u) * uMin + u * uMax;

            //Функция Easing.Ease изменяет u и влияет на характер движения 
            u = Easing.Ease(u, easingType, easingMod);

            //Стандартная формула линйной интерполяции
            p01 = (1 - u) * c0.position + u * c1.position;
            c01 = (1 - u) * matC0.color + u * matC1.color;
            s01 = (1 - u) * c0.localScale + u * c1.localScale;
            //Углы поворота обрабатываются иначе из-за особенностей Quaternion
            r01 = Quaternion.Slerp(c0.rotation, c1.rotation, u);

            //Применить новые значения к Cube01
            transform.position = p01;
            mat.color = c01;
            transform.localScale = s01;
            transform.rotation = r01;
        }
    }
}
