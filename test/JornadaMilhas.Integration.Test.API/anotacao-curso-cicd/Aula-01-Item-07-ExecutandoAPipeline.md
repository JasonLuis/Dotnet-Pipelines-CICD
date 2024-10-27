Temos aqui a nossa primeira pipeline para execução no *GitHub Actions*, definindo as etapas de configuração do ambiente .NET, execução dos testes de unidade e publicação. Vamos agora executar essa pipeline. 

## Realizando o commit e push para a branch `main`
Para isso, faremos um commit para a branch `main`. No próprio *Visual Studio*, na barra inferior, clicamos no ícone de lápis com o número 1 para visualizar as mudanças, que são referentes ao nosso arquivo de configuração `yml`. 

Em seguida, atualizamos a mensagem do commit para "#Update: Nossa primeira pipeline", clicamos na seta à direita de "Confirmar Tudo" e selecionamos a opção "Confirmar Tudo e Enviar por push". O gatilho para a execução da nossa pipeline é justamente o push na branch `main`.

Certifique-se de que já realizaram a atividade de preparação do ambiente, criando um repositório no *GitHub* para executar este projeto com *GitHub Actions*. Agora, abrimos o nosso repositório no *GitHub* e clicamos em "Actions", onde veremos a atualização "#Update: Nossa primeira pipeline" em execução. Vamos clicar nela para acompanhar as etapas.

## Acompanhamento da execução da pipeline
A pipeline iniciou o job, preparou o ambiente, fez o checkout do código do repositório e executou os testes de unidade. Ao clicar na etapa dos testes, veremos os logs do comando `dotnet test`. Todos os testes foram executados e passaram. Em seguida, a pipeline executou o comando `dotnet publish` para a publicação da API. Tudo foi concluído com sucesso, indicado pelo status verde no lado esquerdo.

## Adicionando um erro no teste
Vamos agora testar um cenário onde queremos que a publicação ocorra apenas se os testes passarem. Vamos simular um erro para verificar se a publicação é bloqueada. 

Vamos voltar para o *Visual Studio* e abrir o projeto de teste `JornadaMilhas.Unit.Test`. Nele, vamos localizar `OfertaViagemConstrutor.cs`, abri-lo e adicionar um `Assert.Fail()`, por volta da linha 24, com uma mensagem para simular uma falha. Além disso, vamos comentar a linha `Assert.Equal()`:

```
//assert
//      Assert.Equal(validacao, oferta.EhValido);
        Assert.Fail("Erro");
```

Feito isso, vamos salvar as mudanças, fazer um commit com a mensagem "Update: execução de um teste falhando", pelo próprio *Visual Studio*, e enviar com um push utilizando a opção "Confirmar Tudo e Enviar por push".

## Observando a execução da pipeline com erro
Agora, voltamos ao *GitHub*, acessamos a seção "Actions" e observamos que a nossa pipeline está em execução. Na execução mais recente de "Minha primeira pipeline", veja que o ambiente foi preparado e o código foi baixado com sucesso. No entanto, durante a execução dos testes de unidade, ocorreu um erro, sinalizado em vermelho. 

Examinando os logs, podemos ver que um dos testes falhou, impedindo a publicação da API. A etapa de publicação não foi executada e está esmaecida, indicando que a pipeline foi interrompida devido à falha no teste. O status final da nossa pipeline é de falha.

## Conclusão e próximos passos
Com isso, criamos nossa primeira pipeline no *GitHub Actions*, que executa etapas importantes para a publicação da nossa API. A pipeline primeiro executa os testes e, se todos passarem, realiza a publicação. Caso algum teste falhe, a execução é interrompida, garantindo que a API só será implantada se todos os testes estiverem corretos. Isso aumenta a segurança do processo de publicação!

Nosso próximo passo é preparar a pipeline para executar testes de integração. Faremos isso na sequência, garantindo que todos os testes, tanto de unidade quanto de integração, sejam executados antes da publicação da API.