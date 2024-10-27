Nesta aula temos como foco  trabalhar a escrita de uma pipeline a ser utilizada no GitHub Actions. Para isso definimos as etapas que desejamos automatizar utilizando um arquivo  **`.YML`**. Portanto nesta atividade é necessário:

* Definir um arquivo com extensão  **`.YML`**
* Explicitar os comandos a serem executados na pipeline.

Agora é com você! Bora praticar?


> Já fez as configurações iniciais do projeto? Se tiver alguma dúvida, basta clicar em **Opinião do instrutor**.


Para execução desta prática é necessário primeiramente adicionar um novo item no projeto, você pode fazer isso clicando: com o botão direito sobre a solução, navegar até **`Adicionar`** e depois em **`Novo Item`**.

Escolha a opção de arquivo texto e renomeio com a extensão **`.YML`**, neste arquivo iremos adicionar a configuração abaixo, conforme mostrado em vídeo:

```
#dotnet test ./test/JornadaMilhas.Unit.Test
#dotnet test ./test/JornadaMilhas.Integration.Test.API
#dotnet publish ./src/JornadaMilhas.API
name: Pipeline
on:
 push:
  branches: ["main"]
jobs:
  build:
   name: Minha primeira pipeline  
   runs-on: ubuntu-latest
   steps:     
      - name: Setup .NET
        uses: actions/setup-dotnet@v2
        with:
          dotnet-version: 8.0.x
      - name: Checkout do código
        uses: actions/checkout@v2      
       #dotnet test ./test/JornadaMilhas.Unit.Test
      - name: Execução do teste de unidade 
        run: dotnet test ./test/JornadaMilhas.Unit.Test
       #dotnet publish ./src/JornadaMilhas.API
      - name: Publicando a API
        run: dotnet publish ./src/JornadaMilhas.API     

```

Este arquivo será salvo ou renomeado como **`script.yml`**  e deverá ser movido para a pasta **`.github\workflows`** dentro da pasta do projeto.

Você pode conferir o código completo do que foi desenvolvido nesta aula acessando [o repositório no GitHub](https://github.com/alura-cursos/JornadaMilhas-CICD/tree/aula01-video01.03).