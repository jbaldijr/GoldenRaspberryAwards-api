# Golden Raspberry Awards API

## Visão Geral
A **Golden Raspberry Awards API** é uma aplicação desenvolvida para processar e consultar informações relacionadas aos piores filmes premiados. Com esta API, você pode:

- Fazer upload de arquivos CSV contendo dados de filmes.
- Consultar todos os filmes importados.
- Consultar intervalos de premiação de produtores.

## Funcionalidades Principais

### Endpoints

#### 1. Upload de CSV
- **Método**: `POST`
- **Rota**: `/api/movies/upload`
- **Descrição**: Permite o upload de um arquivo CSV contendo dados de filmes.
- **Corpo da Requisição**:
  - `file` (form-data): Arquivo CSV contendo os campos:
    - `year`
    - `title`
    - `studios`
    - `producers`
    - `winner` (valores `yes` ou `no` para indicar se é vencedor).

#### 2. Consultar Todos os Filmes
- **Método**: `GET`
- **Rota**: `/api/movies/all`
- **Descrição**: Retorna todos os filmes importados na base de dados.

#### 3. Consultar Intervalos de Premiação
- **Método**: `GET`
- **Rota**: `/api/movies/producers/intervals`
- **Descrição**: Retorna os intervalos mínimos e máximos entre prêmios dos produtores vencedores.

---

## Estrutura do Projeto

A aplicação segue uma estrutura modular, dividida em:

1. **Domain**: Contém as interfaces e modelos de dados principais.
2. **Infrastructure**: Responsável pelo acesso ao banco de dados.
3. **Application**: Contém os serviços de negócios e lógica de processamento.
4. **API**: Controladores que expõem os endpoints.

---

## Como Executar a Aplicação

### Pré-requisitos
- .NET SDK 6.0 ou superior.
- Ambiente de desenvolvimento configurado (Visual Studio, VS Code, ou CLI).

### Passos
1. Clone este repositório:
   ```bash
   git clone <URL_DO_REPOSITORIO>
   ```

2. Navegue até o diretório do projeto:
   ```bash
   cd GoldenRaspberryAwards-api
   ```

3. Restaure as dependências:
   ```bash
   dotnet restore
   ```

4. Inicie a aplicação:
   ```bash
   dotnet run
   ```

5. Acesse a interface do Swagger para explorar os endpoints:
   ```
http://localhost:<PORTA>/swagger
```

---

## Documentação Avançada

Para detalhes adicionais sobre:

- **Formato do CSV esperado**
- **Exemplos de uso de cada endpoint**
- **Configurações avançadas da API**

Consulte a pasta [docs](./docs/README.md).

