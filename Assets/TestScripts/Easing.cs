using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Easing 
{
    public enum Type
    {
        linear,
        easeIn,
        easeOut,
        easeInOut,
        sin,
        sinIn,
        sinOut
    }

    static public float Ease(float u, Type eType, float eMod = 2)
    {
        float u2 = u;
        switch (eType)
        {
            case Type.linear:
                u2 = u;
                break;
            case Type.easeIn:
                u2 = Mathf.Pow(u, eMod);
                break;
            case Type.easeOut:
                u2 = 1 - Mathf.Pow(1 - u, eMod);
                break;
            case Type.easeInOut:
                if (u <= 0.5f)
                {
                    u2 = 0.5f * Mathf.Pow(u * 2, eMod);
                }
                else
                {
                    u2 = 0.5f + 0.5f * (1 - Mathf.Pow(1 - (2 * (u - 0.5f)), eMod));
                }
                break;
            case Type.sin:
                //Попробуйте ввести в поле eMod значения 0.15f и -0.2f для Esing.Type.sin
                u2 = u + eMod * Mathf.Sin(2 * Mathf.PI * u);
                break;
            case Type.sinIn:
                //Для SinIn значение eMod игнорируется
                u2 = Mathf.Sin(u * Mathf.PI * 0.5f);
                break;
            case Type.sinOut:
                //Для SinOut значение eMod игнорируется
                u2 = Mathf.Sin(u * Mathf.PI * 0.5f);
                break;
        }

        return (u2);

    }
}
