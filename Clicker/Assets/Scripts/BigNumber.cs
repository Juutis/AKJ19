using System;
using System.Numerics;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BigNumber
{
    public BigInteger value { get; set; } = new(0);
    public BigInteger prevValue { get; set; }

    public BigInteger valueStep { get; set; } = new(123);
    public BigInteger prevValueStep { get; set; }

    private float lerpTime = 0.5f;
    private float lerpStarted = 0.0f;
    private float currentLerp = 0;

    public BigInteger IncrementValue(int stepCount = 1)
    {
        prevValue = new BigInteger(value.ToByteArray());
        var increment = stepCount * valueStep;
        value += increment;

        currentLerp = 0;
        lerpStarted = Time.time;
        return increment;
    }

    public void IncrementStep()
    {
        prevValueStep = valueStep;
        BigInteger multiplier = (BigInteger)(1.1f * 1000f);
        valueStep = (valueStep * multiplier) / 1000;
    }

    public void Increase(int val)
    {
        prevValue = value;
        value += val;
    }

    public void Increase(string val)
    {
        if (BigInteger.TryParse(val, out BigInteger result))
        {
            prevValue = value;
            value += result;
        }
    }

    public void Decrease(string val)
    {
        if (BigInteger.TryParse(val, out BigInteger result))
        {
            prevValue = value;
            value -= result;
        }
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

    public int Compare(string val)
    {
        if (BigInteger.TryParse(val, out BigInteger result))
        {
            if (value > result)
            {
                return 1;
            }
            else if (value < result)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        return 1;
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
