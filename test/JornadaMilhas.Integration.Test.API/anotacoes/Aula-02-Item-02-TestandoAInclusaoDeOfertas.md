Vamos utilizar como estratégia e nomenclatura para a nossa classe de teste o nome da classe que queremos testar, mais o verbo HTTP. Vamos começar testando o POST. 

Para isso, vamos adicionar uma nova classe chamada `OfertaViagem_POST`. Essa deve ser uma classe pública.

```
public class OfertaViagem_POST {

}
```

Agora, vamos escrever o nosso primeiro método de teste. Defina-o como `public async Task Cadastra_OfertaViagem()`. Dentro desse método, vamos definir as nossas sessões AAA: Arrange, Act e Assert.

```
public class OfertaViagem_POST
{
   [Fact]

   public async Task Cadastra_OfertaViagem()
  {
  //Arrange
  //Act
  //Assert

 }
}
```

Na sessão Arrange, vamos definir uma instância do nosso aplicativo e buscar o cliente. Vamos também criar uma oferta de viagem.

Na sessão Act,  vamos criar uma variável `Response` que vai obter o retorno do cliente através do método `PostAsJsonAsync`. Passamos a rota `/ofertas-viagem` e o objeto `ofertaViagem`.

Na sessão Assert, vamos fazer o seguinte: `Assert.Equal` status code OK, que é o 200 que esperamos ser retornado.

> `OfertaViagem_POST.cs`

```
    public class OfertaViagem_POST
    {
       

        [Fact]
        public async Task Cadastra_OfertaViagem()
        {
            //Arrange
            var app = new JornadaMilhasWebApplicationFactory();

           using var client =  app.CreateClient();

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
    }
}
```
Após salvar o nosso método de teste, vamos ao gerenciador de testes para executá-lo.

Clique em "Exibir > Gerenciador de testes". Clique com  obotão direito em "OfertaViagem_POST" e selecione a opção "Executar".

O resultado não ficou verde, não atendeu nossa expectativa de retornar 200 OK.

Se você já sabe o que aconteceu, ótimo! Mas não se preocupe, vamos resolver isso na sequência.