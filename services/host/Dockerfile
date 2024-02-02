#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Build.targets", "."]
COPY ["NuGet.Config", "."]
COPY ["services/host/EHR.Journey.HttpApi.Host/EHR.Journey.HttpApi.Host.csproj", "services/host/EHR.Journey.HttpApi.Host/"]
COPY ["frameworks/src/EHR.Journey.CAP.EntityFrameworkCore/EHR.Journey.CAP.EntityFrameworkCore.csproj", "frameworks/src/EHR.Journey.CAP.EntityFrameworkCore/"]
COPY ["frameworks/src/EHR.Journey.CAP/EHR.Journey.CAP.csproj", "frameworks/src/EHR.Journey.CAP/"]
COPY ["frameworks/src/EHR.Journey.Localization/EHR.Journey.Localization.csproj", "frameworks/src/EHR.Journey.Localization/"]
COPY ["shared/EHR.Journey.Shared.Hosting.Microservices/EHR.Journey.Shared.Hosting.Microservices.csproj", "shared/EHR.Journey.Shared.Hosting.Microservices/"]
COPY ["frameworks/src/EHR.Journey.Core/EHR.Journey.Core.csproj", "frameworks/src/EHR.Journey.Core/"]
COPY ["services/src/EHR.Journey.Application/EHR.Journey.Application.csproj", "services/src/EHR.Journey.Application/"]
COPY ["modules/BasicManagement/src/EHR.Journey.BasicManagement.Application/EHR.Journey.BasicManagement.Application.csproj", "modules/BasicManagement/src/EHR.Journey.BasicManagement.Application/"]
COPY ["modules/BasicManagement/src/EHR.Journey.BasicManagement.Application.Contracts/EHR.Journey.BasicManagement.Application.Contracts.csproj", "modules/BasicManagement/src/EHR.Journey.BasicManagement.Application.Contracts/"]
COPY ["modules/BasicManagement/src/EHR.Journey.BasicManagement.Domain.Shared/EHR.Journey.BasicManagement.Domain.Shared.csproj", "modules/BasicManagement/src/EHR.Journey.BasicManagement.Domain.Shared/"]
COPY ["modules/BasicManagement/src/EHR.Journey.BasicManagement.Domain/EHR.Journey.BasicManagement.Domain.csproj", "modules/BasicManagement/src/EHR.Journey.BasicManagement.Domain/"]
COPY ["modules/DataDictionaryManagement/src/EHR.Journey.DataDictionaryManagement.Application/EHR.Journey.DataDictionaryManagement.Application.csproj", "modules/DataDictionaryManagement/src/EHR.Journey.DataDictionaryManagement.Application/"]
COPY ["modules/DataDictionaryManagement/src/EHR.Journey.DataDictionaryManagement.Application.Contracts/EHR.Journey.DataDictionaryManagement.Application.Contracts.csproj", "modules/DataDictionaryManagement/src/EHR.Journey.DataDictionaryManagement.Application.Contracts/"]
COPY ["modules/DataDictionaryManagement/src/EHR.Journey.DataDictionaryManagement.Domain.Shared/EHR.Journey.DataDictionaryManagement.Domain.Shared.csproj", "modules/DataDictionaryManagement/src/EHR.Journey.DataDictionaryManagement.Domain.Shared/"]
COPY ["modules/DataDictionaryManagement/src/EHR.Journey.DataDictionaryManagement.Domain/EHR.Journey.DataDictionaryManagement.Domain.csproj", "modules/DataDictionaryManagement/src/EHR.Journey.DataDictionaryManagement.Domain/"]
COPY ["modules/Employee/src/Employee/Employee.csproj", "modules/Employee/src/Employee/"]
COPY ["services/src/EHR.Journey.Domain/EHR.Journey.Domain.csproj", "services/src/EHR.Journey.Domain/"]
COPY ["modules/NotificationManagement/src/EHR.Journey.NotificationManagement.Domain/EHR.Journey.NotificationManagement.Domain.csproj", "modules/NotificationManagement/src/EHR.Journey.NotificationManagement.Domain/"]
COPY ["modules/NotificationManagement/src/EHR.Journey.NotificationManagement.Domain.Shared/EHR.Journey.NotificationManagement.Domain.Shared.csproj", "modules/NotificationManagement/src/EHR.Journey.NotificationManagement.Domain.Shared/"]
COPY ["modules/LanguageManagement/src/EHR.Journey.LanguageManagement.Domain/EHR.Journey.LanguageManagement.Domain.csproj", "modules/LanguageManagement/src/EHR.Journey.LanguageManagement.Domain/"]
COPY ["modules/LanguageManagement/src/EHR.Journey.LanguageManagement.Domain.Shared/EHR.Journey.LanguageManagement.Domain.Shared.csproj", "modules/LanguageManagement/src/EHR.Journey.LanguageManagement.Domain.Shared/"]
COPY ["services/src/EHR.Journey.Domain.Shared/EHR.Journey.Domain.Shared.csproj", "services/src/EHR.Journey.Domain.Shared/"]
COPY ["modules/NotificationManagement/src/EHR.Journey.NotificationManagement.Application/EHR.Journey.NotificationManagement.Application.csproj", "modules/NotificationManagement/src/EHR.Journey.NotificationManagement.Application/"]
COPY ["modules/NotificationManagement/src/EHR.Journey.NotificationManagement.Application.Contracts/EHR.Journey.NotificationManagement.Application.Contracts.csproj", "modules/NotificationManagement/src/EHR.Journey.NotificationManagement.Application.Contracts/"]
COPY ["modules/LanguageManagement/src/EHR.Journey.LanguageManagement.Application/EHR.Journey.LanguageManagement.Application.csproj", "modules/LanguageManagement/src/EHR.Journey.LanguageManagement.Application/"]
COPY ["modules/LanguageManagement/src/EHR.Journey.LanguageManagement.Application.Contracts/EHR.Journey.LanguageManagement.Application.Contracts.csproj", "modules/LanguageManagement/src/EHR.Journey.LanguageManagement.Application.Contracts/"]
COPY ["services/src/EHR.Journey.Application.Contracts/EHR.Journey.Application.Contracts.csproj", "services/src/EHR.Journey.Application.Contracts/"]
COPY ["services/src/EHR.Journey.FreeSqlRepository/EHR.Journey.FreeSqlRepository.csproj", "services/src/EHR.Journey.FreeSqlRepository/"]
COPY ["services/src/EHR.Journey.EntityFrameworkCore/EHR.Journey.EntityFrameworkCore.csproj", "services/src/EHR.Journey.EntityFrameworkCore/"]
COPY ["modules/BasicManagement/src/EHR.Journey.BasicManagement.EntityFrameworkCore/EHR.Journey.BasicManagement.EntityFrameworkCore.csproj", "modules/BasicManagement/src/EHR.Journey.BasicManagement.EntityFrameworkCore/"]
COPY ["modules/DataDictionaryManagement/src/EHR.Journey.DataDictionaryManagement.EntityFrameworkCore/EHR.Journey.DataDictionaryManagement.EntityFrameworkCore.csproj", "modules/DataDictionaryManagement/src/EHR.Journey.DataDictionaryManagement.EntityFrameworkCore/"]
COPY ["modules/NotificationManagement/src/EHR.Journey.NotificationManagement.EntityFrameworkCore/EHR.Journey.NotificationManagement.EntityFrameworkCore.csproj", "modules/NotificationManagement/src/EHR.Journey.NotificationManagement.EntityFrameworkCore/"]
COPY ["modules/LanguageManagement/src/EHR.Journey.LanguageManagement.EntityFrameworkCore/EHR.Journey.LanguageManagement.EntityFrameworkCore.csproj", "modules/LanguageManagement/src/EHR.Journey.LanguageManagement.EntityFrameworkCore/"]
COPY ["services/src/EHR.Journey.HttpApi/EHR.Journey.HttpApi.csproj", "services/src/EHR.Journey.HttpApi/"]
COPY ["modules/BasicManagement/src/EHR.Journey.BasicManagement.HttpApi/EHR.Journey.BasicManagement.HttpApi.csproj", "modules/BasicManagement/src/EHR.Journey.BasicManagement.HttpApi/"]
COPY ["modules/DataDictionaryManagement/src/EHR.Journey.DataDictionaryManagement.HttpApi/EHR.Journey.DataDictionaryManagement.HttpApi.csproj", "modules/DataDictionaryManagement/src/EHR.Journey.DataDictionaryManagement.HttpApi/"]
COPY ["modules/NotificationManagement/src/EHR.Journey.NotificationManagement.HttpApi/EHR.Journey.NotificationManagement.HttpApi.csproj", "modules/NotificationManagement/src/EHR.Journey.NotificationManagement.HttpApi/"]
COPY ["modules/LanguageManagement/src/EHR.Journey.LanguageManagement.HttpApi/EHR.Journey.LanguageManagement.HttpApi.csproj", "modules/LanguageManagement/src/EHR.Journey.LanguageManagement.HttpApi/"]
RUN dotnet restore "./services/host/EHR.Journey.HttpApi.Host/./EHR.Journey.HttpApi.Host.csproj"
COPY . .
WORKDIR "/src/services/host/EHR.Journey.HttpApi.Host"
RUN dotnet build "./EHR.Journey.HttpApi.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EHR.Journey.HttpApi.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EHR.Journey.HttpApi.Host.dll"]

