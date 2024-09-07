using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class LightingLevel : MonoBehaviour
{
    public List<Material> materials;
    public Color targetColor;
    public GameObject sunlight;
    public List<Light> lights;
    public float pointLightStrengh = 1.0f;
    public float pointLightExponent = 3.0f;
    public float changeSpeed = 10.0f;

    public Color sunColor = Color.white;

    void Start()
    {
        //lights = new List<Light>();
        lights = GameObject.FindObjectsOfType<Light>().ToList<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        targetColor = Color.black;
        Physics.Raycast(new Ray(transform.position, -sunlight.transform.forward), out RaycastHit hitinfo, 50.0f);
        if (hitinfo.collider == null) targetColor = sunColor; 

        foreach (Light light in lights) {
            if (!light.enabled) continue;
            if (light.type != UnityEngine.LightType.Point) continue;

            //Physics.Raycast(transform.position, -(transform.position - light.transform.position).normalized, out hitinfo, light.range);
            

            Physics.Linecast(transform.position, light.transform.position, out hitinfo);
            if (hitinfo.collider == null)
            {
                float lightDistance = (light.transform.position - transform.position).magnitude;
                float lightDistanceNormalizedInverted = Mathf.Clamp01( (light.range - lightDistance) / light.range );
                float lightStrenghtPowered = Mathf.Pow(lightDistanceNormalizedInverted, pointLightExponent);
                targetColor += light.color * Mathf.Clamp01(light.intensity * lightStrenghtPowered) * pointLightStrengh;
            }
        }

        foreach (Material mat in materials)
        {
            mat.color = Color.Lerp(mat.color, targetColor, Time.deltaTime * changeSpeed);
        }

    }
}
