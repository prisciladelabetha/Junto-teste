# Junto - Teste

Para rodar a aplicação:

1. `docker-compose up -d` - sobe os containers do PostgreSQL (banco na porta 15432, pgAdmin na porta 16543).
2. `cd JuntoUserApplication`
3. `dotnet restore`
4. `dotnet run` - servidor sobe na porta 5000

Para rodar os testes:

1. `cd JuntoUserApplication.Tests`
2. `dotnet restore`
3. `dotnet test`

## Endpoints

- `api/user/signup` - recebe `username` e `password` e retorna dados para autenticação.
- `api/user/login` - recebe `username` e `password`.
- `api/user/change-password` - recebe `oldPassword` e `newPassword`; precisa do Bearer token no header.
