using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalResult
{
    public int skillIndex;
    public float bestResult;
    public float winCount;
    public int loop;

    public Node bestNode;

    public FinalResult(int _skillIndex, float _bestResult, float _winCount, int _loop, Node _bestNode)
    {
        skillIndex = _skillIndex;
        bestResult = _bestResult;
        winCount = _winCount;
        loop = _loop;

        bestNode = _bestNode;
    }

    public float GetWinValue()
    {
        return winCount / (float)loop * 1000;
    }

    public float GetValue()
    {
        return bestResult + (winCount / (float)loop * 1000);
    }

    public FinalResult Compare(FinalResult _data)
    {
        if (_data == null)
        {
            Debug.LogError("Final result comparing data is null");
            return this;
        }


        if (GetValue() >= _data.GetValue())
            return this;

        return _data;
    }
}
