using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Simulates the trajectory of a projectile based on a given force and mass,
/// and visualizes the trajectory using a LineRenderer component. DDetects collisions 
/// with colliders and displays an indicator at the collision point.
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class TrajectorySimulator : MonoBehaviour
{
    private LineRenderer lineRenderer;

    // The maximum number of simulated points
    public int nPoint = 100;
    public float timeStep = 1.0f/24.0f;

    public GameObject indicatorPrefab;
    GameObject indicator;

    private List<Vector3> simPoints;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        simPoints = new List<Vector3>();
        indicator = Instantiate(indicatorPrefab, transform.position, transform.rotation);
        indicator.SetActive(false);
    }

    private float VecSum(Vector3 vec) {
        return vec.x + vec.y + vec.z;
    }


    /// <summary>
    /// Simulation of the projectile trajectory.
    /// </summary>
    /// <param name="force">The force applied to the projectile.</param>
    /// <param name="mass">The mass of the projectile.</param>
    /// <param name="offset">The offset of the projectile's initial position w.r.t the player positions.</param>
    /// <param name="prefabTransform">The transform of the instantiated projectile.</param>
    public void StartSimulate(Vector3 force, float mass, Vector3 offset, Transform prefabTransform) {
        float time = 0.0f;
        simPoints.Clear();
        if (!indicator.activeSelf) {
            indicator.SetActive(true);
        }
 
        Vector3 initialVelocity = (force / mass);

        for (int i = 0; i < nPoint; i++) {
            float dx = time * initialVelocity.x;
            float dy = time * initialVelocity.y - 0.5f * 9.8f * time * time;
            float dz = time * initialVelocity.z;
            Vector3 tmp = new Vector3(dx, dy, dz);// + offset; // In world space
            Vector3 displacement = tmp + transform.TransformPoint(offset);
            simPoints.Add(displacement);

            // Stop simulation when hitting a collider
            Collider[] colliders = Physics.OverlapSphere(displacement, 0.25f);
            colliders = colliders.Where(c => c.gameObject.CompareTag("Projectile") == false).ToArray();
            if (colliders.Length > 0) {
                Collider collider = colliders[0];

                Vector3 collisionPoint;
                if (collider is MeshCollider meshCollider) {
                    collisionPoint = displacement;
                }
                else {
                    collisionPoint = collider.ClosestPoint(displacement);
                }
                Quaternion rotation = Quaternion.FromToRotation(indicatorPrefab.transform.up, collider.transform.up);
                indicator.transform.position = collisionPoint;
                indicator.transform.rotation = rotation;
                break;
            }
            time += timeStep;
        }

        lineRenderer.positionCount = simPoints.ToArray().Length;
        lineRenderer.SetPositions(simPoints.ToArray());
    }
}
