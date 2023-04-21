using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaController : MonoBehaviour
{
    [SerializeField] private Material mat;

    private static readonly int turb = Shader.PropertyToID("_Turbulence");

    [SerializeField] private float duration;
    [SerializeField] private float lerpDuration;
    [SerializeField] private float multiplier;
    private Vector2 prv;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SwapLava());
        StartCoroutine(LerpDirection());
    }

    IEnumerator SwapLava()
    {
        while (true)
        {
            yield return new WaitForSeconds(duration);
            StartCoroutine(LerpDirection());
        }
    }

    private IEnumerator LerpDirection()
    {
        print("Changing Lava turbulence");
        Vector2 r = Random.insideUnitCircle;
        r.x = Mathf.Abs(r.x *multiplier);
        r.y = Mathf.Abs(r.y *multiplier);
        float t = 0;
        while (t  < lerpDuration)
        {
            t += Time.deltaTime;
            mat.SetVector(turb,  Vector2.Lerp(prv, r, t/lerpDuration));
            yield return null;
        }

        prv = r;


    }

}

