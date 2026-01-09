using System.Collections.Generic;

namespace TranslationExtension;

public static class TranslationDefinitions
{
    public record Choice(string Title, string Value);

    public static readonly List<Choice> Providers = new()
    {
        new("Baidu 翻译", "Baidu"),
        new("DeepSeek", "DeepSeek"),
        new("智谱AI", "Glm"),
        new("MiniMax", "Minimax")
    };

    public static readonly List<Choice> DeepSeekModels = new()
    {
        new("deepseek-chat", "deepseek-chat"),
        new("deepseek-reasoner", "deepseek-reasoner")
    };

    public static readonly List<Choice> GlmModels = new()
    {
        new("glm-4.7", "glm-4.7"),
        new("glm-4.6", "glm-4.6"),
        new("glm-4.5-air", "glm-4.5-air"),
        new("glm-4.5-airx", "glm-4.5-airx"),
        new("glm-4.5-flashng", "glm-4.5-flash")
    };

    public static readonly List<Choice> MinimaxModels = new()
    {
        new("MiniMax-M2.1", "MiniMax-M2.1"),
        new("MiniMax-M2.1-lightning", "MiniMax-M2.1-lightning"),
        new("MiniMax-M2", "MiniMax-M2")
    };

    public static string GetChoicesJson(List<Choice> choices)
    {
        var items = new List<string>();
        foreach (var choice in choices)
        {
            items.Add($$"""
            {
                "title": "{{choice.Title}}",
                "value": "{{choice.Value}}"
            }
            """);
        }
        return $"[{string.Join(",", items)}]";
    }
}
