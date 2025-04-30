FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Configure o diretório de trabalho no container
WORKDIR /app

# Copie todos os arquivos do projeto para o container
COPY . .

# Compile o projeto
RUN dotnet build --configuration Release --output /tmp/codecrafters-build-http-server-csharp codecrafters-http-server.csproj

# Use uma imagem base menor para execução
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS runtime

# Configure o diretório de trabalho no container
WORKDIR /app

# Copie os arquivos compilados para o container
COPY --from=build /tmp/codecrafters-build-http-server-csharp /app

# Exponha a porta usada pelo servidor
EXPOSE 4221

# Comando para executar o programa
ENTRYPOINT ["/app/codecrafters-http-server"]
CMD ["--directory", "/tmp/data/codecrafters.io/http-server-tester/"]