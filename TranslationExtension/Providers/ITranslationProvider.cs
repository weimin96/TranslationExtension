using System.Threading.Tasks;

namespace TranslationExtension.Providers;

/// <summary>
/// 翻译提供商接口
/// </summary>
public interface ITranslationProvider
{
    /// <summary>
    /// 执行翻译
    /// </summary>
    /// <param name="text">待翻译的文本</param>
    /// <param name="settings">翻译设置</param>
    /// <returns>翻译结果</returns>
    Task<string> TranslateAsync(string text, TranslationSettings settings);
}
