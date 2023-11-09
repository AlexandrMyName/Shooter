using UnityEngine;


public class SpaceShipPoints : MonoBehaviour
{

    public Vector3 _initialPosition = Vector3.zero;
    public Vector3 _endPosition = Vector3.zero;

    public Quaternion _initialRotation = Quaternion.identity;
    public Quaternion _endRotation = Quaternion.identity;



    [field:SerializeField] public bool ShipIsOut { get;set; }
}
