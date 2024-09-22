FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_URLS=http://+:80;https://+:443

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
RUN mkdir /src
WORKDIR /src
RUN mkdir /src/SuneDoes.UI
COPY SuneDoes.UI.csproj SuneDoes.UI/
WORKDIR "/src/SuneDoes.UI/"
RUN dotnet restore "SuneDoes.UI.csproj"
COPY . .
WORKDIR "/src/SuneDoes.UI"
RUN dotnet build "SuneDoes.UI.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "SuneDoes.UI.csproj" -c $configuration -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SuneDoes.UI.dll"]
