using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ComboScript : MonoBehaviour
{
    [Header("Combo Settings")]
    public float comboResetTime = 2.0f;
    public float heavyAttackCooldownTime = 0.3f;

    private Animator animator;
    private PlayerControls _playerControls;
    public Collider swordCollider;

    private float comboTimer = 0f;
    private float heavyAttackCooldown = 0f;
    private float secondAttackInputBlockTimer = 0f;
    public float secondAttackInputBlockDuration = 0.8f;

    private List<MoveType> currentCombo = new();

    private bool isAttacking = false;
    private bool waitingForSecondAttack = false;
    private bool secondAttackQueued = false;

    private int comboStep = 0;

    public static ComboScript Instance { get; private set; }
    public bool IsAttacking => isAttacking || IsPlayingAttackAnimation();

    private void Awake()
    {
        animator = GetComponent<Animator>();
        Instance = this;

        _playerControls = new PlayerControls();
        _playerControls.Player.LightAttack.performed += ctx => OnLightAttack();
        _playerControls.Player.HeavyAttack.performed += ctx => OnHeavyAttack();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    private void Update()
    {
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

        if (secondAttackInputBlockTimer > 0f)
            secondAttackInputBlockTimer -= Time.deltaTime;

        if (comboTimer > 0f)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0f)
                ResetCombo();
        }

        if (heavyAttackCooldown > 0f)
            heavyAttackCooldown -= Time.deltaTime;

        if (isAttacking && !animator.IsInTransition(0) && currentState.normalizedTime >= 1f)
            isAttacking = false;

        // Combo: wykonanie drugiego ataku
        if (comboStep == 1 && secondAttackQueued && currentState.IsName("lightAttack") && currentState.normalizedTime > 0.5f)
        {
            animator.SetTrigger("lightAttack2");
            isAttacking = true;
            waitingForSecondAttack = false;
            secondAttackQueued = false;
            comboStep = 2;
        }

        // KONIEC animacji – reset z opóźnieniem
        if (currentState.IsName("lightAttack2") && currentState.normalizedTime >= 1f && !animator.IsInTransition(0))
        {
            StartCoroutine(DelayedReset());
        }

        if (currentState.IsName("lightAttack") && currentState.normalizedTime >= 1f && !secondAttackQueued && !animator.IsInTransition(0))
        {
            StartCoroutine(DelayedReset());
        }

        if (currentState.IsName("heavyAttack") && currentState.normalizedTime >= 1f && !animator.IsInTransition(0))
        {
            StartCoroutine(DelayedReset());
        }
    }

    private void OnLightAttack()
    {
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

        if (comboStep == 0 && !isAttacking)
        {
            StartFirstAttack();
        }
        else if (comboStep == 1 && waitingForSecondAttack && !secondAttackQueued && currentState.IsName("lightAttack"))
        {
            secondAttackQueued = true;
        }
    }

    private void OnHeavyAttack()
    {
        if (!isAttacking && heavyAttackCooldown <= 0f)
        {
            StartHeavyAttack();
        }
    }

    private void StartHeavyAttack()
    {
        animator.SetTrigger("heavyAttack");
        currentCombo.Add(MoveType.HeavyAttack);
        comboTimer = comboResetTime;
        isAttacking = true;
        waitingForSecondAttack = false;
        comboStep = 0;
        heavyAttackCooldown = heavyAttackCooldownTime;
        if (swordCollider != null)
            swordCollider.enabled = true;
    }

    private void StartFirstAttack()
    {
        animator.SetTrigger("lightAttack");
        currentCombo.Add(MoveType.LightAttack);
        comboTimer = comboResetTime;
        isAttacking = true;
        waitingForSecondAttack = true;
        comboStep = 1;
        if (swordCollider != null)
            swordCollider.enabled = true;
    }

    private IEnumerator DelayedReset()
    {
        yield return new WaitForSeconds(0.1f); // małe opóźnienie by uniknąć "chodzenia w miejscu"
        ResetCombo();
    }

    private void ResetCombo()
    {
        currentCombo.Clear();
        comboTimer = 0f;
        isAttacking = false;
        waitingForSecondAttack = false;
        secondAttackQueued = false;
        comboStep = 0;

        animator.ResetTrigger("lightAttack");
        animator.ResetTrigger("lightAttack2");
        animator.ResetTrigger("heavyAttack");

        if (swordCollider != null)
            swordCollider.enabled = false;
    }

    public bool IsPlayingAttackAnimation()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        bool isAttackState = state.IsName("lightAttack") || state.IsName("lightAttack2") || state.IsName("heavyAttack");
        return isAttackState && state.normalizedTime < 1f;
    }
}

public enum MoveType
{
    LightAttack,
    HeavyAttack
}
