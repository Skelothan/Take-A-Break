using UnityEngine;

public class KitchenCamera : MonoBehaviour
{

    bool isMoving = false;

    public int currentScene = 0;
    public float cameraTime = 1.0f;

    readonly Vector3[] cameraPositions =
    {
        new Vector3( 1.443f, 2.202f,  0.808f), // Cabinet
        new Vector3(-0.360f, 2.220f, -0.250f), // Sink
        new Vector3(-0.200f, 2.100f,  2.500f) // Table
    };

    readonly Vector3[] cameraRotations =
    {
        new Vector3( 0.00f, 262.881f, 0f), // Cabinet
        new Vector3(47.38f, 242.900f, 0f), // Sink
        new Vector3(44.80f, 252.700f, 0f) // Table
    };

    private float posSpeed;
    private float rotSpeed;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving)
            return;

        float posStep = posSpeed * Time.deltaTime;
        float rotStep = rotSpeed * Time.deltaTime;

        Vector3 targetPosition = cameraPositions[currentScene];
        Vector3 targetRotation = cameraRotations[currentScene];

        if (Vector3.Distance(transform.position, targetPosition) > 0.0001f)
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, posStep);

        if (Vector3.Distance(transform.eulerAngles, targetRotation) > 0.0001f)
            transform.eulerAngles = Vector3.MoveTowards(transform.eulerAngles, targetRotation, rotStep);

        if (Vector3.Distance(transform.position, targetPosition) <= 0.0001f && Vector3.Distance(transform.eulerAngles, targetRotation) <= 0.0001f)
            isMoving = false;

    }

    public void NextScene()
    {
        currentScene++;
        MoveCameraToPosition(currentScene);
    }

    void MoveCameraToPosition(int i)
    {
        if (i >= cameraPositions.Length || i >= cameraRotations.Length)
        {
            return;
        }

        Vector3 targetPosition = cameraPositions[currentScene];
        Vector3 targetRotation = cameraRotations[currentScene];

        float posTransDist = Vector3.Distance(targetPosition, transform.position);
        float rotTransDist = Vector3.Distance(targetRotation, transform.eulerAngles);

        posSpeed = posTransDist/cameraTime;
        rotSpeed = rotTransDist/cameraTime;

        isMoving = true;
    }
}
