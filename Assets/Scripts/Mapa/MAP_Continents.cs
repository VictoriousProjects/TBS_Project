using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAP_Continents : Hexagonal_Map
{
    public int nContinents = 3;
    override public void GenMap()
    {
        base.GenMap();  // take base the virtual func.


        Random.InitState(0);


        int continentsSpacing = nColumns/ nContinents;

        for (int c = 0; c < nContinents; c++)
        {
            int numRand = Random.Range(4, 8);
            for (int i = 0; i < numRand; i++)
            {
                int range = Random.Range(5, 8);
                int y = Random.Range(range, nRows - range);
                int x = Random.Range(0, 10) - y / 2 + (c* continentsSpacing);
                Debug.Log(x + "," + y);
                ElevateArea(x, y, range);
            }

        }


        UpdateHVisual();

    }

    void ElevateArea(int c, int r, int range, float centerAltitude = .8f)
    {
        Hexagon centerH = GetHexagonAt(c, r);
        Hexagon[] areaH = GetHexagonsInRange(centerH, range);

        foreach(Hexagon h in areaH)
        {
        
            h.Altitude += centerAltitude * Mathf.Lerp(1f,0.25f,Hexagon.Distance(centerH,h)/range);
        }
    }

}