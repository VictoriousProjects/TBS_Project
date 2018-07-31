using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    [Header("[--CONTROLS--]")]
    public float moveSpeed= 7.5f;
    public float sprintMoveMultiplierSpeed = 2f;
    [Header("[--MAP--]")]
    [Header("General")]

    public bool allowWrapEastWest = true; /* forma de tubo, cuando la camara se mueve a la derecha, aparecerá por la izquierda & viceversa*/
    public bool allowWrapNorthSouth = false; /* forma de tubo, cuando la camara se mueve abajo , aparecerá por arriba & viceversa*/

    public int map_Rows = 30; //80- 128 mapa enorme en cv - test 66-106
    public int map_Columns = 60;

    public int numberContinents = 2;

    public float mapGlobalTemperature = 26f;

    [Header("Noises")]
    public float noiseResolution = 0.01f;/*resolucion del ruido de generacionde continentes // variable para el perlinNoise*/
    public float noiseScale = 1.5f; /*escala del ruido de generacionde continentes*/
    public float noiseResolutionForest = 0.05f;
    public float noiseScaleForest = 2f;

    [Header("Temperature zones")]

    public float percentBottomPole = 0.1f;
    public float percentBottomCold = 0.3f;
    public float percentBottomHot = 0.4f;
    public float percentTopHot = 0.6f;
    public float percentTopCold = 0.7f;
    public float percentTopPole = 0.9f;

    [Header("Cells Altitude")]
    public float A_Mountain = 1.3f;
    public float A_Hill = 0.8f;
    public float A_Tropical = 0.4f;
    public float A_Plains = 0.1f;
    public float A_Coast = -0.5f;

    [Header("Cells Humidity")]
    public float H_RainForest = 0.0f;
    public float H_Forest = 0.0f;
    public float H_GrassPlains = 0.0f;
    public float H_Plains = 0.0f;

    [Header("Cells Temperature")]
    public float T_tropical = 26.0f;
    public float T_Desert = 35.0f;
    public float T_Ice = -10.0f;
    public float T_Tundre = 15.0f;

    [Header("Probability of gen forests")]
    public float P_ForestMountain = 0.3f;
    public float P_ForestHill = 0.35f;
    public float P_Forest = 0.2f;

    [Header("[--UI--]")]
    public bool activateCoordCells = true;
    

}
