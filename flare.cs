using System.Collections;
using UnityEngine;

public class flare : MonoBehaviour {
    private ThrowFlare throwFlare;
    public float duration;
    private ParticleSystem whiteFlame;
    private ParticleSystem sparksEffect;
    public ParticleSystem sparksEffect2;
    private ParticleSystem smokeEffect;
    private ParticleSystem redSmokeEffect;
    public ParticleSystem effectArea;
    private Light redLight;
    private float redLightmaxIntensity;
    private float redLightActualIntensity;
    private FieldOfView fieldOfView;
    // F

    IEnumerator intensityVariation()
    {
        Debug.Log("Start intensityVariations");
        redLightActualIntensity = redLightmaxIntensity;
        float intensityToApply;
        int i = 0;
        while(true)
        {
            if(i%2 == 0)
                intensityToApply = Random.Range(redLightActualIntensity - 10, redLightActualIntensity-8);
            else intensityToApply = Random.Range(redLightActualIntensity + 10, redLightActualIntensity + 8);
            redLight.intensity = intensityToApply;
            yield return new WaitForSeconds(0.05f);
        }
        
        
    }

    IEnumerator ParticleManagement()
    {
        StartCoroutine("intensityVariation");
        float endSessionDuration = 3;
        float endSparkleDuration = 3;
        float timeBeforEndSession = duration - endSessionDuration;
        yield return new WaitForSeconds(timeBeforEndSession);

        float timeElapsed = 0;
        float lerpParameter = timeElapsed / timeBeforEndSession;

        var mainWhiteFlame = whiteFlame.main;
        var mainSparksEffect = sparksEffect.main;
        var emissionSmokeEffect = smokeEffect.emission;
        var mainRedSmoke = redSmokeEffect.main;
        float sparkMax = mainSparksEffect.startLifetime.constantMax;
        
        while (timeElapsed <= endSessionDuration)
        {
            yield return new WaitForSeconds(0.01f);

            redLightActualIntensity = Mathf.Lerp(redLightmaxIntensity, redLightmaxIntensity/1.5f, lerpParameter);
            mainWhiteFlame.startLifetime = Mathf.Lerp(mainWhiteFlame.startLifetime.constant,0.2f, lerpParameter);
            mainRedSmoke.startLifetime = Mathf.Lerp(mainRedSmoke.startLifetime.constant, 0, lerpParameter);

            float sparkMin = Mathf.Lerp(mainSparksEffect.startLifetime.constantMin, 0.1f, lerpParameter);
            sparkMax = Mathf.Lerp(mainSparksEffect.startLifetime.constantMax, 0.3f, lerpParameter);
            mainSparksEffect.startLifetime = new ParticleSystem.MinMaxCurve(sparkMin, sparkMax);

            timeElapsed += 0.01f;
            lerpParameter = timeElapsed / timeBeforEndSession;
            //Debug.Log(timeElapsed);
        }
        //Debug.Log("FIN PHASE 1");

        fieldOfView.ViewAngle = 0;
        redLightActualIntensity = 0;
        effectArea.Stop();
        sparksEffect2.Stop();
        emissionSmokeEffect.rateOverTime = 0;
        whiteFlame.Stop();
        timeElapsed = 0;
        Debug.Log("STOPFLARE");
        AkSoundEngine.PostEvent("stop_flare", this.gameObject);
        fieldOfView.ViewAngle = 0;

        while (timeElapsed <= endSparkleDuration)
        {
            yield return new WaitForSeconds(0.01f);

            //redLightActualIntensity = Mathf.Lerp(redLightmaxIntensity/1.5f,0, lerpParameter);

            float sparkMin = mainSparksEffect.startLifetime.constantMin - 0.01f;
            sparkMax = mainSparksEffect.startLifetime.constantMax -0.01f;
            if (sparkMax < 0.05f) sparkMax = 0;
            if (sparkMin < 0.05f) sparkMin = 0;
            mainSparksEffect.startLifetime = new ParticleSystem.MinMaxCurve(sparkMin, sparkMax);

            timeElapsed += 0.01f;
            lerpParameter = timeElapsed / endSparkleDuration;
        }
       
        StopAllCoroutines();
    }

    void Start () {
        AkSoundEngine.PostEvent("play_flare", this.gameObject);
        whiteFlame = transform.Find("WhiteFlame").GetComponent<ParticleSystem>();
        sparksEffect = transform.Find("SparksEffect").GetComponent<ParticleSystem>();
        smokeEffect = transform.Find("SmokeEffect").GetComponent<ParticleSystem>();
        redSmokeEffect = transform.Find("RedSmoke").GetComponent<ParticleSystem>();
        redLight = GetComponent<Light>();
        redLightmaxIntensity= redLight.intensity;
        fieldOfView = GetComponent<FieldOfView>();
        StartCoroutine("ParticleManagement");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
