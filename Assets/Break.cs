using UnityEngine;
using UnityEngine.InputSystem;

public class Break : MonoBehaviour
{
    public GameObject intactObject;
    public Transform fragments;

    public float rigidbodyMass = 1f;
    public float forceImpulse = 10f;

    private InputAction testAction;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        testAction = InputSystem.actions.FindAction("Jump");

        if (fragments == null)
        {
            fragments = transform;
        }

        //call it after 2 seconds
        //Invoke("Explode", 2);
    }

    public void Update()
    {
        
        if (testAction.IsPressed())
        {
            print("Boom");
            Explode();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 2)
        {
            Explode();
        }
    }

    public void DisableFragments()
    {
        MeshRenderer[] meshRenderers = fragments.GetComponentsInChildren<MeshRenderer>(true);
        
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            MeshRenderer meshRenderer = meshRenderers[i];

            //leave only the intact object
            if(meshRenderer != intactObject)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void Explode()
    {
        MeshRenderer[] meshRenderers = fragments.GetComponentsInChildren<MeshRenderer>(true);
        Rigidbody originRb = GetComponent<Rigidbody>();
        Vector3 origin = transform.position;
        Vector3 originLinearVelocity = originRb.linearVelocity;
        Vector3 originAngularVelocity = originRb.angularVelocity;

        for (int i = 0; i < meshRenderers.Length; i++)
        {
            MeshRenderer meshRenderer = meshRenderers[i];

            if (meshRenderer == null)
                continue;

            GameObject go = meshRenderer.gameObject;

            MeshFilter meshFilter = go.GetComponent<MeshFilter>();
            if (meshFilter == null || meshFilter.sharedMesh == null)
                continue;

            MeshCollider meshCollider = go.GetComponent<MeshCollider>();
            if (meshCollider == null)
                meshCollider = go.AddComponent<MeshCollider>();

            meshCollider.sharedMesh = meshFilter.sharedMesh;
            meshCollider.convex = true;

            Rigidbody rb = go.GetComponent<Rigidbody>();
            if (rb == null)
                rb = go.AddComponent<Rigidbody>();

            rb.mass = rigidbodyMass;
            rb.linearVelocity = originLinearVelocity;
            rb.angularVelocity = originAngularVelocity;

            Vector3 direction = go.transform.position - origin;

            if (direction.sqrMagnitude > 0.0001f)
                rb.AddForce(direction.normalized * forceImpulse, ForceMode.Impulse);
        }

        //make the intact object disappear
        intactObject.SetActive(false);
    }


}
