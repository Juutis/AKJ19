using System;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BigNumber
{
    public BigInteger value { get; set; } = new(0);
    public BigInteger prevValue { get; set; }

    private BigInteger printBigValue;

    public BigInteger valueStep { get; set; } = new(123);
    public BigInteger prevValueStep { get; set; }

    private float lerpTime = 0.5f;
    private float lerpStarted = 0.0f;
    private float currentLerp = 0;

    public void IncrementValue()
    {
        prevValue = new BigInteger(value.ToByteArray());
        value += valueStep;

        currentLerp = 0;
        lerpStarted = Time.time;
    }
    
    public void IncrementStep ()
    {
        prevValueStep = valueStep;
        BigInteger multiplier = (BigInteger)(1.1f * 1000f);
        valueStep = (valueStep * multiplier) / 1000;
    }

    public string GetUIValue()
    {
        currentLerp = (Time.time - lerpStarted) / lerpTime;

        if (value.ToString("N0").Length < 20)
        {
            return Lerp(prevValue, value, currentLerp).ToString("N0");
        }
        else
        {
            return Lerp(prevValue, value, currentLerp).ToString("E");
        }
    }

    public static BigInteger Lerp(BigInteger start, BigInteger end, float k)
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
