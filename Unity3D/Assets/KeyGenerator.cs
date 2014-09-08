using System.Text;
using System.Security.Cryptography;

/// <summary>
/// Key generator.
/// Implementation based on the suggestions at http://stackoverflow.com/a/1344255
/// </summary>
public class KeyGenerator
{
	/// <summary>
	/// Gets the unique key.
	/// </summary>
	/// <returns>The unique key.</returns>
	/// <param name="maxSize">Max size.</param>
	public static string GetUniqueKey(int maxSize)
	{
		char[] chars = new char[62];
		chars =
			"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
		byte[] data = new byte[1];
		RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
		crypto.GetNonZeroBytes(data);
		data = new byte[maxSize];
		crypto.GetNonZeroBytes(data);
		StringBuilder result = new StringBuilder(maxSize);
		foreach (byte b in data)
		{
			result.Append(chars[b % (chars.Length)]);
		}
		return result.ToString();
	}
}
