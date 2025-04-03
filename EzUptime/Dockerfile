FROM mcr.microsoft.com/dotnet/aspnet:8.0.14-bookworm-slim-arm64v8 AS base

WORKDIR /app

COPY . .

EXPOSE 5050

ENTRYPOINT ["./EzUptime", "--urls=http://*:5050"]
