using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour {
    [SerializeField]
    Behaviour[] componentsToDisable;

    [SerializeField]
    string remoteLayerName = "RemotePlayer";

    private Camera sceneCamera;

    void Start(){
        if(!isLocalPlayer){
            DisableComponents();
            AssignRemoteLayer();
        }
        else{
            sceneCamera = Camera.main;
            if(sceneCamera != null){
                sceneCamera.gameObject.SetActive(false);
            }
        }    

        GetComponent<Player>().Setup();
    }

    public override void OnStartClient(){
        base.OnStartClient();

        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();

        GameManager.RegisterPlayer(_netID, _player);
    }

    void DisableComponents(){
        for (int ii = 0; ii < componentsToDisable.Length; ii++)
        {
            componentsToDisable[ii].enabled = false;
        }
    }

    void AssignRemoteLayer(){
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    void OnDisable() {
        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }

        GameManager.UnregisterPlayer(transform.name);
    }
}
