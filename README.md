
# challenge-coodesh
teste para vaga de Backend C#/.NET Core Developer (Pleno) na Netimóveis Brasil


Organização do projeto

.Context - Contém as configurações do banco de dados (PostgreSQL pois tenho mais familiaridade).

.CrossCutting - Contém configurações que nao se encaixam em nem um outro projeto, como por exemplo o mapeamento de entidade para modelo.

.Domain - Contém os objetos/entidades do banco de dados.

.Interface - Contém as interfaces para utilização dos serviços.

.Models - Contém os modelos dos objetos/entidades que serão utilizados na pagina web.

.Service - Contém os serviços e regras de negócio.

.Repository - Contém os serviços que se comunicam com o repositório.

.WebAPI - Contém o back-end com API REST.

Para conexão com o banco de dados foi utilizado o EntityFrameworkCore.
Foi utilizado o Sonar para auxiliar a manter os padrões de desenvolvimento.
O projeto WebAPI contém um arquivo "Dockerfile" para fácil configuração do projeto em um container docker.

**Configurar a conexão do banco de dados:**
Criar um servidor SQL (utilizei o Postgres).

No projeto WebAPI alterar a chave "FitnessFoodsLC" do arquivo appsettings.json para corresponder ao servidor SQL.

abrir o console de comandos na pasta do projeto WebAPI e executar o comando : dotnet ef database update
Assim o banco de dados será criado 
