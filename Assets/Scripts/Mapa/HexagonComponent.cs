﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonComponent : MonoBehaviour
{
    public Hexagon Hex;
    public Hexagonal_Map HexMap;


    public void UpdatePosition()
    {
        this.transform.position = Hex.PositionFCamera(
            Camera.main.transform.position,
             HexMap.nRows,
            HexMap.nColumns,
            HexMap.OptionsManager.GetComponent<OptionsManager>().allowWrapEastWest,
            HexMap.OptionsManager.GetComponent<OptionsManager>().allowWrapNorthSouth
            );
        
    }
  

}
