Agora, criaremos o método de teste. Para isso, acessamos o arquivo `OfertaViagem_GET.cs`.

Vamos imaginar o seguinte cenário: já temos acesso à base de dados, pois expusemos o contexto e precisamos recuperar o ID.

Então, recuperaremos a primeira ocorrência de uma oferta de viagem na base de dados. Se essa oferta não existir, cadastramos uma e utilizamos esse ID para fazer a **consulta pelo endpoint**. 

Depois, fazemos as assertivas para validar se o objeto é retornado e tem os valores que esperamos. Então, vamos lá!

# Teste de recuperação de `OfertaViagem` por ID

Próximo à linha 21, no `arrange`, cenário em que preparamos o ambiente para o teste, colamos o trecho de código abaixo.

```cs
//Código omitido

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


//Código omitido
```

Estamos definindo uma variável chamada `ofertaExistente` e fazemos uma consulta no contexto, que está sendo exposto no `JornadaMilhasWebApplicationFactory`. 

Se não existir, cadastramos uma nova oferta usando o próprio contexto. Ainda no `arrange`, precisamos definir o `client`. Precisamos ir para o `client` já autenticado na API.

Após, colamos o trecho de código referente ao `act`.

```cs
var response = await client.GetFromJsonAsync<OfertaViagem>("/ofertas-viagem/" + ofertaExistente.Id);
```

Nesse trecho, definimos o objeto `response`, onde fazemos um `GetFromJsonAsync` e esperamos retornar uma `OfertaViagem`. Passamos a rota e o Id da `ofertaExistente`. 

Já na sessão do `assert`, colamos o seguinte código:

```cs
Assert.NotNull(response);
Assert.Equal(ofertaExistente.Preco, response.Preco, 0.001);
Assert.Equal(ofertaExistente.Rota.Origem, response.Rota.Origem);
Assert.Equal(ofertaExistente.Rota.Destino, response.Rota.Destino);
```

Com esse código, validaremos se o objeto retornado contém alguns dados que passamos, com um parâmetro, ou seja, se recuperamos ou se cadastramos.

Passamos um `Assert.NotNull()`, para verificar se conseguimos recuperar alguma informação no ID. Depois, validamos algumas informações. Se da `ofertaExistente`, o preço é igual ao preço retornado na consulta. 

Estamos passando também com um parâmetro, para aumentar a precisão da consulta, por ser um valor monetário, `0.001`. Também estamos verificando com o `Assert.Equal()`, se a `Rota.Origem` é igual a `Rota.Origem` retornada. 

Feito tudo isso, salvamos o código e executaremos o teste. Na lateral esquerda da ferramenta, clicamos em "Gerenciador de Teste". Em `OfertaViagem_GET`, clicamos com o botão direito e depois em "Executar". Deu certo, o teste está passando corretamente.

Criamos um novo teste para testar o `GET` de `OfertaViagem_PorId()`. Porém, nesse teste, estamos acessando a informação do banco de dados. Para isso, expusemos no vídeo anterior, o **acesso ao contexto**. 

Se no contexto existir uma informação cadastrada, a utilizamos. Caso contrário, cadastramos uma informação em uma nova rota, criamos o `client` para chamar o endpoint e fazemos a consulta pelo ID cadastrado. 

Além disso, fazemos algumas asserções, como se não foi retornado nulo e se os valores informados na API estão conforme o retorno. Usando essa estratégia, deixamos nosso **código de teste independente**. Isso porque estamos trabalhando diretamente com a base de dados. 

Por exemplo, se não existir um objeto com a oferta de viagem, então a cadastramos. Isso significa que o teste não depende da execução de outro código para cadastrar uma informação da base de dados ou recuperar.

No próximo vídeo continuaremos testando outros cenários com o `GET`. **Te esperamos lá!**