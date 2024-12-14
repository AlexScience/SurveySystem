# Dockerfile

# Используем базовый образ для ASP.NET Core приложения
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Используем образ для сборки приложения
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SurveySystem/SurveySystem.csproj", "SurveySystem/"]
RUN dotnet restore "SurveySystem/SurveySystem.csproj"
COPY . .
WORKDIR "/src/SurveySystem"
RUN dotnet build "SurveySystem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SurveySystem.csproj" -c Release -o /app/publish

# Настроим конечный образ
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SurveySystem.dll"]

# Определите порты, которые будут доступны для контейнера
ENV ASPNETCORE_URLS=https://+:443;http://+:80
