Realizamos um teste que não foi bem-sucedido e agora vamos implementar a outra parte do TDD para fazer o nosso teste passar.

## Fazendo as correções necessárias para o teste

Vamos abrir o gerente das soluções, que fica na lateral direita, e expandir a aba. Em seguida, acessaremos "src > JornadaMilhas.API > Endpoint" e definiremos essa nova funcionalidade para `OfertaViagemExrtension.cs`.

No final do código, depois do `app.MapGet()` e antes de fechar as chaves, colaremos o código do nosso método:

```c#
// código omitido

        app.MapGet("/ofertas-viagem/", async ([FromServices] OfertaViagemConverter converter, [FromServices] EntityDAL<OfertaViagem> entityDAL, [FromQuery] int pagina = 1, [FromQuery] int tamanhoPorPagina = 25) =>
        {
            var oferta = await entityDAL.ListarPaginado(pagina, tamanhoPorPagina);
            if (oferta is null) return Results.NotFound();
            return Results.Ok(converter.EntityListToResponseList(oferta));
        }).WithTags("Oferta Viagem").WithSummary("Obtem oferta de viagem paginado.").WithOpenApi().RequireAuthorization();

        app.MapGet("/ofertas-viagem/maior-desconto", async ([FromServices] OfertaViagemConverter converter, [FromServices] EntityDAL<OfertaViagem> entityDAL) =>
        {
            var lista = await entityDAL.Listar();
            var oferta = lista.FirstOrDefault();
            if (oferta is null) return Results.NotFound();
            return Results.Ok(converter.EntityToResponse(oferta));
        }).WithTags("Oferta Viagem").WithSummary("Obtem oferta de viagem com maior desconto.").WithOpenApi().RequireAuthorization();


    }
}
```

Nosso objetivo é apenas simular todo esse fluxo de realizarmos o TDD, commitar e ver a nosso pipeline em execução. Colamos o método `"/ofertas-viagem/maior-desconto"` e, dentro dele, fazemos uma consulta genérica na base, retornando o valor.

### Configurando a publicação no portal Azure

Salvamos esse código e podemos commitar para iniciar a nosso pipeline e executar os testes. Também fará a publicação no portal do Azure, então, **antes de fazer esse commit**, acessaremos o portal do Azure DevOps.

Faremos uma última **configuração na nossa pipeline de release**. Na aba "*New release pipeline*", que já deixamos aberta, na aba "Pipeline", temos a seção "*Artefacts*" (Artefatos). No card do artefato, temos um botão com um ícone de raio, no canto superior direito, onde clicaremos.

Esse botão abre uma aba à direita, onde temos a opção "*Continuous deployment trigger*" (Gatilho de deployment contínuo), que tem uma chave atualmente desabilitada (*disabled*). Clicaremos na chave para habilitá-la. Em seguida, clicamos em "*Save*" (Salvar) na parte superior direita.

Com essa configuração, no momento que **executarmos a pipeline que entrega o artefato**, ela iniciará a **pipeline que faz a publicação no portal do Azure**.

## Executando o testes

Após a implementação, retornamos ao Visual Studio, onde clicaremos no botão "2 mudanças", que tem um lápis e o número 2, na barra inferior à direita, ou usamos o atalho "Ctrl + Alt + F7". Isso abre, à direita, a aba "Alterações do Git", onde faremos o `#Update: Maior desconto`.

Abaixo da caixa de texto, selecionamos a opção "Confirmar Tudo e Enviar por push". Quando o botão com o lápis, na barra inferior, zera, temos a confirmação de envio.

Voltando ao portal do Azure DevOps, no menu da esquerda, clicaremos em "Pipelines > Pipelines". Ao acessarmos essa página, notamos que a execução do pipeline que fizemos foi agendada. Ao clicarmos sobre ela, temos mais detalhes do status, que está em andamento.

Clicando na opção com o commit "Update: Maior desconto", vamos para outra página de detalhes, onde temos a seção "Jobs", com um Job". Ao clicarmos nesse "Job", abrimos o Terminal de comando, onde vemos o processo de execução do pipeline. Esse job que roda a nosso pipeline, que executa os testes, publica e gera o artefato.

> **Observação:** quando o job termina de rodar, o instrutor nota que esqueceu de rodar os testes localmente, antes de fazer o commit.

Apesar de não termos rodado o teste localmente, agora temos uma segurança maior. Sabemos que, se o teste não passar, podemos verificar isso na execução do Job e descobrir o que não está passando. Essa é uma segurança maior que o pipeline nos proporciona no Azure DevOps. No caso, o teste passou.

### Conferindo as releases

Acessando, no menu da direita "Pipelines > Releases", temos a "Release-2", que está no "*Stage 1*" (Estágio 1). Se clicarmos sobre a etiqueta de estágio, conseguimos abrir o Terminal para acompanharmos a evolução também. Portanto, podemos acompanhar o processo de execução no Azure.

Aqui já concluiu o download do artefato e o processo de publicação no Azure. Eu já estou com a URL da publicação aberta em outra aba., então vou apenas atualizar. Quando a página carrega, conseguimos ver o processo.

## Conclusão

Nós criamos o método de teste, implementamos a funcionalidade, seguindo o TDD, executamos um commit para o repositório e iniciamos a pipeline. Com isso, foram executados todos os testes e implementada a atualização no portal do Azure.

Finalizamos a criação da nossa pipeline, simulando esse cenário real do cotidiano da pessoa desenvolvedora. Criamos uma nova funcionalidade, iniciamos uma build e uma pipeline, que entregará a nossa solução em outro ambiente, no nosso caso, no portal do Azure.