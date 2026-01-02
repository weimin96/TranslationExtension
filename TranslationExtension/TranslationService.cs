using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TranslationExtension.Providers;

namespace TranslationExtension;

/// <summary>
/// 翻译服务，使用策略模式管理多个翻译提供商
/// </summary>
public class TranslationService
{
    /// <summary>
    /// 翻译提供商字典
    /// </summary>
    private static readonly Dictionary<TranslationProvider, ITranslationProvider> _providers = new()
    {
        { TranslationProvider.Baidu, new BaiduTranslationProvider() },
        { TranslationProvider.DeepSeek, new DeepSeekTranslationProvider() },
        { TranslationProvider.Google, new GoogleTranslationProvider() }
    };

    /// <summary>
    /// 执行翻译
    /// </summary>
    /// <param name="text">待翻译的文本</param>
    /// <param name="promptOverride">可选的提示词覆盖（保留参数以兼容现有调用）</param>
    /// <returns>翻译结果</returns>
    public static async Task<string> TranslateAsync(string text, string? promptOverride = null)
    {
        if (string.IsNullOrWhiteSpace(text)) return string.Empty;

        var settings = SettingsManager.Instance;
        try
        {
            // 根据设置选择对应的翻译提供商
            if (_providers.TryGetValue(settings.Provider, out var provider))
            {
                return await provider.TranslateAsync(text, settings);
            }
            
            // 默认使用百度翻译
            return await _providers[TranslationProvider.Baidu].TranslateAsync(text, settings);
        }
        catch (Exception ex)
        {
            return $"翻译出错: {ex.Message}";
        }
    }
}
