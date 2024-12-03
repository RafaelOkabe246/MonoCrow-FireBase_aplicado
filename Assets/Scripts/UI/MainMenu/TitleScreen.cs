using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using DG.Tweening;

public class TitleScreen : BaseScreen
{
    [Space(20)]
    [SerializeField]
    private PlayerAnimations playerAnim;
    [SerializeField]
    private Image titleImage;
    [SerializeField]
    private Transform camTrans;

    private bool isDisabled;

    protected override void OnEnable()
    {
        base.OnEnable();

        InputSystem.onEvent += OnEvent;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        InputSystem.onEvent -= OnEvent;
    }

    private void OnEvent(InputEventPtr eventPtr, InputDevice device)
    {
        if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>())
            return;
        var controls = device.allControls;
        var buttonPressPoint = InputSystem.settings.defaultButtonPressPoint;
        for (var i = 0; i < controls.Count; ++i)
        {
            var control = controls[i] as ButtonControl;
            if (control == null || control.synthetic || control.noisy)
                continue;
            if (control.ReadValueFromEvent(eventPtr, out var value) && value >= buttonPressPoint)
            {
                //Button Pressed
                CloseTitleScreen();
                break;
            }
        }
    }

    private void CloseTitleScreen()
    {
        if (isDisabled)
            return;

        isDisabled = true;

        GameManager.instance.SetState(GameState.Gameplay);
        GameManager.instance.SetState(GameState.Paused);

        playerAnim.SetTrigger("Fall", false, true);
        CameraController.instance.CameraShake(4f, 4f, 0.5f);

        StartCoroutine(CloseTitleScreenCoroutine());
    }

    private IEnumerator CloseTitleScreenCoroutine() 
    {
        yield return new WaitForSeconds(2f);

        camTrans.DOMove(new Vector3(0f, 0f, -0.25f), 1.5f);
        titleImage.DOColor(new Color(1f, 1f, 1f, 0f), 1.5f);

        yield return new WaitForSeconds(1.5f);

        gameObject.SetActive(false);
    }
}
