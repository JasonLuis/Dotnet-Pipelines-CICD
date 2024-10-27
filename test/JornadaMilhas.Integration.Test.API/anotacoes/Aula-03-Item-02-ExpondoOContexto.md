Escrevemos a classe de teste `OfertaViagem_POST()`, `Cadastra_OfertaViagem()` e `SemAutorização()`. Além disso, fizemos uma refatoração no código, para aproveitar a instância `JornadaMilhasWebApplicationFactory`. 

Fizemos isso, pois essa instância representará para uma API executando em memória. Estamos compartilhando esse recurso entre os métodos dessa classe de teste, utilizando o `IClassFixture`. Continuaremos escrevendo testes para outros cenários e métodos da API. Começaremos testando o `GET`. 

# Criando a classe para testar o `GET`

Na lateral direita da ferramenta, no **Gerenciador de Soluções**, clicamos com o botão direito em `JornadaMilhas.Integration.Test.API` e depois em "Adicionar > Classe". Na janela que abre, a nomeamos de `OfertaViagem_GET` e clicamos em "Adicionar". 

Utilizaremos o recurso da classe `POST`. Para facilitar, no arquivo `OfertaViagem_POST.cs`, copiamos o trecho de código referente a `OfertaViagem_POST` e `private readonly`. Voltamos no `OfertaViagem_GET` e colamos após `internal class`.

```cs
//Código omitido

namespace Jornada Milhas. Integration.Test.API
{

	internal class OfertaViagem_GET :
		IClassFixture<JornadaMilhasWebApplicationFactory>
	{
			private readonly JornadaMilhasWebApplicationFactory app;
	}
}
```

Feito isso, selecionamos o trecho de código `private readonly Jornada MilhasWebApplicationFactory app`, pressionamos "Ctrl + ." e clicamos em "Gerar construtor". Assim, injetamos a dependência.

```cs
//Código omitido

namespace Jornada Milhas. Integration.Test.API
{

	internal class OfertaViagem_GET :
		IClassFixture<JornadaMilhasWebApplicationFactory>
	{
			private readonly JornadaMilhasWebApplicationFactory app;
			
			public OfertaViagem_GET(JornadaMilhasWebApplicationFactory app)
	{
				this.app = app;
	}
}
```

Podemos começar testando a consulta de oferta por ID. Na linha abaixo, nas chaves, adicionamos colchetes e passamos o primeiro método de teste `Fact`. Na linha abaixo, passamos `public async Task` seguido do nome do método `Recupera_OfertaViagem_PorId()`. 

Na linha abaixo, adicionamos chaves e em comentário, passamos o **AAA**, `Arrange`, `Act`, `Assert` e salvamos.

> Aproveitamos para fazer uma correção. Próximo à linha 9, antes da classe `OfertaViagem_GET`, passamos `public class`. O código fica conforme abaixo.

```cs
//Código omitido

namespace JornadaMilhas.Integration.Test.API
{
	public class OfertaViagem_GET :
		IClassFixture<JornadaMilhasWebApplicationFactory>
	{
		private readonly JornadaMilhasWebApplicationFactory app;
		public OfertaViagem_GET(JornadaMilhasWebApplicationFactory app)
		{
			this.app app;
		}
		[Fact]
		public async Task Recupera_OfertaViagem_PorId()
		{
			//Arrange
			//Act
			//Assert
		}
	}
}
```

Queremos recuperar uma oferta pelo ID dela. Então, precisamos ter uma informação na base, ou seja, um ID que exista lá dentro, que seja válido para poder recuperar. 

Para executar esse teste, recuperar uma oferta de viagem pelo ID, primeiro precisamos saber se existe algum ID **cadastrado na base de dados**. Portanto, precisamos saber se existe um ID cadastrado, se é válido, esse tipo de informação. 

Temos a possibilidade de acessar e configurar a aplicação web, a API na classe `WebApplicationFactory`. Repare que temos acesso, por exemplo, as configurações de conexão com a base de dados. 

Precisamos de alguma maneira expor esse acesso para fazermos uma consulta na base para encontrar um ID válido. Faremos isso agora, expondo uma informação importante, que é o contexto. 

# Exposição do contexto para consulta na base de dados

No arquivo `JornadaMilhasWebApplicationFactory.cs`, próximo à linha 15, acima de `protected override void`, passamos `public JornadaMilhasContext Context { get; }`.

```cs
//Código omitido

public JornadaMilhasContext Context { get; }

//Código omitido
```

Agora, precisamos ter acesso a esse contexto reconfigurado para nossa Web API. Para isso, temos a **injeção de dependência**, então acessaremos o container para poder configurar o nosso `DBContext` reconfigurado. 

Para fazer esse acesso de gestão de dependência, definiremos, na linha abaixo de `public JornadaMilhasContext`, um `private` do tipo `IServiceScope` chamado `scope`. 

Na linha abaixo, no construtor dessa classe recuperaremos o `scope` por meio do container de injeção de dependência. Escrevemos `public JornadaMilhasWebApplicationFactory() {}`.

Nas chaves, recuperamos o `this.scope = Services.CreateScope()`. Na linha abaixo, passamos `Context`recebendo `scope.ServiceProvider.GetRequiredService<JornadaMilhasContext>()`. 

```css
//Código omitido

    public class 
		JornadaMilhasWebApplicationFactory:WebApplicationFactory<Program>
    {
        public JornadaMilhasContext Context { get; }

        private IServiceScope scope;

        public JornadaMilhasWebApplicationFactory()
        {
            this.scope = Services.CreateScope();
            Context = scope.ServiceProvider.GetRequiredService<JornadaMilhasContext>();
        }
				
//Código omitido
```

Dessa forma, com a definição do objeto `JornadaMilhasContext` recuperaremos o `scope`, expor o contexto para poder fazer seu uso nos métodos de teste quando necessário.

Daremos continuidade na sequência. **Até lá!**