Olá, pessoal! A Jornada Milhas evoluiu para uma solução de uma *Web API*, que já conta com **alguns *endpoints* para rotas e ofertas de viagem**. Nós, incluindo você e os demais colegas, ficamos responsáveis por **implementar os testes de integração da nossa solução.** 

## Entendendo as ferramentas e estrutura do projeto

Um detalhe importante para acompanhar o desenvolvimento do curso é ter instalado na sua máquina o **Docker Desktop** com o **WSL2**. Inclusive, vamos deixar uma atividade de **preparação do ambiente** para ser executada. 

Agora, vamos executar a aplicação clicando em "Docker Compose" na parte superior. Perfeito! Ele vai iniciar o projeto e abrir o *Swagger* da solução no endereço localhost:50665/swagger/index.html. Temos a "Autenticação", "Oferta Viagem", "Rota Viagem" e alguns *endpoints*. 

**Vamos mostrar também o Docker Desktop rodando**. Temos o Docker Compose com o `JornadaMilhas.API`, o projeto API, e o projeto Banco de Dados em execução (`sqlserver-1`). Portanto, **temos dois *containers***: um da API e outro do Banco de Dados SQL Server. 

Vamos minimizar o `Docker Desktop` e voltar para o projeto. 

Temos a pasta `src` (*source*) do lado direito, onde temos o projeto API, dividimos também o projeto de dados e domínio.

+ src
	+ JornadaMilhasAPI
	+ JornadaMilhas.Dados
	+ JornadaMilhas.Dominio

Também temos uma pasta `test` onde vamos colocar o nosso projeto de teste de integração. 

+ test

**Essa é a arquitetura do projeto**, e agora vamos começar a **praticar**. 

## Adicionando o projeto de teste

Adicionaremos o nosso projeto de teste. Na pasta `test` do lado direito, ao clicar com o botão direito, iremos incluir um novo projeto (Adicionar > Novo Projeto). 

Na tela aberta, faremos uma pesquisa por "XuNIT" no campo de pesquisa. Selecionaremos o "Projeto de Teste do XUnit" na lista de resultados e clicamos no botão "Próximo" no canto inferior direito.

O nome do projeto, digitamos "JornadaMilhas.Integration.Test.API". O local, vamos salvá-lo dentro da pasta `test`, dentro do nosso projeto, nossa pasta de solução. Clicamos em "Próximo", vamos utilizar a estrutura do `.NET 8.0(Suporte de Longo Prazo)`, que é a versão mais recente até a gravação. Clicamos no botão "Criar" na parte inferior direita. 

> JornadaMilhas.Integration.Test.API

```
namespace JornadaMilhas.Integration.Test.API
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {

        }
    }
}
```

**Projeto criado**, para trabalharmos com os projetos de integração, com o teste de integração, na nossa solução, vamos utilizar uma biblioteca da Microsoft chamada `mvc.testing`. 

## Adicionando a biblioteca

Vamos adicionar agora o nosso projeto. Do lado direito, clicamos em "Dependências" com o botão direito e depois optamos por "Gerenciar Pacotes do NuGet".

Do lado superior esquerdo, clicamos em "Procurar" e buscamos por: "Microsoft.AspNetCore.Mvc.Testing". Clicamos na opção exibida, aceitamos, e depois clicamos em "aplicar". 

**A biblioteca está instalada no nosso projeto**, e a partir de agora, vamos começar a **configurar a nossa solução**, para podermos escrever nossos **primeiros testes de integração**, utilizando essa biblioteca. 

O projeto já está configurado com a biblioteca que vamos trabalhar. Vamos fazer uma série de configurações, mas onde vamos buscar esse passo a passo? Na documentação oficial. 

> [Testes de integração no ASP.NET Core](https://learn.microsoft.com/pt-br/aspnet/core/test/integration-tests?view=aspnetcore-8.0)

A documentação oficial dessa biblioteca vai permitir fazer o teste de integração. Nela, temos uma série de configurações que vamos executar, para podermos escrever nossos primeiros testes de integração, utilizando essa biblioteca `mvc.testing`. 

## Próximos Passos

Vamos começar a escrever o nosso primeiro teste de integração, mas antes disso, precisamos fazer uma série de configurações de acordo com a documentação, e vamos fazer isso na sequência.