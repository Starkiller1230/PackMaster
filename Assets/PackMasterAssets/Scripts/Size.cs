using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BaggageSize
{
    Size1x1 = 0,
    Size1x2 = 1,
    Size1x3 = 2,
    Size2x2 = 3,
    Size2x3 = 4,
    Size3x3 = 5,
    Size3x4 = 6,
    Size4x4 = 7

}

public enum ItemSize
{
    Size1x1 = 0,
    Size1x2 = 1,
    Size2x1 = 2,
    Size2x2 = 3
}

static class Size
{
    public static (int width, int height) GetSize(ItemSize itemSize) => ParseSize(itemSize.ToString());

    public static (int width, int height) GetSize(BaggageSize baggageSize) => ParseSize(baggageSize.ToString());

    public static int GetCapasity(ItemSize itemSize) => GetCapasity(itemSize.ToString());

    public static int GetCapasity(BaggageSize baggageSize) => GetCapasity(baggageSize.ToString());
    
    private static int GetCapasity(string str)
    {
        (int width, int height) _size = ParseSize(str);
        return _size.width * _size.height;
    }

    private static (int width, int height) ParseSize(string str)
    {
        (int width, int height) result = (0, 0);
        bool _changeY = true;

        for (int i = str.Length - 1; i >= 0; i--)
        {
            int _temp = 0;
            if (int.TryParse(str[i].ToString(), out _temp) == true)
            {
                _ = _changeY == true ? result.width = _temp : result.height = _temp;
                if (_changeY == false)
                    break;
                else
                    _changeY = false;
            }
        }

        return result;
    }

}