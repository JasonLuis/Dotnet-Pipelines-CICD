Vamos iniciar a nossa jornada para automatizar esta pipeline utilizando o *GitHub Actions*. Já temos um script que define os passos a serem executados, e agora vamos adaptá-lo para o formato que o *GitHub Actions* utiliza.

## Criação do arquivo YML
Primeiro, vamos abrir o *Visual Studio*. Temos um script que realiza os testes de unidade, integração e API:

```
dotnet test ./test/JornadaMilhas. Unit.Test
dotnet test ./test/JornadaMilhas.Integration.Test.API
dotnet publish ./src/JornadaMilhas.AΡΙ
```

Precisamos criar um arquivo `yml` dentro da pasta `GitHub Actions`. Para isso, na pasta no gerenciador de arquivos, vamos copiar o arquivo `script.bat`, que acabamos de criar, navegar até a pasta `.github/workflows` e colar esse arquivo. Em seguida, vamos renomeá-lo para `script.yml` a fim de reaproveitar a lógica.

Feito isso, voltamos ao *Visual Studio* e abrimos este arquivo `script.yml`. Nele, vamos comentar as linhas do script original, pois esse não é o formato que o `script.yml` utiliza no *GitHub Actions*. Para comentar, basta incluir `#` no início de cada linha. Não vamos apagá-las para mantê-las como referência.

```
#dotnet test ./test/JornadaMilhas. Unit.Test
#dotnet test ./test/JornadaMilhas.Integration.Test.API
#dotnet publish ./src/JornadaMilhas.AΡΙ
```

## Definindo a pipeline
Agora, vamos definir a pipeline no novo formato:

```
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
```

## Entendendo o script
Neste script, estamos especificando que vamos trabalhar com um repositório GitHub e utilizaremos a branch principal, `main`.

No bloco `jobs`, definimos o nome da pipeline como `Minha primeira pipeline`. Em seguida, começamos a configuração do ambiente e das dependências. Vamos preparar um ambiente rodando em uma máquina Ubuntu, que é um sistema operacional Linux.

Na primeira etapa, configuramos a versão do .NET, que será a versão 8.0, e fazemos o checkout do código do repositório para execução da pipeline.

## Execução do teste e publicação da API
O próximo passo é começar a executar nossos comandos. Vamos inserir o `name` no arquivo, criando uma nova etapa chamada `Execução do teste de unidade`.

```
- name: Execução do teste de unidade 
```

Um detalhe importante é que a formatação do `yml` é bem específica; precisamos alinhar corretamente os espaços, caso contrário, o arquivo não será lido corretamente.

Em seguida, copiamos o comando `dotnet test`, que deixamos salvo no início do código, usamos o comando `run:` e colamos o trecho copiado.

```
      - name: Execução do teste de unidade 
        run: dotnet test ./test/JornadaMilhas.Unit.Test
```

Assim, configuramos a primeira pipeline utilizando o arquivo `yml`, definindo o ambiente e os comandos que serão executados. Por enquanto, vamos focar na execução dos testes de unidade, pois para os testes de integração precisaremos de outras configurações importantes. 

Depois dos testes de unidade, queremos realizar a publicação da API. Vamos adicionar uma nova etapa com `- name: Publicando a API` e, em seguida, o comando `dotnet publish` da nossa API.

```
      - name: Execução do teste de unidade 
        run: dotnet test ./test/JornadaMilhas.Unit.Test
      - name: Publicando a API
        run: dotnet publish ./src/JornadaMilhas.API
```

O código completo ficará assim:

```
#dotnet test ./test/JornadaMilhas. Unit.Test
#dotnet test ./test/JornadaMilhas.Integration.Test.API
#dotnet publish ./src/JornadaMilhas.AΡΙ

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
      - name: Execução do teste de unidade 
        run: dotnet test ./test/JornadaMilhas.Unit.Test
      - name: Publicando a API
        run: dotnet publish ./src/JornadaMilhas.API
```

Agora, vamos salvar o arquivo. Lembrando que, neste exemplo didático, criamos um script e depois o renomeamos para um arquivo `yml` para ajustar e criar nossa pipeline no *GitHub Actions*. No dia a dia, você pode começar diretamente no arquivo `yml`.

## Próximos passos
Com o arquivo criado, o próximo passo é executar a nossa pipeline e validar se os testes são executados corretamente e se a publicação da API ocorre de forma automatizada. Isso será feito no momento em que fizermos um push para a branch `main`. Vamos seguir com essa etapa logo na sequência.