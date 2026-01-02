using System.Threading.Tasks;

namespace TranslationExtension.Providers;

/// <summary>
/// Google 翻译提供商实现（占位）
/// </summary>
public class GoogleTranslationProvider : ITranslationProvider
{
    public Task<string> TranslateAsync(string text, TranslationSettings settings)
    {
        // 简化的 Google 翻译占位实现
        return Task.FromResult($"[Google] 翻译结果: {text} (请配置有效的 Google API 处理逻辑)");
    }
}
