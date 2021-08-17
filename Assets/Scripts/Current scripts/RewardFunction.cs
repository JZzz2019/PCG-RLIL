using UnityEngine;
using GeneratorClass;

public class RewardFunction
{
    private TileGenerationV2 TGV2;
    public RewardFunction(TileGenerationV2 tgv2)
    {
        TGV2 = tgv2;
    }

    //Compare previous number of tile with current number and then normalize it to output a number between -1 and 1
    //this result will be returned and multiplies by the default reward which is 0.25f
    //this makes it dynamic for rewarding the agent 
    public float CalculateReward(int PreValue, int CurValue)
    {
        int MinValue = -1 * (TGV2.GridX * TGV2.GridY);
        int MaxValue = (TGV2.GridX * TGV2.GridY);
        float value = (float)PreValue - (float)CurValue; //-25 and 25 between these range, in extreme scenario, if pre value = 25
        //and cur value = 0, 25-0 = 0, conversely, 0-25 = -25
        //this sets the minimum bound and maximum bound
        
        //Debug.Log(value + " value");
        //Handle exceptions
        if (CurValue == TGV2.TargetNum) //?equals to target
        {
            value = MaxValue;
        }
        else if (CurValue == 0) //?equals to zero // Like previous value 1 - current value 0 = 1, but value of 1 would give positive reward
        {
            value = MinValue;
        }
        else if (CurValue == TGV2.TargetNum + 1) //if not, output else
        {
            //Preventing to output maximum reward value when the current value is 2(target number + 1), but not 1(target number)
            value = MaxValue - (MaxValue/4);
            //Have added one more condition in this exception
            //to check if previous value equals to 1, if yes, make it negative reward
            if (PreValue == 1)
            {
                value = -value;
            }
        }
        else if (PreValue == 0) //Last check to prevent giving reward at the start when previous value is zero
        {
            value = 0f;
        }
        else
        {
            //Check how much tile left to reach to the target number of tile type
            int NumLeftToTar = CurValue - TGV2.TargetNum;
            value = (value / NumLeftToTar);
            value = value * MaxValue;
        }

        //Debug.Log("I am in Calculate Reward");
        //Debug.Log(value + " value");

        int a = 2;
        float b = value - (MinValue);
        float c = MaxValue - (MinValue);
        float d = b / c;
        float result = a * d - 1;
        //Debug.Log(value + " value");
        //Debug.Log(a + " a");
        //Debug.Log(b + " b");
        //Debug.Log(c + " c");
        //Debug.Log(d + " d");
        //Debug.Log(result + " result");
        return result;
    }
}
