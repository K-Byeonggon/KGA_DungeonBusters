using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float initialPositionZ;
    [SerializeField] float resetPositionZ;

    public void StartMove(float moveDuration)
    {
        StartCoroutine(MoveCorridor(moveDuration));
    }


    private IEnumerator MoveCorridor(float moveDuration)
    {
        float elapsedTime = 0f;

        while(elapsedTime < moveDuration)
        {
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);

            if(transform.position.z <= resetPositionZ)
            {
                Vector3 newPosition = transform.position;
                newPosition.z = initialPositionZ;
                transform.position = newPosition;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
    }
}
