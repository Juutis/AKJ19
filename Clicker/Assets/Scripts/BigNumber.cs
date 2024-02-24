using System;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BigNumber
{
    private float value;
    private BigInteger bigValue;

    private float prevValue;
    private BigInteger prevBigValue;

    private float printValue;
    private BigInteger printBigValue;

    private float adder = 1.00001f;
    private BigInteger bigAdder;

    private const int limit = 1000000;

    private float lerpTime = 0.5f;
    private float lerpStarted = 0.0f;
    private float currentLerp = 0;

    public void IncrementValue()
    {
        if (value < limit)
        {
            prevValue = value;
            value += adder;
        }
        else
        {
            prevBigValue = new BigInteger(bigValue.ToByteArray());
            bigValue += bigAdder;
        }
        currentLerp = 0;
        lerpStarted = Time.time;
    }
    
    public void IncrementStep ()
    {
        if (value < limit)
        {
            adder *= 1.51f;
        }
        else
        {
            BigInteger multiplier = (BigInteger)(1.1f * 1000f);
            bigAdder = (bigAdder * multiplier) / 1000;
        }
    }

    public string GetNumber()
    {
        string number = "";
        currentLerp = (Time.time - lerpStarted) / lerpTime;

        if (value < limit)
        {
            return Mathf.Lerp(prevValue, value, currentLerp).ToString();
        }
        else
        {

            if (bigValue.IsZero)
            {
                bigValue = new BigInteger(value);
                bigAdder = new BigInteger(adder);
            }

            if (bigValue.ToString("N0").Length < 20)
            {
                return Lerp(prevBigValue, bigValue, currentLerp).ToString("N0");
            }
            else
            {
                return Lerp(prevBigValue, bigValue, currentLerp).ToString("E");
            }
        }
    }

    private BigInteger Lerp(BigInteger start, BigInteger end, float k)
    {
        int mult = 10000;

        if (k <= 0)
        {
            return start;
        }
        else if (k >= 1)
        {
            return end;
        }
        else
        {
            int kDiv = (int)((1 / k) * mult);

            BigInteger diff = mult * (end - start);
            BigInteger step = BigInteger.Divide(diff, new BigInteger(kDiv));
            return step + start;
        }
    }
}
