using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//script pour gérer les parties du boss
public class BossParts : MonoBehaviour
{
    public int number = 7;
    public Target target;
    public GameObject healthBar;
    public GameObject death;

    //enleve une partie du boss
    public void remove()
    {
        //la tete peut etre détruite
        if (--number <= 1)
        {
            target.enabled = true;
            healthBar.SetActive(true);
        }
        
        //le boss est mort
        if (number <= 0)
        {
            GameObject vfx;
            if (death != null)
            {
                vfx = Instantiate(death, gameObject.transform.position, Quaternion.identity);
                vfx.transform.localScale = new Vector3(2, 2, 2);
            }
            Destroy(gameObject);
        }
    }
}
