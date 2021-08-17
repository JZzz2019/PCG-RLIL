using UnityEngine;
using GeneratorClass;

public class Reward
{
    private TileGenerationV2 tgv2;
    public Reward(TileGenerationV2 tgv2)
    {
        this.tgv2 = tgv2;
    }

    //Stage 1 of the reward function
    public void PositiveReward(int caseValue)
    {
        if (caseValue == 0)
        {
            //Promote ignoring tile
            //tgv2.AddReward(0.1f);
            Debug.Log("Reward for ignoring current tile");
        }
        if (caseValue == 1)
        {
            //tgv2.AddReward(0.1f);
            Debug.Log("Player Reward");
        }
        if (caseValue == 2)
        {
            //tgv2.AddReward(0.1f);
            Debug.Log("Key Reward");
        }
        if (caseValue == 3)
        {
            //tgv2.AddReward(0.1f);
            Debug.Log("Door Reward");
        }
        if (caseValue == 4)
        {
            //tgv2.AddReward(0.1f);
            Debug.Log("Wall Reward");
        }
        if (caseValue == 5)
        {
            //tgv2.AddReward(0.1f);
            Debug.Log("Ground Reward");
        }

    }

    public void NegativeReward(int caseValue)
    {
        //Debug.Log("Negative Reward");
        if (caseValue == 0)
        {
            //tgv2.AddReward(-0.1f);
        }
        if (caseValue == 1)
        {
            //tgv2.AddReward(-0.1f);
        }
        if (caseValue == 2)
        {
            //tgv2.AddReward(-0.1f);
        }
        if (caseValue == 3)
        {
            //tgv2.AddReward(-0.1f);
        }
        if (caseValue == 4)
        {
            //tgv2.AddReward(-0.1f);
        }
        if (caseValue == 5)
        {
            //tgv2.AddReward(-0.1f);
        }
    }

}
