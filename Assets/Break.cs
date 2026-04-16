using UnityEngine;

public class Break : MonoBehaviour
{
    public GameObject intactObject;
    public Transform fragments;

    public float rigidbodyMass = 1f;
    public float forceImpulse = 10f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (fragments == null)
        {
            fragments = transform;
        }

        //call it after 2 seconds
        //Invoke("Explode", 2);
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            print("Boom");
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
        Vector3 origin = transform.position;

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

            Vector3 direction = go.transform.position - origin;

            if (direction.sqrMagnitude > 0.0001f)
                rb.AddForce(direction.normalized * forceImpulse, ForceMode.Impulse);
        }

        //make the intact object disappear
        intactObject.SetActive(false);
    }


}
