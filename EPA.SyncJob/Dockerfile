FROM hub.nexdev.net/base/dotnet/core/sdk:9.0-alpine AS builder

WORKDIR /src
# Copy solution and project files
COPY ["nuget.config", "."]
COPY EPA.SyncJob/*.csproj EPA.SyncJob/

# Copy the rest of the source code
RUN dotnet nuget add source https://nuget.nexdev.net/api/v2
# Restore dependencies

RUN dotnet restore -v n -r linux-x64 EPA.SyncJob/EPA.SyncJob.csproj

# Copy source codes

COPY EPA.SyncJob/ EPA.SyncJob/

# Publish the application
WORKDIR /src/EPA.SyncJob
RUN dotnet publish -r linux-x64 --self-contained false -c Release -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:9.0-alpine AS base

WORKDIR /app
COPY --from=builder /app .

ENV TZ="America/La_Paz"
ENV COMPlus_EnableDiagnostics=0

# Don’t run as root
RUN adduser --disabled-password \
    --home /app \
    --gecos '' dotnetuser && chown -R dotnetuser /app && chown -R dotnetuser /tmp
USER dotnetuser
#

ENTRYPOINT ["dotnet", "EPA.SyncJob.dll"]
