using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;

public class ConnectionManager : MonoBehaviour
{
    [SerializeField] private InputField ipAddress;
    [SerializeField] private Button posChange;

    [SerializeField] private Button HostButton;
    [SerializeField] private Button ClientButton;
    [SerializeField] private Button ServerButton;

    void Update()
    {
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            ShowStartButtons(true);
        }
        else
        {
            ShowStartButtons(false);
            ipAddress.gameObject.SetActive(false);
            posChange.gameObject.SetActive(true);

            if (NetworkManager.Singleton.IsServer)
            {
                posChange.transform.GetChild(0).GetComponent<Text>().text = "Move";
            }
            else
            {
                posChange.transform.GetChild(0).GetComponent<Text>().text = "Request Position Change";
            }
        }
    }

    public void OnEndEdit()
    {
        if (ipAddress.text == "")
        {
            ipAddress.text = "127.0.0.1";
        }

        GameObject.Find("NetworkManager").GetComponent<MLAPI.Transports.UNET.UNetTransport>().ConnectAddress = ipAddress.text;
    }

    private void ShowStartButtons(bool doShow)
    {
        HostButton.gameObject.SetActive(doShow);
        ClientButton.gameObject.SetActive(doShow);
        ServerButton.gameObject.SetActive(doShow);
    }

    public void Host()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void Client()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void Server()
    {
        NetworkManager.Singleton.StartServer();
    }

    public void SubmitNewPosition()
    {
        if (NetworkManager.Singleton.ConnectedClients.TryGetValue(NetworkManager.Singleton.LocalClientId,
                out var networkedClient))
        {
            var player = networkedClient.PlayerObject.GetComponent<PlayerMovement>();
            if (player)
            {
                player.Move();
            }
        }
    }
}
