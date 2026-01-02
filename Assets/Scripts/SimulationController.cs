using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SimulationController : MonoBehaviour
{
    //* Base Vars
    [Header("Base Vars")]
    [HideInInspector] public Animator animator;
    [SerializeField] AnimationClip simClipLooped;
    [SerializeField] AnimationClip simClipNonLooped;
    [SerializeField] AnimatorOverrideController[] overrideController;

    // Sim Prams
    [Header("Simulation Prams")]
    [Range(0.1f, 4f)] public float simSpeed = 1f;
    public bool isPaused;
    public bool isLooped = true;

    // Camers
    [Header("Camera Views")]
    public Camera[] simulationCameras;
    public int currentCameraIndex = 0;

    [Header("Other Scripts")]
    [SerializeField] private UIHandeler uiHandeler;

    // Constents
    public const string STATE_NAME = "Main Simulation";

    private void Start()
    {
        animator.runtimeAnimatorController = overrideController[0];
    }

    //* Simulation Controls
    public void PauseSimulation()
    {
        isPaused = true;
        animator.speed = 0;
    }
    public void PlaySimulation()
    {
        isPaused = false;
        simSpeed = 1;
        animator.speed = simSpeed;
    }
    public void ChangeSimulationSpeed(float speed)
    {
        isPaused = false;
        simSpeed = speed;
        animator.speed = simSpeed;
    }
    public void ChangeLoopStatus()
    {
        if (simClipLooped == null || simClipNonLooped == null)
        {
            Debug.LogWarning("Missing one or both simulation clips!");
            return;
        }

        var info = animator.GetCurrentAnimatorStateInfo(0);
        float currentNormalizedTime = info.normalizedTime % 1f; // keep it between 0–1

        isLooped = !isLooped;
        animator.runtimeAnimatorController = isLooped ? overrideController[0] : overrideController[1];
        animator.Play(STATE_NAME, 0, currentNormalizedTime);
    }

    //* Camera Selection
    //For setting camera view based on index
    public void SetCameraView(int camIndex)
    {
        foreach (var cam in simulationCameras)
        {
            cam.gameObject.SetActive(false);
        }
        simulationCameras[camIndex].gameObject.SetActive(true);
        currentCameraIndex = camIndex;
        uiHandeler.ActivateCameraBtnSprite();

        if (simulationCameras[currentCameraIndex].gameObject.tag == "FreeMovementCamera")
            uiHandeler.freeMovementCameraBtnsPanal.SetActive(true);
        else
            uiHandeler.freeMovementCameraBtnsPanal.SetActive(false);
    }
    //For cycling through cameras
    public void CycleCameraView(int direction)
    {
        direction = (currentCameraIndex + direction) < 0 ? simulationCameras.Length - 1 : direction;

        simulationCameras[currentCameraIndex].gameObject.SetActive(false);
        currentCameraIndex = (currentCameraIndex + direction) % simulationCameras.Length;
        simulationCameras[currentCameraIndex].gameObject.SetActive(true);
        uiHandeler.ActivateCameraBtnSprite();

        if (simulationCameras[currentCameraIndex].gameObject.tag == "FreeMovementCamera")
            uiHandeler.freeMovementCameraBtnsPanal.SetActive(true);
        else
            uiHandeler.freeMovementCameraBtnsPanal.SetActive(false);
    }
}