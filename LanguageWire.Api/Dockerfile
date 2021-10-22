FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["LanguageWire.Api/LanguageWire.Api.csproj", "LanguageWire.Api/"]
RUN dotnet restore "LanguageWire.Api/LanguageWire.Api.csproj"
COPY . .
WORKDIR "/src/LanguageWire.Api"
RUN dotnet build "LanguageWire.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LanguageWire.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LanguageWire.Api.dll"]
