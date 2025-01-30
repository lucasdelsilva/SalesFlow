# Documentação da API SalesFlow

## Visão Geral
SalesFlow é uma API RESTful construída com C# e ASP.NET Core para gerenciamento de vendas. O sistema gerencia pedidos de clientes, produtos e acompanhamento de vendas, com foco em escalabilidade e manutenibilidade.
API REST em .NET 8 para gestão de comandas e mesas de restaurante.

# Requisitos de Instalação
- Docker Desktop
- MySQL (Gerenciamento do banco)
  
# Opcional
- Dbeaver (Utilização para o gerenciamento de banco de dados)

# Instalação rápida:
1. Clone o repositório
2. Acesse a pasta `.docker`
3. Execute `docker-compose up -d`
4. Configure a conexão MySQL:
```
Host: localhost
Database: myapp_db
Usuário: user
Senha: userpass
```
5. Update na migration para criação das Tabelas:
```bash
cd src
dotnet ef database update --project .\SalesFlow.Infrastructure\ --startup-project .\SalesFlow.API\
```
6. Execute a aplicação.
7. Pronto! A aplicação já está funcionando.

## Endpoints da API

### Autenticação e Autorização
#### Registro de Usuário
- **POST** `api/user/register`
- Registra um novo usuário no sistema
- Corpo da Requisição:
```json
{
  "name": "string",
  "email": "user@gmail.com",
  "password": "Exemplo@123",
  "passwordConfirm": "Exemplo@123"
}
```

#### Login
- **POST** `api/user/login`
- Autentica um usuário existente
- Corpo da Requisição:
```json
{
  "email": "string",
  "password": "Exemplo@123"
}
```
- Resposta (200 OK):
```json
{
  "name": "string",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

#### Utilização do Token
- O token JWT deve ser incluído no header de todas as requisições autenticadas:
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

## Headers
### Opcional
- Para erros em português utilizar:
{ key: Accept-Language; Value : pt-BR}

- Para erros em inglês não precisa utilizar nada (A aplicação reconhece EN de forma automatica), porém caso queira passar utilizar:
{ key: Accept-Language; Value : en}

### Vendas
#### Criar Venda
- **POST** `/api/sales`
- Cria um novo registro de venda
- Corpo da Requisição:
```json
{
  "customerName": "string",
  "items": [
    {
      "productName": "string",
      "quantity": "integer",
      "unitPrice": "decimal"
    }
  ]
}
```
- Resposta: 201 Created com ID da venda

#### Atualizar Venda
- **PUT** `/api/sales/{id}`
- Atualiza uma venda existente
- Resposta: 200 OK
```json
{
  "customerName": "string"
}
```
#### Atualizar Item à Venda
- **PUT** `/api/sales/{id}/items{idVenda}`
- Atualiza os itens a uma venda existente
- Corpo da Requisição:
```json
{
  "productName": "string",
  "quantity": "integer",
  "unitPrice": "decimal"
}
```
- Resposta: 200 OK

#### Excluir Venda
- **DELETE** `/api/sales/{id}`
- Remove uma venda do sistema
- Resposta: 204 No Content

#### Listar Todas as Vendas
- **GET** `/api/sales`
- Recupera todos os registros de vendas
- Resposta: 200 OK com lista de vendas

#### Buscar Venda por ID
- **GET** `/api/sales/{id}`
- Recupera um registro específico de venda
- Resposta: 200 OK com detalhes da venda

## Logs
- Logging usando Serilog
- Informação: Operações regulares
- Body (request)
- Erro: Exceções do sistema

## Testes Unitários & Testes de integração
- UseCases
- Lógica de dominio
- Endpoints da API
- Repositórios
- Banco em memória
