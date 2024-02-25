using System;
using System.Numerics;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class BigNumber : IComparable
{
    public BigInteger value { get; set; } = new(0);
    public BigInteger prevValue { get; set; } = new(0);

    private float lerpTime = 0.5f;
    private float lerpStarted = 0.0f;
    private float currentLerp = 0;

    public BigNumber()
    {
    }

    public BigNumber(int initialValue)
    {
        value = new(initialValue);
    }

    public BigNumber(BigInteger val)
    {
        value = val;
    }

    public BigNumber(string val)
    {
        if (BigInteger.TryParse(val, out BigInteger result))
        {
            value = result;
        }
    }

    public BigInteger IncrementValue(BigNumber amount, int count = 1)
    {
        prevValue = new BigInteger(value.ToByteArray());
        value += count * amount.value;

        currentLerp = 0;
        lerpStarted = Time.time;
        return count * amount.value;
    }

    public void Increase(int val)
    {
        prevValue = value;
        value += val;
    }

    public void Increase(BigNumber val)
    {
        prevValue = value;
        value += val.value;
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

    public void Multiply(int multiplier)
    {
        prevValue *= value;
        value *= multiplier;
    }

    public static BigNumber Multiply(BigNumber a, float k)
    {
        int scale = 1000;
        int kScaled = (int)(scale * k);
        BigInteger val = new(a.value.ToByteArray());
        val = BigInteger.Divide(val * kScaled, scale);

        return new BigNumber(val);
    }

    public void Set(string val)
    {
        prevValue = value;
        if (BigInteger.TryParse(val, out BigInteger result))
        {
            value = result;
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

    public int CompareTo(object other)
    {
        if (other == null)
        {
            return -1;
        }
        else if (this == other)
        {
            return 0;
        }
        else if (other is string)
        {
            return CompareTo(other as string);
        }
        else if (other is int || other is double || other is float)
        {
            return value.CompareTo(new BigInteger((int)other));
        }
        else if (other is BigInteger)
        {
            return value.CompareTo(other);
        }
        else if (!(other is BigNumber))
        {
            return -1;
        }

        BigNumber otherNum = (BigNumber)other;
        return value.CompareTo(otherNum.value);
    }

    public int CompareTo(string val)
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
