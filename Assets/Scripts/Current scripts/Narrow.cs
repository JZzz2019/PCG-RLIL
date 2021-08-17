using UnityEngine;
using UnityEngine.UI;
using GeneratorClass;

public class Narrow
{
    private TileGenerationV2 TGV2;
    private GameObject o;

    private Reward reward;
    private Visuals visuals;
    public Narrow(TileGenerationV2 TGV2, GameObject Frame, GameObject SelectedFrame)
    {
        this.TGV2 = TGV2;
        reward = new Reward(TGV2);
        visuals = new Visuals(TGV2, Frame, SelectedFrame);
    }

    public void NarrowSelection(int actionToTake)
    {
        if (TGV2.CurrentCount <= TGV2.GridX * TGV2.GridY)
        {
            for (int x = TGV2.CountX; x <= TGV2.CountX; x++)
            {
                for (int y = TGV2.CountY; y <= TGV2.CountY; y++)
                {
                    //Debug.Log(GridData[x, y]);
                    //GameObject o = GridData[x, y];
                    o = TGV2.Grid[x, y];
                    visuals.SelectTile(x, y);
                    switch (actionToTake)
                    {
                        //Negative reward for changing to the same tile type
                        case 0: //ignore tile
                            reward.PositiveReward(actionToTake);
                            break;
                        case 1: //change to Player  
                            TGV2.ChangeTile(o, actionToTake, x, y);
                            if (o.name == "Player") { reward.NegativeReward(actionToTake); }
                            else { reward.PositiveReward(actionToTake); }
                            break;
                        case 2: //change to Key
                            TGV2.ChangeTile(o, actionToTake, x, y);
                            if (o.name == "Key") { reward.NegativeReward(actionToTake); }
                            else { reward.PositiveReward(actionToTake); }
                            break;
                        case 3: //change to Door
                            TGV2.ChangeTile(o, actionToTake, x, y);
                            if (o.name == "Door") { reward.NegativeReward(actionToTake); }
                            else { reward.PositiveReward(actionToTake); }
                            break;
                        case 4: //change to Wall
                            TGV2.ChangeTile(o, actionToTake, x, y);
                            if (o.name == "Wall") { reward.NegativeReward(actionToTake); }
                            else { reward.PositiveReward(actionToTake); }
                            break;
                        case 5: //change to Ground
                            TGV2.ChangeTile(o, actionToTake, x, y);
                            if (o.name == "Ground") { reward.NegativeReward(actionToTake); }
                            else { reward.PositiveReward(actionToTake); }
                            break;
                    }
                }
            }
            if (TGV2.CountY < TGV2.GridY - 1)
            {
                TGV2.CountY++;
            }
            else
            {
                TGV2.CountY = 0;
                TGV2.CountX++;
            }

            TGV2.CurrentCount++;
        }

    }
}




