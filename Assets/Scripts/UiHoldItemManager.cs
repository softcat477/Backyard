using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiHoldItemManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> projectileIcons = new List<GameObject>();
    int projectileIndex = 0;

    void Start()
    {
        foreach(GameObject icon in projectileIcons) {
            icon.SetActive(false);
        }
        projectileIcons[0].SetActive(true);

        PlayerController.ProjectileIndexChangeEvent += OnProjectileIndexChange;
    }

    void OnProjectileIndexChange(int newProjectileIndex) {
        projectileIcons[projectileIndex].SetActive(false);
        projectileIndex = newProjectileIndex;
        projectileIcons[projectileIndex].SetActive(true);
    }
}
