using UnityEngine;
using System.Collections;

public class GhostAnimation : MonoBehaviour 
{
    public bool isAnimated = false;

    public bool isRotating = false;
    public bool isFloating = false;
    public bool isScaling = false;

    public Vector3 rotationAngle;
    public float rotationSpeed;

    public float floatSpeed;
    public float floatAmplitude; // Add this for the width of the floating movement
    private Vector3 initialPosition;

    public Vector3 startScale;
    public Vector3 endScale;

    private bool scalingUp = true;
    public float scaleSpeed;
    public float scaleRate;
    private float scaleTimer;

    private void Start() 
    {
        initialPosition = transform.position; // Store the initial position
    }

    private void Update() 
    {
        if(isAnimated)
        {
            if(isRotating)
            {
                transform.Rotate(rotationAngle * rotationSpeed * Time.deltaTime);
            }

            if(isFloating)
            {
                float newX = initialPosition.x + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
                transform.position = new Vector3(newX, transform.position.y, transform.position.z);
            }

            if(isScaling)
            {
                scaleTimer += Time.deltaTime;

                if (scalingUp)
                {
                    transform.localScale = Vector3.Lerp(transform.localScale, endScale, scaleSpeed * Time.deltaTime);
                }
                else if (!scalingUp)
                {
                    transform.localScale = Vector3.Lerp(transform.localScale, startScale, scaleSpeed * Time.deltaTime);
                }

                if(scaleTimer >= scaleRate)
                {
                    scalingUp = !scalingUp;
                    scaleTimer = 0;
                }
            }
        }
    }
}
