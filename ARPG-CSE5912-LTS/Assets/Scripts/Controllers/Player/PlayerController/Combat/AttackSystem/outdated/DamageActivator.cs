using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DamageActivator : MonoBehaviour, IInteractable
{
    public AudioSource audioSource;
    public AudioClip audioClip;
    bool takeDamage = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out Player player))
        {
            player.Interactable = this;

            Debug.Log("triggered");


            GameObject cap;
            cap = this.transform.Find("Capsule").gameObject;
            cap.GetComponent<Renderer>().material.color = new Color(0, 204, 102);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out Player player))
        {
            if (player.Interactable is DamageActivator damageActivator && damageActivator == this)
            {
                player.Interactable = null;
                takeDamage = false;

                GameObject cap;
                cap = this.transform.Find("Capsule").gameObject;
                cap.GetComponent<Renderer>().material.color = new Color(178f/255f, 6f/255f, 6f/255f);
            }
        }
    }
    public void Interact(Player player)
    {
        takeDamage = true;
    }
    void Update()
    {
        if (takeDamage == true)
        {
            for (int i = 0; i < 50; i++)
            {
                audioSource.PlayOneShot(audioClip);
            }
        }
    }
}
