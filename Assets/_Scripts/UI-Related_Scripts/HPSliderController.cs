using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the UI of the HP Slider.  
/// </summary>
public class HPSliderController : MonoBehaviour {

    /// <summary>
    /// m_HPSlider is a public variable so that other classes can reference it to control it.  
    /// </summary>
    public Slider m_HPSlider;

    /// <summary>
    /// Use the public variable m_CurrentMaxHP to modify its private variable counterpart m_currentMaximumHP through code by calling this variable with the class that controls the player's HP.
    /// m_CurrentMaxHP is a public variable so that other classes can reference it to control it.  
    /// It allows the player's current maximum HP to be increased if the player picks up power-ups.  
    /// </summary>
    private float m_currentMaximumHP;
    public float m_CurrentMaxHP { get { return m_currentMaximumHP; } set { m_currentMaximumHP = value; } }

    /// <summary>
    /// Use the public variable m_CurrentHP to modify its private variable counterpart m_currentPlayerHP through code by calling this variable with the class that controls the player's HP.  
    /// The value in m_currentPlayerHP will change the UI value of the chalice slider for indicating HP.  
    /// </summary>
    private float m_currentPlayerHP;
    public float m_CurrentHP { get { return m_currentPlayerHP; } set { m_currentPlayerHP = value; } }

    // Start is called before the first frame update
    void Start() {
        m_HPSlider.minValue = 0.0f;
        m_HPSlider.maxValue = m_currentMaximumHP;
    }

    // Update is called once per frame
    void Update() {
        m_HPSlider.value = m_currentPlayerHP;
    }
}
