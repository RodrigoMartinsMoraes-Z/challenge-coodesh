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
.Web - Contém as paginas da web e todo front-end.
.WebAPI - Contém o back-end com API REST.

Para conexão com o banco de dados foi utilizado o EntityFrameworkCore.
O front foi feito com Razor/Js de uma maneira simples a fim de atender os requisitos do projeto.
Foi utilizado o Sonar para auxiliar a manter os padrões de desenvolvimento.
