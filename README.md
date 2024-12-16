# **Golden Raspberry Awards API**

## **Visão Geral**

A **Golden Raspberry Awards API** é uma aplicação desenvolvida para processar e consultar informações de filmes, identificando os **produtores com os menores e maiores intervalos entre vitórias** no prêmio Golden Raspberry Awards. A API permite o upload de arquivos CSV contendo os dados dos filmes e expõe endpoints para consultas.

---

## **Funcionalidades**

- **Upload de Arquivo CSV**: Processa um arquivo contendo dados de filmes.
- **Consulta de Intervalos de Prêmios**: Identifica os produtores com os **menores** e **maiores** intervalos entre vitórias.
- **Consulta de Todos os Filmes**: Retorna todos os filmes carregados no banco de dados.

---

## **Tecnologias Utilizadas**

- **.NET 8** (Web API)
- **Entity Framework Core** (InMemory Database)
- **CsvHelper** (Leitura de arquivos CSV)
- **Swashbuckle** (Swagger UI para documentação da API)
- **XUnit** (Testes de Integração)
- **FluentAssertions** (Validação em testes)

---

## **Pré-requisitos**

Para executar a aplicação, você precisará de:

- **.NET SDK 8.0** ou superior.
- Ferramenta de desenvolvimento: **Visual Studio**, **Visual Studio Code** ou **CLI do .NET**.

---

## **Execução da Aplicação**

### **1. Clone o Repositório**

```bash
git clone https://github.com/jbaldijr/GoldenRaspberryAwards-api.git
```
### **2. Navegue até o endereço onde clonou o repositório**

```bash
cd GoldenRaspberryAwards-api/src
```

### **3. Restaure as Dependências**

```bash
dotnet restore GoldenRaspberryAwards.Api 
```
### **4. Rode os testes**
```bash
dotnet test GoldenRaspberryAwards.Tests.Integration/GoldenRaspberryAwards.Tests.Integration.csproj
```

### **5. Execute a Aplicação**

```bash
dotnet run --project GoldenRaspberryAwards.Api
```

### **6. Acesse o Swagger**
A documentação dos endpoints estará disponível em:
```bash
http://localhost:5096/swagger/index.html
```


