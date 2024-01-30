namespace EHR.Journey.Cli.Utils;

public static class ReplacePackageReferenceExtensions
{
    public static string ReplacePackageReferenceCore(this string content)
    {
        return content
                .Replace("<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\frameworks\\src\\EHR.Journey.Core\\EHR.Journey.Core.csproj\"/>",
                    "<PackageReference Include=\"EHR.Journey.Core\"/>")
                .Replace("<ProjectReference Include=\"..\\..\\..\\..\\aspnet-core\\frameworks\\src\\EHR.Journey.Core\\EHR.Journey.Core.csproj\"/>",
                    "<PackageReference Include=\"EHR.Journey.Core\"/>")
                .Replace("<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\shared\\EHR.Journey.Shared.Hosting.Microservices\\EHR.Journey.Shared.Hosting.Microservices.csproj\"/>",
                    "<PackageReference Include=\"EHR.Journey.Shared.Hosting.Microservices\"/>")
                .Replace("<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\shared\\EHR.Journey.Shared.Hosting.Gateways\\EHR.Journey.Shared.Hosting.Gateways.csproj\"/>",
                    "<PackageReference Include=\"EHR.Journey.Shared.Hosting.Gateways\"/>")
            ;
    }

    public static string ReplacePackageReferenceBasicManagement(this string content)
    {
        return content
            .Replace(
                "<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\modules\\BasicManagement\\src\\EHR.Journey.BasicManagement.Application\\EHR.Journey.BasicManagement.Application.csproj\"/>",
                "<PackageReference Include=\"EHR.Journey.BasicManagement.Application\"/>")
            .Replace(
                "<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\modules\\BasicManagement\\src\\EHR.Journey.BasicManagement.Application.Contracts\\EHR.Journey.BasicManagement.Application.Contracts.csproj\"/>",
                "<PackageReference Include=\"EHR.Journey.BasicManagement.Application.Contracts\"/>")
            .Replace("<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\modules\\BasicManagement\\src\\EHR.Journey.BasicManagement.Domain\\EHR.Journey.BasicManagement.Domain.csproj\"/>",
                "<PackageReference Include=\"EHR.Journey.BasicManagement.Domain\"/>")
            .Replace(
                "<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\modules\\BasicManagement\\src\\EHR.Journey.BasicManagement.Domain.Shared\\EHR.Journey.BasicManagement.Domain.Shared.csproj\"/>",
                "<PackageReference Include=\"EHR.Journey.BasicManagement.Domain.Shared\"/>")
            .Replace(
                "<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\modules\\BasicManagement\\src\\EHR.Journey.BasicManagement.EntityFrameworkCore\\EHR.Journey.BasicManagement.EntityFrameworkCore.csproj\"/>",
                "<PackageReference Include=\"EHR.Journey.BasicManagement.EntityFrameworkCore\"/>")
            .Replace(
                "<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\modules\\BasicManagement\\src\\EHR.Journey.BasicManagement.FreeSqlRepository\\EHR.Journey.BasicManagement.FreeSqlRepository.csproj\"/>",
                "<PackageReference Include=\"EHR.Journey.FreeSqlRepository\"/>")
            .Replace("<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\modules\\BasicManagement\\src\\EHR.Journey.BasicManagement.HttpApi\\EHR.Journey.BasicManagement.HttpApi.csproj\"/>",
                "<PackageReference Include=\"EHR.Journey.BasicManagement.HttpApi\"/>")
            .Replace(
                "<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\modules\\BasicManagement\\src\\EHR.Journey.BasicManagement.HttpApi.Client\\EHR.Journey.BasicManagement.HttpApi.Client.csproj\"/>",
                "<PackageReference Include=\"EHR.Journey.BasicManagement.HttpApi.Client\"/>");
    }

    public static string ReplacePackageReferenceDataDictionaryManagement(this string content)
    {
        return content
            .Replace(
                "<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\modules\\DataDictionaryManagement\\src\\EHR.Journey.DataDictionaryManagement.Application\\EHR.Journey.DataDictionaryManagement.Application.csproj\"/>",
                "<PackageReference Include=\"EHR.Journey.DataDictionaryManagement.Application\"/>")
            .Replace(
                "<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\modules\\DataDictionaryManagement\\src\\EHR.Journey.DataDictionaryManagement.Application.Contracts\\EHR.Journey.DataDictionaryManagement.Application.Contracts.csproj\"/>",
                "<PackageReference Include=\"EHR.Journey.DataDictionaryManagement.Application.Contracts\"/>")
            .Replace(
                "<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\modules\\DataDictionaryManagement\\src\\EHR.Journey.DataDictionaryManagement.Domain\\EHR.Journey.DataDictionaryManagement.Domain.csproj\"/>",
                "<PackageReference Include=\"EHR.Journey.DataDictionaryManagement.Domain\"/>")
            .Replace(
                "<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\modules\\DataDictionaryManagement\\src\\EHR.Journey.DataDictionaryManagement.Domain.Shared\\EHR.Journey.DataDictionaryManagement.Domain.Shared.csproj\"/>",
                "<PackageReference Include=\"EHR.Journey.DataDictionaryManagement.Domain.Shared\"/>")
            .Replace(
                "<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\modules\\DataDictionaryManagement\\src\\EHR.Journey.DataDictionaryManagement.EntityFrameworkCore\\EHR.Journey.DataDictionaryManagement.EntityFrameworkCore.csproj\"/>",
                "<PackageReference Include=\"EHR.Journey.DataDictionaryManagement.EntityFrameworkCore\"/>")
            .Replace(
                "<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\modules\\DataDictionaryManagement\\src\\EHR.Journey.DataDictionaryManagement.FreeSqlRepository\\EHR.Journey.DataDictionaryManagement.FreeSqlRepository.csproj\"/>",
                "<PackageReference Include=\"EHR.Journey.FreeSqlRepository\"/>")
            .Replace(
                "<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\modules\\DataDictionaryManagement\\src\\EHR.Journey.DataDictionaryManagement.HttpApi\\EHR.Journey.DataDictionaryManagement.HttpApi.csproj\"/>",
                "<PackageReference Include=\"EHR.Journey.DataDictionaryManagement.HttpApi\"/>")
            .Replace(
                "<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\modules\\DataDictionaryManagement\\src\\EHR.Journey.DataDictionaryManagement.HttpApi.Client\\EHR.Journey.DataDictionaryManagement.HttpApi.Client.csproj\"/>",
                "<PackageReference Include=\"EHR.Journey.DataDictionaryManagement.HttpApi.Client\"/>");
    }

    public static string ReplacePackageReferenceFileManagement(this string content)
    {
        return content
            .Replace(
                "<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\modules\\FileManagement\\src\\EHR.Journey.FileManagement.Application\\EHR.Journey.FileManagement.Application.csproj\"/>",
                "<PackageReference Include=\"EHR.Journey.FileManagement.Application\"/>")
            .Replace(
                "<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\modules\\FileManagement\\src\\EHR.Journey.FileManagement.Application.Contracts\\EHR.Journey.FileManagement.Application.Contracts.csproj\"/>",
                "<PackageReference Include=\"EHR.Journey.FileManagement.Application.Contracts\"/>")
            .Replace("<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\modules\\FileManagement\\src\\EHR.Journey.FileManagement.Domain\\EHR.Journey.FileManagement.Domain.csproj\"/>",
                "<PackageReference Include=\"EHR.Journey.FileManagement.Domain\"/>")
            .Replace(
                "<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\modules\\FileManagement\\src\\EHR.Journey.FileManagement.Domain.Shared\\EHR.Journey.FileManagement.Domain.Shared.csproj\"/>",
                "<PackageReference Include=\"EHR.Journey.FileManagement.Domain.Shared\"/>")
            .Replace(
                "<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\modules\\FileManagement\\src\\EHR.Journey.FileManagement.EntityFrameworkCore\\EHR.Journey.FileManagement.EntityFrameworkCore.csproj\"/>",
                "<PackageReference Include=\"EHR.Journey.FileManagement.EntityFrameworkCore\"/>")
            .Replace(
                "<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\modules\\FileManagement\\src\\EHR.Journey.FileManagement.FreeSqlRepository\\EHR.Journey.FileManagement.FreeSqlRepository.csproj\"/>",
                "<PackageReference Include=\"EHR.Journey.FreeSqlRepository\"/>")
            .Replace("<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\modules\\FileManagement\\src\\EHR.Journey.FileManagement.HttpApi\\EHR.Journey.FileManagement.HttpApi.csproj\"/>",
                "<PackageReference Include=\"EHR.Journey.FileManagement.HttpApi\"/>")
            .Replace(
                "<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\modules\\FileManagement\\src\\EHR.Journey.FileManagement.HttpApi.Client\\EHR.Journey.FileManagement.HttpApi.Client.csproj\"/>",
                "<PackageReference Include=\"EHR.Journey.FileManagement.HttpApi.Client\"/>");
    }

    public static string ReplacePackageReferenceLanguageManagement(this string content)
    {
        return content
            .Replace(
                "<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\modules\\LanguageManagement\\src\\EHR.Journey.LanguageManagement.Application\\EHR.Journey.LanguageManagement.Application.csproj\"/>",
                "<PackageReference Include=\"EHR.Journey.LanguageManagement.Application\"/>")
            .Replace(
                "<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\modules\\LanguageManagement\\src\\EHR.Journey.LanguageManagement.Application.Contracts\\EHR.Journey.LanguageManagement.Application.Contracts.csproj\"/>",
                "<PackageReference Include=\"EHR.Journey.LanguageManagement.Application.Contracts\"/>")
            .Replace(
                "<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\modules\\LanguageManagement\\src\\EHR.Journey.LanguageManagement.Domain\\EHR.Journey.LanguageManagement.Domain.csproj\"/>",
                "<PackageReference Include=\"EHR.Journey.LanguageManagement.Domain\"/>")
            .Replace(
                "<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\modules\\LanguageManagement\\src\\EHR.Journey.LanguageManagement.Domain.Shared\\EHR.Journey.LanguageManagement.Domain.Shared.csproj\"/>",
                "<PackageReference Include=\"EHR.Journey.LanguageManagement.Domain.Shared\"/>")
            .Replace(
                "<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\modules\\LanguageManagement\\src\\EHR.Journey.LanguageManagement.EntityFrameworkCore\\EHR.Journey.LanguageManagement.EntityFrameworkCore.csproj\"/>",
                "<PackageReference Include=\"EHR.Journey.LanguageManagement.EntityFrameworkCore\"/>")
            .Replace(
                "<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\modules\\LanguageManagement\\src\\EHR.Journey.LanguageManagement.FreeSqlRepository\\EHR.Journey.LanguageManagement.FreeSqlRepository.csproj\"/>",
                "<PackageReference Include=\"EHR.Journey.FreeSqlRepository\"/>")
            .Replace(
                "<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\modules\\LanguageManagement\\src\\EHR.Journey.LanguageManagement.HttpApi\\EHR.Journey.LanguageManagement.HttpApi.csproj\"/>",
                "<PackageReference Include=\"EHR.Journey.LanguageManagement.HttpApi\"/>")
            .Replace(
                "<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\modules\\LanguageManagement\\src\\EHR.Journey.LanguageManagement.HttpApi.Client\\EHR.Journey.LanguageManagement.HttpApi.Client.csproj\"/>",
                "<PackageReference Include=\"EHR.Journey.LanguageManagement.HttpApi.Client\"/>");
    }

    public static string ReplacePackageReferenceNotificationManagement(this string content)
    {
        return content
            .Replace(
                "<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\modules\\NotificationManagement\\src\\EHR.Journey.NotificationManagement.Application\\EHR.Journey.NotificationManagement.Application.csproj\"/>",
                "<PackageReference Include=\"EHR.Journey.NotificationManagement.Application\"/>")
            .Replace(
                "<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\modules\\NotificationManagement\\src\\EHR.Journey.NotificationManagement.Application.Contracts\\EHR.Journey.NotificationManagement.Application.Contracts.csproj\"/>",
                "<PackageReference Include=\"EHR.Journey.NotificationManagement.Application.Contracts\"/>")
            .Replace(
                "<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\modules\\NotificationManagement\\src\\EHR.Journey.NotificationManagement.Domain\\EHR.Journey.NotificationManagement.Domain.csproj\"/>",
                "<PackageReference Include=\"EHR.Journey.NotificationManagement.Domain\"/>")
            .Replace(
                "<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\modules\\NotificationManagement\\src\\EHR.Journey.NotificationManagement.Domain.Shared\\EHR.Journey.NotificationManagement.Domain.Shared.csproj\"/>",
                "<PackageReference Include=\"EHR.Journey.NotificationManagement.Domain.Shared\"/>")
            .Replace(
                "<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\modules\\NotificationManagement\\src\\EHR.Journey.NotificationManagement.EntityFrameworkCore\\EHR.Journey.NotificationManagement.EntityFrameworkCore.csproj\"/>",
                "<PackageReference Include=\"EHR.Journey.NotificationManagement.EntityFrameworkCore\"/>")
            .Replace(
                "<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\modules\\NotificationManagement\\src\\EHR.Journey.NotificationManagement.FreeSqlRepository\\EHR.Journey.NotificationManagement.FreeSqlRepository.csproj\"/>",
                "<PackageReference Include=\"EHR.Journey.FreeSqlRepository\"/>")
            .Replace(
                "<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\modules\\NotificationManagement\\src\\EHR.Journey.NotificationManagement.HttpApi\\EHR.Journey.NotificationManagement.HttpApi.csproj\"/>",
                "<PackageReference Include=\"EHR.Journey.NotificationManagement.HttpApi\"/>")
            .Replace(
                "<ProjectReference Include=\"..\\..\\..\\..\\..\\aspnet-core\\modules\\NotificationManagement\\src\\EHR.Journey.NotificationManagement.HttpApi.Client\\EHR.Journey.NotificationManagement.HttpApi.Client.csproj\"/>",
                "<PackageReference Include=\"EHR.Journey.NotificationManagement.HttpApi.Client\"/>");
    }

    public static string ReplaceEHRPackageVersion(this string context, string version)
    {
        return context.Replace("8.0.0.0", version);
    }
}