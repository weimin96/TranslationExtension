# TranslationExtension (PowerToys 翻译扩展)

<div align="center">

![License](https://img.shields.io/github/license/weimin96/TranslationExtension)
![Dotnet Version](https://img.shields.io/badge/.NET-9.0-blue?style=flat-square&logo=dotnet)
![Platform](https://img.shields.io/badge/Platform-Windows-0078D4?style=flat-square&logo=windows)
![Stars](https://img.shields.io/github/stars/weimin96/TranslationExtension?style=flat-square)
![Issues](https://img.shields.io/github/issues/weimin96/TranslationExtension?style=flat-square)
![Forks](https://img.shields.io/github/forks/weimin96/TranslationExtension?style=flat-square)

</div>

这是一个专为 PowerToys 深度定制的翻译扩展工具。它旨在无缝集成到 Windows 工作流中，为开发者提供极速、简洁且高效的文本翻译体验。

---

## 🚀 核心特性

- **⚡ 极速响应**：基于 .NET 9 高性能运行时，翻译请求毫秒级响应。
- **🧩 PowerToys 集成**：深度适配 PowerToys 插件体系，呼之即来，挥之即去。
- **🤖 智能感应**：支持中英文自动检测与互译，无需手动切换源语言。
- **⚙️ 灵活配置**：内置统一的设置面板，支持多种 API 凭证管理（目前已完善支持百度翻译）。
- **🎨 现代 UI**：采用 WinUI 3 框架，完美融入 Windows 11 设计风格，支持深色/浅色模式。

## 🛠️ 技术栈

- **语言**: [C# 13](https://learn.microsoft.com/dotnet/csharp/)
- **框架**: [.NET 9.0](https://dotnet.microsoft.com/download/dotnet/9.0)
- **UI 框架**: [Windows App SDK / WinUI 3](https://learn.microsoft.com/windows/apps/winui/winui3/)
- **API 集成**: RESTful API (HttpClient)

## 📥 环境要求

- **操作系统**: Windows 10 version 19041.0 或更高版本
- **开发工具**: [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) (需安装 "Windows 应用程序开发" 工作负荷)
- **运行时**: [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

## 🏎️ 快速开始

### 1. 克隆代码仓库
```powershell
git clone https://github.com/weimin96/TranslationExtension.git
cd TranslationExtension
```

### 2. 编译与运行
- 使用 Visual Studio 2026 打开 `TranslationExtension.sln`。
- 确认已解决 NuGet 依赖。
- 将 `TranslationExtension` 设为 **启动项目**。
- 按 `F5` 启动调试，或通过 `生成 > 发布` 进行打包。

## ⚙️ 翻译提供商配置

在使用翻译功能之前，需要至设置页面配置服务商凭证：

### 百度翻译 (Baidu)
1. 注册并登录 [百度翻译开放平台](http://api.fanyi.baidu.com/)。
2. 在 **开发者信息** 中获取您的 `App ID` 和 `Secret Key`。
3. 打开本扩展的 **Settings (设置)** 页面。
4. 选择 **Baidu** 提供商，填入对应的凭证并保存。

> [!TIP]
> 百度翻译为新用户提供一定额度的每月免费调用字数，适合个人开发者使用。

## 📂 项目结构

```text
TranslationExtension/
├── TranslationExtension/          # 核心插件逻辑
│   ├── Assets/                    # 图标与静态资源
│   ├── Pages/                     # WinUI 页面 (设置与主界面)
│   ├── TranslationService.cs      # API 请求核心逻辑
│   ├── TranslationSettings.cs     # 配置模型定义
│   └── SettingsManager.cs         # 本地持久化管理
├── Directory.Build.props          # 全局构建配置
└── TranslationExtension.sln       # 解决方案入口
```

## 🤝 参与贡献

我们非常欢迎来自社区的贡献！
1. Fork 本仓库。
2. 创建您的特性分支 (`git checkout -b feature/AmazingFeature`)。
3. 提交您的更改 (`git commit -m 'Add some AmazingFeature'`)。
4. 推送到分支 (`git push origin feature/AmazingFeature`)。
5. 开启一个 Pull Request。

## 📄 开源协议

本项目基于 **MIT** 协议开源。详情请参阅 [LICENSE](LICENSE) 文件。
