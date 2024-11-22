Acabamos de configurar o recurso de banco de dados no Azure. Esse recurso é importante para a publicação da nossa aplicação web. Agora, vamos definir o próximo recurso, que é a WebApp, propriamente dita. Precisamos colocar os arquivos da API dentro da WebApp para ser publicada.

## Criando recurso da WebApp

Na página inicial do portal da Azure, vamos clicar em "*Create a Resource*", encontrar a opção "WebApp" e clicar em "*Create*".

No formulário de detalhes do projeto, vamos selecionar o grupo de recursos que queremos, que será `rg-jornadamilhas`. Também vamos definir um nome para essa WebApp. Nesse caso, chamareso de `jornadamilhas-api`.

Em "*Publish*", manteremos a opção "*Code*" selecionada. Também existe a possibilidade de publicar no Docker Container ou com uma aplicação web estática.

Em "*Runtime stack*", vamos defini-lo como ".NET 8 (LTS)". Em "*Operating System*", deixaremos o sistema operacional como "Windows" mesmo. Por fim, vamos definir a região com "East US". 

Em "*Pricing plan*", o ideal é que a precificação esteja marcada como "Free F1 (Shared infrastructure)" para economizar recursos de créditos no Azure.

Na parte superior da página, estávamos na aba "*Basics*". Agora, vamos direto para a aba "*Monitoring*", porque por padrão o *Application Insights* vem habilitados, o que consume alguns créditos do Azure. Portanto, vamos marcar "*No*" para desabilitá-lo.

Agora, sim, podemos clicar no botão "*Review + Create*" e, em seguida, "*Create*" no canto inferior direito da página.

Após esperar pela implementação, podemos clicar em "*Go to Resource*" para conferir a página da WebApp criada dentro do portal do Azure.

Em visão geral, temos os dados essenciais da aplicação, principalmente o *default domain*, que é a URL que vai conter a aplicação web. Vamos poder acessar a nossa aplicação através desse link.

Para não criar uma WebApp vazia, sem nada, é criada uma página padrão indicando que a aplicação está sendo executado, mas que aguarda algum conteúdo. Podemos fechá-la e voltar para o portal.

## Conectando a WebApp ao GitHub

No menu lateral à esquerda, vamos clicar em "*Deployment Center*" para configurar a WebApp para recuperar o código do repositório do GitHub e fazer as configurações para publicar a WebApp. 

Em "*Source*", vamos selecionar a opção "GitHub" para configurar com a nossa conta. 

Em *Organization*", vamos selecionar seu usuário no GitHub. No campo "*Repository*", é preciso selecionar o repositório onde está o seu código para fazer essa publicação. No nosso caso, será o `JornadaMilhas-CICD`. A "*Branch*" será a `main`.

Em "*Workflow option*", vamos manter a opção "*Add a workflow*" marcada para adicionarmos esse *workflow*. Assim, vai gerar um arquivo YML e adicioná-lo na *branch*. E aí, vamos utilizar esse arquivo para entender para podermos ajustar o nosso arquivo YML. 

Em "*Authentication type*", vamos selecionar a opção "*Basic Authentication*" e clicar no botão "*Preview file*" para ter uma pré-visualização de como será gerado esse arquivo.

> Pré-visualização de `main_jornadamilhas-api.yml`:

```yml
# Docs for the Azure Web Apps Deploy action: https://github.com/Azure/webapps-deploy
# More GitHub Actions for Azure: https://github.com/Azure/actions

name: Build and deploy ASP.Net Core app to Azure Web App - jornadamilhas-api

on:
  push:
    branches:
      - main
  workflow_dispatch:

jobs:
  build:
    runs-on: windows-latest

    steps:
      - uses: actions/checkout@v4

      - name: Set up .NET Core
        uses: actions/setup-dotnet@v1
        with:
          dotnet-version: '8.x'
          include-prerelease: true

      - name: Build with dotnet
        run: dotnet build --configuration Release

      - name: dotnet publish
        run: dotnet publish -c Release -o ${{env.DOTNET_ROOT}}/myapp

      - name: Upload artifact for deployment job
        uses: actions/upload-artifact@v3
        with:
          name: .net-app
          path: ${{env.DOTNET_ROOT}}/myapp

  deploy:
    runs-on: windows-latest
    needs: build
    environment:
      name: 'Production'
      url: ${{ steps.deploy-to-webapp.outputs.webapp-url }}

    steps:
      - name: Download artifact from build job
        uses: actions/download-artifact@v3
        with:
          name: .net-app

      - name: Deploy to Azure Web App
        id: deploy-to-webapp
        uses: Azure/webapps-deploy@v2
        with:
          app-name: 'jornadamilhas-api'
          slot-name: 'Production'
          package: .
          publish-profile: ${{ secrets.AZUREAPPSERVICE_PUBLISHPROFILE_F50DF42F4CE1477EACC60CC00B54CE93 }}
```

Ele define um *job* onde ele faz a preparação do ambiente com o `.NET`, faz a publicação da aplicação e também faz o *deploy*.

Podemos clicar em "*Close*" para fechar a pré-visualização. Em seguida, clicamos em "*Save*" na parte superior para salvar a WebApp. Com isso, ele vai criar esse novo arquivo em YML no nosso repositório.

## Próximos passos

Na sequência, vamos conferir o arquivo em YML gerado pelo portal do Azure e comparar com o que fizemos até agora. Dessa forma, vamos finalizar a escrita do nosso *pipeline* e entregar a aplicação web dentro do Azure.