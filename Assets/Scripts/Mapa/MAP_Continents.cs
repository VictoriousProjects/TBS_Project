﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAP_Continents : Hexagonal_Map
{
    //public int nContinents = 3;
    override public void GenMap()
    {
        base.GenMap();  // take base the virtual func.
       
        // Random.InitState(0);

        int nContinents = (int)OptionsManager.GetComponent<OptionsManager>().numberContinents; ;
        int continentsSpacing = nColumns / nContinents;

        for (int c = 0; c < nContinents; c++)
        {
            int numRand = Random.Range(4, 8);

            for (int i = 0; i < numRand; i++)
            {
                int range = Random.Range(5, 8);
                int y = Random.Range(range, nRows - range);
                int x = Random.Range(0, 10) - y / 2 + (c * continentsSpacing);

                ElevateArea(x, y, range);
            }

        }
        //noise


        float nResolution = (float)OptionsManager.GetComponent<OptionsManager>().noiseResolution;
        float nScale = (float)OptionsManager.GetComponent<OptionsManager>().noiseScale;
        float mapTemperature = (float)OptionsManager.GetComponent<OptionsManager>().mapGlobalTemperature;

        float percentBottomPole = (float)OptionsManager.GetComponent<OptionsManager>().percentBottomPole;
        float percentBottomCold = (float)OptionsManager.GetComponent<OptionsManager>().percentBottomCold;
        float percentBottomHot = (float)OptionsManager.GetComponent<OptionsManager>().percentBottomHot;
        float percentTopHot = (float)OptionsManager.GetComponent<OptionsManager>().percentTopHot;
        float percentTopCold = (float)OptionsManager.GetComponent<OptionsManager>().percentTopCold;
        float percentTopPole = (float)OptionsManager.GetComponent<OptionsManager>().percentTopPole;

        Vector2 noiseOffset = new Vector2(Random.Range(0f,1f),Random.Range(0f,1f));

        for (int column = 0; column < nColumns; column++)
        {
            for (int row = 0; row < nRows; row++)
            {
                Hexagon h = GetHexagonAt(column, row);
                float temp = (Mathf.PerlinNoise( 
                    ((float)column / Mathf.Max(nRows, nColumns) / nResolution)+ noiseOffset.x,
                    ((float)row /Mathf.Max(nRows, nColumns) / nResolution) + noiseOffset.y) - 0.5f) ;
                h.Altitude += temp * nScale;
            }
        }

        // temperature

        for (int row = 0; row < nRows; row++)
        {
        
            if(row >= nRows * percentTopCold || row <= nRows * percentBottomCold)
            {
                for (int column = 0; column < nColumns; column++)
                {
                    Hexagon h = GetHexagonAt(column, row);
                   
                    h.Temperature = mapTemperature + Random.Range(-mapTemperature*0.5f, -mapTemperature);

                }
            }
            else if (row <= nRows * percentTopHot && row >= nRows * percentBottomHot)
            {
                for (int column = 0; column < nColumns; column++)
                {
                    Hexagon h = GetHexagonAt(column, row);
                    h.Temperature = mapTemperature + Random.Range(mapTemperature * 0.15f, mapTemperature * 0.5f);
                }
            }
            else
            {

                for (int column = 0; column < nColumns; column++)
                {
                    Hexagon h = GetHexagonAt(column, row);
                    h.Temperature = mapTemperature + Random.Range(-mapTemperature * 0.15f, mapTemperature * 0.15f);

                    if(row < nRows * percentBottomPole || row > nRows * percentTopPole)
                    {
                        h.Temperature = Random.Range(-25f, 0f);
                    }
                }

            }
                       
        }


        // Forest
        nResolution = (float)OptionsManager.GetComponent<OptionsManager>().noiseResolutionForest;
        nScale = (float)OptionsManager.GetComponent<OptionsManager>().noiseScaleForest;

        noiseOffset = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));

        for (int column = 0; column < nColumns; column++)
        {
            for (int row = 0; row < nRows; row++)
            {
                Hexagon h = GetHexagonAt(column, row);
                float temp = (Mathf.PerlinNoise(
                    ((float)column / Mathf.Max(nRows, nColumns) / nResolution) + noiseOffset.x,
                    ((float)row / Mathf.Max(nRows, nColumns) / nResolution) + noiseOffset.y) - 0.5f);
                h.Humidify = temp * nScale;
            }
        }

        UpdateHVisual();
    }

    void ElevateArea(int c, int r, int range, float centerAltitude = .8f)
    {

        Hexagon centerH = GetHexagonAt(c, r);
        //centerH.Altitude = 0.5f;


        Hexagon[] areaH = GetHexagonsInRange(centerH, range);

        foreach (Hexagon h in areaH)
        {
          
            h.Altitude = centerAltitude * Mathf.Lerp(1f, 0.25f, Mathf.Pow(Hexagon.Distance(centerH, h) / range, 2f));

        
        }

    }
}
