using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Health playerHealth;
    public GameObject[] Hearts;
    public Sprite[] HeartImages; // 0 => Red, 1 => Black
    void Awake() {
        playerHealth = GameObject.FindWithTag("Player").GetComponent<Health>();
    }
    private void setBlackHearts(int stopAtIndex) {
        for (int i = Hearts.Length - 1; i > stopAtIndex; i--) {
            Hearts[i].GetComponent<Image>().sprite = HeartImages[1];
        }
    }
    private void setRedHearts(int startFrom) {
        for (int i = startFrom - 1; i >= 0; i--) {
            Hearts[i].GetComponent<Image>().sprite = HeartImages[0];
        }
    }
    void Update() {
        float segments = playerHealth.MaxHitpoints / 4;
        if (playerHealth.Hitpoints <= 0) {
            setBlackHearts(-1);
        } else if (playerHealth.Hitpoints <= segments * 1) {
            setBlackHearts(0);
            setRedHearts(1);
        } else if (playerHealth.Hitpoints <= segments * 2) {
            setBlackHearts(1);
            setRedHearts(2);
        } else if (playerHealth.Hitpoints <= segments * 3) {
            setBlackHearts(2);
            setRedHearts(3);
        } else {
            setRedHearts(4);
        }
    }
}
