FROM ghcr.io/openfaas/of-watchdog:0.9.9 as watchdog

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
COPY --from=watchdog /fwatchdog /usr/bin/
RUN chmod +x /usr/bin/fwatchdog
ENV DOTNET_CLI_TELEMETRY_OPTOUT 1
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ENV DOTNET_CLI_TELEMETRY_OPTOUT 1
WORKDIR /src
COPY ["root/root.csproj", "root/"]
COPY ["function/function.csproj", "function/"]
RUN dotnet restore "root/root.csproj"
COPY . .
WORKDIR "/src/root"
RUN dotnet build "root.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "root.csproj" --no-restore -c Release -o /app/publish /p:UseAppHost=false 

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build /src/function/wwwroot/ wwwroot/

ENV mode="http"
ENV fprocess="dotnet root.dll"
ENV upstream_url="http://127.0.0.1:80"

ENTRYPOINT ["fwatchdog"]