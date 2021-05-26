using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;

public class PlayerMovement : NetworkBehaviour
{
    public NetworkVariableVector3 Position = new NetworkVariableVector3(new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.ServerOnly,
        ReadPermission = NetworkVariablePermission.Everyone
    });

    public NetworkVariable<Color> Colour = new NetworkVariable<Color>(new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.ServerOnly,
        ReadPermission = NetworkVariablePermission.Everyone
    });

    public override void NetworkStart()
    {
        Move();
    }

    public void Move()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            var randomPosition = GetRandomPositionOnPlane();
            transform.position = randomPosition;
            Position.Value = randomPosition;

            var randomColour = GetRandomColor();
            gameObject.GetComponent<Renderer>().material.SetColor("_Color", randomColour);
            Colour.Value = randomColour;
        }
        else
        {
            SubmitPositionRequestServerRpc();
            SubmitColorRequestServerRpc();
        }
    }

    [ServerRpc]
    void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
    {
        Position.Value = GetRandomPositionOnPlane();
    }

    [ServerRpc]
    void SubmitColorRequestServerRpc(ServerRpcParams rpcParams = default)
	{
        Colour.Value = GetRandomColor();
	}

    static Vector3 GetRandomPositionOnPlane()
    {
        return new Vector3(UnityEngine.Random.Range(-3f, 3f), 1f, UnityEngine.Random.Range(-3f, 3f));
    }

    static Color GetRandomColor()
	{
        return new Color(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f));
	}

    void Update()
    {
        transform.position = Position.Value;
        gameObject.GetComponent<Renderer>().material.SetColor("_Color", Colour.Value);
    }
}
