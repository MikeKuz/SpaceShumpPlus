using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Interpolator : MonoBehaviour
{
    [Header("Set in Inspector")]
    public Vector3 p0 = new Vector3(0, 0, 0);
    public Vector3 p1 = new Vector3(3, 4, 5);
    public float timeDuration = 1;
    //Установите флажок checkToStart что бы запустить перемещение
    public bool checkToStart = false;

    [Header("Set Dynamically")]
    public Vector3 p01;
    public bool moving = false;
    public float timeStart;


    //Update вызывается в каждом кадре
    void Update()
    {
        if (checkToStart)
        {
            checkToStart = false;

            moving = true;
            timeStart = Time.time;

        }

        if (moving) 
        { 
            float u = (Time.time - timeStart) / timeDuration;
            if(u>=1)
            {
                u = 1;
                moving = false;
            }

            //Стандартная формула линейной интерпояции
            p01 = (1 - u) * p0 + u * p1;
            transform.position = p01;
        }
    }

}
