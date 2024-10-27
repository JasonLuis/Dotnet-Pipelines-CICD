Ótimo! Acabamos de criar o nosso método `Cadastra_OfertaViagem`. Ele está passando corretamente no teste.

Mas vamos testar outro cenário aqui.

Precisamos sempre garantir que para cada endpoint temos autorização para fazer o consumo desse endpoint. Então vamos registrar isso através de um método de teste também.

Vamos aproveitar esse método que acabamos de criar, vamos copiar e colar logo abaixo para fazermos as alterações necessárias.

```
	[Fact]
    	public async Task Cadastra_OfertaViagem()
    	{
        	//Arrange
       	var app = new JornadaMilhasWebApplicationFactory();

       	using var client = await app.GetClientWithAccessTokenAsync();

        	var ofertaViagem = new OfertaViagem()
        	{
            	Preco = 100,
            	Rota = new Rota("Origem", "Destino"),
            	Periodo = new Periodo(DateTime.Parse("2024-03-03"), DateTime.Parse("2024-03-06"))
        	};
        	//Act
        	var response = await client.PostAsJsonAsync("/ofertas-viagem", ofertaViagem);

        	//Assert
        	Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    	}
```


Vamos começar renomeando. Ele vai se chamar `Cadastra_OfertaViagem_SemAutorizacao()`.

Em seguida vamos deixar como `using var client = app.CreateClient();`.

Agora nossa expectativa é que dê um não autorizado, um código 401. Então no *assert* vai ser `Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode)`.


```
    	[Fact]
    	public async Task Cadastra_OfertaViagem_SemAutorizacao()
    	{
        	//Arrange   
        	var app = new JornadaMilhasWebApplicationFactory();

        	using var client = app.CreateClient();

        	var ofertaViagem = new OfertaViagem()
        	{
            	Preco = 100,
            	Rota = new Rota("Origem", "Destino"),
            	Periodo = new Periodo(DateTime.Parse("2024-03-03"), DateTime.Parse("2024-03-06"))
        	};
        	//Act
        	var response = await client.PostAsJsonAsync("/ofertas-viagem", ofertaViagem);

        	//Assert
        	Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    	}
```

Vamos salvar. Vamos executar nossos testes.Vamos executar o teste da classe `OfertaViagem_POST`.

Perfeito. Está tudo passando. Tudo verde. Nosso objetivo era esse, mas observe o seguinte. Na linha 19 estamos criando uma instância chamada `JornadaMilhasWebApplicationFactory();`. E na linha 40 também.

Vamos criar métodos de teste para outros cenários também. Para o GET, para o DELETE. E se continuarmos usando essa estratégia de sempre criar em cada método de teste esse objeto. E esse objeto é caro. Por quê? Porque ele cria sempre uma instância da nossa API em memória.

Então na execução desses dois testes temos duas instâncias da API. Pode ter um momento em que teremos, por exemplo, 40 métodos de teste. Não podemos ter 40 instâncias da nossa API rodando simultaneamente em memória. Porque a memória é um recurso caro, computacionalmente falando.

Então temos o recurso do XUnit que são as `ClassFixtures`, que permite reaproveitar esse recurso.

Então agora na nossa classe de teste `OfertaViagem_POST`. E aqui na nossa classe de teste na linha 13. Vamos fazer essa classe herdar:

```
 public class OfertaViagem_POST:IClassFixture<JornadaMilhasWebApplicationFactory>
```

Precisamos definir agora uma instância dessa classe. Estamos utilizando aqui o `App`. Então vamos fazer isso aqui:

	public class OfertaViagem_POST:IClassFixture<JornadaMilhasWebApplicationFactory>
	{
    	private readonly JornadaMilhasWebApplicationFactory app;


Agora, podemos injetar essa instância via construtor nas classes de teste.

Então vamos selecionar aqui na linha 15. Selecionar ou dar simplesmente ctrl ponto. E vamos selecionar a opção "Gerar construtor "Oferta Viagem_POST(Jornada Milhas WebApplicationFactory app)".

```
	public class OfertaViagem_POST:IClassFixture<JornadaMilhasWebApplicationFactory>
	{
    	private readonly JornadaMilhasWebApplicationFactory app;

    	public OfertaViagem_POST(JornadaMilhasWebApplicationFactory app)
    	{
        	this.app = app;
    	}
```

Então definimos aqui. Podemos remover dos *arrange* a linha  `var app = new JornadaMilhasWebApplicationFactory();` do nosso código.

Vamos salvar. Vamos executar novamente nossos testes.

Perfeito. Nossa aplicação continua funcionando. Agora ela está rodando um pouco melhor. Porque agora só tem uma instância dessa API rodando na nossa memória. Utilizando um recurso do `ClassFixture`, que nos permite reaproveitar esse recurso em outro método de teste.

Só lembrando que para compartilhar recurso na própria classe, utilizamos o recurso do `ClassFixture`. Se precisarmos compartilhar recursos com outras classes de teste, utilizamos outro recurso do XUnit chamado `ICollectionFixture`.

Vamos continuar testando outros cenários, outros verbos HTTP. Continuaremos na próxima aula!