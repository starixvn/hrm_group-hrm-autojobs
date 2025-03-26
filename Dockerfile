FROM hub.nexdev.net/base/dotnet/core/sdk:9.0-alpine AS builder

WORKDIR /src
# Copy solution and project files
COPY ./src/HRM.LetStalk.Core/*.csproj ./src/HRM.LetStalk.Core/
COPY ./src/HRM.Job/*.csproj ./src/HRM.Job/
COPY ./HRM.AutoJobs/*.csproj ./HRM.AutoJobs/

# Copy the rest of the source code
RUN dotnet nuget add source https://nuget.nexdev.net/api/v2
# Restore dependencies
RUN dotnet restore -v n -r linux-x64 HRM.AutoJobs/HRM.AutoJobs.csproj

# Copy source codes
COPY ./src/HRM.LetStalk.Core/ ./src/HRM.LetStalk.Core/
COPY ./src/HRM.Job/ ./src/HRM.Job/
COPY ./HRM.AutoJobs/ ./HRM.AutoJobs/

# Publish the application
WORKDIR /src/HRM.AutoJobs
RUN dotnet publish -r linux-x64 --self-contained false -c Release -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:9.0-alpine AS base

WORKDIR /app
COPY --from=builder /app .

ENV TZ="America/La_Paz"
ENV COMPlus_EnableDiagnostics=0

# Dont run as root
RUN adduser --disabled-password \
    --home /app \
    --gecos '' dotnetuser && chown -R dotnetuser /app && chown -R dotnetuser /tmp
USER dotnetuser
#

ENTRYPOINT ["dotnet", "HRM.AutoJobs.dll"]
