using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollection : MonoBehaviour
{
    [SerializeField] private AudioClip _CollectionSound;

    private AudioSource m_AudioSource;

    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    public void Collect()
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        m_AudioSource.clip = _CollectionSound;
        m_AudioSource.Play();
        SessionManager.instance.CoinCollected(this.gameObject);
    }
}
