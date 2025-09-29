# 🚗 Consórcio de Veículos API

Bem-vindo ao projeto **Consórcio de Veículos API**! Este projeto consiste em uma API RESTful desenvolvida em .NET Core para gerenciar veículos e administradores, incluindo funcionalidades de autenticação e validação de dados. 🚀

## 🛠️ Tecnologias Utilizadas

- **Linguagem**: C#
- **Framework**: .NET Core (ASP.NET Core)
- **Banco de Dados**: MySQL (via Entity Framework Core)
- **ORM**: Entity Framework Core
- **Autenticação**: JWT (JSON Web Tokens) - _inferido pela presença de `Authorization` e `LoginDTO`_

## 📂 Estrutura do Projeto

O projeto está organizado em duas pastas principais: `API` (a aplicação principal) e `Teste` (para testes unitários e de integração).

### `API/` - Aplicação Principal

Esta pasta contém a lógica central da aplicação, incluindo a API RESTful e a camada de domínio.

- **`ConsorcioVeiculos.csproj`**: Arquivo de projeto C# para a API.
- **`Program.cs`**: Ponto de entrada da aplicação, responsável pela configuração do host e inicialização.
- **`Startup.cs`**: Configuração dos serviços, middleware e pipeline de requisições HTTP (inferido pela estrutura padrão de projetos ASP.NET Core).
- **`appsettings.json` / `appsettings.Development.json`**: Arquivos de configuração da aplicação, incluindo strings de conexão com o banco de dados.

#### `API/Dominio/` - Camada de Domínio

Contém as regras de negócio, entidades, DTOs e serviços da aplicação.

- **`Authorization/`**: 🔐 Lógica relacionada à autenticação e autorização. Provavelmente contém classes para geração e validação de tokens JWT, bem como políticas de autorização.
  - `TokenService.cs`: Serviço para criação de tokens JWT.
- **`DTOs/`**: 📥 Data Transfer Objects (Objetos de Transferência de Dados) utilizados para entrada e saída de dados da API.
  - `AdministradorDTO.cs`: Representa os dados de um administrador para operações como criação ou atualização.
  - `LoginDTO.cs`: Utilizado para as credenciais de login (e-mail e senha).
  - `LoginAdminDTO.cs`: DTO específico para login de administradores.
  - `VeiculoDTO.cs`: Representa os dados de um veículo para operações de criação ou atualização.
  - `ValidarAdministradorDTO.cs`: 📝 Classe para validação dos dados de `AdministradorDTO`, verificando formato de e-mail, tamanho da senha e perfil.
  - `ValidarVeiculoDTO.cs`: 📝 Classe para validação dos dados de `VeiculoDTO`, verificando nome, marca e ano do veículo.
  - `Interfaces/IValidacaoDTO.cs`: Interface para padronizar as classes de validação de DTOs.
- **`Entidades/`**: 🏛️ Classes que representam as entidades do domínio e são mapeadas para o banco de dados.
  - `Administrador.cs`: Define a entidade `Administrador` com propriedades como `Id`, `Email`, `Senha` e `Perfil`.
  - `Veiculo.cs`: Define a entidade `Veiculo` com propriedades como `Id`, `Nome`, `Marca` e `Ano`.
- **`Enums/`**: 🏷️ Enumerações utilizadas no projeto.
  - `Perfil.cs`: Define os perfis de usuário (ex: `Leader`, `Admin`).
- **`Interfaces/`**: 🤝 Interfaces que definem contratos para os serviços da aplicação.
  - `IAdministradorServico.cs`: Contrato para o serviço de gerenciamento de administradores.
  - `IVeiculosServico.cs`: Contrato para o serviço de gerenciamento de veículos.
- **`ModelViews/`**: 📊 Modelos de visualização ou objetos de resposta.
  - `ErrosDeValidacao.cs`: Classe para encapsular mensagens de erro de validação.
- **`Servicos/`**: ⚙️ Implementações dos serviços que contêm a lógica de negócio.
  - `AdministradorServico.cs`: Implementa a lógica para operações CRUD e outras funcionalidades relacionadas a administradores.
  - `VeiculosServico.cs`: Implementa a lógica para operações CRUD e outras funcionalidades relacionadas a veículos.

#### `API/Infraestrutura/` - Camada de Infraestrutura

Responsável pela persistência de dados e outras preocupações de infraestrutura.

- **`DB/`**: 🗄️ Configuração do contexto do banco de dados.
  - `DbContexto.cs`: Classe que herda de `DbContext` do Entity Framework Core, configurando as entidades (`Administrador`, `Veiculo`) e a conexão com o MySQL. Inclui a seed inicial de um administrador padrão.

#### `API/Migrations/` - Migrações do Banco de Dados

Contém os scripts de migração gerados pelo Entity Framework Core para criar e atualizar o esquema do banco de dados.

### `Teste/` - Projeto de Testes

Esta pasta contém o projeto de testes para a aplicação, garantindo a qualidade e o correto funcionamento das funcionalidades.

- **`Teste.csproj`**: Arquivo de projeto C# para os testes.
- **`Requests/`**: 🧪 Testes relacionados a requisições HTTP para os endpoints da API.
  - `AdministradorRequestTest.cs`: Testes para os endpoints de `Administrador`.
  - `VeiculoRequestTest.cs`: Testes para os endpoints de `Veículo`.
- **`Test1.cs`**: Exemplo de arquivo de teste.

## 🚀 Funcionalidades Principais

Com base na estrutura do projeto, as principais funcionalidades incluem:

- **Gerenciamento de Veículos**: 🚗
  - Criação, leitura, atualização e exclusão de registros de veículos.
  - Validação de dados de veículos (nome, marca, ano).
- **Gerenciamento de Administradores**: 🧑‍💻
  - Criação, leitura, atualização e exclusão de registros de administradores.
  - Validação de dados de administradores (e-mail, senha, perfil).
- **Autenticação e Autorização**: 🔑
  - Login de administradores com e-mail e senha.
  - Geração de tokens JWT para acesso seguro à API.
  - Controle de acesso baseado em perfis (`Leader`, `Admin`).
- **Persistência de Dados**: 💾
  - Utilização de MySQL como banco de dados.
  - Mapeamento objeto-relacional via Entity Framework Core.
  - Migrações para gerenciamento do esquema do banco de dados.
- **Validação de Dados**: ✅
  - Validações personalizadas para DTOs de entrada, garantindo a integridade dos dados.

## ⚙️ Como Rodar o Projeto

1.  **Pré-requisitos**: Certifique-se de ter o .NET SDK e o MySQL instalados em sua máquina.
2.  **Configuração do Banco de Dados**: Atualize a string de conexão no `appsettings.json` ou `appsettings.Development.json` para apontar para sua instância MySQL.
3.  **Migrações**: Execute as migrações do Entity Framework Core para criar o banco de dados e as tabelas:
    ```bash
    dotnet ef database update
    ```
4.  **Executar a API**: Navegue até a pasta `API` e execute o comando:
    ```bash
    dotnet run
    ```
    A API estará disponível em `https://localhost:5001` (ou outra porta configurada).

## 🧪 Como Rodar os Testes

1.  Navegue até a pasta `Teste`.
2.  Execute os testes com o comando:
    ```bash
    dotnet test
    ```

---
