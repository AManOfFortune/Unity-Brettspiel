using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float roationSpeed;
    public float zoomToDiceArenaSpeed;
    public Vector3 arenaCameraPosition = new (11.5f, 18.5f, 8.5f);
    public GameObject DiceArena;
    public GameObject childCamera;

    private Vector3 regularCenterPosition;
    private Vector3 regularCameraPosition;

    private void Start()
    {
        regularCenterPosition = transform.position;
        regularCameraPosition = childCamera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal") * -1;

        transform.Rotate(Vector3.up, horizontalInput * roationSpeed * Time.deltaTime);
    }

    public void MoveToDiceArena()
    {
        StartCoroutine(MoveToPosition(gameObject, DiceArena.transform.position, zoomToDiceArenaSpeed)); // Move rotaton center to arena
        StartCoroutine(MoveToPosition(childCamera, arenaCameraPosition, zoomToDiceArenaSpeed, true)); // "Zoom" camera in at the same time to have a better view of said arena
    }

    public void ResetPosition()
    {
        StartCoroutine(ResetPositionWithDelay(1f));
    }

    private IEnumerator ResetPositionWithDelay(float waitForSeconds)
    {
        yield return new WaitForSeconds(waitForSeconds);

        StartCoroutine(MoveToPosition(gameObject, regularCenterPosition, zoomToDiceArenaSpeed)); // Move rotaton center to regular spot
        StartCoroutine(MoveToPosition(childCamera, regularCameraPosition, zoomToDiceArenaSpeed, true)); // "Zoom" camera back to regular spot
    }

    public IEnumerator MoveToPosition(GameObject objectToMove, Vector3 position, float timeToGetThere, bool useLocalCoords = false)
    {
        if (useLocalCoords)
        {
            var currentPos = objectToMove.transform.localPosition;
            var t = 0f;

            while (t < 1)
            {
                t += Time.deltaTime / timeToGetThere;
                objectToMove.transform.localPosition = Vector3.Lerp(currentPos, position, t);
                yield return null;
            }
        }
        else
        {
            var currentPos = objectToMove.transform.position;
            var t = 0f;

            while (t < 1)
            {
                t += Time.deltaTime / timeToGetThere;
                objectToMove.transform.position = Vector3.Lerp(currentPos, position, t);
                yield return null;
            }
        }
    }
}
