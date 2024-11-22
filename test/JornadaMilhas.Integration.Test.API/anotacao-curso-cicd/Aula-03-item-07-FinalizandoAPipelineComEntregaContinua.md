No vídeo anterior, criamos a WebApp. Agora, faremos uma **vinculação** do recurso no Azure com o repositório. Nesse processo é criado um arquivo VM no repositório. Usaremos esse arquivo como base para completar a Pipeline.

No projeto `main_jornadamilhas-api.yml`, fizemos um `Push` e um `Pull` no repositório para baixar o arquivo gerado. O arquivo define um `push` na branch `main`, que o ambiente executado será um o `windows`, define as etapas e configuração do ambiente. 

Depois faz um `Build with dotnet`, seguido de um `publish`. Ele faz a etapa `deploy` em um ambiente Windows também, e segue as etapas, fazendo o download dos arquivos gerados na etapa anterior e a publicação dentro do Azure. Utilizaremos esse arquivo como base para a criação do nosso. 

### Alterando a Configuração de Publicação

Voltamos para o arquivo `script.yml`. A primeira configuração que alteraremos será na parte da publicação. Para isso, apagamos as linhas 23 e 24 e no lugar colamos um trecho de código pronto, conforme abaixo.

> `script.yml`

```yml

//Código omitido

#dotnet publish ./src/JornadaMilhas.API
- name: Publicando a API
run: dotnet publish -c Release -o ${{env.DOTNET_ROOT}}/myapp ./src/JornadaMilhas.API

```

Fazemos um `dotnet pblish`, publicando a API. Porém, agora estamos passando alguns parâmetros para execução do comando, que é o local e o caminho da API do projeto.

Feito isso, voltamos para o arquivo `main_jornadamilhas-api.yml` e copiamos a parte de `deploy` que faz a publicação, da linha 37 até a 57. 

Usaremos essa estratégia para esse arquivo, pois ele já configura uma série de variáveis de ambiente no GitHub para podermos fazer essa vinculação com a WebApp. Voltamos para `script.yml` e colamos no fim do código. 

### Configurando o ambiente e variáveis no deploy

Agora, faremos as alterações necessárias. Quando rodamos a pipeline, estamos fazendo o `deploy` em uma máquina Ubuntu. Então, mudamos de `windows-latest` para `ubuntu-latest`.

Temos a parte da `build`, que necessita da parte ou job anterior. Após, fazemos as etapas de download do arquivo gerado, o processo de `Build`. Em seguida o `Deploy` no Azure, já configurado com a `secret` e as variáveis necessárias para fazer essa vinculação.

```yml
//Código omitido
deploy:
	runs-on: ubuntu-latest
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
			uses: azure/webapps-deploy@v2
			with:
				app-name: 'jornadamilhas-api'
				slot-name: 'Production'
				package: .
				publish-profile: ${{ secrets.AZUREAPPSERVICE_PUBLISHPROFILE_F50DF42F4CE1477EACC60CC00B54CE93 }}
```

### Desabilitando o Workflow no GitHub

Salvamos o arquivo. Mas, antes de commitar, abrimos o GitHub. Na barra de menu superior, clicamos em "Actions". Na nova página, na lateral esquerda, clicamos em "Build and deploy ASP.Net Core". 

Após, na lateral superior direita, clicamos no botão identificado por três pontos e depois em "Disable workflow", para desabilitar o workflow. 

# Fazendo o commit

Agora podemos commitar. No VS Code, na lateral direita, em Alterações do Git, nomeamos de "#Update: Finalizando nossa pipeline". Após, clicamos em "Confirmar tudo > Confirmar tudo e enviar por push". 

Voltamos para o GitHub e na barra de menu superior, clicamos em "Actions". Depois, no menu lateral esquerdo em "Pipeline" e abrimos o "#Update: Finalizando nossa pipeline".

Executando a Pipeline que definimos, após fazer a parte do `Deploy`, que é a implantação no Azure. Nossa primeira Pipeline executou com sucesso, mas a parte do `Deploy` não funcionou. Isso aconteceu, pois não foi possível encontrar o arquivo. 

### Corrigindo e Atualizando o script.yml

Então, voltamos para o arquivo `script.yml` no VS Code. Próximo à linha 26, falta um trecho de código. Precisamos fazer o upload desse arquivo para o `Deploy`, então colamos um novo trecho de código. Ficando da seguinte forma:

```yml
//Código omitido

#dotnet publish ./src/JornadaMilhas.API
- name: Publicando a API
	run: dotnet publish -c Release -o ${{env.DOTNET_ROOT}}/myapp ./src/JornadaMilhas.API
- name: Upload de artefato para deploy
	uses: actions/upload-artifact@v3
	with:
		name: .net-app
		path: ${{env.DOTNET_ROOT}}/myapp
deploy:
runs-on: ubuntu-latest
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
	uses: azure/webapps-deploy@v2
	with:
		app-name: 'jornadamilhas-api'
		slot-name: 'Production'
		package: .
		publish-profile: ${{ secrets.AZUREAPPSERVICE_PUBLISHPROFILE_F50DF42F4CE1477EACC60CC00B54CE93 }}
```

Feito isso, salvamos, commitamos novamente e voltamos ao GitHub. Clicamos em "Actions" e abrimos a nova atualização. Visualizamos as duas etapas, a "Minha primeira pipeline" e o `Deploy`. 

Após executar, ele faz a parte de configuração do ambiente, roda os testes de integração, publica a API e faz o upload dessa API para um lugar que possa ser recuperado, que é o `Deploy`. O job `Deploy` faz o download do artefato e publica no Azure. Então, rodou com sucesso.

Como expandimos a parte do `Deploy` no Azure, ele gera para nós a URL da nossa aplicação. Copiamos, abrimos uma nova janela no navegador e colamos. 

Agora, testaremos se realmente foi criado o banco e se está tudo configurado no Azure. Fazemos então o login. Depois, clicamos em "POST". Na lateral direita, clicamos no botão "Try it out". 

### Gerando o Token de Acesso

No campo de código, colamos o usuário e senha e clicamos em "Execute".

```
}
"email": "tester@email.com", 
"password": "Senha123@"
}
```

### Concluindo o ciclo CI/CD

Feito isso é gerado o token. Isso significa que a API está rodando no Azure. O interessante é que agora temos uma pipeline que faz o CI/CD, a integração e entrega contínua

Assim, fazemos a alteração no código, uma nova implementação, correção de bug e executamos os testes. Se os testes passarem, damos prosseguimento na pipeline e fazemos a publicação da API. Depois, teremos outra etapa que pegará esse arquivo e publicar no Azure. 

Dessa forma, concluímos o ciclo CI/CD com os testes de integração utilizando o GitHub Action. Em sequência, conheceremos outros recursos de implementação.

**Até a próxima aula!**