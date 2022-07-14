using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Spinner : MonoBehaviour,IDragHandler, IPointerDownHandler, IPointerUpHandler
{
	[SerializeField] private float m_RotationSpeed;
	[SerializeField] private AnimationCurve m_RotationEaseCurve;
	[SerializeField] private AnimationCurve m_FrictionToRotationCurve;
	[SerializeField] private Transform m_Fidget;

	private float m_PointerDowntime;	 
	private float m_DragInteractionTime;
	private Vector3 m_PointerDownPos; 
	private Vector3 m_PointerUpPos;
	private Vector3 m_RotationAxis;
	void Update()
	{
		if (m_RotationSpeed > 0f)
		{
			m_Fidget.Rotate(m_RotationAxis, m_RotationEaseCurve.Evaluate(m_RotationSpeed));
			m_RotationSpeed -= m_FrictionToRotationCurve.Evaluate(m_RotationSpeed) * Time.deltaTime;
		}
	}
	public void OnPointerDown(PointerEventData eventData)
	{
		m_PointerDowntime = Time.time;
		m_PointerDownPos = eventData.position;
		m_Fidget.transform.rotation = Quaternion.identity;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		m_DragInteractionTime = Time.time - m_PointerDowntime;
		m_PointerUpPos = eventData.position;
		GetRotationSpeed();
		GetRotationDirection();
	}
	private void GetRotationDirection()
	{
		if (m_PointerDownPos.x > m_PointerUpPos.x || m_PointerDownPos.y < m_PointerUpPos.y)
			m_RotationAxis = -Vector3.forward;
		else
			m_RotationAxis = Vector3.forward;
	}

	private void GetRotationSpeed()
	{
		float l_Dis = Vector3.Distance(m_PointerUpPos, m_PointerDownPos);
		float l_SpeedIncrementalFactor = l_Dis / m_DragInteractionTime;
		m_RotationSpeed += l_SpeedIncrementalFactor;		
	}

    public void OnDrag(PointerEventData eventData)
    {
        float l_AngelToRotate = Mathf.Atan(eventData.delta.magnitude);
		m_Fidget.Rotate(new Vector3(0f, 0f, l_AngelToRotate));
    }
}
