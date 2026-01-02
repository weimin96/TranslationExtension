using System;
using System.Text;
using System.Threading.Tasks;
using TranslationExtension.Utils;

namespace TranslationExtension.Providers;

/// <summary>
/// 百度翻译提供商实现
/// </summary>
public class BaiduTranslationProvider : ITranslationProvider
{
    public async Task<string> TranslateAsync(string text, TranslationSettings settings)
    {
        if (string.IsNullOrEmpty(settings.BaiduAppId) || string.IsNullOrEmpty(settings.BaiduSecretKey))
            return "请先在设置中配置 Baidu API Credentials";

        // 原文
        string q = text;
        // 源语言，百度支持自动识别
        string from = "auto";
        // 目标语言，根据是否包含中文自动判定
        string to = TranslationUtils.ContainsChinese(text) ? "en" : "zh";

        string appId = settings.BaiduAppId;
        string secretKey = settings.BaiduSecretKey;

        // 随机数
        Random rd = new Random();
        string salt = rd.Next(10000, 99999).ToString(System.Globalization.CultureInfo.InvariantCulture);

        // 计算签名：sign = md5(appid + q + salt + secretKey)
        string sign = TranslationUtils.MD5Encrypt(appId + q + salt + secretKey);

        // 百度翻译 API 地址
        string url = "https://api.fanyi.baidu.com/api/trans/vip/translate?";
        url += "q=" + System.Net.WebUtility.UrlEncode(q);
        url += "&from=" + from;
        url += "&to=" + to;
        url += "&appid=" + appId;
        url += "&salt=" + salt;
        url += "&sign=" + sign;

        var response = await TranslationUtils.HttpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var resultJson = await response.Content.ReadAsStringAsync();

        using var doc = System.Text.Json.JsonDocument.Parse(resultJson);
        var root = doc.RootElement;

        // 检查错误码
        if (root.TryGetProperty("error_code", out var errorCode))
        {
            string code = errorCode.ToString();
            if (code != "52000" && code != "0")
            {
                if (root.TryGetProperty("error_msg", out var errorMsg))
                    return $"Baidu Error: {errorMsg.GetString()} ({code})";
                return $"Baidu Error Code: {code}";
            }
        }

        // 解析翻译结果
        if (root.TryGetProperty("trans_result", out var transResult))
        {
            var sb = new StringBuilder();
            foreach (var item in transResult.EnumerateArray())
            {
                if (sb.Length > 0) sb.AppendLine();
                sb.Append(item.GetProperty("dst").GetString());
            }
            return sb.ToString();
        }

        return "未获取到翻译结果";
    }
}
