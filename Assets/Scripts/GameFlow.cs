using System;
using DG.Tweening;
using Extensions;
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
        PerformingShot,
        MovingPlayer,
    }
    
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private UIView uiView;
    [SerializeField] private GameplaySettings settings;
    [SerializeField] private GameplayData gameplayData;
    [SerializeField] private Player player;
    [SerializeField] private Shot shot;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Transform obstaclesParent;
    [SerializeField] private HitIndicator hitIndicator;
    [SerializeField] private CapsuleCollider obstaclePrefabCollider;
    
    private readonly RaycastHit[] _raycastHits = new RaycastHit[1];

    private State _state;
    private SubState _subState;
    private InputAction _shotChargeInputAction;
    private Transform _playerTransform;
    private Transform _shotTransform;
    private float _obstacleRadius;
    private Tween _movementTween;

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
        
        _playerTransform = player.transform;
        _shotTransform = shot.transform;
        _obstacleRadius = obstaclePrefabCollider.radius;
        
        player.Initialize();
        targetTransform.rotation = Quaternion.LookRotation(targetTransform.position.WithY(0f));

        CalculateMinMaxShotPowerModifiers();
        
        CurrentState = State.MainMenu;
        uiView.ShowMainMenu();
    }

    private void OnDestroy()
    {
        _movementTween?.Kill();
    }

    private void CalculateMinMaxShotPowerModifiers()
    {
        // Considering 100 steps for curve testing gives enough precision
        const float step = 0.01f;

        gameplayData.MinShotPowerModifier = float.MaxValue;
        gameplayData.MaxShotPowerModifier = float.MinValue;

        var curve = settings.ExplosionPowerCurve;
        for (var progress = 0f; progress <= 1f; progress += step)
        {
            var powerModifier = shot.GetPowerModifier(progress);

            if (gameplayData.MinShotPowerModifier > powerModifier)
                gameplayData.MinShotPowerModifier = powerModifier;

            if (gameplayData.MaxShotPowerModifier < powerModifier)
                gameplayData.MaxShotPowerModifier = powerModifier;
        }
    }


    [UsedImplicitly]
    public void StartGame()
    {
        obstaclesParent.gameObject.SetActive(true);
        
        player.Radius = settings.PlayerInitialRadius;
        _playerTransform.position = Vector3.zero.WithY(player.Radius);
        
        gameplayData.ShotDirectionNormalized = (targetTransform.position - _playerTransform.position).WithY(0f).normalized;
        CurrentState = State.Gameplay;
        CurrentSubState = SubState.ReadyForShot;
        uiView.ShowGameplayUI();
    }

    public void SetWinState()
    {
        CurrentState = State.Win;
        uiView.ShowWinScreen();
        _movementTween.Kill();
        gameplayData.Reset();
    }
    
    private void SetLostState()
    {
        CurrentState = State.Lost;
        uiView.ShowLoseScreen();
        hitIndicator.gameObject.SetActive(false);
        gameplayData.Reset();
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
            case SubState.PerformingShot:
                break;
            case SubState.MovingPlayer:
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
            _shotTransform.position = GetShotInitialPosition();
            CurrentSubState = SubState.ChargingShot;
            hitIndicator.gameObject.SetActive(true);
        }
    }

    private Vector3 GetShotInitialPosition()
    {
        var shotPosition = _playerTransform.position +
                           gameplayData.ShotDirectionNormalized * (player.Radius + shot.Radius);
        return shotPosition.WithY(shot.Radius);
    }

    private void UpdateChargingShot()
    {
        if (!_shotChargeInputAction.IsPressed())
        {
            PerformShot();
            return;
        }
        
        var shotChargeDelta = settings.ShotChargeRate * Time.deltaTime;

        if (player.ConsumableRadius < shotChargeDelta)
        {
            SetLostState();
            return;
        }

        player.Radius -= shotChargeDelta;
        shot.Radius += shotChargeDelta;

        UpdateShotHitPosition();
        // TODO: Highlight obstacles in explosion range
    }

    private void PerformShot()
    {
        CurrentSubState = SubState.PerformingShot;
        var shotMovementDuration = Vector3.Distance(_shotTransform.position, gameplayData.ShotFinalPosition) /
                                   settings.ShotSpeed;
        _movementTween = _shotTransform.DOMove(gameplayData.ShotFinalPosition, shotMovementDuration)
            .SetEase(Ease.Linear)
            .OnComplete(OnShotReachedDestination);
    }

    private void OnShotReachedDestination()
    {
        hitIndicator.gameObject.SetActive(false);
        shot.gameObject.SetActive(false);

        if (gameplayData.ObstacleWasHit)
            DestroyObstaclesWithExplosion();

        //CurrentSubState = SubState.ReadyForShot;
        SwitchToPlayerMovementSubstate();
    }

    private void SwitchToPlayerMovementSubstate()
    {
        CurrentSubState = SubState.MovingPlayer;
        var startPosition = _playerTransform.position;
        var endPosition = targetTransform.position;
        var obstacleWasHit = TryHitObstacle(startPosition, endPosition, player.Radius);
        
        // If obstacle was hit, we move closer to obstacle with some safe distance
        // otherwise we move directly to the exit
        if (obstacleWasHit) 
        {
            endPosition = _raycastHits[0].point + _raycastHits[0].normal * player.Radius;
            
            // If we are already closer to finish than hit position, then don't move
            if ((targetTransform.position - endPosition).sqrMagnitude >= (targetTransform.position - startPosition).sqrMagnitude)
            {
                StartNextTurn();
                return;
            }

            // 3xRadius: 2 for max possible next shot size + 1 for player's sphere offset from it's own center 
            var desiredDistance = 3f * player.Radius + settings.PlayerExtraSafeDistance;
            var currentDistance = Vector3.Distance(startPosition, endPosition);
            
            // If we are already close enough to the obstacle, we don't need to move either
            if (currentDistance <= desiredDistance)
            {
                StartNextTurn();
                return;
            }
            
            endPosition = startPosition + gameplayData.ShotDirectionNormalized * (currentDistance - desiredDistance);
        }

        var movementDuration = Vector3.Distance(startPosition, endPosition) / settings.PlayerMovementSpeed;
        _movementTween = _playerTransform.DOMove(endPosition, movementDuration)
            .SetUpdate(UpdateType.Fixed) // We expect player hit the exit
            .OnComplete(StartNextTurn);
    }

    private void StartNextTurn()
    {
        _subState = SubState.ReadyForShot;
    }

    private void DestroyObstaclesWithExplosion()
    {
        // Obstacle radius added, because we are not checking intersections of circles here,
        // but checking the distance between centers of explosion and obstacle instead 
        var explosionRadius = shot.GetExplosionRadius(shot.PowerProgress) + _obstacleRadius;
        var explosionRadiusSqr = explosionRadius * explosionRadius;

        var index = obstaclesParent.childCount - 1;
        var explosionEpicenter = gameplayData.ShotFinalPosition.ToVector2XZ();
        while (index >= 0)
        {
            var obstacleTransform = obstaclesParent.GetChild(index);
            if ((obstacleTransform.position.ToVector2XZ() - explosionEpicenter).sqrMagnitude <= explosionRadiusSqr)
                Destroy(obstacleTransform.gameObject);

            index--;
        }
    }

    private void UpdateShotHitPosition()
    {
        var startPosition = _shotTransform.position;
        var endPosition = targetTransform.position.WithY(startPosition.y);

        gameplayData.ObstacleWasHit = TryHitObstacle(startPosition, endPosition, shot.Radius);
        gameplayData.ShotFinalPosition = gameplayData.ObstacleWasHit
                ? _raycastHits[0].point + _raycastHits[0].normal * shot.Radius
                : endPosition;
    }
    
    private bool TryHitObstacle(Vector3 startPosition, Vector3 endPosition, float radius)
    {
        var ray = new Ray(startPosition, endPosition - startPosition);
        return Physics.SphereCastNonAlloc(ray, radius, _raycastHits,
            Vector3.Distance(startPosition, endPosition),
            settings.ObstacleLayerMask) > 0;
    }
}