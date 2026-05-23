# PetFamily API

API RESTful para gerenciamento veterinário que permite registrar tutores, pets, consultas e lembretes de cuidados. Ideal para clínicas veterinárias e proprietários de pets.

---

## Tecnologias

| Tecnologia | Versão |
|---|---|
| .NET | 9.0 |
| ASP.NET Core Web API | 9.0 |
| Entity Framework Core | 9.0.5 |
| Oracle.EntityFrameworkCore | 9.23.60 |
| Swashbuckle.AspNetCore | 7.3.1 |
| xUnit + Moq | Testes unitários |

---

## Arquitetura

```
PetFamily.sln
├── src/
│   ├── PetFamily.Domain          # Entidades e Exceptions do domínio
│   ├── PetFamily.Application     # DTOs, Interfaces, Services
│   ├── PetFamily.Infrastructure  # DbContext, Repositories, Migrations
│   └── PetFamily.API             # Controllers, Middleware, Program.cs
└── tests/
    └── PetFamily.Tests           # Testes unitários (xUnit + Moq)
```

**Padrões utilizados:** Clean Architecture · Repository Pattern · Dependency Injection · DTOs · RESTful API · Global Exception Handling

---

## Entidades

| Entidade | Campos |
|---|---|
| Tutor | Id, Nome, Email (único), Telefone |
| Pet | Id, Nome, Especie, Raca, DataNascimento, TutorId (FK) |
| Consulta | Id, DataConsulta, Observacoes, PetId (FK) |
| Lembrete | Id, Titulo, Descricao, DataLembrete, PetId (FK) |

---

## Pré-requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- dotnet-ef CLI: `dotnet tool install --global dotnet-ef`
- Acesso à rede FIAP (necessário para conectar ao Oracle)

---

## Instalação e Execução

### 1. Clonar o repositório

```bash
git clone <url-do-repositorio>
cd NET
```

### 2. Restaurar dependências

```bash
dotnet restore
```

### 3. Executar a API

```bash
dotnet run --project src/PetFamily.API/PetFamily.API.csproj
```

> As migrations são aplicadas automaticamente na inicialização.

### 4. Acessar o Swagger

```
http://localhost:5288/swagger
```

---

## Migrations

### Criar nova migration

```bash
dotnet ef migrations add <NomeDaMigration> --project src/PetFamily.Infrastructure/PetFamily.Infrastructure.csproj --startup-project src/PetFamily.API/PetFamily.API.csproj
```

### Aplicar migrations ao banco

```bash
dotnet ef database update --project src/PetFamily.Infrastructure/PetFamily.Infrastructure.csproj --startup-project src/PetFamily.API/PetFamily.API.csproj
```

### Remover última migration

```bash
dotnet ef migrations remove --project src/PetFamily.Infrastructure/PetFamily.Infrastructure.csproj --startup-project src/PetFamily.API/PetFamily.API.csproj
```

---

## Executar Testes

```bash
dotnet test tests/PetFamily.Tests/PetFamily.Tests.csproj
```

---

## Rotas da API

### Tutores — `/api/tutores`

| Método | Rota | Descrição | Retorno |
|---|---|---|---|
| GET | `/api/tutores` | Lista todos os tutores | 200 |
| GET | `/api/tutores/{id}` | Busca tutor por ID | 200 / 404 |
| GET | `/api/tutores/email/{email}` | Busca tutor por e-mail | 200 / 404 |
| GET | `/api/tutores/buscar/{nome}` | Busca tutores por nome (parcial) | 200 |
| POST | `/api/tutores` | Cria novo tutor | 201 / 400 |
| PUT | `/api/tutores/{id}` | Atualiza tutor | 200 / 400 / 404 |
| DELETE | `/api/tutores/{id}` | Remove tutor | 204 / 404 |

### Pets — `/api/pets`

| Método | Rota | Descrição | Retorno |
|---|---|---|---|
| GET | `/api/pets` | Lista todos os pets | 200 |
| GET | `/api/pets/{id}` | Busca pet por ID | 200 / 404 |
| GET | `/api/pets/tutor/{tutorId}` | Lista pets de um tutor | 200 |
| GET | `/api/pets/especie/{especie}` | Lista pets por espécie | 200 |
| GET | `/api/pets/raca/{raca}` | Lista pets por raça | 200 |
| POST | `/api/pets` | Cria novo pet | 201 / 400 |
| PUT | `/api/pets/{id}` | Atualiza pet | 200 / 400 / 404 |
| DELETE | `/api/pets/{id}` | Remove pet | 204 / 404 |

### Consultas — `/api/consultas`

| Método | Rota | Descrição | Retorno |
|---|---|---|---|
| GET | `/api/consultas` | Lista todas as consultas | 200 |
| GET | `/api/consultas/{id}` | Busca consulta por ID | 200 / 404 |
| GET | `/api/consultas/pet/{petId}` | Lista consultas de um pet | 200 |
| GET | `/api/consultas/futuras` | Lista consultas futuras | 200 |
| GET | `/api/consultas/periodo?inicio=&fim=` | Consultas por período | 200 / 400 |
| POST | `/api/consultas` | Cria nova consulta | 201 / 400 |
| PUT | `/api/consultas/{id}` | Atualiza consulta | 200 / 400 / 404 |
| DELETE | `/api/consultas/{id}` | Remove consulta | 204 / 404 |

### Lembretes — `/api/lembretes`

| Método | Rota | Descrição | Retorno |
|---|---|---|---|
| GET | `/api/lembretes` | Lista todos os lembretes | 200 |
| GET | `/api/lembretes/{id}` | Busca lembrete por ID | 200 / 404 |
| GET | `/api/lembretes/pet/{petId}` | Lista lembretes de um pet | 200 |
| GET | `/api/lembretes/data/{data}` | Lembretes de uma data (yyyy-MM-dd) | 200 / 400 |
| GET | `/api/lembretes/proximos/{dias}` | Lembretes dos próximos N dias | 200 / 400 |
| POST | `/api/lembretes` | Cria novo lembrete | 201 / 400 |
| PUT | `/api/lembretes/{id}` | Atualiza lembrete | 200 / 400 / 404 |
| DELETE | `/api/lembretes/{id}` | Remove lembrete | 204 / 404 |

---

## Exemplos de Requests

### Criar um Tutor

```http
POST /api/tutores
Content-Type: application/json

{
  "nome": "Maria Silva",
  "email": "maria.silva@email.com",
  "telefone": "11999998888"
}
```

### Criar um Pet

```http
POST /api/pets
Content-Type: application/json

{
  "nome": "Rex",
  "especie": "Cachorro",
  "raca": "Labrador",
  "dataNascimento": "2021-03-15T00:00:00",
  "tutorId": 1
}
```

### Criar uma Consulta

```http
POST /api/consultas
Content-Type: application/json

{
  "dataConsulta": "2026-06-10T10:00:00",
  "observacoes": "Vacinação anual",
  "petId": 1
}
```

### Criar um Lembrete

```http
POST /api/lembretes
Content-Type: application/json

{
  "titulo": "Vacina da gripe",
  "descricao": "Aplicar vacina anual contra gripe canina",
  "dataLembrete": "2026-06-01T09:00:00",
  "petId": 1
}
```

### Buscar consultas futuras

```http
GET /api/consultas/futuras
```

### Buscar lembretes dos próximos 30 dias

```http
GET /api/lembretes/proximos/30
```

### Buscar consultas por período

```http
GET /api/consultas/periodo?inicio=2026-06-01&fim=2026-06-30
```

---

## Troubleshooting

### A API não inicia

Verifique se o .NET 9 SDK está instalado:
```bash
dotnet --version  # deve mostrar 9.x.x
```

### Erro de conexão com Oracle

```
ORA-12514 ou connection timeout
```

Verifique:
1. Você está conectado na rede/VPN da FIAP?
2. Teste o ping: `ping oracle.fiap.com.br`
3. As credenciais em `appsettings.json` estão corretas?

> A API inicia mesmo sem conexão com o banco — o erro de migration é logado e a execução continua.

### Porta já em uso

Se a porta 5288 estiver ocupada, rode em outra porta:
```bash
dotnet run --project src/PetFamily.API/PetFamily.API.csproj --urls=http://localhost:5001
```

Swagger ficará em: `http://localhost:5001/swagger`

### Erro ao criar migration

Certifique-se que o `dotnet-ef` está instalado:
```bash
dotnet tool install --global dotnet-ef
dotnet tool update --global dotnet-ef
```

---

## Porta padrão

| Protocolo | URL |
|---|---|
| HTTP | `http://localhost:5288` |
| Swagger | `http://localhost:5288/swagger` |

---

## Connection String

Configurada em `src/PetFamily.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "OracleConnection": "User Id=rm566551;Password=100505;Data Source=oracle.fiap.com.br:1521/ORCL"
  }
}
```

---

## Integrantes

| Nome | RM |
|---|---|
| Pedro Vaz | RM566551 |
| João Victor Luiz Oliveira Resende | RM565139 |

---
