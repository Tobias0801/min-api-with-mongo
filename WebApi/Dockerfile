FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /build
COPY . .
# installiert nuget packages
RUN  dotnet restore
# -c: konfiguration, -o: output
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
LABEL description="Minimal API with MongoDB"
LABEL author="Tobias Kuhn"
WORKDIR /app
COPY --from=build-env /build/out .
ENV ASPNETCORE_URLS="http://*:5001"
EXPOSE 5001
ENTRYPOINT [ "dotnet", "WebApi.dll" ]