using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox
{
    public class Projectile : MonoBehaviour
    {
        IShooter owner;
        float damage { get { return (owner as Ship).Config.shooterConfig.ProjectileDamage; } }

        private void OnTriggerEnter(Collider other)
        {
            //Se il gameobject con cui è entrato in collisione è diverso da quello che lo ha sparato, allora entra nell'if.
            if (other.gameObject.GetComponent<IShooter>() != null)
                if (owner.GetOwner() == other.gameObject.GetComponent<IShooter>().GetOwner())
                    return;

            //Se collide con un perno o con i muri si distrugge
            if (other.gameObject.layer == 9 || other.gameObject.tag == "Wall" || other.gameObject.tag == "Core")
                Destroy(gameObject);

            // Controlla se l'oggetto con cui ha colliso ha l'interfaccia IDamageable e salva un riferimento di tale interfaccia           
            IDamageable damageables = other.gameObject.GetComponent<IDamageable>();
            if (damageables != null)
            {
                //Controlla se all'interno della lista di oggetti Danneggiabili, contenuta da Owner (chi ha sparato il proiettile)
                foreach (IDamageable item in owner.GetDamageable())
                {
                    // E' presente l'oggetto con cui il proiettile è entrato in collisione.
                    if (item.GetType() == damageables.GetType())
                    {
                        damageables.Damage(damage, owner.GetOwner());         // Se è un oggetto che può danneggiare, richiama la funzione che lo danneggia e se lo distrugge assegna i punti dell'uccisione all'agente che lo ha ucciso     
                        Destroy(gameObject);                //Distrugge il proiettile
                        break;                              // Ed esce dal foreach.
                    }
                }
            }
        }

        #region Interface

        //Setta chi è il proprietario del proiettile, cioé chi lo spara.
        public void Init(IShooter _owner)
        {
            owner = _owner;
        }

        #endregion
    }
}
