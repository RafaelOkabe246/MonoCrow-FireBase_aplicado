using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TimeManager : Singleton<TimeManager>
{
	private bool isSleeping;
	private bool inSlowMotion;

	private float currentSleepDuration;

	public void CallSlowMotion(float newTimeScale, bool forceSlowMotion) 
	{
		if (inSlowMotion && !forceSlowMotion)
			return;

		inSlowMotion = newTimeScale != 1f;
		Time.timeScale = newTimeScale;
	}

	public void CallSlowMotionWithDelay(float newTimeScale, float delayToStop, bool forceSlowMotion) 
	{
		StartCoroutine(PerformSlowMotionWithDelay(newTimeScale, delayToStop, forceSlowMotion));
	}

	private IEnumerator PerformSlowMotionWithDelay(float newTimeScale, float delayToStop, bool forceSlowMotion) 
	{
		CallSlowMotion(newTimeScale, forceSlowMotion);

		yield return new WaitForSecondsRealtime(delayToStop);

		CallSlowMotion(1f, forceSlowMotion);
	}

	public void CallGradualSlowMotion(float newTimeScale, float slowMotionTime, bool forceSlowMotion) 
	{
		if (inSlowMotion && !forceSlowMotion)
			return;

		inSlowMotion = newTimeScale != 1f;
		float oldTimeScale = Time.timeScale;

		DOTween.To(x => Time.timeScale = x, oldTimeScale, newTimeScale, slowMotionTime);
	}

	public void Sleep(float duration)
	{
		if (currentSleepDuration > duration)
			return;

		StopAllCoroutines();
		StartCoroutine(nameof(PerformSleep), duration);
	}

    private IEnumerator PerformSleep(float duration)
	{
		Time.timeScale = 0;
		isSleeping = true;
		currentSleepDuration = duration;

		yield return new WaitForSecondsRealtime(duration);

		isSleeping = false;
		Time.timeScale = 1;
		currentSleepDuration = 0f;
	}
}
