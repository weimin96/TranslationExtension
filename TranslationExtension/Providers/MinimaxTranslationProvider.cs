using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TranslationExtension.Utils;

namespace TranslationExtension.Providers;

/// <summary>
/// MiniMax 翻译提供商实现 (使用 Anthropic 兼容接口)
/// </summary>
public class MinimaxTranslationProvider : ITranslationProvider
{
    public async Task<string> TranslateAsync(string text, TranslationSettings settings)
    {
        if (string.IsNullOrEmpty(settings.MinimaxApiKey))
            return "请先在设置中配置 MiniMax API Key";

        // 自动判定翻译方向
        string targetLang = TranslationUtils.ContainsChinese(text) ? "英文" : "中文";
        string systemPrompt = $"你是一个专业的翻译助手。请将用户输入的文本翻译成{targetLang},只返回翻译结果,不要添加任何解释或额外内容。";

        // 构建请求体 (Anthropic 风格)
        var requestBody = new MinimaxRequest
        {
            Model = settings.MinimaxModel,
            MaxTokens = 2000,
            System = systemPrompt,
            Messages = new[]
            {
                new MinimaxMessage 
                { 
                    Role = "user", 
                    Content = new[] { new MinimaxContent { Type = "text", Text = text } } 
                }
            },
            Stream = false
        };

        var jsonContent = JsonSerializer.Serialize(requestBody, TranslationSettingsContext.Default.MinimaxRequest);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        // 使用 MiniMax 的 Anthropic 兼容端点
        const string apiUrl = "https://api.minimaxi.com/anthropic/v1/messages";
        using var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
        request.Headers.Add("x-api-key", settings.MinimaxApiKey);
        request.Content = httpContent;

        var response = await TranslationUtils.HttpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var resultJson = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize(resultJson, TranslationSettingsContext.Default.MinimaxResponse);

        // 检查基础错误
        if (result?.BaseResp != null && result.BaseResp.StatusCode != 0)
        {
            return $"MiniMax Error: {result.BaseResp.StatusMsg ?? "API 调用失败"}";
        }

        // 解析响应内容块 (Anthropic 风格)
        if (result?.Content != null && result.Content.Length > 0)
        {
            // 拼接所有文本类型的块
            var translatedParts = result.Content
                .Where(c => c.Type == "text" && !string.IsNullOrEmpty(c.Text))
                .Select(c => c.Text);
            
            var translatedText = string.Join("", translatedParts).Trim();
            
            if (!string.IsNullOrEmpty(translatedText))
            {
                return translatedText;
            }
        }

        return "未获取到翻译结果";
    }
}
