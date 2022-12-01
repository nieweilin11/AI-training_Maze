using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class control :Agent
{
    private const float MAX_DISTANCE = 28.28427f;
    public float MoveSpeed = 5.0f;
    public float RotateSpeed = 10.0f;
    public GameObject player;
    public Transform TargetTransform;
    public int goal = 0;
    public int hitWall = 0;


    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(0, 0.5f, 0);
        float xPosition = UnityEngine.Random.Range(-4, 4);
        float zPosition = UnityEngine.Random.Range(-4, 4);
        TargetTransform.transform.localPosition = new Vector3(xPosition, 0, zPosition);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // The position of the agent
        sensor.AddObservation(transform.localPosition.x);
        sensor.AddObservation(transform.localPosition.z);

        // The position of the enemy prefab
        sensor.AddObservation(TargetTransform.localPosition.x);
        sensor.AddObservation(TargetTransform.localPosition.z);

        //sensor.AddObservation(Vector3.Distance(TargetTransform.localPosition, transform.localPosition));
    }


    public override void OnActionReceived(ActionBuffers actions)
    {
        var actionTaken = actions.ContinuousActions;

        float actionSpeed = actionTaken[0];//(actionTaken[0] + 1) / 2; // [0, +1]
        float actionSteering = actionTaken[1]; // [-1, +1]

        transform.Translate(actionSpeed * Vector3.forward * MoveSpeed * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Euler(new Vector3(0, actionSteering * 180, 0));

        float distance_scaled = Vector3.Distance(TargetTransform.localPosition, transform.localPosition) / MAX_DISTANCE;

        AddReward(-distance_scaled / 10); // [0, 0.1]
    }

    public void OnCollisionEnter(Collision collision){
            if (collision.collider.tag == "coin")
            {
            AddReward(10);
            goal++;
            EndEpisode();

             }
            else if (collision.gameObject.tag == "wall")
            {
            AddReward(-1);
            hitWall++;
            player.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    /*    void Update()
        {
            if (this.transform.up.y > 0 && this.transform.up.y <= 10)
                if (Input.GetKey(KeyCode.W))
                {

                    if (MoveSpeed <= 30)
                    {
                        MoveSpeed = MoveSpeed + 10 * Time.deltaTime;
                    }
                    this.transform.Translate(Vector3.forward * Time.deltaTime * MoveSpeed);

                    if (Input.GetKey(KeyCode.A))
                    {
                        this.transform.Rotate(Vector3.up * Time.deltaTime * -RotateSpeed);
                    }
                    else if (Input.GetKey(KeyCode.D))
                    {
                        this.transform.Rotate(Vector3.up * Time.deltaTime * RotateSpeed);
                    }

                }
                else if (Input.GetKey(KeyCode.S))
                {

                    MoveSpeed = 15f;

                    this.transform.Translate(Vector3.forward * Time.deltaTime * -MoveSpeed);
                    if (Input.GetKey(KeyCode.A))
                    {
                        this.transform.Rotate(Vector3.up * Time.deltaTime * -RotateSpeed);
                    }
                    else if (Input.GetKey(KeyCode.D))
                    {
                        this.transform.Rotate(Vector3.up * Time.deltaTime * RotateSpeed);
                    }
                }

                else if (Input.GetKey(KeyCode.A))
                {
                    print("left");
                    float MoveSpeed = 15f;
                    this.transform.Translate(Vector3.left * Time.deltaTime * MoveSpeed);
                  this.transform.Rotate(Vector3.up * Time.deltaTime * -RotateSpeed);
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    MoveSpeed = 15f;
                    this.transform.Translate(Vector3.right * Time.deltaTime  * MoveSpeed);
                    this.transform.Rotate(Vector3.up * Time.deltaTime * RotateSpeed);
                }


        }*/

}
