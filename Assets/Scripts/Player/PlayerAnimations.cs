using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    #region COMPONENTS
    [Header("Components")]
    [SerializeField]
    private Animator goAnim;
    [SerializeField]
    private Animator gfxAnim;
    [SerializeField]
    private SpriteRenderer gfx;
    #endregion

    [Space(10)]
    [Header("Particle Systems")]
    [SerializeField]
    private ParticleSystem runningParticles;
    [SerializeField]
    private ParticleSystem jumpingParticle;

    public void SetBool(string booleanName, bool result, bool goAnimation, bool gfxAnimation)
    {
        if (!goAnimation && !gfxAnimation)
            return;

        if (goAnimation) goAnim.SetBool(booleanName, result);
        if (gfxAnimation) gfxAnim.SetBool(booleanName, result);
    }

    public void TriggerPlayerIdle() 
    {
        SetTrigger("IdleTick", false, true);
    }

    public void SetTrigger(string triggerName, bool goAnimation, bool gfxAnimation)
    {
        if (!goAnimation && !gfxAnimation)
            return;

        if (goAnimation) goAnim.SetTrigger(triggerName);
        if (gfxAnimation) gfxAnim.SetTrigger(triggerName);
    }

    public void TurnPlayer(bool isFacingRight)
    {
        float newRotY = (isFacingRight) ? 180f : 0f;
        Vector3 newRotation = new Vector3(transform.rotation.x, newRotY, transform.rotation.z);
        transform.rotation = Quaternion.Euler(newRotation);
    }

    #region FEEDBACKS

    public void ToggleRunParticles(bool result)
    {
        if (runningParticles == null)
            return;

        if (result && !runningParticles.isEmitting)
            runningParticles.Play();
        else if (!result && !runningParticles.isStopped)
            runningParticles.Stop();
    }

    public void CallLandingFeedback()
    {
        jumpingParticle.Play();
    }

    public void CallJumpFeedback()
    {
        jumpingParticle.Play();
    }

    public void SetGFXRotationX(float angle)
    {
        gfx.transform.localEulerAngles = new Vector3(angle, gfx.transform.localEulerAngles.y, gfx.transform.localEulerAngles.z);
    }

    public void SetGFXRotationY(float angle)
    {
        gfx.transform.localEulerAngles = new Vector3(gfx.transform.localEulerAngles.x, angle, gfx.transform.localEulerAngles.z);
    }

    public void SetGFXRotationZ(float angle)
    {
        gfx.transform.localEulerAngles = new Vector3(gfx.transform.localEulerAngles.x, gfx.transform.localEulerAngles.y, angle);
    }
    #endregion

    #region GETTERS

    public SpriteRenderer GetGfx()
    {
        return gfx;
    }

    #endregion
}
