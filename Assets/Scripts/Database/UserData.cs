using System;

[Serializable]
public class UserData
{
	public string nickName;
	public CharacterType type;
	public string position;

	public UserData(string nickName, CharacterType type, string position)
	{
		this.nickName = nickName;
		this.type = type;
		this.position = position;
	}
}

public enum CharacterType
{
	Man,
	WoMan
}
