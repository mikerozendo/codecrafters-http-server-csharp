FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /app

COPY . .

RUN dotnet build --configuration Release --output /tmp/codecrafters-build-http-server-csharp codecrafters-http-server.csproj

FROM mcr.microsoft.com/dotnet/runtime:9.0 AS runtime

WORKDIR /app

COPY --from=build /tmp/codecrafters-build-http-server-csharp /app

EXPOSE 4221

ENTRYPOINT ["/app/codecrafters-http-server"]

#Used for tests at  // only used for tests at https://app.codecrafters.io/courses/http-server/stages/ap6
#CMD ["--directory", "/tmp/data/codecrafters.io/http-server-tester/"]