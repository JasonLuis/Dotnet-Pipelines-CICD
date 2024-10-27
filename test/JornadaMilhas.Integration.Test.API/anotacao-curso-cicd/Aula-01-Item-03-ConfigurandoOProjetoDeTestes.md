Te desejo as boas-vindas a mais um curso da Formação de Testes em .NET! 

Neste curso, enfrentaremos o desafio de entregar nossa solução, a Jornada Milhas API, em um ambiente externo, como um ambiente de produção na nuvem, como Azure ou AWS. Para isso, precisamos gerar alguns arquivos para publicação.

## Publicação do projeto
Primeiro, em nosso projeto, podemos clicar com o botão direito do mouse em `JornadaMilhas.API`, no Gerenciador de Soluções à direita, e selecionar "Publicar" para liberar os arquivos necessários para a implantação do projeto. No entanto, estamos focando bastante em testes, então o ideal é rodarmos os testes primeiro para garantir a qualidade do nosso projeto antes de gerar os arquivos para publicação.

## Automação com script
Podemos executar todos os testes indo ao Gerenciador de Testes, à esquerda, e executar todos os testes. No entanto, esse processo ainda seria muito manual, pois precisaríamos primeiro executar os testes e depois publicar a API. Podemos melhorar esse fluxo automatizando essas etapas com um script.

Para isso, podemos adicionar um novo item ao nosso projeto, criando um arquivo de script (`script.bat`). Nele, colocamos os comandos necessários para executar os testes e publicar a API, como `dotnet test` para os testes de unidade e integração, e `dotnet publish` para publicar a API.

```
dotnet test ./test/JornadaMilhas. Unit.Test
dotnet test ./test/JornadaMilhas.Integration.Test.API
dotnet publish ./src/JornadaMilhas.AΡΙ
```

## Execução do script
Após salvar o script, abrimos um terminal e o executamos com `.\script.bat`. No entanto, percebemos que o teste de integração não foi executado devido a uma dependência do Docker, que precisa estar em execução para que os testes de integração funcionem.

Vamos iniciar novamente os nossos contêineres e executar o script novamente. Ao fazer isso, ele deve rodar os testes de unidade, os testes de integração e, em seguida, gerar os arquivos para publicação.

## Introdução às pipelines
Nosso objetivo é criar um processo menos dependente do ambiente local, onde possamos automatizar todas essas etapas: executar os testes, gerar o *build* e publicar a solução, garantindo que tudo esteja funcionando corretamente antes da implantação.

Essas etapas são conhecidas como **pipeline**. Vamos trabalhar com ferramentas que permitem criar pipelines automatizadas, que executam essas etapas de forma eficiente e confiável, culminando na implantação em um ambiente externo.

## Próximos passos
Uma dessas ferramentas que abordaremos é o *GitHub Actions*, que utilizaremos nas próximas aulas para configurar nossa pipeline automatizada.