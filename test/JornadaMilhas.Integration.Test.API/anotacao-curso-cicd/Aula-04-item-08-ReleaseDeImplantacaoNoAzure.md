Finalizamos a criação da nossa pipeline, que realiza o *build* do projeto, executa os testes de integração e de unidade, e publica a nossa API como um artefato. Agora, vamos **implantar esse artefato no portal do Azure**. 

> Lembrando que criamos um serviço de WebApp no portal do Azure, configuramos o banco e o nosso servidor de banco de dados.

## Criando a conexão com o portal do Azure

Agora, precisamos fazer uma conexão com o portal do Azure. 

Para isso, clicamos em "***Project Settings***" no menu lateral esquerdo, depois em "***Service Connection***" no menu de configurações e clicamos em "***New service connection***" no canto superior direito. 

Na aba de configuração da conexão, optaremos pelo serviço "***Azure Resource Manager***". Selecionamos e clicamos em "***Next***".

Em "***Authentication method***" (método de autenticação), deixaremos marcada a opção recomendada, a "***Workload Identify federation (automatic)***", e clicaremos em "Next". 

No formulário de criação da *service connection*, definiremos o ***Scope level*** como "***Subscription***". Com isso, a "Subscription" em si é trazida do nosso portal do Azure, no campo "*Subscription*" abaixo. 

Selecionaremos o ***Resource group*** `rg-jornada milhas` e chamaremos essa *service connection* de `azure_portal`. Clicamos em "***Save***".

## Criando a *pipeline* de *release*

Com a nossa *service connection* criada, vamos clicar no foguete azul de Pipelines no menu à esquerda e selecionaremos a opção "***Releases***". Feito isso, clicamos em "***New pipeline***" no centro da tela. 

Ou seja, vamos criar uma *pipeline* de uma *release* para fazer a implantação do nosso artefato no portal do Azure.

Na nova página, vamos selecionar o *template* na aba aberta à direita da tela. Selecionaremos a opção "**Azure App Service deployment**" e clicaremos em "*Apply*" para aplicar.

Será criado um "*Stage 1*" para essa pipeline, na aba "***Tasks***". Vamos clicar no link `1 job, 1 task` desse Stage 1. 

Na nova página, vamos invocar o `azure_portal` (a *service connection* que acabamos de criar) no primeiro campo da seção de configuração "*Parameters*", de "***Azure subscription***". 

O tipo do app (campo "***App type***") será `Web App on Windows`, então deixamos essa opção marcada.

Por fim, no campo "***App service name***", selecionaremos a nossa Web App do portal do Azure: `jornadamilhas-api`.

Clicamos em "*Save*" no canto superior direito da tela para salvar as alterações.

Vamos voltar à aba "**Pipeline**" para deixá-la configurada também, porque precisamos pegar esse artefato de algum lugar.

Então, clicaremos no cartão "***Add an artifact***" (adicionar um artefato). 

Na aba de configuração, selecionaremos a ***Build*** como tipo de fonte (*Source type*), para utilizarmos a *Build* que fizemos no vídeo anterior, selecionando `JornadaMilhas-API` no campo "***Project***".

No campo "***Source***", vamos selecionar o repositório da nossa pipeline. No caso do instrutor, é o repositório `bessax.JornadaMilhas-CICD`. Feito isso, vamos clicar em "***Add***" para adicionar as configurações.

Em seguida, podemos clicar em "***Save***" para salvar as alterações. 

## Executando a pipeline de publicação

Agora, queremos executar a pipeline de publicação que temos em "Stage 1". Vamos clicar nela para selecioná-la. 

Feito isso, criaremos uma *release* clicando em "***Create release***" no canto superior direito. 

No campo "***Stages for a trigger change from automated to manual***" do formulário de criação de *release*, selecionaremos o `Stage 1`, e clicaremos em "***Create***". 

Agora, vamos executar a nossa pipeline que faz a implantação no portal do Azure. 

Para isso, clicamos em "***All pipelines***" no canto superior esquerdo. Na aba "*Releases*", clicaremos em "***Stage 1***" da "***Release-1***", à direita.

Na tela do "*Release-1 > Stage 1*", clicaremos em "***Pipeline***" no canto superior esquerdo. Depois, passamos o cursor pelo card de "*Stage 1"*, clicamos no botão "***Deploy***" e confirmamos clicando emk "*Deploy*" de novo na aba à direita.

Esse primeiro passo (*Stage 1*) da nossa Pipeline começará a ser executado. Podemos clicar nele para acompanhar o progresso. Terminada a execução, todos os passos de publicação serão um *check* verde de confirmação.

Como fizemos na nossa pipeline que faz o *build* o projeto que gera o artefato, temos **algumas etapas** nessa publicação:

- `Initialize job`: inicialização do Job;
- `Download artifact`: download do artefato criado no processo anterior;
- `Deploy Azure App Service`: deploy no Azure. 

Vamos clicar no link da etapa de "Deploy". Podemos reparar que a implantação foi feita com sucesso, e temos disponível o **link para acessar a nossa Web App** ao final desse arquivo de logs. No caso do instrutor, o link é `https://jornadamilhas-api.azurewebsites.net`. O seu deverá ficar igual ou parecido, dependendo do nome que você definiu ao longo das configurações. 

Então, vamos copiar esse link, abrir uma nova aba no navegador e acessar a Web App pelo link. Se a página carregar com sucesso, confirmamos que **conseguimos implantar**!

## Teste de requisição

Vamos fazer um teste do endpoint de Login, por exemplo. Para isso, clicaremos em `/auth-login` e depois no botão "***Try it out***".

No corpo da requisição, colaremos o usuário e senha de teste que temos:

> `POST` /auth-login: **Request body**

```json
{
  "email": "tester@email.com",
  "password": "Senha123@"
}
```

Clicamos em "***Execute***" para executar. 

Essa requisição retorna o status 200 e o token para acesso aos outros Endpoints da API. Funcionou!

## Conclusão

Conseguimos criar uma pipeline que faz a entrega e a implantação da nossa API dentro do portal do Azure. 

Mas ainda não acabou! Agora vamos fazer todo o processo: desde a criação de uma nova feature para a nossa API, executar os testes e fazer a implantação. 

Faremos isso na sequência!