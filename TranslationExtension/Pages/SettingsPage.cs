using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace TranslationExtension;

/// <summary>
/// 翻译插件设置页面
/// 使用 Adaptive Cards 1.6 schema 动态渲染设置 UI
/// </summary>
internal sealed partial class SettingsPage : ContentPage
{
    private readonly SettingsFormContent _formContent = new();

    public SettingsPage()
    {
        Title = "翻译插件设置";
        Name = "Settings";
        Icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png");
    }

    /// <summary>
    /// 返回页面内容
    /// 使用 SettingsFormContent 渲染 Adaptive Card 表单
    /// </summary>
    public override IContent[] GetContent()
    {
        _formContent.Refresh();
        return [_formContent];
    }
}
