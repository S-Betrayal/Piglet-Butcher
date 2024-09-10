using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject playerMesh;
    Animator animator;
    CharacterController characterController;

    [HideInInspector] public bool cursorVisible = false;
    [HideInInspector] public bool cameraCanMove = true;
    [HideInInspector] public bool invertCamera = false;
    public float mouseSensitivity = 1.0f;
    public float maxLookAngle = 85f;
    public GameObject cameraCrane;
    float pitch, yaw = 0;

    public float walkSpeed = 1.65f;
    public float runSpeed = 3.2f;
    public float crouchSpeed = 0.8f;
    float turningSpeed = 30f;

    public int equipedWeapon = 0;
    public List<GameObject> weaponModels = new List<GameObject>();
    public LayerMask attackLayerMask;

    private Vector3 targetForward; //Это целевое значение поворота меша игрока, к которому он будет плавно поворачиваться лерпом
    void Start()
    {
        animator = playerMesh.GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        targetForward = -playerMesh.transform.forward; 
    }

    void Update()
    {
        #region CameraAndMovement
        
        Cursor.visible = cursorVisible;     
        if (cameraCanMove)
        {
            yaw = cameraCrane.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;
            if (!invertCamera) pitch -= mouseSensitivity * Input.GetAxis("Mouse Y"); else pitch += mouseSensitivity * Input.GetAxis("Mouse Y");
            pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);
            cameraCrane.transform.localEulerAngles = new Vector3(pitch, yaw, 0);
        }
        
        
        Vector3 cameraProjection = Vector3.ProjectOnPlane(cameraCrane.transform.forward, Vector3.up).normalized;
        Vector2 inputvector = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
        if (inputvector.magnitude > 1.0f) inputvector.Normalize();
        Vector3 moveVector = inputvector.x * cameraProjection + inputvector.y * cameraCrane.transform.right;
        

        float moveSpeedModifier = walkSpeed;
        if (Input.GetButton("Sprint")) moveSpeedModifier = runSpeed;
        else if (Input.GetButton("Crouch")) moveSpeedModifier = crouchSpeed;

        characterController.Move(moveVector * moveSpeedModifier * Time.deltaTime);
        animator.SetFloat("Velocity", Mathf.Lerp(animator.GetFloat("Velocity"), moveVector.magnitude * moveSpeedModifier, Time.deltaTime * 20.0f));

        if (moveVector.magnitude > 0) targetForward = moveVector;
        playerMesh.transform.forward = -Vector3.Lerp(-playerMesh.transform.forward, targetForward, Time.deltaTime * turningSpeed);
        #endregion CameraAndMovement

        #region Weapons

        if (Input.GetKeyDown(KeyCode.Alpha0)) equipedWeapon = 0;
        if (Input.GetKeyDown(KeyCode.Alpha1)) equipedWeapon = 1;

        if (equipedWeapon == 0)
        {
            foreach (GameObject weaponModel in weaponModels) weaponModel.SetActive(false);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 5.0f));
        }

        if (equipedWeapon == 1)
        {
            foreach (GameObject weaponModel in weaponModels) weaponModel.SetActive(false);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 5.0f));
            weaponModels[0].SetActive(true);

            if (Input.GetButtonDown("Attack"))
            { 
                animator.SetTrigger("Stab");
                targetForward = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
                //-Vector3.Lerp(-playerMesh.transform.forward, moveVector, Time.deltaTime * turningSpeed);
            }
        }
        
        #endregion weapons
    }

    public void Attack()
    {
        Ray ray = new Ray(transform.position+Vector3.up, Camera.main.transform.forward);
        Debug.DrawRay(transform.position+Vector3.up, Camera.main.transform.forward, Color.red, 0.5f);
        if (Physics.Raycast(ray, out RaycastHit hit, 2.0f, attackLayerMask))
        {
            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.gameObject.tag == "Victim")
            {
                Victim victim = hit.collider.gameObject.GetComponent<Victim>();
                victim.GetHit(60.0f);
            }
        }
        
    }


}
