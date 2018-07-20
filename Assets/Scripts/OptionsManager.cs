using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManager : MonoBehaviour
{

    [Header("MAP")]

    public bool allowWrapEastWest = true; /* forma de tubo, cuando la camara se mueve a la derecha, aparecerá por la izquierda & viceversa*/
    public bool allowWrapNorthSouth = false; /* forma de tubo, cuando la camara se mueve abajo , aparecerá por arriba & viceversa*/

    public int numberContinents = 2;
    [Tooltip("Noise in World Gen")]
    public float noiseResolution = 0.1f;/*resolucion del ruido de generacionde continentes // variable para el perlinNoise*/
    public float noiseScale = 3f; /*escala del ruido de generacionde continentes*/
    [Tooltip("Ice Zone")]

    public float percentBottomPole = 0.1f;
    public float percentTopPole = 0.9f;

    public bool activateCoordCells = true;

}
