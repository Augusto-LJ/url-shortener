# 🔗 Encurtador de URLs

Uma aplicação web simples e funcional que permite encurtar links longos, gerando URLs curtas para fácil compartilhamento. Ideal para quem precisa organizar ou compartilhar links com praticidade e clareza.

Ciclo de vida de desenvolvimento de software ([SDLC](https://aws.amazon.com/pt/what-is/sdlc/), ou Software Development Life Cycle) da aplicação:
- ✅ Planejamento
- 🛠️ **Projeto  (em andamento)**
- 🔜 Implementação
- 🔜 Teste
- 🔜 Implantação
- 🔜 Manutenção

O modelo de SDLC utilizado neste projeto é o **Ágil**.

## 🚀 Tecnologias

- **Backend**: ASP.NET Core 8
- **Frontend**: Vue.js 3 + Vite
- **ORM**: Entity Framework Core
- **Banco de dados**: SQL Server (Developer local durante o desenvolvimento)
- **IDE**: Visual Studio
- **Controle de versão**: Git + GitHub
- **Padrões**: RESTful API, Camadas (Controller, Service, Model)
- **Gerenciamento de pacotes**:
  - Backend: NuGet
  - Frontend: npm
- **Outros**: Axios (requisições HTTP no frontend), Tailwind CSS

## 🎯 Funcionalidades (a serem desenvolvidas)

- Geração de uma URL encurtada a partir de um link original
- Redirecionamento da URL curta para a URL original
- Armazenamento das URLs encurtadas em banco de dados
- Página com formulário para encurtar URLs
- Validação da URL digitada pelo usuário

## 💻 Demonstração

A preencher

## 🛠️ Como vai funcionar o MVP?

- O usuário acessa uma tela com um campo de input
- O usuário insere uma URL longa no campo
- Ao clicar em "Encurtar", a aplicação envia a URL para o backend
- O backend retorna a URL encurtada ao usuário (ex: `https://meuencurtador.com/abc123`)
- Quando alguém acessa `https://meuencurtador.com/abc123`, o sistema redireciona para a URL original

## 🔜 Futuras melhorias

- Autenticação e login de usuário
- Usuário pode visualizar suas URLs encurtadas favoritas
- Possibilidade de deletar ou desativar URLs
- Relatórios de cliques por URL (quantidade, data, etc)
- Expiração automática de URLs (por data ou por número de acessos)
- Integração com serviços externos (ex: APIs de analytics)
- Área administrativa para gerenciamento de URLs
