using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TranslationExtension.Utils;

namespace TranslationExtension.Providers;

/// <summary>
/// DeepSeek 翻译提供商实现
/// </summary>
public class DeepSeekTranslationProvider : ITranslationProvider
{
    public async Task<string> TranslateAsync(string text, TranslationSettings settings)
    {
        if (string.IsNullOrEmpty(settings.DeepSeekApiKey))
            return "请先在设置中配置 DeepSeek API Key";

        // 自动判定翻译方向
        string targetLang = TranslationUtils.ContainsChinese(text) ? "英文" : "中文";
        string systemPrompt = $"你是一个专业的翻译助手。请将用户输入的文本翻译成{targetLang}，只返回翻译结果，不要添加任何解释或额外内容。";

        // 构建请求体
        var requestBody = new
        {
            model = "deepseek-chat",
            messages = new[]
            {
                new { role = "system", content = systemPrompt },
                new { role = "user", content = text }
            },
            stream = false
        };

        var jsonContent = JsonSerializer.Serialize(requestBody);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        // 设置请求头
        using var request = new HttpRequestMessage(HttpMethod.Post, "https://api.deepseek.com/chat/completions");
        request.Headers.Add("Authorization", $"Bearer {settings.DeepSeekApiKey}");
        request.Content = httpContent;

        var response = await TranslationUtils.HttpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var resultJson = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(resultJson);
        var root = doc.RootElement;

        // 检查错误
        if (root.TryGetProperty("error", out var error))
        {
            if (error.TryGetProperty("message", out var errorMsg))
                return $"DeepSeek Error: {errorMsg.GetString()}";
            return "DeepSeek API 调用失败";
        }

        // 解析翻译结果
        if (root.TryGetProperty("choices", out var choices) && choices.GetArrayLength() > 0)
        {
            var firstChoice = choices[0];
            if (firstChoice.TryGetProperty("message", out var message) &&
                message.TryGetProperty("content", out var content))
            {
                return content.GetString() ?? "未获取到翻译结果";
            }
        }

        return "未获取到翻译结果";
    }
}
