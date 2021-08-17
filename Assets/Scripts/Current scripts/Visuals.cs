using UnityEngine;
using GeneratorClass;

public class Visuals
{
    private TileGenerationV2 _TGV2;
    private GameObject Frame;
    private GameObject SelectedFrame;
    public Visuals(TileGenerationV2 TGV2, GameObject Frame, GameObject SelectedFrame)
    {
        _TGV2 = TGV2;
        this.Frame = Frame;
        this.SelectedFrame = SelectedFrame;
    }
    public void SelectTile(int x, int y)
    {
        if (Frame.activeSelf != true && SelectedFrame.activeSelf != true)
        {
            Frame.SetActive(true);
            SelectedFrame.SetActive(true);
        }
        Vector3 pos = new Vector3(x, y, -0.1f);
        Frame.transform.position = pos;
        Vector3 posAhead = new Vector3(pos.x, pos.y + 1, -0.1f);
        if (posAhead.y == _TGV2.GridY)
        {
            if (posAhead.x == _TGV2.GridX - 1)
            {
                posAhead.x = 0;
                posAhead.y = 0;
            }
            else
            {
                posAhead.x += 1;
                posAhead.y = 0;
            }
        }
        SelectedFrame.transform.position = posAhead;
    }

    
}
