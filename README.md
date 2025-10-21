# CashFlow

[![.NET](https://img.shields.io/badge/.NET-6-blue?logo=.net)](#)  
[![Status do Projeto](https://img.shields.io/badge/status-em%20desenvolvimento-yellow)](#)  

## 📌 Sobre

O **CashFlow** é uma aplicação de gerenciamento e controle financeiro pessoal/empresarial, desenvolvida em C#. Seu objetivo é ajudar usuários a monitorar receitas, despesas, fluxo de caixa e obter insights visuais (gráficos, relatórios) para tomar decisões mais estratégicas com base em dados reais.

Este projeto demonstra boas práticas de design, arquitetura e testes, e é ideal para quem busca entender ou contribuir em sistemas backend com foco em finanças.

### Motivação

- Ajudar pessoas e pequenas empresas a terem controle financeiro eficiente.  
- Demonstrar habilidades técnicas (modelagem, testes, arquitetura de software).  
- Servir como portfólio para posições de desenvolvimento backend / full stack.

---

## ✨ Principais Features

- Cadastro e gestão de **receitas** e **despesas**.  
- Visualização de fluxo de caixa por período (mensal, semanal, anual).  
- Relatórios e gráficos para acompanhamento (ex: categoria, comparativo ano a ano).  
- Tratamento de exceções e validações de entrada (dados financeiros).  
- Camada de testes automatizados (unitários / integração).  
- Estrutura modular, arquitetura limpa (separação de camadas: domínio, infra, aplicação).  
- (Possível extensão) exportação de relatórios (PDF, Excel) / integração com APIs bancárias.

---

## 📂 Estrutura do Projeto

```
CashFlow/
├── src/            # Código-fonte principal
│   ├── Domain/     # Entidades, regras de negócio
│   ├── Application/ # Casos de uso, serviços
│   ├── Infrastructure/ # Persistência, repositórios
│   └── API/        # (se houver camada web / API)
├── testes/         # Projetos de teste (unitário / integração)
├── CashFlow.sln    # Solução .NET
├── .gitignore
└── README.md
```

Essa organização facilita entendimento, manutenção e escalabilidade do sistema.

---

## 📥 Como Instalar / Rodar Localmente

```bash
# 1. Clone o repositório
git clone https://github.com/Mateus-R-De-Lima/CashFlow.git
cd CashFlow

# 2. Restaurar dependências
dotnet restore

# 3. Configurar banco de dados
# - Crie um banco (por exemplo, SQL Server, PostgreSQL ou outro)
# - Configure a _connection string_ no arquivo de configuração (ex: appsettings.json)
# - Execute as migrações (se estiver usando EF Core):
dotnet ef database update --project src/Infrastructure --startup-project src/API

# 4. Executar a aplicação
dotnet run --project src/API

# 5. Rodar testes
dotnet test
```

Se você estiver usando Docker ou contêineres, pode incluir algo como:

```bash
docker-compose up -d
```

---

## 🛠️ Tecnologias e Ferramentas

- Linguagem: **C# / .NET 6+**  
- ORM / Mapeamento: **Entity Framework Core** (ou similar)  
- Banco de dados: SQL Server / PostgreSQL / outro relacional  
- Testes: xUnit / NUnit / MSTest  
- Versionamento: Git / GitHub  
- CI/CD (integração contínua) — GitHub Actions, Azure DevOps, etc.  
- Tratamento de logs, injeção de dependência, padrão Repository / Unit of Work  

---

## ✅ Boas Práticas e Qualidades Técnicas

- **Código limpo e legível**: com nomes expressivos e separação de responsabilidades.  
- **Testes automatizados**, garantindo que funcionalidades não se quebrem com mudanças.  
- **Tratamento de erros e validações**, para tornar aplicação mais robusta.  
- **Extensibilidade**, pensando em futuras integrações ou novas funcionalidades.  
- **Documentação**: manter README, comentários úteis, e (opcional) documentação de API (Swagger, OpenAPI).  
- **Controle de versão e commits claros**, mostrando evolução incremental do projeto.

---


