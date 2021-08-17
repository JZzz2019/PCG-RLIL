using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class TestAgent : Agent
{
    //Reinforcement Learning Cycle: Observation, Decision, Action, Reward and loop
    //Max steps in the inspector defines the number of episodes can be ran
    //There can be multiple training be done at the same time with many agents
    //However, I must ensure that they are set to local position, not global position
    //Otherwise, it will go to global position
    //Hyperparameters are parameters that can be adjusted 

    //Heuristics refers to manual training by developer
    //Inference refers trained model
    //Default refers to training by the agent itself

    [SerializeField] private Transform targetTransform;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer floorMeshRenderer;
   
    //determines the starting position of the agent when a new episode arrives
    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-3f, +1f), 0, Random.Range(-1f, +3f));
        targetTransform.localPosition = new Vector3(Random.Range(-3.2f, +3.2f), 0, Random.Range(-3f, +2.5f));
    }
    //observation of the environment
    public override void CollectObservations(VectorSensor sensor) 
    {
        sensor.AddObservation(transform.localPosition); //Inputs x,y,z
        sensor.AddObservation(targetTransform.localPosition); //Target Inputs x,y,z: Total of 6 space in observation
    }

    //actions to take regarding the observation
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        float moveSpeed = 3f;
        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
    }

    //actions to be done by player-developer
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }
    //Reward given to actions taken by agent
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Goal>(out Goal goal))
        {
            //Reward
            //Doesnt matter if there is one, only matters when there are other rewards
            SetReward(1f);
            floorMeshRenderer.material = winMaterial;
            EndEpisode();
        }
        if (other.TryGetComponent<Wall>(out Wall wall))
        {
            //Penalty
            SetReward(-1f);
            floorMeshRenderer.material = loseMaterial;
            EndEpisode();
        }
        
    }
}
