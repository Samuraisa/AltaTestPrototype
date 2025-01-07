using System;
using Alta.Gameplay;
using Gameplay;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameFlow : MonoBehaviour
{
    private enum State
    {
        None,
        MainMenu,
        Gameplay,
        Win,
        Lost
    }
    
    private enum SubState
    {
        None,
        ReadyForShot,
        ChargingShot,
        Shot,
        MovePlayer,
    }
    
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private UIView uiView;
    [SerializeField] private GameplaySettings settings;
    [SerializeField] private GameplayData gameplayData;
    [SerializeField] private Player player;
    [SerializeField] private Shot shot;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private HitIndicator hitIndicator;

    private State _state;
    private SubState _subState;
    private InputAction _shotChargeInputAction;

    private State CurrentState
    {
        get => _state;
        set
        {
            _state = value;
#if UNITY_EDITOR && DEBUG_FSM
            Debug.Log($"State changed to {_state}");
#endif
        }
    }
    
    private SubState CurrentSubState
    {
        get => _subState;
        set
        {
            _subState = value;
            Debug.Log($"SubState changed to {_subState}");
        }
    }

    private void Start()
    {
        _shotChargeInputAction = playerInput.actions
            .FindActionMap(InputConsts.InputMapShot)
            .FindAction(InputConsts.ActionChargeShot);
        
        CurrentState = State.MainMenu;
        uiView.ShowMainMenu();
    }


    [UsedImplicitly]
    public void StartGame()
    {
        gameplayData.ShotDirectionNormalized = (targetTransform.position - player.transform.position).WithY(0f).normalized;
        CurrentState = State.Gameplay;
        CurrentSubState = SubState.ReadyForShot;
        uiView.ShowGameplayUI();
    }

    private void Update()
    {
        if (CurrentState != State.Gameplay)
            return;

        switch (CurrentSubState)
        {
            case SubState.ReadyForShot:
                UpdateReadyForShot();
                break;
            case SubState.ChargingShot:
                UpdateChargingShot();
                break;
            case SubState.Shot:
                break;
            case SubState.MovePlayer:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void UpdateReadyForShot()
    {
        if (_shotChargeInputAction.WasPressedThisFrame())
        {
            shot.gameObject.SetActive(true);
            shot.Radius = settings.ShotMinRadius;
            shot.transform.position = GetShotInitialPosition();
            CurrentSubState = SubState.ChargingShot;
        }
    }

    private Vector3 GetShotInitialPosition()
    {
        var shotPosition = player.transform.position +
                           gameplayData.ShotDirectionNormalized * (player.Radius + shot.Radius);
        return shotPosition.WithY(shot.Radius);
    }

    private void UpdateChargingShot()
    {
        throw new NotImplementedException();
    }
}