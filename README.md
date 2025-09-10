# Ambev Developer Evaluation

## Configuração de Ambiente

- **Pré-requisitos:**
  - .NET 8 SDK
  - Node.js 20+
  - Docker e Docker Compose
  - Postgresql (pode ser via Docker)
  - MongoDB (pode ser via Docker)
  - RabbitMQ (pode ser via Docker)

- **Variáveis de ambiente:**
  - Configure as strings de conexão do MongoDB e RabbitMQ nos arquivos de configuração do backend (`appsettings.json`).

## Comandos para Rodar a Aplicação

### Backend
```sh
cd backend
# Restaurar pacotes
 dotnet restore
# Gerar migrations e aplicar estrutura do banco
 dotnet ef database update
# Rodar aplicação
 dotnet run --project src/Ambev.DeveloperEvaluation.WebApi
```

### Frontend
```sh
cd frontend/ambev-frontend
npm install
npm start
```

### Docker Compose
```sh
cd backend
# Sobe MongoDB, RabbitMQ, PostgreSql e WebApi
 docker-compose up -d
# Para e remove volumes
 docker-compose down -v
```

## Tecnologias Utilizadas
- **Backend:** .NET Core 8, C#, MediatR, Rebus, Serilog, MongoDB, RabbitMQ, Docker, xUnit
- **Frontend:** Angular 20, Bootstrap, SCSS
- **Testes:** xUnit (unitários, integração, funcional)
- **Outros:** Swagger, Postman, Serilog

## Decisões de Arquitetura
- **.NET Core 8:** Modernidade, performance e suporte a containers.
- **RabbitMQ:** Utilizado para orquestrar eventos de venda, permitindo integração com serviços externos (ex: envio de email, processamento de cartão).
- **MongoDB:** Persistência de eventos de venda para auditoria e relatórios, flexibilidade de schema.
- **Rebus:** Abstração para mensageria, desacoplando o fluxo de eventos.
- **Serilog:** Logging estruturado, fácil visualização no console e integração com outros sinks.
- **Angular + Bootstrap:** Interface moderna, responsiva e produtiva para CRUD e fluxo de vendas.

## Fluxo da Aplicação
1. **Cadastro de Usuário:**
   - Usuário é criado via frontend ou API, validado e persistido no banco relacional.
2. **Cadastro de Produto:**
   - Produtos são cadastrados e ficam disponíveis para seleção no carrinho.
3. **Carrinho:**
   - Usuário adiciona produtos ao carrinho, define quantidade e pode finalizar o carrinho.
4. **Venda:**
   - Carrinho é convertido em venda, disparando evento RabbitMQ para processamento assíncrono (ex: email, cartão).
   - Evento de venda é gravado no MongoDB para auditoria/relatórios.

## RabbitMQ
- Utilizado para publicar eventos de venda finalizada.
- Possibilita integração com microserviços para envio de email, processamento de pagamento, etc.

## MongoDB
- Armazena eventos de venda para auditoria e geração de relatórios.
- Permite consultas flexíveis e escaláveis.

## Logs com Serilog
- Todos os eventos e ações relevantes são logados no console via Serilog.
- Para visualizar, basta acompanhar o terminal do backend.

## Testes e Validação
- **Swagger:** Disponível em `/swagger` para testar endpoints da API.
- **Postman:** Coleção disponível para importação e testes manuais (Ambev.DeveloperEvaluation.postman_collection.json).
- **Angular:** Interface completa para testes de fluxo e validação visual.

## Executando Testes
### Unitários
```sh
cd backend/tests/Ambev.DeveloperEvaluation.Unit
 dotnet test
```
### Integração
```sh
cd backend/tests/Ambev.DeveloperEvaluation.Integration
 dotnet test
```
### Funcionais
```sh
cd backend/tests/Ambev.DeveloperEvaluation.Functional
 dotnet test
```

- **Unitários:** Validam regras de negócio isoladas.
- **Integração:** Testam integração entre componentes, banco e serviços.
- **Funcionais:** Simulam cenários completos de uso, do cadastro à venda.

---

Para dúvidas, consulte os arquivos de configuração, documentação dos projetos ou entre em contato com o responsável técnico.
