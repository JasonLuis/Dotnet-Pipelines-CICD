Acabamos de criar nossa primeira *pipeline* no GitHub Actions e realizamos alguns testes nessa *pipeline*. 

Inclusive, devido a esses testes, vamos voltar ao nosso código e desfazer o que fizemos antes, removendo o `Assert.Fail("Erro")` e também descomentando o `Assert.Equal()`, selecionando esse trecho de código e pressionando o atalho "Ctrl + K + U. 

> `OfertaViagemConstrutor.cs`

```cs
public void RetornaEhValidoDeAcordoComDadosDeEntrada(string origem, string destino, string dataIda, string dataVolta, double preco, bool validacao)
{
// código omitido

		//assert
		Assert.Equal(validacao, oferta.EhValido);

}
```

A seguir, precisamos inserir os **testes de integração** nessa *pipeline*. Vamos fazer isso agora. Mas, como dependemos do Docker rodando esse ambiente local para a execução desses testes, precisamos resolver essa situação. 

Para isso, vamos recorrer à **biblioteca `TestContainer`**, com a qual já trabalhamos em alguns cursos dessa formação.

## Istalando a biblioteca TestContainer

Vamos configurar nosso projeto de teste de integração para receber essa biblioteca. Clicamos com o botão direito em `JornadaMilhas.Integration.Test.API` no Gerenciador de Soluções à direita, e selecionamos a opção "**Gerenciar pacotes do NuGet**". 

Na janela que se abrir, temos um campo "Procurar" no canto superior esquerdo. Nele, vamos digitar "*TestContainer*". Selecionamos a opção **`Testcontainers.MsSql`** na lista de resultados, depois clicamos em "**Instalar**" na aba à direita da lista de resultados e, para finalizar, clicamos em "**Aplicar**" na caixa de diálogo. 

Biblioteca instalada!

##

Agora, vamos voltar para nosso projeto de integração. Vamos abrir o arquivo `JornadaMilhasWebApplicationFactory.cs` para fazer as nossas configurações.

Começaremos com a criação de um ***container*** usando essa biblioteca, para podermos utilizá-la via código C#. Para isso, escreveremos o seguinte trecho de código logo após a declaração do `scope`:

> `JornadaMilhasWebApplicationFactory.cs`

```cs
		private readonly MsSqlContainer _mssqlContainer = new MsSqlBuilder()
 .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
 .Build();
```

Nesse código, estamos definindo um **objeto `MsSqlContainer`** e passando a instância do *container* que queremos criar e uma imagem do SQL Server `2022-latest`. Essa é a primeira configuração que faremos.

Vamos alterar também a *string* de conexão que definimos para nosso projeto, mais abaixo no código, em `UseSqlServer`. Vamos apagar o conteúdo atual (`"Server=localhost,11433;Database=JornadaMilhasV3;User Id=sa;Password=Alura#2024;Encrypt=false;TrustServerCertificate=true;MultipleActiveResultSets=true;"`) e substituir por `_mssqlContainer.GetConnectionString()`, usando a conexão do *container*:

```cs
// código omitido
options
.UseLazyLoadingProxies()
.UseSqlServer(_mssqlContainer.GetConnectionString()));
});
```

Podemos salvar o arquivo.

## Recapitulando

Essas são as primeiras configurações necessárias para nosso projeto de teste de integração:

- Instalamos a biblioteca `TestContainer`;
- Criamos um objeto que vai representar esse *container* em memória utilizando uma imagem do SQL Server;
- Configuramos a *string* de conexão. No momento em que iniciarmos nossa API em memória, ela utilizará a conexão com esse *container* que acabamos de criar. 

Na sequência, realizaremos as próximas etapas dessa configuração.