using System;

[Serializable]
public class UserData
{
	public string nickName;
	public CharacterType type;
	public string position;
	public float posX;
	public float posY;
	public float posZ;
	public string scene;

	public UserData(string nickName, CharacterType type, string position, float x, float y, float z, string scene)
	{
		this.nickName = nickName;
		this.type = type;
		this.position = position;
		this.posX = x;
		this.posY = y;
		this.posZ = z;
		this.scene = scene;
	}
}

public enum CharacterType { Man, WoMan }
