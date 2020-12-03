using System.Collections;
using System.Collections.Generic;
using Entities.MonoBehaviours.Player;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public PlayerModel playerModel;
    public Image slider;
    public Text text;
    void FixedUpdate()
    {
        slider.fillAmount = (float)playerModel.CurrentHealth / (float)playerModel.StartingHealth;
        text.text = $"{playerModel.CurrentHealth}/{playerModel.StartingHealth}";
    }
}
