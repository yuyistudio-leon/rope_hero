using System;
using UnityEngine;
using UnityEngine.UI;

/*
 * 使用方法：
 * 添加到GameObject，并指定用于显示FPS的Text；
 * 或者：
 * 添加到包含Text组件的GameObject
 */
namespace LyLib
{
    public class FPSCounter : MonoBehaviour
    {
        const float fpsMeasurePeriod = 0.5f;
        private int m_FpsAccumulator = 0;
        private float m_FpsNextPeriod = 0;
        private int m_CurrentFps;
        const string display = "{0} FPS";
        public Text m_GuiText;


        private void Start()
        {
            m_FpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
            if (m_GuiText == null)
            {
                m_GuiText = GetComponent<Text>();
            }
            if (m_GuiText == null)
            {
                Debug.LogError("FPS counter error");
            }
        }


        private void Update()
        {
            // measure average frames per second
            m_FpsAccumulator++;
            if (Time.realtimeSinceStartup > m_FpsNextPeriod)
            {
                m_CurrentFps = (int) (m_FpsAccumulator/fpsMeasurePeriod);
                m_FpsAccumulator = 0;
                m_FpsNextPeriod += fpsMeasurePeriod;
                m_GuiText.text = string.Format(display, m_CurrentFps);
            }
        }
    }
}
