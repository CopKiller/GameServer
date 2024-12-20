# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copiar os arquivos do projeto
COPY . ./

# Restaurar dependências e compilar o projeto
RUN dotnet restore
RUN dotnet publish Server/Server/Server.csproj -c Release -f net9.0 -o /app/out

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/runtime:9.0
WORKDIR /app

# Copiar os arquivos publicados para o container final
COPY --from=build /app/out .

# Expor as portas necessárias (se seu servidor usa uma porta específica)
EXPOSE 4001

# Comando para iniciar o servidor
ENTRYPOINT ["dotnet", "Server.dll"]
