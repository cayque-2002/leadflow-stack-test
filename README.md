# 🚀 LeadFlow

Sistema de gerenciamento de leads e tarefas desenvolvido como solução para o desafio técnico.

A aplicação permite gerenciar leads em um fluxo visual (Kanban), além de controlar tarefas associadas a cada lead, com uma interface moderna e integração completa com API.

---

## 🧠 Arquitetura

O backend foi estruturado seguindo princípios de **DDD (Domain-Driven Design)** com separação clara de responsabilidades:

```
backend/
  src/
    LeadFlow.Api          → Camada de apresentação (Controllers)
    LeadFlow.Application  → Regras de negócio e serviços
    LeadFlow.Domain       → Entidades e enums
    LeadFlow.Infra        → Acesso a dados (EF Core)
```

---

## 🛠️ Tecnologias utilizadas

### 🔹 Backend

* .NET 8
* ASP.NET Core
* Entity Framework Core
* SQLite
* Arquitetura em camadas (DDD)

### 🔹 Frontend

* Angular
* Bootstrap
* TypeScript

---

## ⚙️ Funcionalidades

### Leads

* Criar, editar, excluir e listar leads
* Filtro por nome, e-mail e status
* Validações:

  * Nome com no mínimo 3 caracteres
  * E-mail válido
* Visualização em formato **Kanban**

### Tasks

* Criar, editar e excluir tarefas por lead
* Definição de status (Pendente / Concluída)
* Data limite opcional
* Gerenciamento via modal

---

## 🎨 Interface

* Layout em **tema escuro (dark mode)**
* Organização em **Kanban por status**
* Uso de **modais** para criação e edição
* Feedback visual ao usuário (mensagens de sucesso/erro)

---

## ▶️ Como executar o projeto

### 🔹 Backend

```bash
cd backend/src/LeadFlow.Api
dotnet restore
dotnet run
```

API disponível em:

```
http://localhost:5000
```

---

### 🔹 Frontend

```bash
cd frontend/leadflow-web
npm install
ng serve
```

Aplicação disponível em:

```
http://localhost:4200
```

---

## 🔗 Integração

O frontend consome a API via:

```
http://localhost:5000/api/leads
```

---

## 📌 Decisões técnicas

* Utilização de arquitetura em camadas para melhor organização e manutenção
* Services centralizando regras de negócio
* Uso de modais ao invés de múltiplas páginas para simplificar UX
* Implementação de Kanban para melhor visualização do fluxo de leads
* Filtros realizados no backend para melhor performance e escalabilidade

---

## 📈 Possíveis melhorias

* Autenticação (JWT)
* Drag & Drop no Kanban
* Paginação e ordenação
* Testes automatizados
* Deploy em ambiente cloud

---

## 👨‍💻 Autor

Cayque Guilherme
Desenvolvedor Backend .NET

---
