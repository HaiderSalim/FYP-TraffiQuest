using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIHandeler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject hudPanal;

    [Header("Free Movement Camera Btns")]
    public GameObject freeMovementCameraBtnsPanal;
    [SerializeField] private Slider simulationSlider;
    [SerializeField] private Button forwardBtn;
    [SerializeField] private Button backwardBtn;
    [SerializeField] private Button leftBtn;
    [SerializeField] private Button rightBtn;
    public bool forwardHeld;
    public bool backwardHeld;
    public bool leftHeld;
    public bool rightHeld;

    [Header("Camera Selection Btns")]
    [SerializeField] private Image[] cameraImges;
    [SerializeField] private Sprite cameraActiveSprite;
    [SerializeField] private Sprite cameraInactiveSprite;
    public TMP_Text speedBtnText;

    // Other scripts
    [Header("Other Scripts")]
    [SerializeField] private FreeMovementCamera freeMovementCamera;
    [SerializeField] private SimulationController simulationController;

    private bool isDraggingSlider = false;

    private void Start()
    {
        // Add custom event listeners for each button
        AddHoldListener(forwardBtn, () => forwardHeld = true, () => forwardHeld = false);
        AddHoldListener(backwardBtn, () => backwardHeld = true, () => backwardHeld = false);
        AddHoldListener(leftBtn, () => leftHeld = true, () => leftHeld = false);
        AddHoldListener(rightBtn, () => rightHeld = true, () => rightHeld = false);

        ActivateCameraBtnSprite();
    }

    private void Update()
    {
        Inputs();
        UpdateSimulationSlider();
    }

    private void Inputs()
    {
        if (forwardHeld)
            freeMovementCamera.GetVerticalInput(1);
        if (backwardHeld)
            freeMovementCamera.GetVerticalInput(-1);
        if (leftHeld)
            freeMovementCamera.GetHorizontalInput(-1);
        if (rightHeld)
            freeMovementCamera.GetHorizontalInput(1);

        if (!forwardHeld && !backwardHeld)
            freeMovementCamera.GetVerticalInput(0);
        if (!leftHeld && !rightHeld)
            freeMovementCamera.GetHorizontalInput(0);
    }
    private void AddHoldListener(Button button, System.Action onDown, System.Action onUp)
    {
        // Add pointer down & up handlers dynamically
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = button.gameObject.AddComponent<EventTrigger>();

        // On Pointer Down
        var entryDown = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        entryDown.callback.AddListener((_) => onDown());
        trigger.triggers.Add(entryDown);

        // On Pointer Up
        var entryUp = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        entryUp.callback.AddListener((_) => onUp());
        trigger.triggers.Add(entryUp);
    }
    public void OnPointerDown(PointerEventData eventData) { }
    public void OnPointerUp(PointerEventData eventData) { }

    // Camera Btns Methods
    public void ResetCameraBtnSprites()
    {
        foreach (var img in cameraImges)
        {
            img.sprite = cameraInactiveSprite;
        }
    }
    public void ActivateCameraBtnSprite()
    {
        var camIndex = simulationController.currentCameraIndex;
        ResetCameraBtnSprites();
        if (camIndex >= 0 && camIndex < cameraImges.Length)
        {
            cameraImges[camIndex].sprite = cameraActiveSprite;
        }
    }

    // Special Btns Methords
    public void HideOrShowHUD()
    {
        hudPanal.SetActive(!hudPanal.activeSelf);
    }
    private void UpdateSimulationSlider()//updating every frame
    {
        if (isDraggingSlider || simulationController.isLooped == false) return;

        var info = simulationController.animator.GetCurrentAnimatorStateInfo(0);
        Debug.Log(info.normalizedTime % 1f);
        simulationSlider.value = info.normalizedTime % 1f; // keep it between 0–1
    }
    public void SetSimulationSlider()
    {
        if (!isDraggingSlider) return;
        simulationController.PauseSimulation();
        simulationController.animator.Play(SimulationController.STATE_NAME, 0, simulationSlider.value);
    }
    public void DragSimulationSlider()
    {
        isDraggingSlider = true;
        simulationController.PauseSimulation();
    }
    public void ReleaseSimulationSlider()
    {
        if (isDraggingSlider)
        {
            isDraggingSlider = false;
            simulationController.PlaySimulation();
        }
    }

    private void OnDisable()
    {
        // Reset all held states
        forwardHeld = false;
        backwardHeld = false;
        leftHeld = false;
        rightHeld = false;
    }
}
