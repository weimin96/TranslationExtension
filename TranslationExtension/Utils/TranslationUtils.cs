using System.Net.Http;
using System.Text;

namespace TranslationExtension.Utils;

/// <summary>
/// 翻译相关的公共工具方法
/// </summary>
public static class TranslationUtils
{
    /// <summary>
    /// 共享的 HttpClient 实例
    /// </summary>
    public static readonly HttpClient HttpClient = new HttpClient();

    /// <summary>
    /// 检测文本是否包含中文字符
    /// </summary>
    /// <param name="text">待检测的文本</param>
    /// <returns>如果包含中文返回 true，否则返回 false</returns>
    public static bool ContainsChinese(string text)
    {
        foreach (char c in text)
        {
            // 中文字符的 Unicode 范围
            if (c >= 0x4E00 && c <= 0x9FFF)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 计算字符串的 MD5 哈希值
    /// </summary>
    /// <param name="str">待加密的字符串</param>
    /// <returns>小写的 32 位 MD5 哈希字符串</returns>
    public static string MD5Encrypt(string str)
    {
        var byteOld = Encoding.UTF8.GetBytes(str);
        // 调用加密方法
        var byteNew = System.Security.Cryptography.MD5.HashData(byteOld);
        // 将加密结果转换为字符串
        var sb = new StringBuilder();
        foreach (var b in byteNew)
        {
            // 将字节转换成16进制表示的字符串
            sb.Append(b.ToString("x2", System.Globalization.CultureInfo.InvariantCulture));
        }
        return sb.ToString();
    }
}
