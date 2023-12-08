FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
RUN apt-get update && apt-get install -y libgdiplus
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0.301-alpine3.13-amd64 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "Menchul.MCode/Menchul.MCode.Api/Menchul.MCode.Api.csproj"
WORKDIR "/src/Menchul.MCode/Menchul.MCode.Api"
RUN dotnet build "Menchul.MCode.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Menchul.MCode.Api.csproj" -c Release -o /app/publish
RUN dotnet tool install --tool-path /app/publish/external_tools dotnet-certificate-tool

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
VOLUME /app/logs
ENTRYPOINT ["dotnet", "Menchul.MCode.Api.dll"]