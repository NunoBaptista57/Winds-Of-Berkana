using System.Collections;
using UnityEngine;
using TMPro;
using Cinemachine;

public class ShipHp : MonoBehaviour
{
    [Header("Settings")]
    public int hp = 3;
    public float invulnerabilityDuration = 3f; 
    public TextMeshProUGUI hpText; 
    bool saveBirdLocation = false;
    public Checkpoints checkpointsScript;
    public Transform bird;

    private bool isInvulnerable = false;
    private int initialHp;

    private void Start()
    {
        initialHp = hp;
        UpdateUI();
    }

    private void Update()
    {
        if (hp <= 0)
        {
            if (checkpointsScript == null)
            {
                print("ShipHp - No checkpoint system linked");
                return;
            }
            gameObject.GetComponent<BoatMovement>().currentSpeed = 0;
            gameObject.GetComponent<BoatMovement>().respawn();
            gameObject.transform.position = checkpointsScript.GetPlayerSpawnPoint().position;
            gameObject.transform.rotation = checkpointsScript.GetPlayerSpawnPoint().rotation;
            hp = initialHp;
            UpdateUI();

            if (bird != null)
            {
                if (checkpointsScript.isfirstCheckpoint())
                {
                    bird.gameObject.GetComponent<pathFollow>().restart();
                }
                bird.SetPositionAndRotation(checkpointsScript.GetBirdSpawnPoint().position, checkpointsScript.GetBirdSpawnPoint().rotation);  
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!isInvulnerable)
        {
            hp--;
            UpdateUI();
            StartCoroutine(InvulnerabilityCooldown());
        }
    }

    IEnumerator InvulnerabilityCooldown()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityDuration);
        isInvulnerable = false;
    }

    private void UpdateUI()
    {
        if (hpText != null)
        {
            hpText.text = hp.ToString();
        }
    }
}
