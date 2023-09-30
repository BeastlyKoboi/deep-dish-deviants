using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;


public class Controller : MonoBehaviour
{
    [SerializeField]
    float speed = 5f;

    public Camera cam;
    static float height;
    float width;
    public Rigidbody2D playerRigidBody;
    Vector3 vehiclePosition = new Vector3(0, 0, 0);


    Vector3 direction = new Vector3(0, 0, 0);
    Vector3 velocity = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        vehiclePosition = transform.position;
        height = 2f * cam.orthographicSize;
        width = height * cam.aspect;
       
    }
    void Awake()
    {
        playerRigidBody = gameObject.GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        direction = Quaternion.Euler(0, 0, 1 * Time.deltaTime) * direction;

        velocity = direction * speed * Time.deltaTime;

        vehiclePosition += velocity;
        //transform.position = vehiclePosition;
        transform.rotation = Quaternion.identity;

        // top and bottom are screen wrapping, while left and right are hard walls
        if (vehiclePosition.x <= cam.transform.position.x - width / 2)
        {
            vehiclePosition.x = cam.transform.position.x - width / 2;
        }

        if (vehiclePosition.x >= cam.transform.position.x + width / 2)
        {
            vehiclePosition.x = cam.transform.position.x + width / 2;
        }

        if (vehiclePosition.y < cam.transform.position.y - height / 2)
        {
            vehiclePosition.y = cam.transform.position.y + height / 2;
        }

        if (vehiclePosition.y > cam.transform.position.y + height / 2)
        {
            vehiclePosition.y = cam.transform.position.y - height / 2;
        }
    }
    private void FixedUpdate()
    {
        playerRigidBody.MovePosition(transform.position + direction * speed * Time.deltaTime);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();

        //transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
    }

    public void OnFire(InputAction.CallbackContext context)
    {
       GetComponent<Player>().isInteracting= true;
    }


}