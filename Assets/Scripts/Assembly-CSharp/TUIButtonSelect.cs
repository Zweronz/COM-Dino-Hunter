using UnityEngine;

[AddComponentMenu("TUI/Control/Select Button")]
public class TUIButtonSelect : TUIButton
{
	private TUIControl m_Control;

	public const int CommandSelect = 1;

	public int index;

	private new void Start()
	{
		base.Start();
		m_Control = (TUIControl)this;
	}

	public bool IsSelected()
	{
		return m_bPressed;
	}

	public void SetSelected(bool selected)
	{
		m_bPressed = selected;
		m_iFingerId = -1;
		Show();
	}

	public override void Reset()
	{
		if (name.StartsWith("Btn_Item"))
		{
			m_Control.Reset();
			m_bDisable = false;
			m_bPressed = false;
			Show();
		}
		else
		{
			base.Reset();
		}
	}

	public override bool HandleInput(TUIInput input)
	{
		if (m_bDisable)
		{
			return false;
		}
		if (input.inputType == TUIInputType.Began)
		{
			if (PtInControl(input.position))
			{
				m_iFingerId = input.fingerId;
				return true;
			}
			return false;
		}
		if (input.fingerId == m_iFingerId)
		{
			if (input.inputType == TUIInputType.Ended)
			{
				m_iFingerId = -1;
				if (PtInControl(input.position) && !m_bPressed)
				{
					m_bPressed = true;
					Show();
					PostEvent(this, 1, 0f, 0f, null);
				}
			}
			return true;
		}
		return false;
	}

	public void Event_CommandSelect()
	{
		m_bPressed = true;
		m_iFingerId = -1;
		Show();
		PostEvent(this, 1, 0f, 0f, null);
	}
}
