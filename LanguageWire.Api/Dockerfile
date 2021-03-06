FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
RUN apt-get update -y && apt-get install python3-pip python3 -y && pip3 install requests
WORKDIR /app
EXPOSE 5000

ENV ASPNETCORE_URLS=http://+:5000

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
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
