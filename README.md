🚀 **轻量级 C# 版本信息管理工具**

一个简单易用的 .NET 库，用于管理应用程序的版本信息，支持动态版本显示和程序集信息自动应用。

## **✨ 特性**

- 🎯 **简洁 API**：一行代码完成版本信息配置
- 🔧 **多版本类型**：支持 Release、Beta、Dev、Alpha 等版本类型
- 📦 **自动应用**：版本信息自动应用到当前程序集
- 🎨 **专业格式**：生成标准化版本号格式 `V9.1.2 [Dev] Build 2025.08.13-1545`
- 💉 **依赖注入**：完美支持 Prism、.NET DI 等 IoC 容器

## **🚀 快速开始基本用法**

### **安装**

```csharp
# NuGet 包管理器
Install-Package VersionInfo

# 或通过 .NET CLI
dotnet add package VersionInfo
```

### **基本用法**

```csharp
// 注册服务
containerRegistry.RegisterSingleton<IVersionService>(() =>
{
    var versionService = new VersionService();
    versionService.UpdateVersionInfo("9.1.2", VersionType.Dev, "新增自动重连功能", "2025.08.13-1545");
    return versionService;
});

// 使用服务
public MainWindow(IVersionService versionService)
{
    var fullVersion = versionService.FullVersion; 
    // 输出: V9.1.2 [Dev] Build 2025.08.13-1545
}
```

### **在 WPF 中使用**

```csharp
public partial class App : PrismApplication
{
    private const string VERSION = "9.1.2";
    private const VersionType VERSION_TYPE = VersionType.Dev;
    private const string BUILD_TIME = "2025.08.13-1545";
    
    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterSingleton<IVersionService>(() =>
        {
            var service = new VersionService();
            service.UpdateVersionInfo(VERSION, VERSION_TYPE, "功能描述", BUILD_TIME);
            return service;
        });
    }
}
```

## 📋 **版本类型**

| 类型 | 说明 |
| --- | --- |
| `Release` | 正式版本 |
| `Beta` | 测试版本 |
| `Dev` | 开发版本 |
| `Hotfix` | 热修复版本 |

## **🎯 显示效果**

| **版本类型** | **显示格式** |
| --- | --- |
| Release | V9.1.2 [Release] Build 2025.08.13-1545 |
| Dev | V9.1.2 [Dev] Build 2025.08.13-1545 |
| Beta | V9.1.2 [Beta] Build 2025.08.13-1545 |
| Hotfix | V9.1.2 [Hotfix] Build 2025.08.13-1545 |

## **🔧 适用场景**

✅ WPF/WinForms 桌面应用✅ 上位机软件版本管理✅ 企业内部工具✅ 需要版本追踪的应用程序

## ⚠️ **已知问题**

> 注意： 该工具目前为快速开发版本，存在以下已知问题：
> 

### 🔴 **主要问题**

1. **程序集版本未同步**
    - 当前只是在软件内部动态组装显示版本信息
    - 实际的 `AssemblyVersion`、`AssemblyFileVersion` 等并未真正修改
    - **理想方案**：应该能同时修改程序集的文件版本号
2. **编译时间为手动维护**
    - 当前的 Build Time 需要手动填写
    - **理想方案**：应该在编译时自动生成真实的编译时间戳

### 🟡 **计划改进**

- [ ]  集成 MSBuild Target，自动修改 `AssemblyInfo`
- [ ]  自动生成编译时间，无需手动维护

### 🔄 **当前状态**

该工具能满足**基础的版本信息显示需求**，适合快速项目使用。如需完整的版本管理方案，请等待后续版本更新。

## **🤝 贡献**

欢迎提交 Issue 和 Pull Request！特别欢迎针对上述已知问题的改进方案。

⭐ 如果这个工具对你有帮助，请给个 Star 支持一下！

📧 **问题反馈**：[提交 Issue](https://github.com/leon-kay/VersionInfo/issues)
