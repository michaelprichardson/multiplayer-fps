using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player : NetworkBehaviour {

    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    [SerializeField]
    private int maxHealth = 100;

    [SyncVar]
    private int currentHealth;

    [SerializeField]
    private Behaviour[] disabledOnDeath;
    private bool[] wasEnabled;

    public void Setup(){
        wasEnabled = new bool[disabledOnDeath.Length];
        for(int ii = 0; ii < wasEnabled.Length; ii++){
            wasEnabled[ii] = disabledOnDeath[ii].enabled;
        }

        SetDefaults();
    }

    void Update(){
        if(!isLocalPlayer) return;

        if(Input.GetKeyDown(KeyCode.K)){
            RpcTakeDamage(1000);
        }
    }

    [ClientRpc]
    public void RpcTakeDamage(int _amount){
        if(isDead) return;
        currentHealth -= _amount;
        Debug.Log(transform.name + " now has " + currentHealth + " health.");

        if(currentHealth <= 0){
            Die();
        }
    }

    public void SetDefaults(){
        isDead = false;
        currentHealth = maxHealth;

        for (int ii = 0; ii < disabledOnDeath.Length; ii++){
            disabledOnDeath[ii].enabled = wasEnabled[ii];
        }

        Collider _col = GetComponent<Collider>();
        if(_col != null){
            _col.enabled = true;
        }
    }

    private void Die(){
        isDead = true;

        // Disable components
        for (int ii = 0; ii < disabledOnDeath.Length; ii++){
            disabledOnDeath[ii].enabled = false;
        }

        Debug.Log(transform.name + " is dead.");

        // Call respawn method
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn(){
        Debug.Log("Respawning...");
        yield return new WaitForSeconds(GameManager.singleton.matchSettings.respawnTime);

        SetDefaults();
        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;

        Debug.Log(transform.name + " respawned.");
    }
}
