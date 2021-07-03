using UnityEngine;
using MLAPI;
using System.Text;

public class ConnManager : MonoBehaviour
{
    //refs
    [SerializeField] private GameObject connButtonPanel;

    //vars
    private string passphrase;

    private void Start()
    {
        connButtonPanel.SetActive(true);
        // TODO: generate random passphrase
        passphrase = "password";
    }

    #region ServerMethods
    public void Host()
    {
        // set the connection data to the passphrase
        // Note: we could also use this byte array to store the game version as well to ensure the client is updated
        connButtonPanel.SetActive(false);
        NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.ASCII.GetBytes(passphrase);
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.StartHost(GenerateSpawnLocation(), Quaternion.identity);
    }

    private void ApprovalCheck(byte[] connData, ulong clientID, NetworkManager.ConnectionApprovedDelegate callback)
    {
        // custom client approval code here
        bool didApprove = Encoding.ASCII.GetString(connData) == passphrase;
        if (didApprove)
        {
            Debug.Log($"Approving connection from {clientID}");
        }
        else
        {
            Debug.Log($"Rejecting connection from {clientID}");
        }

        // inform network manager of outcome
        // params: spawn player object, prefab hash (null for default), connection approved, pos, rot
        callback(true, null, didApprove, GenerateSpawnLocation(), Quaternion.identity);
    }

    private Vector3 GenerateSpawnLocation()
    {
        float xPos = Random.Range(-3.0f, 3.0f);
        float zPos = Random.Range(-3.0f, 3.0f);
        return new Vector3(xPos, 0.0f, zPos);
    }
    #endregion

    #region ClientMethods
    public void Join()
    {
        connButtonPanel.SetActive(false);
        // tell the network manager to connect with custom connection data which we will use as a password
        NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.ASCII.GetBytes(passphrase);
        NetworkManager.Singleton.StartClient();
    }
    #endregion
}
