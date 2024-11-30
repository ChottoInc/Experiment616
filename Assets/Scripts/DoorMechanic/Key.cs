using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] Helper.KeyType keyType;

    public Helper.KeyType KeyType => keyType;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Player player))
        {
            AudioManager.Instance.PlayEffect("Key");
            player.AddKey(keyType);
            Destroy(gameObject);
        }
    }
}
