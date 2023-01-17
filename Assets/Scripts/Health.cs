using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    const float DEFAULT_HP = 100;
    const float SHAKE_MAGNITUDE = 0.7f;
    public float MaxHitpoints { get; private set; } = DEFAULT_HP;
    public float Hitpoints { get; private set; } = DEFAULT_HP;
    public bool Dead { get; private set; } = false;
    public GameObject DeadText;
    void Update()
    {
        Dead = Hitpoints <= 0;
        if (Dead) {
            DeadText.SetActive(true);
        }
    }
    // percent = decimal e.g. 0.5 for 50%
    public void TakePercentageDamage(float percent)
    {
        Hitpoints -= MaxHitpoints * percent;
        damageCameraEffect();
    }

    private void damageCameraEffect()
    {
        Camera camera = GameObject.FindObjectOfType<Camera>();
        var initialPosition = camera.transform.localPosition;
        Coroutine shaker = StartCoroutine(doShake(camera, initialPosition));
        StartCoroutine(waitThenKill(0.25f, shaker));
    }

    private IEnumerator waitThenKill(float seconds, Coroutine coroutine)
    {
        yield return new WaitForSeconds(seconds);
        StopCoroutine(coroutine);
    }

    private IEnumerator doShake(Camera camera, Vector3 initialPosition)
    {
        while (true)
        {
            camera.transform.localPosition = initialPosition + Random.insideUnitSphere * SHAKE_MAGNITUDE;
            yield return new WaitForEndOfFrame();
        }
    }
}
