using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformsController : MonoBehaviour
{
    public Rigidbody platformRB;
    public Transform[] platformPositions;
    public float platformSpeed;

    private int actualPosition = 0;
    private int nextPosition = 1;
    public bool moveToTheNext = true;
    public float waitTime;

    // Update is called once per frame
    void Update()
    {
        MovePlatform();
    }

    //movimiento de la plataforma
    void MovePlatform()
    {
        
        //se mueve ala siguiente posicion del arreglo hasta llegar al ultimo
        if (moveToTheNext)
        {
            StopCoroutine(WaitForMove(0));
            platformRB.MovePosition(Vector3.MoveTowards(platformRB.position, platformPositions[nextPosition].position, platformSpeed * Time.deltaTime));
        }

        //hace un conteo de las posiciones que hay y si execeden al arreglo hace un tiempo de espera y regresa ala primera posicion
        if (Vector3.Distance(platformRB.position, platformPositions[nextPosition].position) <= 0)
        {
            StartCoroutine(WaitForMove(waitTime));
            actualPosition = nextPosition;
            nextPosition++;

            if (nextPosition > platformPositions.Length - 1)
            {
                nextPosition = 0;
            }
        }
    
    }
    IEnumerator WaitForMove(float time)
    {
        moveToTheNext = false;
        yield return new WaitForSeconds(time);
        moveToTheNext = true;
    }
}
