using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TranslationExtension.Utils;

namespace TranslationExtension.Providers;

/// <summary>
/// GLM (智谱AI) 翻译提供商实现
/// </summary>
public class GlmTranslationProvider : ITranslationProvider
{
    public async Task<string> TranslateAsync(string text, TranslationSettings settings)
    {
        if (string.IsNullOrEmpty(settings.GlmApiKey))
            return "请先在设置中配置 GLM API Key";

        // 自动判定翻译方向
        string targetLang = TranslationUtils.ContainsChinese(text) ? "英文" : "中文";
        string systemPrompt = $"你是一个专业的翻译助手。请将用户输入的文本翻译成{targetLang},只返回翻译结果,不要添加任何解释或额外内容。";

        // 构建请求体
        var requestBody = new GlmRequest
        {
            Model = settings.GlmModel,
            Messages = new[]
            {
                new GlmMessage { Role = "system", Content = systemPrompt },
                new GlmMessage { Role = "user", Content = text }
            },
            Stream = false
        };

        var jsonContent = JsonSerializer.Serialize(requestBody, TranslationSettingsContext.Default.GlmRequest);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        // 设置请求头
        using var request = new HttpRequestMessage(HttpMethod.Post, "https://open.bigmodel.cn/api/paas/v4/chat/completions");
        request.Headers.Add("Authorization", $"Bearer {settings.GlmApiKey}");
        request.Content = httpContent;

        var response = await TranslationUtils.HttpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var resultJson = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize(resultJson, TranslationSettingsContext.Default.GlmResponse);

        // 检查错误
        if (result?.Error != null)
        {
            return $"GLM Error: {result.Error.Message ?? "API 调用失败"}";
        }

        // 解析翻译结果
        if (result?.Choices != null && result.Choices.Length > 0)
        {
            var firstChoice = result.Choices[0];
            if (firstChoice.Message != null && !string.IsNullOrEmpty(firstChoice.Message.Content))
            {
                return firstChoice.Message.Content;
            }
        }

        return "未获取到翻译结果";
    }
}
