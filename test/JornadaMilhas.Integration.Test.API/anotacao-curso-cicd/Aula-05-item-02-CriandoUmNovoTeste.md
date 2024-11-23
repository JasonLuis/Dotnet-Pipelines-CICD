Acabamos de criar nosso pipeline que realiza a entrega da nossa API no portal do Azure, já configurado para nós. Agora nosso objetivo é **completar todo o circuito**:

> **Circuito completo**
> 
> * Criar uma nova funcionalidade;
> * Executar a pipeline;
> * Fazer build, executar os teste e gerar artefato;
> * Publicação.

Temos que criar uma nova funcionalidade, executar nosso pipeline que realiza os testes, fazer a build, executar os testes e gerar o artefato. Depois, temos o outro pipeline que pega esse artefato e publica no portal do Azure. Portanto, criaremos uma nova feature para nossa API e seguir com essa implementação com base nos testes.

## Criando a feature para o circuito completo

Vamos abrir o Visual Studio, onde trabalharemos em uma feature com maior desconto. Para isso, abriremos o  "test > JornadaMilhas.Integration.Test.API > `OfertaViagem_GET.cs`".

Podemos fechar o gerenciador de soluções, à direita, e adicionar um novo teste ao final do código, ou seja, na linha 145. Eu já tenho o código desse teste salvo na memória, então vou apenas o colar.

```c#
[Fact]
public async Task Recuperar_OfertaViagem_Com_Maior_Desconto()
{
		//Arrange  
		var ofertaExistente = app.Context.OfertasViagem.FirstOrDefault();
		if (ofertaExistente is null)
		{
				ofertaExistente = new OfertaViagem()
				{
						Preco = 100,
						Rota = new Rota("Origem", "Destino"),
						Periodo = new Periodo(DateTime.Parse("2024-03-03"), DateTime.Parse("2024-03-06"))
				};
				app.Context.Add(ofertaExistente);
				app.Context.SaveChanges();
		}

		using var client = await app.GetClientWithAccessTokenAsync();

		//Act
		var response = await client.GetFromJsonAsync<OfertaViagem>("/ofertas-viagem/maior-desconto");

		//Assert
		Assert.NotNull(response);
}
```

> **Observação:** Apenas disponibilizamos o código para copiar e colar porque ele é parecido com o criado anteriomente.

Então recuperamos uma oferta existente. Se não existir o cadastro, a oferta é buscada na base de dados. Depois recuperamos o `client` para fazer conexão com o `client` autenticado na nossa API que está rodando na memória. Por fim, temos o *act* `response`, que confere, na rota da `OfertaViagem`, na parte do `maior-desconto` e faz um `Assert.NotNull()`. 

### Conferindo o funcionamento

Salvamos essa alteração e abrimos o gerenciador de testes no lado esquerdo. Clicamos com o botão direito em `Recuperar_OfertaViagem_Com_Maior_Desconto` e selecionamos "Executar", ou usamos o atalho "Ctrl + R" no documento. 

Ao executarmos o teste, descobrimos que ele nosso teste não está passando. Então voltaremos para nosso método, no `OfertaViagem_GET.cs`.

Criamos um método de teste para testar o `Recuperar_OfertaViagem_Com_Maior_Desconto`. Nosso objetivo é testar a integração com endpoint, usando uma rota que ainda não existe, ou seja, não foi implementada. 

Estamos trabalhando com TDD. Primeiro pensamos na funcionalidade que queremos, baseando a criação dessa nova feature **com base nos nossos testes**. 

**Criamos um teste que não passará a princípio**. Depois fazemos a implementação necessária para que esse teste passe. Podemos ter que **refatorar o teste** para melhorar a escrita do teste, seguindo o **fluxo TDD**. 

O próximo passo na codificação é **implementar essa funcionalidade**, fazendo nosso teste passar, para entregar mais uma feature. Em seguida, executamos nosso pipeline.

Faremos isso no próximo vídeo.