using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Layer Wrapper Class. Makes for easier selection in the editor
/// </summary>
[System.Serializable]
public class UnityLayer {

    /// <summary>
    /// The actual Layer Index
    /// </summary>
    [SerializeField]
    private int m_LayerIndex = 0;

    /// <summary>
    /// Get
    /// </summary>
    public int LayerIndex {
        get { return m_LayerIndex; }
    }

    /// <summary>
    /// Set LayerIndex
    /// </summary>
    /// <param name="_layerIndex"></param>
    public void Set(int _layerIndex) {
        if (_layerIndex > 0 && _layerIndex < 32) {
            m_LayerIndex = _layerIndex;
        }
    }

    /// <summary>
    /// Returns the Mask value, needed for LayerMask
    /// </summary>
    public int Mask {
        get { return 1 << m_LayerIndex; }
    }
}