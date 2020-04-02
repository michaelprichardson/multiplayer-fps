using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour {
    [SerializeField]
    Behaviour[] componentsToDisable;

    [SerializeField]
    string remoteLayerName = "RemotePlayer";

    [SerializeField]
    string dontDrawLayerName = "DontDraw";
    [SerializeField]
    GameObject playerGraphics;
    [SerializeField]
    GameObject playerUIPrefab;
    private GameObject playerUIInstance;

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

            // Disable player graphics for local player
            SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));

            // Create player UI
            playerUIInstance = Instantiate(playerUIPrefab);
            playerUIInstance.name = playerUIPrefab.name;
        }

        GetComponent<Player>().Setup();
    }

    void SetLayerRecursively(GameObject obj, int newLayer){
        obj.layer = newLayer;

        foreach (Transform child in obj.transform){
            SetLayerRecursively(child.gameObject, newLayer);
        }
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
        // Destroy player UI
        Destroy(playerUIInstance);

        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }

        GameManager.UnregisterPlayer(transform.name);
    }
}
