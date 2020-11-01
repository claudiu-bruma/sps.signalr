#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["SPS.SignalrClient/SPS.SignalrClient.csproj", "SPS.SignalrClient/"]
RUN dotnet restore "SPS.SignalrClient/SPS.SignalrClient.csproj"
COPY . .
WORKDIR "/src/SPS.SignalrClient"
RUN dotnet build "SPS.SignalrClient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SPS.SignalrClient.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
VOLUME "D:\Temp\app\logs":"/app/logs"
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SPS.SignalrClient.dll"]