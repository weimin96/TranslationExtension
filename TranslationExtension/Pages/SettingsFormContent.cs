using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace TranslationExtension;

/// <summary>
/// 基于 Adaptive Cards 的设置表单内容
/// 使用 Adaptive Card 1.6 schema 动态渲染设置页面
/// </summary>
internal sealed partial class SettingsFormContent : FormContent
{
    

    // 静态构造函数加载 JSON 模板
    public SettingsFormContent()
    {
        TemplateJson = GetTemplate();
        Refresh();
    }

    /// <summary>
    /// 刷新表单数据
    /// </summary>
    public void Refresh()
    {
        DataJson = $$"""
        {
          "Provider": "{{SettingsManager.Instance.Provider}}",
          "BaiduAppId": "{{SettingsManager.Instance.BaiduAppId}}",
          "BaiduSecretKey": "{{SettingsManager.Instance.BaiduSecretKey}}",
          "DeepSeekApiKey": "{{SettingsManager.Instance.DeepSeekApiKey}}",
          "DeepSeekModel": "{{SettingsManager.Instance.DeepSeekModel}}",
          "GlmApiKey": "{{SettingsManager.Instance.GlmApiKey}}",
          "GlmModel": "{{SettingsManager.Instance.GlmModel}}",
          "MinimaxApiKey": "{{SettingsManager.Instance.MinimaxApiKey}}",
          "MinimaxModel": "{{SettingsManager.Instance.MinimaxModel}}"
        }
        """;
    }

    /// <summary>
    /// 处理表单提交
    /// </summary>
    /// <param name="payload">用户提交的 JSON 数据</param>
    /// <returns>命令执行结果</returns>
    public override ICommandResult SubmitForm(string payload)
    {
        try
        {
            using var doc = JsonDocument.Parse(payload);
            var root = doc.RootElement;

            // 解析 Provider
            if (root.TryGetProperty("Provider", out var providerElement))
            {
                var providerStr = providerElement.GetString();
                if (Enum.TryParse<TranslationProvider>(providerStr, out var provider))
                {
                    SettingsManager.Instance.Provider = provider;
                }
            }

            // 解析 Baidu 配置
            if (root.TryGetProperty("BaiduAppId", out var baiduAppIdElement))
            {
                SettingsManager.Instance.BaiduAppId = baiduAppIdElement.GetString() ?? string.Empty;
            }

            if (root.TryGetProperty("BaiduSecretKey", out var baiduSecretKeyElement))
            {
                SettingsManager.Instance.BaiduSecretKey = baiduSecretKeyElement.GetString() ?? string.Empty;
            }

            // 解析 DeepSeek 配置
            if (root.TryGetProperty("DeepSeekApiKey", out var deepSeekApiKeyElement))
            {
                SettingsManager.Instance.DeepSeekApiKey = deepSeekApiKeyElement.GetString() ?? string.Empty;
            }

            if (root.TryGetProperty("DeepSeekModel", out var deepSeekModelElement))
            {
                SettingsManager.Instance.DeepSeekModel = deepSeekModelElement.GetString() ?? "deepseek-chat";
            }

            // 解析 GLM 配置
            if (root.TryGetProperty("GlmApiKey", out var glmApiKeyElement))
            {
                SettingsManager.Instance.GlmApiKey = glmApiKeyElement.GetString() ?? string.Empty;
            }

            if (root.TryGetProperty("GlmModel", out var glmModelElement))
            {
                SettingsManager.Instance.GlmModel = glmModelElement.GetString() ?? "glm-4-flash";
            }

            // 解析 MiniMax 配置
            if (root.TryGetProperty("MinimaxApiKey", out var minimaxApiKeyElement))
            {
                SettingsManager.Instance.MinimaxApiKey = minimaxApiKeyElement.GetString() ?? string.Empty;
            }

            if (root.TryGetProperty("MinimaxModel", out var minimaxModelElement))
            {
                SettingsManager.Instance.MinimaxModel = minimaxModelElement.GetString() ?? "abab6.5s-chat";
            }

            // 保存设置
            SettingsManager.Save(SettingsManager.Instance);
            
            // 返回成功结果，关闭页面
            return CommandResult.GoHome();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to save settings: {ex.Message}");
            return CommandResult.KeepOpen();
        }
    }

    /// <summary>
    /// 备用模板（当外部文件不可用时使用）
    /// </summary>
    private static string GetTemplate()
    {
        return $$"""
        {
            "$schema": "http://adaptivecards.io/schemas/adaptive-card.json",
            "type": "AdaptiveCard",
            "version": "1.6",
            "body": [
                {
                    "type": "TextBlock",
                    "text": "翻译插件设置",
                    "size": "Large",
                    "weight": "Bolder",
                    "wrap": true
                },
                {
                    "type": "TextBlock",
                    "text": "配置您的翻译服务",
                    "size": "Small",
                    "isSubtle": true,
                    "wrap": true,
                    "spacing": "None"
                },
                {
                    "type": "Container",
                    "spacing": "Large",
                    "items": [
                        {
                            "type": "TextBlock",
                            "text": "翻译服务商",
                            "weight": "Bolder",
                            "wrap": true
                        },
                        {
                            "type": "Input.ChoiceSet",
                            "id": "Provider",
                            "value": "${Provider}",
                            "choices": {{TranslationDefinitions.GetChoicesJson(TranslationDefinitions.Providers)}},
                            "placeholder": "选择翻译服务商"
                        }
                    ]
                },
                {
                    "id": "baiduSettings",
                    "type": "Container",
                    "spacing": "Medium",
                    "separator": true,
                    "isVisible": true,
                    "items": [
                        {
                            "type": "TextBlock",
                            "text": "Baidu 翻译配置",
                            "weight": "Bolder",
                            "wrap": true
                        },
                        {
                            "type": "TextBlock",
                            "text": "输入百度翻译 API 凭据",
                            "size": "Small",
                            "isSubtle": true,
                            "wrap": true,
                            "spacing": "None"
                        },
                        {
                            "type": "Input.Text",
                            "id": "BaiduAppId",
                            "label": "App ID",
                            "placeholder": "输入百度翻译 API App ID",
                            "value": "${BaiduAppId}"
                        },
                        {
                            "type": "Input.Text",
                            "id": "BaiduSecretKey",
                            "label": "Secret Key",
                            "placeholder": "输入百度翻译 API 密钥",
                            "value": "${BaiduSecretKey}",
                            "style": "Password"
                        }
                    ]
                },
                {
                    "id": "deepSeekSettings",
                    "type": "Container",
                    "spacing": "Medium",
                    "separator": true,
                    "isVisible": true,
                    "items": [
                        {
                            "type": "TextBlock",
                            "text": "DeepSeek 配置",
                            "weight": "Bolder",
                            "wrap": true
                        },
                        {
                            "type": "TextBlock",
                            "text": "输入 DeepSeek API 密钥和模型",
                            "size": "Small",
                            "isSubtle": true,
                            "wrap": true,
                            "spacing": "None"
                        },
                        {
                            "type": "Input.Text",
                            "id": "DeepSeekApiKey",
                            "label": "API Key",
                            "placeholder": "输入 DeepSeek API 密钥",
                            "value": "${DeepSeekApiKey}",
                            "style": "Password"
                        },
                        {
                            "type": "Input.ChoiceSet",
                            "id": "DeepSeekModel",
                            "label": "模型",
                            "value": "${DeepSeekModel}",
                            "choices": {{TranslationDefinitions.GetChoicesJson(TranslationDefinitions.DeepSeekModels)}},
                            "placeholder": "选择模型"
                        }
                    ]
                },
                {
                    "id": "glmSettings",
                    "type": "Container",
                    "spacing": "Medium",
                    "separator": true,
                    "isVisible": true,
                    "items": [
                        {
                            "type": "TextBlock",
                            "text": "GLM (智谱AI) 配置",
                            "weight": "Bolder",
                            "wrap": true
                        },
                        {
                            "type": "TextBlock",
                            "text": "输入智谱AI API 密钥和模型",
                            "size": "Small",
                            "isSubtle": true,
                            "wrap": true,
                            "spacing": "None"
                        },
                        {
                            "type": "Input.Text",
                            "id": "GlmApiKey",
                            "label": "API Key",
                            "placeholder": "输入智谱AI API 密钥",
                            "value": "${GlmApiKey}",
                            "style": "Password"
                        },
                        {
                            "type": "Input.ChoiceSet",
                            "id": "GlmModel",
                            "label": "模型",
                            "value": "${GlmModel}",
                            "choices": {{TranslationDefinitions.GetChoicesJson(TranslationDefinitions.GlmModels)}},
                            "placeholder": "选择模型"
                        }
                    ]
                },
                {
                    "id": "minimaxSettings",
                    "type": "Container",
                    "spacing": "Medium",
                    "separator": true,
                    "isVisible": true,
                    "items": [
                        {
                            "type": "TextBlock",
                            "text": "MiniMax 配置",
                            "weight": "Bolder",
                            "wrap": true
                        },
                        {
                            "type": "TextBlock",
                            "text": "输入 MiniMax API 凭据和模型",
                            "size": "Small",
                            "isSubtle": true,
                            "wrap": true,
                            "spacing": "None"
                        },
                        {
                            "type": "Input.Text",
                            "id": "MinimaxApiKey",
                            "label": "API Key",
                            "placeholder": "输入 MiniMax API 密钥",
                            "value": "${MinimaxApiKey}",
                            "style": "Password"
                        },
                        {
                            "type": "Input.ChoiceSet",
                            "id": "MinimaxModel",
                            "label": "模型",
                            "value": "${MinimaxModel}",
                            "choices": {{TranslationDefinitions.GetChoicesJson(TranslationDefinitions.MinimaxModels)}},
                            "placeholder": "选择模型"
                        }
                    ]
                }
            ],
            "actions": [
                {
                    "type": "Action.Submit",
                    "title": "保存设置",
                    "style": "positive"
                }
            ]
        }
        """;
    }
}
